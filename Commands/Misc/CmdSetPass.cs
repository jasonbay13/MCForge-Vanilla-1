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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;

namespace CommandDll.Misc
{
    class CmdSetPass : ICommand
    {
        public string Name { get { return "SetPass"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Use(Player p, string[] args)
        {
            if (!p.IsVerified) { p.SendMessage("&cYou already have a password set. " + Server.DefaultColor + "You &ccannot change " + Server.DefaultColor + "it unless &cyou verify it with &a/pass [Password]. " + Server.DefaultColor + "If you have &cforgotten " + Server.DefaultColor + "your password, contact the server host and they can &creset it!"); return; }
            if (args[0] == "") { Help(p); return; }
            if (p.group.permission < Server.VerifyGroup.permission) { p.SendMessage("Only " + Server.VerifyGroup.color + Server.VerifyGroup.name + "s " + Server.DefaultColor + "and above need to verify."); return; }
            int number = args[0].Split(' ').Length;
            if (number > 1)
            {
                p.SendMessage("Your password must be one word!");
                return;
            }
            Crypto.EncryptStringAES(args[0], "MCForgeEncryption", p);
            p.SendMessage("Your password has &asuccessfully &abeen set to:");
            p.SendMessage("&c" + args[0]);
            return;
        }
        public class Crypto
        {
            private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");

            /// <summary>
            /// Encrypt the given string using AES.  The string can be decrypted using 
            /// DecryptStringAES().  The sharedSecret parameters must match.
            /// </summary>
            /// <param name="plainText">The text to encrypt.</param>
            /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
            public static string EncryptStringAES(string plainText, string sharedSecret, Player who)
            {
                if (string.IsNullOrEmpty(plainText))
                    throw new ArgumentNullException("plainText");
                if (string.IsNullOrEmpty(sharedSecret))
                    throw new ArgumentNullException("sharedSecret");

                string outStr = null;                       // Encrypted string to return
                RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

                try
                {
                    // generate the key from the shared secret and the salt
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {

                                //Write all data to the stream.
                                swEncrypt.Write(plainText);
                            }
                        }
                        outStr = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
                finally
                {
                    // Clear the RijndaelManaged object.
                    if (aesAlg != null)
                        aesAlg.Clear();
                }

                // Return the encrypted bytes from the memory stream.
                if (!Directory.Exists("extra/passwords"))
                {
                    Directory.CreateDirectory("extra/passwords");
                }
                try
                {
                    if (File.Exists("extra/passwords/" + who.Username + ".xml"))
                    {
                        File.Delete("extra/passwords/" + who.Username + ".xml");
                    }
                    StreamWriter SW = new StreamWriter(File.Create("extra/passwords/" + who.Username + ".xml"));
                    SW.WriteLine(outStr);
                    SW.Flush();
                    SW.Close();
                    File.WriteAllText("extra/passwords/" + who.Username + ".xml", outStr);
                }
                catch
                {
                    who.SendMessage("&cFailed to Save Password, &aTry Again Later!");
                    Server.Log(who.Username + " failed to save password.");
                }
                return outStr;

            }
        }
        public void Help(Player p)
        {
            p.SendMessage("/setpass <password> - set your verification password.");
        }
        public void Initialize()
        {
            Command.AddReference(this, "setpass");
        }
    }
}
