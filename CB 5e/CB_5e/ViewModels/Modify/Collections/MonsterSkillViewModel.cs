using OGL.Base;
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
    public class MonsterSkillViewModel : BaseViewModel
    {
        public MonsterSkillBonus Entry { get; private set; }
        public MonsterSkillViewModel(MonsterSkillBonus te) => Entry = te;
        public void Refresh() => OnPropertyChanged("");
        public string Text { get => Entry.Skill + (Entry.Ability != OGL.Base.Ability.None ? " [" + Entry.Ability.ToString() + "]" : "") + " " + Entry.Bonus.ToString("+#;-#;+0"); }
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

        public string Skill { get => Entry.Skill; set { if (value == Skill) return; Entry.Skill = value; OnPropertyChanged("Skill"); } }
        private List<String> skills = new List<string>();
        public List<string> Skills { get => skills; set { skills = value; OnPropertyChanged("Skills"); } }
    }
}