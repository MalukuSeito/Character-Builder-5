using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OGL.Descriptions
{
    public class TableDescription : Description
    {
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
    }
}
