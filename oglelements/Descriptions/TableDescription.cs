using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class TableDescription : Description
    {
        public int Amount { get; set; }
        public String UniqueID { get; set; }
        public TableDescription() { }
        public String TableName { get; set; }
        public List<TableEntry> Entries = new List<TableEntry>();
        public TableDescription(String name, String text, String tablename, String uniqueID, int amount=1) : base(name,text)
        {
            Name = name;
            Text = text;
            TableName = tablename;
            Entries = new List<TableEntry>();
            UniqueID = uniqueID;
            Amount = amount;
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
        }
    }
}
