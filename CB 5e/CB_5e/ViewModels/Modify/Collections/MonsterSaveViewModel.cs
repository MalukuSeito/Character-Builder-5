using OGL.Descriptions;
using OGL.Features;
using OGL.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Modify.Collections
{
    public class MonsterSaveViewModel : BaseViewModel
    {
        public MonsterSaveBonus Entry { get; private set; }
        public MonsterSaveViewModel(MonsterSaveBonus te) => Entry = te;
        public void Refresh() => OnPropertyChanged("");
        public string Text { get => Entry.Ability.ToString() + " " + Entry.Bonus.ToString("+#;-#;+0"); }
        public string Detail { get => Entry.Text; }
        private bool moving = false;
        public bool Moving { get => moving; set => SetProperty(ref moving, value, "", () => OnPropertyChanged("TextColor")); }
        public Color TextColor { get => Moving ? Color.Orange : Color.Default; }
        public int Bonus { get => Entry.Bonus; set { Entry.Bonus = value; OnPropertyChanged("BonusValue"); } }
        public int BonusValue { get => Entry.Bonus + 20; set { Entry.Bonus = value - 20; OnPropertyChanged("Bonus"); } }
        public string Ability
        {
            get => Entry.Ability.ToString();
            set
            {
                if (value == Ability) return;
                if (Enum.TryParse(value, out OGL.Base.Ability a)) Entry.Ability = a;
                else Entry.Ability = OGL.Base.Ability.None;
            }
        }

        public List<string> Abilities { get; set; } = Enum.GetNames(typeof(OGL.Base.Ability)).ToList();
    }
}
