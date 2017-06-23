using OGL.Base;
using System;
using System.Collections.Generic;

namespace Character_Builder
{
    public class JournalEntry
    {
        public String Title { get; set; }
        public String Text { get; set; }
        public String Time { get; set; }
        public String Session { get; set; }
        public String DM { get; set; }
        public DateTime Added { get; set; }
        public int XP { get; set; }
        public int PP { get; set; }
        public int GP { get; set; }
        public int EP { get; set; }
        public int SP { get; set; }
        public int CP { get; set; }
        public int Downtime { get; set; }
        public int MagicItems { get; set; }
        public int Renown { get; set; }
        public bool InSheet { get; set; }

        public JournalEntry ()
        {
            Added = DateTime.Now;
            InSheet = true;
        }

        public JournalEntry(String title, Price p)
        {
            Added = DateTime.Now;
            Title = title;
            CP = -p.cp;
            SP = -p.sp;
            EP = -p.ep;
            GP = -p.gp;
            PP = -p.pp;
            InSheet = false;
        }

        public override string ToString()
        {
            return Added.ToString("d") + ": " + Title + getChanges();
        }

        private string getChanges()
        {
            List<string> c = new List<string>();
            if (XP > 0) c.Add("+" + XP + " XP");
            else if (XP < 0) c.Add(XP + " XP");
            if (PP > 0) c.Add("+" + PP + " pp");
            else if (PP < 0) c.Add(PP + " pp");
            if (GP > 0) c.Add("+" + GP + " gp");
            else if (GP < 0) c.Add(GP + " gp");
            if (EP > 0) c.Add("+" + EP + " ep");
            else if (EP < 0) c.Add(EP + " ep");
            if (SP > 0) c.Add("+" + SP + " sp");
            else if (SP < 0) c.Add(SP + " sp");
            if (CP > 0) c.Add("+" + CP + " cp");
            else if (CP < 0) c.Add(CP + " cp");

            if (Downtime > 0) c.Add("+" + Downtime + " downtime");
            else if (Downtime < 0) c.Add(Downtime + " downtime");
            if (Renown > 0) c.Add("+" + Renown + " renown");
            else if (Renown < 0) c.Add(Renown + " renown");
            if (MagicItems > 0) c.Add("+" + MagicItems + " magic items");
            else if (MagicItems < 0) c.Add(MagicItems + " magic items");
            if (c.Count > 0) return " (" + String.Join(", ", c) + ")";
            return "";
        }

        public string getMoney()
        {
            List<string> c = new List<string>();
            if (PP > 0) c.Add("+" + PP + " pp");
            else if (PP < 0) c.Add(PP + " pp");
            if (GP > 0) c.Add("+" + GP + " gp");
            else if (GP < 0) c.Add(GP + " gp");
            if (EP > 0) c.Add("+" + EP + " ep");
            else if (EP < 0) c.Add(EP + " ep");
            if (SP > 0) c.Add("+" + SP + " sp");
            else if (SP < 0) c.Add(SP + " sp");
            if (CP > 0) c.Add("+" + CP + " cp");
            else if (CP < 0) c.Add(CP + " cp");
            if (c.Count > 0) return String.Join(", ", c);
            return "";
        }
    }
}
