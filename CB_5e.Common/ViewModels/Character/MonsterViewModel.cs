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

namespace CB_5e.ViewModels.Character
{
    public class MonsterViewModel :  INotifyPropertyChanged, IComparable<MonsterViewModel>
    {
        public Monster Monster { get; set; }
        public MonsterViewModel(Monster s)
        {
            Monster = s;
        }

        public Command Prepare { get; set; }
        public Command ShowInfo { get; set; }
        public bool BadChoice { get; set; }
        public bool secl;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Selected
        {
            get => secl;
            set
            {
                secl = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Prepared"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PreparedColor"));
            }
        }

        public string Text { get => Monster.ToString(); }
        public string Desc { get {
                StringBuilder type = new StringBuilder();
                type.Append("CR: ").Append(Monster.CR).Append(", ");
                type.Append(Monster.Size.ToString());
                List<Keyword> t = new List<Keyword>(Monster.Keywords);
                if (t.Count >= 1)
                {
                    type.Append(" ").Append(t[0].ToString().ToLowerInvariant());
                    t.RemoveAt(0);
                    if (t.Count >= 1) type.Append(" (").Append(string.Join(", ", t.Select(s => s.ToString())).ToLowerInvariant()).Append(")");
                }
                if (Monster.Alignment != null && Monster.Alignment != "") type.Append(", ").Append(Monster.Alignment);
                return type.ToString();
            }
        }

        public int CompareTo(MonsterViewModel other)
        {
            return Monster.CompareTo(other.Monster);
        }
        public Color PreparedColor { get => BadChoice ? Color.DarkRed : Selected ? Color.DarkBlue : Color.Default; }
        public Color DisplayColor { get => Color.Default; }
        
        public string Name { get => Monster.Name; }
        public string Source { get => Monster.Source; }
    }
}
