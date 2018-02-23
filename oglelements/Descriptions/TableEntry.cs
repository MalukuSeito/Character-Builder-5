using OGL.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OGL.Descriptions
{
    public class TableEntry: IMatchable
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

        public string ToFullString()
        {
            if (Title != null && Title != "") return roll() + Title + " " + Text;
            return roll() + Text;
        }
        private string roll()
        {
            if (MinRoll >= 0)
            {
                if (MaxRoll > MinRoll) return MinRoll + "-" + MaxRoll + ": ";
                return MinRoll + ": ";
            }
            return "";
        }

        public string Save()
        {
            return new TableDescription(null, null, null, null, new List<TableEntry>() { this }).Save();
        }

        public bool Matches(string text, bool nameOnly)
        {
            CultureInfo Culture = CultureInfo.InvariantCulture;
            if (nameOnly) return false;
            return Culture.CompareInfo.IndexOf(Title ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Text ?? "", text, CompareOptions.IgnoreCase) >= 0;
        }
    }
}
