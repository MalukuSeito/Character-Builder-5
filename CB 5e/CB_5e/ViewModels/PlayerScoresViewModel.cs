using CB_5e.Helpers;
using Character_Builder;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels
{
    public class PlayerScoresViewModel : SubModel
    {
        public ScoresModelViewModel Scores {get; set;}
        public PlayerScoresViewModel(PlayerModel parent) : base(parent, "Ability Scores")
        {
            Scores = new ScoresModelViewModel(Context);
            UpdateAbilityChoices();
            parent.PlayerChanged += Parent_PlayerChanged;
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            Scores = new ScoresModelViewModel(Context);
            UpdateAbilityChoices();
            OnPropertyChanged("Scores");
        }

        public int Str {
            get => Context.Player.BaseStrength;
            set
            {
                if (value <= 0) value = 0;
                if (value > Scores.StrengthMax) value = Scores.StrengthMax;
                if (value == Context.Player.BaseStrength) return;
                MakeHistory("BaseStrength");
                Context.Player.BaseStrength = value;
                OnPropertyChanged("Str");
                Parent.FirePlayerChanged();
                Save();
            }
        }
        public int Dex
        {
            get => Context.Player.BaseDexterity;
            set
            {
                if (value <= 0) value = 0;
                if (value > Scores.DexterityMax) value = Scores.DexterityMax;
                if (value == Context.Player.BaseDexterity) return;
                MakeHistory("BaseDexterity");
                Context.Player.BaseDexterity = value;
                OnPropertyChanged("Dex");
                Parent.FirePlayerChanged();
                Save();
            }
        }
        public int Con
        {
            get => Context.Player.BaseConstitution;
            set
            {
                if (value <= 0) value = 0;
                if (value > Scores.ConstitutionMax) value = Scores.ConstitutionMax;
                if (value == Context.Player.BaseConstitution) return;
                MakeHistory("BaseConstitution");
                Context.Player.BaseConstitution = value;
                OnPropertyChanged("Con");
                Parent.FirePlayerChanged();
                Save();
            }
        }
        public int Int
        {
            get => Context.Player.BaseIntelligence;
            set
            {
                if (value <= 0) value = 0;
                if (value > Scores.IntelligenceMax) value = Scores.IntelligenceMax;
                if (value == Context.Player.BaseIntelligence) return;
                MakeHistory("BaseIntelligence");
                Context.Player.BaseIntelligence = value;
                OnPropertyChanged("Int");
                Parent.FirePlayerChanged();
                Save();
            }
        }
        public int Wis
        {
            get => Context.Player.BaseWisdom;
            set
            {
                if (value <= 0) value = 0;
                if (value > Scores.WisdomMax) value = Scores.WisdomMax;
                if (value == Context.Player.BaseWisdom) return;
                MakeHistory("BaseWisdom");
                Context.Player.BaseWisdom = value;
                OnPropertyChanged("Wis");
                Parent.FirePlayerChanged();
                Save();
            }
        }
        public int Cha
        {
            get => Context.Player.BaseCharisma;
            set
            {
                if (value <= 0) value = 0;
                if (value > Scores.CharismaMax) value = Scores.CharismaMax;
                if (value == Context.Player.BaseCharisma) return;
                MakeHistory("BaseCharisma");
                Context.Player.BaseCharisma = value;
                OnPropertyChanged("Cha");
                Parent.FirePlayerChanged();
                Save();
            }
        }

        public string PointsLeft { get => Utils.GetPointsRemaining(Context.Player, Context); }

        public ObservableRangeCollection<ChoiceViewModel> AbilityChoices { get; set; } = new ObservableRangeCollection<ChoiceViewModel>();
        public void UpdateAbilityChoices()
        {
            List<ChoiceViewModel> choices = new List<ChoiceViewModel>();
            
            foreach (AbilityScoreFeatFeature asff in Context.Player.GetAbilityIncreases())
            {
                choices.Add(new AbilityFeatChoiceModel(this, asff));
            }

            foreach (Feature f in Context.Player.GetCommonFeaturesAndFeats())
            {
                ChoiceViewModel c = ChoiceViewModel<Feature>.GetChoice(this, f);
                if (c != null) choices.Add(c);
            }
            AbilityChoices.ReplaceRange(choices);

        }

    }
}
