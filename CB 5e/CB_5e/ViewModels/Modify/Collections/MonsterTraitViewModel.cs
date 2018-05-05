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
    public class MonsterTraitViewModel : BaseViewModel
    {
        public MonsterTrait Entry { get; private set; }
        public MonsterTraitViewModel(MonsterTrait te) => Entry = te;
        public void Refresh() => OnPropertyChanged("");
        public string Text { get => Entry.Name; }
        public string Detail { get => Entry.Text; }
        private bool moving = false;
        public bool Moving { get => moving; set => SetProperty(ref moving, value, "", () => OnPropertyChanged("TextColor")); }
        public Color TextColor { get => Moving ? Color.Orange : Color.Default; }

        public int AttackBonus { get => (Entry as MonsterAction)?.AttackBonus ?? 0; set { if (Entry is MonsterAction ma) ma.AttackBonus = value; OnPropertyChanged("AttackBonusValue"); } }
        public int AttackBonusValue { get => AttackBonus + 20; set { if (Entry is MonsterAction ma) ma.AttackBonus = value - 20; OnPropertyChanged("AttackBonus"); } }
    }
}
