using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class TableEntry
    {
        public int MinRoll { get; set; }
        public int MaxRoll { get; set; }
        public String Title { get; set; }
        public String Text { get; set; }
        public TableEntry() { }
        public TableEntry(int min, String text, int max = 0)
        {
            if (max == 0) max = min;
            Text = text;
            Title = "";
            MinRoll = min;
            MaxRoll = max;
        }
        public TableEntry(int min, String title, String text, int max = 0)
        {
            if (max == 0) max = min;
            Text = text;
            Title = title;
            MinRoll = min;
            MaxRoll = max;
        }
        public override string ToString()
        {
            if (Title != null && Title != "") return Title + " " + Text;
            return Text;
        }
    }
}
