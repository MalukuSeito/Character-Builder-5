using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Character_Builder
{
    public class Possession : IComparable<Possession>, IHTML
    {
        [XmlIgnore]
        private static XmlSerializer serializer = new XmlSerializer(typeof(DisplayPossession));
        [XmlIgnore]
        private static XslCompiledTransform transform = new XslCompiledTransform();
        public String Name { get; set; }
        public String BaseItem { get; set; }
        public string Equipped { get; set; }
        public bool Attuned { get; set; }
        public List<string> MagicProperties;
        public int ChargesUsed { get; set; }
        public int Count { get; set; }
        public String Description { get; set; }
        public double Weight { get; set; }
        public bool Hightlight { get; set; }
        [XmlIgnore]
        public List<MagicProperty> Magic
        {
            get
            {
                return (from mp in MagicProperties select MagicProperty.Get(mp, null)).ToList();
            }
        }
        [XmlIgnore]
        public Item Item
        {
            get
            {
                if (BaseItem != null && BaseItem != "") return Item.Get(BaseItem, null);
                return null;
            }
        }
        public Possession()
        {
            MagicProperties = new List<string>();
        }
        public Possession(string name, string text, int count, double weight=0.0)
        {
            Name = name;
            Description = text;
            Count = count;
            BaseItem = "";
            Equipped = EquipSlot.None;
            Attuned = false;
            ChargesUsed = 0;
            Weight = weight;
            Hightlight = false;
            MagicProperties = new List<string>();
        }
        public Possession(Item Base, int count)
        {
            Name = "";
            Description = "";
            Count = count;
            if (Base != null) BaseItem = Base.Name + " " + ConfigManager.SourceSeperator + " " + Base.Source;
            else BaseItem = "";
            Equipped = EquipSlot.None;
            Attuned = false;
            ChargesUsed = 0;
            Hightlight = false;
            MagicProperties = new List<string>();
            Weight = -1;
        }
        public Possession(string Base, int count)
        {
            Name = "";
            Description = "";
            Count = count*Item.Get(Base, null).StackSize;
            BaseItem = Base;
            Equipped = EquipSlot.None;
            Attuned = false;
            ChargesUsed = 0;
            Hightlight = false;
            MagicProperties = new List<string>();
            Weight = -1;
        }
        public Possession(Possession p)
        {
            Name = p.Name;
            Description = p.Description;
            Count = 1;
            BaseItem = p.BaseItem;
            if (Item != null) Count = Item.StackSize;
            Equipped = EquipSlot.None;
            Attuned = false;
            ChargesUsed = 0;
            Hightlight = false;
            MagicProperties = new List<string>(p.MagicProperties);
            Weight = -1;
        }
        public Possession(Possession p, MagicProperty magic)
        {
            Name = p.Name;
            Description = p.Description;
            Count = 1;
            BaseItem = p.BaseItem;
            if (Item != null) Count = Item.StackSize;
            Equipped = EquipSlot.None;
            Attuned = false;
            ChargesUsed = 0;
            Hightlight = false;
            MagicProperties = new List<string>(p.MagicProperties);
            MagicProperties.Add(magic.Name + " " + ConfigManager.SourceSeperator + " " + magic.Source);
            Weight = -1;
        }
        public Possession(Item Base, MagicProperty magic)
        {
            Name = "";
            Description = "";
            Count = 1;
            BaseItem = "";
            if (Base!=null) BaseItem = Base.Name + " " + ConfigManager.SourceSeperator + " " + Base.Source;
            Equipped = EquipSlot.None;
            Attuned = false;
            ChargesUsed = 0;
            Hightlight = false;
            MagicProperties = new List<string>() {magic.Name + " " + ConfigManager.SourceSeperator + " " + magic.Source };
            Weight = -1;
        }
        public double getWeight()
        {
            if (Weight < 0)
            {
                int count = 1;
                if (Count > 1) count = Count;
                if (BaseItem != null && BaseItem != "")
                {
                    int stacksize = Item.StackSize;
                    if (stacksize < 0) stacksize = 1;
                    return Item.Weight / stacksize * count;
                }
                return 0.0;
            }
            return Weight;
        }
        public override string ToString()
        {
            string name = Name;
            if (name == null || name == "") name = Item.Get(BaseItem, null).Name;
            foreach (string mp in MagicProperties) name = MagicProperty.Get(mp, null).GetName(name);
            if (Count > 1) name = name + " (" + Count + (Item != null && Item.Unit != null ? " " + Item.Unit : "") + ")";
            else if (Count == 1 && Item != null && Item.SingleUnit != null) name = name + " (" + Item.SingleUnit + ")";
            else if (Count == 1 && Item != null && Item.Unit != null) name = name + "(" + Count + " " + Item.Unit + ")";
            if (Count == 0) name = name + " (lost)";
            if (Attuned) name = name + " (attuned)";
            if (string.Equals(Equipped, EquipSlot.Armor, StringComparison.InvariantCultureIgnoreCase)) name = name + " (worn)";
            else if (string.Equals(Equipped, EquipSlot.MainHand, StringComparison.InvariantCultureIgnoreCase)) name = name + " (main hand)";
            else if (string.Equals(Equipped, EquipSlot.OffHand, StringComparison.InvariantCultureIgnoreCase)) name = name + " (off hand)";
            else if (!string.Equals(Equipped, EquipSlot.None, StringComparison.InvariantCultureIgnoreCase)) name = name + " (" + Equipped + ")";
            if (ChargesUsed > 0) name = name + " (" + ChargesUsed + " charges used)";
            return name;
        }

        public virtual String ToHTML()
        {
            try
            {
                if (transform.OutputSettings == null) transform.Load(ConfigManager.Transform_Possession.FullName);
                using (MemoryStream mem = new MemoryStream())
                {
                    serializer.Serialize(mem, new DisplayPossession(this));
                    ConfigManager.RemoveDescription(mem);
                    mem.Seek(0, SeekOrigin.Begin);
                    using (TextWriter writer = new StringWriter())
                    {
                        serializer.Serialize(writer, new DisplayPossession(this));
                    }
                    XmlReader xr = XmlReader.Create(mem);
                    using (StringWriter textWriter = new StringWriter())
                    {
                        using (XmlWriter xw = XmlWriter.Create(textWriter))
                        {
                            transform.Transform(xr, xw);
                            return textWriter.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "<html><body><b>Error generating output:</b><br>" + ex.Message + "<br>" + ex.InnerException + "<br>" + ex.StackTrace + "</body></html>";
            }
        }
        public int CompareTo(Possession other) {
            return this.ToString().CompareTo(other.ToString());
        }

        public IEnumerable<Feature> Collect(int level, IChoiceProvider provider, bool pretendEquipped = false)
        {
            List<Feature> result = new List<Feature>();
            bool equip = !EquipSlot.None.Equals(Equipped, StringComparison.InvariantCultureIgnoreCase);
            foreach (string mp in MagicProperties) result.AddRange(MagicProperty.Get(mp, null).Collect(level, equip || pretendEquipped, Attuned, provider));
            return result;
        }
        public IEnumerable<Feature> CollectOnUse(int level, IChoiceProvider provider)
        {
            List<Feature> result = new List<Feature>();
            foreach (string mp in MagicProperties) result.AddRange(MagicProperty.Get(mp, null).CollectOnUse(level, provider, Attuned));
            return result;
        }
    }
}
