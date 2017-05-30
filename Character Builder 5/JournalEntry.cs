using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Character_Builder_5
{
    public class JournalEntry
    {
        public String Title { get; set; }
        public String Text { get; set; }
        public String Time { get; set; }
        public DateTime Added { get; set; }
        public int XP { get; set; }
        public int PP { get; set; }
        public int GP { get; set; }
        public int EP { get; set; }
        public int SP { get; set; }
        public int CP { get; set; }

        public JournalEntry ()
        {
            Added = DateTime.Now;
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
        }

        public override string ToString()
        {
            return Added.ToShortDateString() + ": " + Title + getChanges();
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
            if (c.Count > 0) return " (" + String.Join(", ", c) + ")";
            return "";
        }
    }
}
