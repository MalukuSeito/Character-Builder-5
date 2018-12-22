using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace OGL.Descriptions
{
    public class TableDescription : Description
    {
        public override string InfoText => (Text?.Trim(new char[] { ' ', '\r', '\n', '\t' }) ?? "d" + (Entries?.Max(n => (int?)n?.MaxRoll) ?? 0)) + (Entries?.Count > 0 ? "\n" : "") + String.Join("\n", Entries?.Where(n => n != null)?.Select(n => n.ToFullString()) ?? new List<String>());
        public int Amount { get; set; }
        public String UniqueID { get; set; }
        public TableDescription() { }
        public String TableName { get; set; }
        [XmlArrayItem(Type = typeof(TableEntry))]
        public List<TableEntry> Entries { get; set; } = new List<TableEntry>();
        public BackgroundOption BackgroundOption { get; set; }
        public TableDescription(String name, String text, String tablename, String uniqueID, int amount=1) : base(name,text)
        {
            Name = name;
            Text = text;
            TableName = tablename;
            Entries = new List<TableEntry>();
            UniqueID = uniqueID;
            Amount = amount;
            BackgroundOption = BackgroundOption.None;
        }
        public TableDescription(String name, String text, String tablename, String uniqueID, List<TableEntry> table, int amount = 1)
            : base(name, text)
        {
            Name = name;
            Text = text;
            TableName = tablename;
            Entries = new List<TableEntry>(table);
            UniqueID = uniqueID;
            Amount = amount;
            BackgroundOption = BackgroundOption.None;
        }

        public override bool Matches(string text, bool nameOnly)
        {
            CultureInfo Culture = CultureInfo.InvariantCulture;
            if (nameOnly) return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0;
            return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Text ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Entries.Exists(s => s.Matches(text, nameOnly));
        }
    }
}
