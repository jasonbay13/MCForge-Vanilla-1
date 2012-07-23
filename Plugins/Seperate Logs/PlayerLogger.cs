using System;
using System.Collections.Generic;
using System.IO;
using MCForge.API;
using MCForge.API.Events;
using MCForge.Interface.Plugin;
using MCForge.Entity;

namespace Plugins.PlayerLog
{
    public class Main : IPlugin
    {
        public string Name { get { return "separatelogging"; } }
        public int Version { get { return 1; } }
        public string Author { get { return "Sinjai"; } }
        public string CUD { get { return ""; } }
        bool all;
        List<string> players = new List<string>();
        public void OnLoad(string[] args)
        {
            Player.OnAllPlayersChat.Normal += OnChat6;
            Player.OnAllPlayersConnect.Normal += OnConnect6;
            Player.OnAllPlayersCommand.Normal += OnCommand6;
            if (!Directory.Exists("logs/Separate")) Directory.CreateDirectory("logs/Separate");
            if (!File.Exists("SeparateLogging.config"))
                using (StreamWriter w = new StreamWriter(File.Create("SeparateLogging.config")))
                {
                    w.WriteLine("### The line below this determines whether everyone is logged separately, or whether only select players will be logged separately. Valid options are all or select. ###");
                    w.WriteLine("all");
                    w.WriteLine("### Put the name(s) of the player(s) you want to log separately below. (Not case sensitive.) ###");
                    w.WriteLine("Notch");
                    w.WriteLine("Herobrine");
                    w.Close();
                    w.Dispose();
                }
            string[] lines = File.ReadAllLines("SeparateLogging.config");
            all = lines[1] == "all";
            if (!all)
                foreach (string l in lines)
                    if (l != "all" && !l.StartsWith("#") && !l.StartsWith(" "))
                        players.Add(l.ToLower());
            if (all) players = null;
        }
        public void OnChat6(Player p, ChatEventArgs e) {
            OnChat(p, e.Message);
        }
        public void OnConnect6(Player p, ConnectionEventArgs e) {
            OnConnect(p);
        }
        public void OnCommand6(Player p, CommandEventArgs e) {
            OnCommand(e.Command, p, String.Join(" ", e.Args));
        }
        public void OnConnect(Player p)
        {
            if (!all && players.Contains(p.Username.ToLower()) && !File.Exists("logs/Separate/" + p.Username + ".txt"))
                File.Create("logs/Separate/" + p.Username + ".txt");
            if (all)
                File.Create("logs/Separate/" + p.Username + ".txt").Close();
        }
        public void OnChat(Player p, string message)
        {
            if (all)
                using (StreamWriter w = File.AppendText("logs/Separate/" + p.Username + ".txt"))
                {
                    w.WriteLine("[" + DateTime.Now + "] " + p.Username + ": " + message);
                    w.Close();
                    w.Dispose();
                }
            if (!all && players.Contains(p.Username.ToLower()))
                using (StreamWriter w = File.AppendText("logs/Separate/" + p.Username + ".txt"))
                {
                    w.WriteLine("[" + DateTime.Now + "] " + p.Username + ": " + message);
                    w.Close();
                    w.Dispose();
                }
        }
        public void OnCommand(string cmd, Player p, string message)
        {
            if (all)
                using (StreamWriter w = File.AppendText("logs/Separate/" + p.Username + ".txt"))
                {
                    w.WriteLine("[" + DateTime.Now + "] " + p.Username + " used " + cmd + " " + message);
                    w.Close();
                    w.Dispose();
                }
            if (!all && players.Contains(p.Username.ToLower()))
                using (StreamWriter w = File.AppendText("logs/Separate/" + p.Username + ".txt"))
                {
                    w.WriteLine("[" + DateTime.Now + "] " + p.Username + " used " + cmd + " " + message);
                    w.Close();
                    w.Dispose();
                }
        }
        public void OnUnload()
        {
            Player.OnAllPlayersChat.Normal -= OnChat6;
            Player.OnAllPlayersCommand.Normal -= OnCommand6;
            Player.OnAllPlayersConnect.Normal -= OnConnect6;
            players = null;
        }
    }
}