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
        public int AP { get; set; }
        public int T1TP { get; set; }
        public int T2TP { get; set; }
        public int T3TP { get; set; }
        public int T4TP { get; set; }

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
            return Added.ToString("d") + ": " + Title + GetChanges();
        }

        public string GetChanges()
        {
            List<string> c = new List<string>();
            if (XP > 0) c.Add("+" + XP + " XP");
            else if (XP < 0) c.Add(XP + " XP");
            if (AP > 0) c.Add("+" + AP + " Advancement Checkpoints");
            else if (AP < 0) c.Add(AP + " Advancement Checkpoints");
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

            if (T1TP > 0) c.Add("+" + T1TP + " Tier 1 Treasure Points");
            else if (T1TP < 0) c.Add(T1TP + " Tier 1 Treasure Points");
            if (T2TP > 0) c.Add("+" + T2TP + " Tier 2 Treasure Points");
            else if (T2TP < 0) c.Add(T2TP + " Tier 2 Treasure Points");
            if (T3TP > 0) c.Add("+" + T3TP + " Tier 3 Treasure Points");
            else if (T3TP < 0) c.Add(T3TP + " Tier 3 Treasure Points");
            if (T4TP > 0) c.Add("+" + T4TP + " Tier 4 Treasure Points");
            else if (T4TP < 0) c.Add(T4TP + " Tier 4 Treasure Points");

            if (c.Count > 0) return " (" + String.Join(", ", c) + ")";
            
            return "";
        }

        public string GetMoney()
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
