using Character_Builder;
using OGL;
using OGL.Keywords;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class SpellViewModel :  INotifyPropertyChanged, IComparable<SpellViewModel>
    {
        public Spell Spell { get; set; }
        public SpellViewModel(Spell s)
        {
            Spell = s;
        }

        public SpellViewModel(ModifiedSpell s)
        {
            Spell = s;
            s.includeRecharge = false;
            s.includeResources = false;
        }
        public Command Highlight { get; set; }
        public Command Prepare { get; set; }
        public Command ShowInfo { get; set; }
        public bool prep;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Prepared
        {
            get => AddAlwaysPreparedToName || prep;
            set
            {
                prep = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Prepared"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PreparedColor"));
            }
        }
        private bool high;
        public bool IsHightlighted {
            get => high;
            set {
                high = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsHightlighted"));
            }
        }
        public string FullName { get => Spell.ToString(); }
        public string School { get => Spell.Level > 0 ? SpellSlotInfo.AddOrdinal(Spell.Level) + " Level " + GetSchool() : GetSchool() + " Cantrip"; }


        private static List<Keyword> Schools = new List<Keyword>()
        {
            new Keyword("Abjuration"), new Keyword("Conjuration"), new Keyword("Divination"), new Keyword("Enchantment"), new Keyword("Evocation"), new Keyword("Illusion"), new Keyword("Necromancy"), new Keyword("Transmutation")
        };

        private string GetSchool()
        {
            List<string> s = new List<string>();
            foreach (Keyword k in Schools) if (Spell.Keywords.Contains(k)) s.Add(k.Name.ToLowerInvariant());
            foreach (Keyword k in Schools) if (Spell is ModifiedSpell ms && ms.AdditionalKeywords.Contains(k)) s.Add(k.Name.ToLowerInvariant());
            string res = string.Join(", ", s);
            return char.ToUpper(res[0]) + res.Substring(1);
        }

        public int CompareTo(SpellViewModel other)
        {
            return Spell.CompareTo(other.Spell);
        }

        public Color PreparedColor { get => Prepared ? Color.DarkBlue : Color.Default; }
        public bool AddAlwaysPreparedToName {
            get => Spell is ModifiedSpell && ((ModifiedSpell)Spell).AddAlwaysPreparedToName;
            set
            {
                if (Spell is ModifiedSpell ms)
                {
                    ms.AddAlwaysPreparedToName = value;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddAlwaysPreparedToName"));
            }
        }
        public string Name { get => Spell.Name; }
        public string Source { get => Spell.Source; }
    }
}
