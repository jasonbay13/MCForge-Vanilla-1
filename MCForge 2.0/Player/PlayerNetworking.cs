/*
Copyright 2011 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/

using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Windows.Forms;
using MCForge.API.PlayerEvent;
using MCForge.Core;
using MCForge.World;
using MCForge.Interface.Command;
using MCForge.Groups;
using MCForge.Utilities.Settings;

namespace MCForge.Entity
{
	public partial class Player
	{
		#region Incoming Data
		protected static void Incoming(IAsyncResult result)
		{
			while (!Server.Started)
				Thread.Sleep(100);

			Player p = (Player)result.AsyncState;

			if (!p.isOnline) return;

			try
			{
				int length = p.socket.EndReceive(result);
				if (length == 0)
				{
					p.CloseConnection();
					if (!p.beingkicked)
					{
						UniversalChat(p.color + p.USERNAME + " has disconnected.");
					}
					return;
				}
				byte[] b = new byte[p.buffer.Length + length];
				Buffer.BlockCopy(p.buffer, 0, b, 0, p.buffer.Length);
				Buffer.BlockCopy(p.tempBuffer, 0, b, p.buffer.Length, length);
				p.buffer = p.HandlePacket(b);
				p.socket.BeginReceive(p.tempBuffer, 0, p.tempBuffer.Length, SocketFlags.None, new AsyncCallback(Incoming), p);
			}
			catch (SocketException e)
			{
				SocketException rawr = e;
				p.CloseConnection();
				return;
			}
			catch (Exception e)
			{
				Exception rawr = e;
				p.Kick("Error!");
				Server.Log(e);
				return;
			}
		}
		protected byte[] HandlePacket(byte[] buffer)
		{
			try
			{
				int length = 0; byte msg = buffer[0];
				// Get the length of the message by checking the first byte
				switch (msg)
				{
					case 0: length = 130; break; // login
					case 2: SMPKick("This is not an SMP Server!"); break; // login??
					case 5: length = 8; break; // blockchange
					case 8: length = 9; break; // input
					case 13: length = 65; break; // chat
					default: Kick("Unhandled message id \"" + msg + "\"!"); return new byte[0];
				}
				if (buffer.Length > length)
				{
					byte[] message = new byte[length];
					Buffer.BlockCopy(buffer, 1, message, 0, length);

					byte[] tempbuffer = new byte[buffer.Length - length - 1];
					Buffer.BlockCopy(buffer, length + 1, tempbuffer, 0, buffer.Length - length - 1);

					buffer = tempbuffer;

					ThreadPool.QueueUserWorkItem(delegate
					{
						switch (msg)
						{
							case 0: HandleLogin(message); break;
							case 5: HandleBlockchange(message); break;
							case 8: HandleIncomingPos(message); break;
							case 13: HandleChat(message); break;
						}
					});

					if (buffer.Length > 0)
						buffer = HandlePacket(buffer);
					else
						return new byte[0];


				}
			}
			catch (Exception e)
			{
				Kick("CONNECTION ERROR: (0x03)");
				Server.Log("[ERROR]: PLAYER MESSAGE RECIEVE ERROR (0x03)", ConsoleColor.Red, ConsoleColor.Black);
				Server.Log(e);
			}
			return buffer;
		}

		protected void HandleLogin(byte[] message)
		{
			try
			{
				if (isLoggedIn) return;
				byte version = message[0];
				USERNAME = enc.GetString(message, 1, 64).Trim();
				string verify = enc.GetString(message, 65, 32).Trim();
				byte type = message[129];
				if (!VerifyAccount(USERNAME, verify)) return;
				if (version != ServerSettings.Version) { SKick("Wrong Version!."); return; }

				OnPlayerConnect e = new OnPlayerConnect(this);
				e.Call();
				if (e.IsCanceled)
					return;

				//TODO Database Stuff

				Server.Log("[System]: " + ip + " logging in as " + USERNAME + ".", ConsoleColor.Green, ConsoleColor.Black);
				UniversalChat(USERNAME + " joined the game!");

				CheckDuplicatePlayers(USERNAME);

				foreach (PlayerGroup g in PlayerGroup.groups)
					if (g.players.Contains(USERNAME.ToLower()))
						group = g;

				SendMotd();

				isLoading = true;
				SendMap();
				if (!isOnline) return;
				isLoggedIn = true;

				id = FreeId();
				UpgradeConnectionToPlayer();

				short x = (short)((0.5 + level.SpawnPos.x) * 32);
				short y = (short)((1 + level.SpawnPos.y) * 32);
				short z = (short)((0.5 + level.SpawnPos.z) * 32);

				Pos = new Vector3(x, z, y);
				Rot = level.SpawnRot;
				oldPos = Pos;
				oldRot = Rot;

				SpawnThisPlayerToOtherPlayers();
				SpawnOtherPlayersForThisPlayer();
				SendSpawn(this);

				isLoading = false;

				foreach (string w in ServerSettings.GetSetting("welcomemessage").Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries))
					SendMessage(w);

			}
			catch (Exception e)
			{
				Server.Log(e);
			}
		}
		protected void HandleBlockchange(byte[] message)
		{
			if (!isLoggedIn) return;

			ushort x = packet.NTHO(message, 0);
			ushort y = packet.NTHO(message, 2);
			ushort z = packet.NTHO(message, 4);
			byte action = message[6];
			byte newType = message[7];

			lastClick = new Vector3(x, z, y);

			if (newType > 49 || (newType == 7 && !isAdmin))
			{
				Kick("HACKED CLIENT!");
				//TODO Send message to op's for adminium hack
				return;
			}

			byte currentType = level.GetBlock(x, z, y);
			if (currentType == (byte)Blocks.Types.zero)
			{
				Kick("HACKED CLIENT!");
				return;
			}

			//TODO Check for permissions to build and distance > max
			bool placing = false;
			if (action == 1) placing = true;
			OnPlayerBlockChange b = new OnPlayerBlockChange(x, y, z, (placing ? ActionType.Place : ActionType.Delete), this, newType);
			b.Call();
			if (b.IsCanceled)
				return;
			if (blockChange != null)
			{
				SendBlockChange(x, z, y, currentType);

				BlockChangeDelegate tempBlockChange = blockChange;
				object tempPassBack = PassBackData;

				blockChange = null;
				PassBackData = null;

				ThreadPool.QueueUserWorkItem(delegate { tempBlockChange.Invoke(this, x, z, y, newType, placing, tempPassBack); });
				return;
			}

			if (action == 0) //Deleting
			{
				level.BlockChange(x, z, y, (byte)Blocks.Types.air);
			}
			else //Placing
			{
				level.BlockChange(x, z, y, newType);
			}
		}
		protected void HandleIncomingPos(byte[] message)
		{
			if (!isLoggedIn)
				return;

			byte thisid = message[0];

			if (thisid != 0xFF && thisid != id && thisid != 0)
			{
				//TODO Player.GlobalMessageOps("Player sent a malformed packet!");
				Kick("Hacked Client!");
				return;
			}

			ushort x = packet.NTHO(message, 1);
			ushort y = packet.NTHO(message, 3);
			ushort z = packet.NTHO(message, 5);
			byte rotx = message[7];
			byte roty = message[8];
			OnPlayerMove m = new OnPlayerMove(this, Pos, new Vector3(x, z, y));
			m.Call();
			if (m.IsCanceled)
				return;
			Pos.x = (short)x;
			Pos.y = (short)y;
			Pos.z = (short)z;
			Rot = new byte[2] { rotx, roty };
		}
		protected void HandleChat(byte[] message)
		{
			if (!isLoggedIn) return;

			string incomingText = enc.GetString(message, 1, 64).Trim();

			byte incomingID = message[0];
			if (incomingID != 0xFF && incomingID != id && incomingID != 0)
			{
				//TODO Player.GlobalMessageOps("Player sent a malformed packet!");
				Kick("Hacked Client!");
				return;
			}

			incomingText = Regex.Replace(incomingText, @"\s\s+", " ");
			foreach (char ch in incomingText)
			{
				if (ch < 32 || ch >= 127 || ch == '&')
				{
					Kick("Illegal character in chat message!");
					return;
				}
			}
			if (incomingText.Length == 0)
				return;
			//Fixes crash
			if (incomingText[0] == '/' && incomingText.Length == 1)
			{
				SendMessage("You didn't specify a command!");
				return;
			}
			//Get rid of whitespace
			while (incomingText.Contains("  "))
				incomingText.Replace("  ", " ");

			//This allows people to use //Command and have it appear as /Command in the chat.
			if (incomingText[0] == '/' && incomingText[1] == '/')
			{
				incomingText = incomingText.Remove(0, 1);
				goto Meep;
			}
			if (incomingText[0] == '/')
			{
				incomingText = incomingText.Remove(0, 1);

				string[] args = incomingText.Split(' ');
				HandleCommand(args);
				return;
			}

			//Meep is used above for //Command
			Meep:

			if (!File.Exists("text/badwords.txt")) { File.Create("text/badwords.txt").Close(); }
			if (!File.Exists("text/replacementwords.txt")) { File.Create("text/replacementwords.txt").Close(); }

			string textz = File.ReadAllText("text/replacementwords.txt");
			if (textz == "") { File.WriteAllText("text/replacementwords.txt", "Pepper"); }
			StreamReader w = File.OpenText("text/replacementwords.txt");
			while (!w.EndOfStream) replacement.Add(w.ReadLine());
			w.Dispose();

			string[] badwords = File.ReadAllLines("text/badwords.txt");
			string[] replacementwords = File.ReadAllLines("text/replacementwords.txt");

			foreach (string word in badwords)
			{
				string text = incomingText;
				if (text.Contains(word))
				{
					incomingText = Regex.Replace(text, word, replacement[new Random().Next(0, replacement.Count)]);
				}
			}
			if (muted) { SendMessage("You are muted!"); return; }
			if (Server.moderation && !voiced && !Server.devs.Contains(USERNAME)) { SendMessage("You can't talk during chat moderation!"); return; }
			if (jokered)
			{
				Random r = new Random();
				int a = r.Next(0, Server.jokermessages.Count);
				incomingText = Server.jokermessages[a];
			}
			//Message appending stuff.
			if (ServerSettings.GetSettingBoolean("messageappending"))
			{
				if (storedMessage != "")
				{
					if (!incomingText.EndsWith(">") && !incomingText.EndsWith("<"))
					{
						incomingText = storedMessage.Replace("|>|", " ").Replace("|<|", "") + incomingText;
						storedMessage = "";
					}
				}
				if (incomingText.EndsWith(">"))
				{
					storedMessage += incomingText.Replace(">", "|>|");
					SendMessage("Message appended!");
					return;
				}
				else if (incomingText.EndsWith("<"))
				{
					storedMessage += incomingText.Replace("<", "|<|");
					SendMessage("Message appended!");
					return;
				}
			}

			/*if (nextChat != null) AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH
			{
				NextChatDelegate tempNextChat = nextChat;
				object tempPassBack = PassBackData;

				nextChat = null;
				PassBackData = null;

				ThreadPool.QueueUserWorkItem(delegate { tempNextChat.Invoke(this, incomingText, tempPassBack); });

				return;
			}*/

			OnPlayerChat e = new OnPlayerChat(this, incomingText);
			e.Call();
			if (e.IsCanceled)
				return;
			incomingText = e.GetMessage();

			if (Server.voting)
			{
				if (Server.kickvote && Server.kicker == this) { SendMessage("You're not allowed to vote!"); return; }
				if (voted) { SendMessage("You have already voted..."); return; }
				string vote = incomingText.ToLower();
				if (vote == "yes" || vote == "y") { Server.YesVotes++; voted = true; SendMessage("Thanks for voting!"); return; }
				else if (vote == "no" || vote == "n") { Server.NoVotes++; voted = true; SendMessage("Thanks for voting!"); return; }
				else { SendMessage("Use either %aYes " + Server.DefaultColor + "or %cNo " + Server.DefaultColor + " to vote!"); }
			}
			Server.Log("<" + USERNAME + "> " + incomingText);
			UniversalChat(voicestring + group.colour + USERNAME + ": &f" + incomingText);
		}

		#endregion
		#region Outgoing Packets
		protected void SendPacket(packet pa)
		{
			try
			{
				lastPacket = (packet.types)pa.bytes[0];
			}
			catch (Exception e) { Server.Log(e); }
			for (int i = 0; i < 3; i++)
			{
				try
				{
					socket.BeginSend(pa.bytes, 0, pa.bytes.Length, SocketFlags.None, delegate(IAsyncResult result) { }, null);
					return;
				}
				catch
				{
					continue;
				}
			}
			CloseConnection();
		}
		protected void SendMessage(byte PlayerID, string message)
		{
			packet pa = new packet();


			for (int i = 0; i < 10; i++)
			{
				message = message.Replace("%" + i, "&" + i);
				message = message.Replace("&" + i + " &", "&");
			}
			for (char ch = 'a'; ch <= 'f'; ch++)
			{
				message = message.Replace("%" + ch, "&" + ch);
				message = message.Replace("&" + ch + " &", "&");
			}
			message = Server.DefaultColor + message;

			pa.Add(packet.types.Message);
			pa.Add(PlayerID);

			try
			{
				foreach (string line in LineWrapping(message))
				{
					if (pa.bytes.Length < 64)
						pa.Add(line, 64);
					else
						pa.Set(2, line, 64);

					SendPacket(pa);
				}
			}
			catch (Exception e)
			{
				Server.Log(e);
			}

		}
		protected void SendMotd()
		{
			if (isAdmin) SendPacket(MOTD_Admin);
			else SendPacket(MOTD_NonAdmin);
		}
		protected void SendMap()
		{
			try
			{
				SendPacket(mapSendStartPacket); //Send the pre-fab map start packet

				packet pa = new packet(); //Create a packet to handle the data for the map
				pa.Add(level.TotalBlocks); //Add the total amount of blocks to the packet
				byte[] blocks = new byte[level.TotalBlocks]; //Temporary byte array so we dont have to keep modding the packet array

				byte block; //A block byte outside the loop, we save cycles by not making this for every loop iteration
				level.ForEachBlock(delegate(int pos)
				{
					//Here we loop through the whole map and check/convert the blocks as necesary
					//We then add them to our blocks array so we can send them to the player
					block = level.data[pos];
					if (block < 50) blocks[pos] = block;
					else blocks[pos] = Blocks.CustomBlocks[block].VisibleType;
				});

				pa.Add(blocks); //add the blocks to the packet
				pa.GZip(); //GZip the packet

				int number = (int)Math.Ceiling(((double)(pa.bytes.Length)) / 1024); //The magic number for this packet

				for (int i = 1; pa.bytes.Length > 0; ++i)
				{
					short length = (short)Math.Min(pa.bytes.Length, 1024);
					byte[] send = new byte[1027];
					packet.HTNO(length).CopyTo(send, 0);
					Buffer.BlockCopy(pa.bytes, 0, send, 2, length);
					byte[] tempbuffer = new byte[pa.bytes.Length - length];
					Buffer.BlockCopy(pa.bytes, length, tempbuffer, 0, pa.bytes.Length - length);
					pa.bytes = tempbuffer;
					send[1026] = (byte)(i * 100 / number);

					packet Send = new packet(send);
					Send.AddStart(new byte[1] { (byte)packet.types.MapData });

					SendPacket(Send);
				}

				pa = new packet();
				pa.Add(packet.types.MapEnd);
				pa.Add((short)level.Size.x);
				pa.Add((short)level.Size.y);
				pa.Add((short)level.Size.z);
				SendPacket(pa);

				isLoading = false;
			}
			catch (Exception e)
			{
				Server.Log(e);
			}
		}
		protected void SendSpawn(Player p)
		{
			byte ID = 0xFF;
			if (p != this)
				ID = p.id;

			packet pa = new packet();
			pa.Add(packet.types.SendSpawn);
			pa.Add((byte)ID);
			pa.Add(p.USERNAME, 64);
			pa.Add(p.Pos.x);
			pa.Add(p.Pos.y);
			pa.Add(p.Pos.z);
			pa.Add(p.Rot);
			SendPacket(pa);
		}
		/// <summary>
		/// This send a blockchange to the player only. (Not other players)
		/// </summary>
		/// <param name="x"></param> The position the block will be placed in (x)
		/// <param name="z"></param> The position the block will be placed in (z)
		/// <param name="y"></param> The position the block will be placed in (y)
		/// <param name="type"></param> The type of block that will be placed.
		public void SendBlockChange(ushort x, ushort z, ushort y, byte type)
		{
			if (x < 0 || y < 0 || z < 0 || x >= level.Size.x || y >= level.Size.y || z >= level.Size.z) return;

			packet pa = new packet();
			pa.Add(packet.types.SendBlockchange);
			pa.Add(x);
			pa.Add(y);
			pa.Add(z);

			if (type > 49) type = Blocks.CustomBlocks[type].VisibleType;
			pa.Add(type);

			SendPacket(pa);
		}
		protected void SendKick(string message)
		{
			packet pa = new packet();
			pa.Add(packet.types.SendKick);
			pa.Add(message, 64);
			SendPacket(pa);
		}
		protected void SMPKick(string a)
		{
			//TODO SMPKICK
		}
		protected void SendPing()
		{
			SendPacket(pingPacket);
		}

		/// <summary>
		/// Send this player a message
		/// </summary>
		/// <param name="message">The message to send</param>
		public void SendMessage(string message)
		{
			SendMessage(id, message); // 0xFF is NOT a valid player ID
		}
		/// <summary>
		/// Exactly what the function name is, it might be useful to change this players pos first ;)
		/// </summary>
		public void SendThisPlayerTheirOwnPos()
		{
			packet pa = new packet();
			pa.Add((byte)8);
			pa.Add(Pos.x);
			pa.Add(Pos.y);
			pa.Add(Pos.z);
			pa.Add(Rot);
			SendPacket(pa);
		}
		/// <summary>
		/// Kick this player with the specified message, the message broadcasts across the server
		/// </summary>
		/// <param name="message">The message to send</param>
		public void Kick(string message)
		{
			//GlobalMessage(message);
			beingkicked = true;
			SKick(message);
		}
		/// <summary>
		/// Kick this player with a specified message, this message will only get sent to op's
		/// </summary>
		/// <param name="message">The message to send</param>
		public void SKick(string message)
		{
			Server.Log("[Info]: Kicked: *" + USERNAME + "* " + message, ConsoleColor.Yellow, ConsoleColor.Black);
			SendKick(message);
			//CloseConnection();
		}
		/// <summary>
		/// Sends the specified player to the specified coordinates.
		/// </summary>
		/// <param name="_pos"></param>Vector3 coordinate to send to.
		/// <param name="_rot"></param>Rot to send to.
		public void SendToPos(Vector3 _pos, byte[] _rot)
		{
			oldPos = Pos; oldRot = Rot;

			packet pa = new packet();
			pa.Add(packet.types.SendTeleport);
			pa.Add(unchecked((byte)-1)); //If the ID is not greater than one it doesn't work :c
			pa.Add(_pos.x);
			pa.Add(_pos.y);
			pa.Add(_pos.z);
			pa.Add(Rot);

			Server.ForeachPlayer(delegate(Player p)
			{
				if (p.level == level && p.isLoggedIn && !p.isLoading)
				{
					p.SendPacket(pa);
				}
			});
		}

		protected void UpdatePosition(bool ForceTp)
		{
			byte changed = 0;   //Denotes what has changed (x,y,z, rotation-x, rotation-y)

			Vector3 tempOldPos = oldPos;
			Vector3 tempPos = Pos;
			byte[] tempRot = Rot;
			byte[] tempOldRot = oldRot;

			oldPos = Pos; oldRot = Rot;

			int diffX = tempPos.x - tempOldPos.x;
			int diffZ = tempPos.z - tempOldPos.z;
			int diffY = tempPos.y - tempOldPos.y;
			int diffR0 = tempRot[0] - tempRot[0];
			int diffR1 = tempRot[1] - tempRot[1];

			if (ForceTp) changed = 4;
			else
			{
				//TODO rewrite local pos change code
				if (diffX == 0 && diffY == 0 && diffZ == 0 && diffR0 == 0 && diffR1 == 0)
				{
					return; //No changes
				}
				if (Math.Abs(diffX) > 100 || Math.Abs(diffY) > 100 || Math.Abs(diffZ) > 100)
				{
					changed = 4; //Teleport Required
				}
				else if (diffR0 == 0 && diffR1 == 0)
				{
					changed = 1; //Pos Update Required
				}
				else
				{
					changed += 2; //Rot Update Required

					if (diffX != 0 || diffY != 0 || diffZ != 0)
					{
						changed += 1;
					}
				}
			}

			packet pa = new packet();

			switch (changed)
			{
				case 1: //Pos Change
					pa.Add(packet.types.SendPosChange);
					pa.Add(id);
					pa.Add((sbyte)(diffX));
					pa.Add((sbyte)(diffY));
					pa.Add((sbyte)(diffZ));
					break;
				case 2: //Rot Change
					pa.Add(packet.types.SendRotChange);
					pa.Add(id);
					pa.Add(new byte[2] { (byte)diffR0, (byte)diffR1 });
					break;
				case 3: //Pos AND Rot Change
					pa.Add(packet.types.SendPosANDRotChange);
					pa.Add(id);
					pa.Add(diffX);
					pa.Add(diffY);
					pa.Add(diffZ);
					pa.Add(new byte[2] { (byte)diffR0, (byte)diffR1 });
					break;
				case 4: //Teleport Required
					pa.Add(packet.types.SendTeleport);
					pa.Add(id);
					pa.Add(tempPos.x);
					pa.Add(tempPos.y);
					pa.Add(tempPos.z);
					pa.Add(Rot);
					break;
			}

			Server.ForeachPlayer(delegate(Player p)
			{
				if (p != this && p.level == level && p.isLoggedIn && !p.isLoading)
				{
					p.SendPacket(pa);
				}
			});
		}
		#endregion

		/// <summary>
		/// Spawns this player to all other players in the server.
		/// </summary>
		protected void SpawnThisPlayerToOtherPlayers()
		{
			Server.ForeachPlayer(delegate(Player p)
			{
				if (p != this) p.SendSpawn(this);
			});
		}
		/// <summary>
		/// Spawns all other players of the server to this player.
		/// </summary>
		protected void SpawnOtherPlayersForThisPlayer()
		{
			Server.ForeachPlayer(delegate(Player p)
			{
				if (p != this) SendSpawn(p);
			});
		}

		internal static void GlobalBlockchange(Level l, ushort x, ushort z, ushort y, byte block)
		{
			Server.ForeachPlayer(delegate(Player p)
			{
				if (p.level == l)
					p.SendBlockChange(x, z, y, block);
			});
		}
		/// <summary>
		/// Kill this player for everyone.
		/// </summary>
		public void GlobalDie()
		{
			packet pa = new packet(new byte[2] { (byte)packet.types.SendDie, id });
			Server.ForeachPlayer(delegate(Player p)
			{
				if (p != this)
				{
					p.SendPacket(pa);
				}
			});
		}
		/// <summary>
		/// Send a message to everyone, on every world
		/// </summary>
		/// <param name="text">The message to send.</param>
		public static void UniversalChat(string text)
		{
			Server.ForeachPlayer(delegate(Player p)
			{
				p.SendMessage(text);
			});
		}

		protected void CloseConnection()
		{
			isLoggedIn = false;
			isOnline = false;

			GlobalDie();
			Server.Log("[System]: " + USERNAME + " Has DC'ed (" + lastPacket + ")", ConsoleColor.Gray, ConsoleColor.Black);

			pingTimer.Stop();

			Server.RemovePlayer(this);
			Server.Connections.Remove(this);

			socket.Close();
		}

	}
}
