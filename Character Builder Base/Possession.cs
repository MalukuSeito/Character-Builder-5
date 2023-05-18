using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Character_Builder
{
    public class Possession : IComparable<Possession>, IInfoText, IChoiceProvider
    {

        [XmlIgnore]
        public Dictionary<Feature, int> ChoiceCounter = new Dictionary<Feature, int>(new ObjectIdentityEqualityComparer());
        public bool ShouldSerializeChoiceCounter() => false;
        [XmlIgnore]
        public Dictionary<string, int> ChoiceTotal = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public bool ShouldSerializeChoiceTotal() => false;

        [XmlIgnore]
        public BuilderContext Context;
        public bool ShouldSerializeContext() => false;

        [XmlIgnore]
        public bool Free = false;
		public bool ShouldSerializeFree() => false;

		public String Name { get; set; }
        public String BaseItem { get; set; }
        public string Equipped { get; set; }
        public bool Attuned { get; set; }
        public List<string> MagicProperties { get; set; }
        public int ChargesUsed { get; set; }
        public int Count { get; set; }
        public String Description { get; set; }
        public double Weight { get; set; }
        public bool Hightlight { get; set; }

        public bool? ConsumableOverride { get; set; }
        public Rarity? RarityOverride { get; set; }
        public List<Choice> Choices { get; set; } = new List<Choice>();

        public Choice GetChoice(string ID)
        {
            Choice v = (from c in Choices where c.UniqueID == ID select c).FirstOrDefault();
            if (v == null && Context != null && Context.Player.GetChoice(ID) is Choice cc)
            {
                Choices.Add(cc);
                Context.Player.RemoveChoice(ID);
                return cc;
            }
            return v;
        }
        public void SetChoice(String ID, String Value)
        {
            Choice c = GetChoice(ID);
            if (c != null) c.Value = Value;
            else Choices.Add(new Choice(ID, Value));
        }
        public void RemoveChoice(String ID)
        {
            Choices.RemoveAll(c => c.UniqueID == ID);
        }
        public int GetChoiceOffset(Feature f, string uniqueID, int amount)
        {
            if (!ChoiceCounter.ContainsKey(f))
            {
                ChoiceCounter.Add(f, GetChoiceTotal(uniqueID));
                ChoiceTotal[uniqueID] += amount;
            }
            return ChoiceCounter[f];
        }
        public void ResetChoices()
        {

        }
        public int GetChoiceTotal(string uniqueID)
        {
            if (!ChoiceTotal.ContainsKey(uniqueID)) ChoiceTotal.Add(uniqueID, 0);
            return ChoiceTotal[uniqueID];
        }

        [XmlIgnore]
        public List<MagicProperty> Magic
        {
            get
            {
                return (from mp in MagicProperties select Context.GetMagic(mp, null)).ToList();
            }
        }
        [XmlIgnore]
        public Item Item
        {
            get
            {
                if (BaseItem != null && BaseItem != "") return Context.GetItem(BaseItem, null);
                return null;
            }
        }
        public Possession()
        {
            MagicProperties = new List<string>();
        }
        public Possession(BuilderContext context, string name, string text, int count, double weight = 0.0)
        {
            Context = context;
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
        public Possession(BuilderContext context, Item Base, int count)
        {
            Context = context;
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
        public Possession(BuilderContext context, string Base, int count, bool free = false)
        {
            Context = context;
            Name = "";
            Description = "";
            Count = count * Context.GetItem(Base, null).StackSize;
            BaseItem = Base;
            Equipped = EquipSlot.None;
            Attuned = false;
            ChargesUsed = 0;
            Hightlight = false;
            MagicProperties = new List<string>();
            Weight = -1;
            Free = free;
        }
        public Possession(Possession p)
        {
            Context = p.Context;
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
            Context = p.Context;
            Name = p.Name;
            Description = p.Description;
            Count = 1;
            BaseItem = p.BaseItem;
            if (Item != null) Count = Item.StackSize;
            Equipped = EquipSlot.None;
            Attuned = false;
            ChargesUsed = 0;
            Hightlight = false;
            MagicProperties = new List<string>(p.MagicProperties)
            {
                magic.Name + " " + ConfigManager.SourceSeperator + " " + magic.Source
            };
            Weight = -1;
        }
        public Possession(BuilderContext context, Item Base, MagicProperty magic)
        {
            Context = context;
            Name = "";
            Description = "";
            Count = 1;
            BaseItem = "";
            if (Base != null) BaseItem = Base.Name + " " + ConfigManager.SourceSeperator + " " + Base.Source;
            Equipped = EquipSlot.None;
            Attuned = false;
            ChargesUsed = 0;
            Hightlight = false;
            MagicProperties = new List<string>() { magic.Name + " " + ConfigManager.SourceSeperator + " " + magic.Source };
            Weight = -1;
        }
        public double GetWeight()
        {
            if (Weight < 0)
            {
                int count = 1;
                if (Count > 1) count = Count;
                if (Count == 0) return 0;
                if (BaseItem != null && BaseItem != "")
                {
                    int stacksize = Item.StackSize;
                    if (stacksize < 0) stacksize = 1;
                    return Item.Weight / stacksize * count;
                }
                return 0.0;
            }
            return Weight * Count;
        }

        public string FullName { get
            {
                string name = Name;
                if (name == null || name == "") name = Context.GetItem(BaseItem, null).Name;
                else return Name;
                if (Context.GetItem(BaseItem, null) is Scroll) name = "Scroll of " + name;
                foreach (string mp in MagicProperties) name = Context.GetMagic(mp, null).GetName(name);
                return name;
            }
        }

		public string BaseName
		{
			get
			{
				string name = Context.GetItem(BaseItem, null).Name;
				if (Context.GetItem(BaseItem, null) is Scroll) name = "Scroll of " + name;
				foreach (string mp in MagicProperties) name = Context.GetMagic(mp, null).GetName(name);
				return name;
			}
		}

		public string Amount
        {
            get {
                if (Count > 1) return Count + (Item != null && Item.Unit != null ? " " + Item.Unit : "");
                else if (Count == 1 && Item != null && Item.SingleUnit != null) return Item.SingleUnit;
                else if (Count == 1 && Item != null && Item.Unit != null) return Count + " " + Item.Unit;
                if (Count == 0) return "lost";
                return null;
            }
        }

        public string Stats
        {
            get
            {
                List<string> stats = new List<string>();
                OGL.Item i = Item;
                if (i is Item)
                {
                    if (MagicProperties.Count > 0) stats.Add("Magic " + i.GetType().Name);
                    else stats.Add(i.GetType().Name);
                }
                else if (MagicProperties.Count > 0) stats.Add("Wonderous Item");
                else stats.Add("Custom Item");

                if (string.Equals(Equipped, EquipSlot.Armor, StringComparison.OrdinalIgnoreCase)) stats.Add("Worn");
                else if (string.Equals(Equipped, EquipSlot.MainHand, StringComparison.OrdinalIgnoreCase)) stats.Add("Main Hand");
                else if (string.Equals(Equipped, EquipSlot.OffHand, StringComparison.OrdinalIgnoreCase)) stats.Add("Off Hand");
                else if (!string.Equals(Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase)) stats.Add("Equipped");
                if (ChargesUsed > 0) stats.Add(ChargesUsed + " Charges Used");
                return String.Join(", ", stats);
            }
        }

        public string InfoTitle => ToInfo(false);


        public string InfoText
        {
            get
            {
                Item item = null;
                item = Context.GetItem(BaseItem, null);
                string keywords = null;
                string description = Description;
                if (item != null && !item.autogenerated)
                {
                    if (item.Keywords != null)
                    {
                        keywords = String.Join(", ", item.Keywords.Select(s => s.ToString()));
                        if (keywords.Length == 0) keywords = null;
                        else keywords = keywords.Substring(0, 1).ToUpperInvariant() + keywords.Substring(1).ToLowerInvariant();
                    }
                    if (description == null || description == "")
                    {
                        description = item.Description;
                    }
                }
                string damage = "";
                if (item is Weapon w && w.Damage != null) damage = ", " + w.Damage + " " + w.DamageType ?? "";
                if (item is Armor a && a.BaseAC > 0) damage = ", " + a.BaseAC + " AC";
                return description;
            }
        }

        public override string ToString()
        {
            string name = Name;
            if (name == null || name == "")
            {
                if (BaseItem != null)
                {
                    var item = Context.GetItem(BaseItem, null);
                    if (item is Scroll) name = item.ToString();
                    else name = item.Name;
                }
            }
            if (Name == null || Name == "") foreach (string mp in MagicProperties) name = Context.GetMagic(mp, null).GetName(name);
            if (Count > 1) name = name + " (" + Count + (Item != null && Item.Unit != null ? " " + Item.Unit : "") + ")";
            else if (Count == 1 && Item != null && Item.SingleUnit != null) name = name + " (" + Item.SingleUnit + ")";
            else if (Count == 1 && Item != null && Item.Unit != null) name = name + "(" + Count + " " + Item.Unit + ")";
            if (Count == 0) name = name + " (lost)";
            if (Attuned) name = name + " (attuned)";
            if (string.Equals(Equipped, EquipSlot.Armor, StringComparison.OrdinalIgnoreCase)) name = name + " (worn)";
            else if (string.Equals(Equipped, EquipSlot.MainHand, StringComparison.OrdinalIgnoreCase)) name = name + " (main hand)";
            else if (string.Equals(Equipped, EquipSlot.OffHand, StringComparison.OrdinalIgnoreCase)) name = name + " (off hand)";
            else if (!string.Equals(Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase)) name = name + " (" + Equipped + ")";
            if (ChargesUsed > 0) name = name + " (" + ChargesUsed + " charges used)";
            return name;
        }


        public int CompareTo(Possession other) {
            return this.ToString().CompareTo(other.ToString());
        }

        public IEnumerable<Feature> Collect(int level, IChoiceProvider provider, BuilderContext context, bool pretendEquipped = false, bool includeOnUseFeatures = false, bool pretendAttuned = false)
        {
            Context = context;
            List<Feature> result = new List<Feature>();
            bool equip = !EquipSlot.None.Equals(Equipped, StringComparison.OrdinalIgnoreCase);
            foreach (string mp in MagicProperties) result.AddRange(context.GetMagic(mp, null).Collect(level, equip || pretendEquipped, Attuned || pretendAttuned, this, context, includeOnUseFeatures));
            return result;
        }
        public IEnumerable<Feature> CollectOnUse(int level, IChoiceProvider provider, BuilderContext context)
        {
            Context = context;
            List<Feature> result = new List<Feature>();
            foreach (string mp in MagicProperties) result.AddRange(context.GetMagic(mp, null).CollectOnUse(level, this, Attuned, context));
            return result;
        }

        public string ToInfo(bool desc = false)
        {
            string name = Name;
            Item item = null;
            item = Context.GetItem(BaseItem, null);
            if (name == null || name == "") {
                name = item.Name;
                if (item is Scroll) name = item.ToString();
            }
            if (Name == null || Name == "") foreach (string mp in MagicProperties) name = Context.GetMagic(mp, null).GetName(name);
            if (Count > 1) name = name + " (" + Count + (Item != null && Item.Unit != null ? " " + Item.Unit : "") + ")";
            else if (Count == 1 && Item != null && Item.SingleUnit != null) name = name + " (" + Item.SingleUnit + ")";
            else if (Count == 1 && Item != null && Item.Unit != null) name = name + "(" + Count + " " + Item.Unit + ")";
            if (Count == 0) name = name + " (lost)";
            if (Attuned) name = name + " (attuned)";
            if (string.Equals(Equipped, EquipSlot.Armor, StringComparison.OrdinalIgnoreCase)) name = name + " (worn)";
            else if (string.Equals(Equipped, EquipSlot.MainHand, StringComparison.OrdinalIgnoreCase)) name = name + " (main hand)";
            else if (string.Equals(Equipped, EquipSlot.OffHand, StringComparison.OrdinalIgnoreCase)) name = name + " (off hand)";
            else if (!string.Equals(Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase)) name = name + " (" + Equipped + ")";
            if (ChargesUsed > 0) name = name + " (" + ChargesUsed + " charges used)";
            string keywords = null;
            string description = Description;
            if (item != null && !item.autogenerated) {
                if (item.Keywords != null)
                {
                    keywords = String.Join(", ", item.Keywords.Select(s => s.ToString()));
                    if (keywords.Length == 0) keywords = null;
                    else keywords = keywords.Substring(0, 1).ToUpperInvariant() + keywords.Substring(1).ToLowerInvariant();
                }
                if (description == null || description == "")
                {
                    description = item.Description;
                }
            }
            string damage = "";
            if (item is Weapon w && w.Damage != null) damage = ", " + w.Damage + " " + w.DamageType ?? "";
            if (item is Armor a && a.BaseAC > 0) damage = ", " + a.BaseAC + " AC";
            return name + (keywords != null ? " [" + keywords + "]" : "") + damage + (description != null && description != "" && desc ? ": " + description : "");
        }

        public string ToInfo(bool kw, bool stats)
        {
            string name = Name;
            Item item = null;
            item = Context.GetItem(BaseItem, null);
            if (name == null || name == "")
            {
                name = item.Name;
                if (item is Scroll) name = item.ToString();
            }
            if (Name == null || Name == "") foreach (string mp in MagicProperties) name = Context.GetMagic(mp, null).GetName(name);
            if (Count > 1) name = name + " (" + Count + (Item != null && Item.Unit != null ? " " + Item.Unit : "") + ")";
            else if (Count == 1 && Item != null && Item.SingleUnit != null) name = name + " (" + Item.SingleUnit + ")";
            else if (Count == 1 && Item != null && Item.Unit != null) name = name + "(" + Count + " " + Item.Unit + ")";
            if (Count == 0) name = name + " (lost)";
            if (Attuned) name = name + " (attuned)";
            if (string.Equals(Equipped, EquipSlot.Armor, StringComparison.OrdinalIgnoreCase)) name = name + " (worn)";
            else if (string.Equals(Equipped, EquipSlot.MainHand, StringComparison.OrdinalIgnoreCase)) name = name + " (main hand)";
            else if (string.Equals(Equipped, EquipSlot.OffHand, StringComparison.OrdinalIgnoreCase)) name = name + " (off hand)";
            else if (!string.Equals(Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase)) name = name + " (" + Equipped + ")";
            if (ChargesUsed > 0) name = name + " (" + ChargesUsed + " charges used)";
            string keywords = null;
            string description = Description;
            if (item != null && !item.autogenerated)
            {
                if (kw && item.Keywords != null)
                {
                    keywords = String.Join(", ", item.Keywords.Select(s => s.ToString()));
                    if (keywords.Length == 0) keywords = null;
                    else keywords = keywords.Substring(0, 1).ToUpperInvariant() + keywords.Substring(1).ToLowerInvariant();
                }
            }
            string damage = "";
            if (stats && item is Weapon w && w.Damage != null) damage = ", " + w.Damage + " " + w.DamageType ?? "";
            if (stats && item is Armor a && a.BaseAC > 0) damage = ", " + a.BaseAC + " AC";
            return name + (keywords != null ? " [" + keywords + "]" : "") + damage;
        }

		public bool Matches(string expression, List<string> additionalKeywords = null, int classlevel = 0, int level = 0)
        {
            return Context.Player.Matches(expression, additionalKeywords, classlevel, level);
        }
        public bool GetConsumable()
        {
            if (Context != null && Item is Scroll) return true;
            if (MagicProperties.Count == 0) return false;
            if (Context == null) return false;
            var con = Magic.Select(mp => mp.GetConsumable()).Where(b => b != null).Select(b=>b??true).ToList();
            if (con.Count == 0) return false;
            return con.All(b=> b);
        }
        public Rarity GetRarity()
        {
            if (Context == null) return Rarity.None;
            var scrollRarity = Rarity.None;
            if (Item is Scroll s)
            {
                scrollRarity = s.Spell.Level switch
                {
                    0 => Rarity.Common,
                    1 => Rarity.Common,
                    2 => Rarity.Uncommon,
                    3 => Rarity.Uncommon,
                    4 => Rarity.Rare,
                    5 => Rarity.Rare,
                    6 => Rarity.VeryRare,
                    7 => Rarity.VeryRare,
                    8 => Rarity.VeryRare,
                    9 => Rarity.Legendary,
                    _ => Rarity.None
                };
            }
            if (MagicProperties.Count == 0) return scrollRarity;
            return Magic.Select(mp => mp.Rarity).Union(new Rarity[] {scrollRarity}).Max();

        }
        public bool Consumable { get => ConsumableOverride ?? GetConsumable(); set => ConsumableOverride = value; }
        public Rarity Rarity { get => RarityOverride ?? GetRarity(); set => RarityOverride = value; }
    }
}
