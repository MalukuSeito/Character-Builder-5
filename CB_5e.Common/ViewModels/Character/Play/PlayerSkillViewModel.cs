using CB_5e.Helpers;
using Character_Builder;
using OGL.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character.Play
{
    public class PlayerSkillViewModel: SubModel
    {
        public PlayerSkillViewModel(PlayerModel parent) : base(parent, "Skills")
        {
            DeselectSkill = new Command(async () =>
            {
                SkillBusy = true;
                SkillSearch = null;
                SelectedSkill = null;
                await Task.Delay(500); //Stupid Refereshindicator
                SkillBusy = false;

            });
            skills = Context.Player.GetSkills();
            Skills.ReplaceRange(skills);
            skillSearch = null;
            parent.PlayerChanged += Parent_PlayerChanged;
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            Skills.ReplaceRange(Context.Player.GetSkills());
        }

        public List<Ability> Abilities { get; set; } = new List<Ability>() { Ability.Strength, Ability.Constitution, Ability.Dexterity, Ability.Intelligence, Ability.Wisdom, Ability.Charisma };

        private Ability _baseAbility;
        public int SkillBaseIndex { get { return Abilities.IndexOf(_baseAbility); } set { SetProperty(ref _baseAbility, Abilities[value]); OnPropertyChanged("SkillValue"); } }
        private List<SkillInfo> skills;
        private string skillSearch;
        public string SkillSearch { get { return skillSearch; } set { SetProperty(ref skillSearch, value); UpdateSkills(); } }
        public ObservableRangeCollection<SkillInfo> Skills { get; set; } = new ObservableRangeCollection<SkillInfo>();
        public String SkillValue
        {
            get
            {
                if (SelectedSkill != null) return ": " + Context.Player.GetSkill(SelectedSkill.Skill, _baseAbility).ToString("+#;-#;0");
                return ": 0";
            }
        }

        private void UpdateSkills()
        {

            if (skillSearch == null || skillSearch == "") Skills.ReplaceRange(skills);
            else
            {
                Skills.ReplaceRange(from c in skills where Culture.CompareInfo.IndexOf(c.Desc ?? "", skillSearch, CompareOptions.IgnoreCase) >= 0 || Culture.CompareInfo.IndexOf(c.Skill?.Description ?? "", skillSearch, CompareOptions.IgnoreCase) >= 0 select c);
            }
        }

        private SkillInfo _selectedSkill = null;
        public SkillInfo SelectedSkill { get { return _selectedSkill; } set { SetProperty(ref _selectedSkill, value); if (value != null) SkillBaseIndex = Abilities.FindIndex(a => value.Base.HasFlag(a)); } }

        public Command DeselectSkill { get; private set; }
        private bool skillbusy = false;

        

        public bool SkillBusy { get { return skillbusy; } set { SetProperty(ref skillbusy, value); } }
    }
}
