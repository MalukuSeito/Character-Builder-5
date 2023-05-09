using OGL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public class JournalPossession: Possession
    {
        public Guid Guid { get; set; }
        public bool Deleted { get; set; }
        public bool Banked { get; set; }
        public bool New { get; set; }
        public JournalPossession() {
        }
        public JournalPossession(Possession p) : base(p)
        {
            Guid = Guid.NewGuid();
            New = true;
        }

        public JournalPossession(JournalPossession p) : base(p)
        {
            Guid = p.Guid;
            Banked = p.Banked;
            Count = p.Count;
            Equipped = p.Equipped;
            Attuned = p.Attuned;
            ChargesUsed = p.ChargesUsed;
            Hightlight = p.Hightlight;
            MagicProperties = new List<string>(p.MagicProperties);
            Weight = p.Weight;
            Choices = p.Choices.Select(x => new OGL.Common.Choice(x)).ToList();

        }

        public string InfoName()
        {
            return (Deleted ? "-" : "") + (New ? "+" : "") + FullName;
        }

        public override string ToString()
        {
            return (Deleted ? "-" : "") + base.ToString();
        }
    }
}
