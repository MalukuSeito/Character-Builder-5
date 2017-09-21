using OGL.Base;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{
    public class SaveProficiencyFeatureEditModel : FeatureEditModel<SaveProficiencyFeature>
    {
        public SaveProficiencyFeatureEditModel(SaveProficiencyFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
        }
        public bool SaveStrength
        {
            get => Feature.Ability.HasFlag(Ability.Strength);
            set
            {
                MakeHistory("Ability");
                if (value) Feature.Ability |= Ability.Strength;
                else Feature.Ability &= ~Ability.Strength;
                OnPropertyChanged("SaveStrength");
            }
        }
        public bool SaveDexterity
        {
            get => Feature.Ability.HasFlag(Ability.Dexterity);
            set
            {
                MakeHistory("Ability");
                if (value) Feature.Ability |= Ability.Dexterity;
                else Feature.Ability &= ~Ability.Dexterity;
                OnPropertyChanged("SaveDexterity");
            }
        }
        public bool SaveConstitution
        {
            get => Feature.Ability.HasFlag(Ability.Constitution);
            set
            {
                MakeHistory("Ability");
                if (value) Feature.Ability |= Ability.Constitution;
                else Feature.Ability &= ~Ability.Constitution;
                OnPropertyChanged("SaveConstitution");
            }
        }
        public bool SaveIntelligence
        {
            get => Feature.Ability.HasFlag(Ability.Intelligence);
            set
            {
                MakeHistory("Ability");
                if (value) Feature.Ability |= Ability.Intelligence;
                else Feature.Ability &= ~Ability.Intelligence;
                OnPropertyChanged("SaveIntelligence");
            }
        }
        public bool SaveWisdom
        {
            get => Feature.Ability.HasFlag(Ability.Wisdom);
            set
            {
                MakeHistory("Ability");
                if (value) Feature.Ability |= Ability.Wisdom;
                else Feature.Ability &= ~Ability.Wisdom;
                OnPropertyChanged("SaveWisdom");
            }
        }
        public bool SaveCharisma
        {
            get => Feature.Ability.HasFlag(Ability.Charisma);
            set
            {
                MakeHistory("Ability");
                if (value) Feature.Ability |= Ability.Charisma;
                else Feature.Ability &= ~Ability.Charisma;
                OnPropertyChanged("SaveCharisma");
            }
        }
    }
}
