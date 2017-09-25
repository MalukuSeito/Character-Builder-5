using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CB_5e.Helpers;
using CB_5e.Views;
using OGL;
using PCLStorage;
using OGL.Descriptions;
using OGL.Features;
using OGL.Base;

namespace CB_5e.ViewModels.Modify
{
    public class ClassEditModel : EditModel<ClassDefinition>
    {
        
        public ClassEditModel(ClassDefinition cls, OGLContext context): base(cls, context)
        {
            if (cls.MulticlassingAbilityScores != Ability.None)
            {
                List<string> cond = new List<string>();
                if (cls.MulticlassingCondition == null || cls.MulticlassingCondition.Length == 0) cond.Add(cls.MulticlassingCondition);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Strength)) cond.Add("Strength >= " +context.Config.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Dexterity)) cond.Add("Dexterity >= " + context.Config.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Constitution)) cond.Add("Constitution >= " + context.Config.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Intelligence)) cond.Add("Intelligence >= " + context.Config.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Wisdom)) cond.Add("Wisdom >= " + context.Config.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Charisma)) cond.Add("Charisma >= " + context.Config.MultiClassTarget);
                if (cond.Count > 0) cls.MulticlassingCondition = String.Join(" and ", cond);
                else cls.MulticlassingCondition = "true";
                cls.MulticlassingAbilityScores = Ability.None;
            }
            ShowImage = new Command(async () =>
            {
                await Navigation.PushAsync(new ImageEditor(Image, Model.ImageData, SaveImage, "Image"));
            });
            SaveImage = new Command(async (par) =>
            {
                MakeHistory();
                Model.ImageData = par as byte[];
                OnPropertyChanged("Image");
                await Navigation.PopAsync();
            });
        }

        public string Name { get => Model.Name; set { if (value == Name) return; MakeHistory("Name"); Model.Name = value; OnPropertyChanged("Name"); } }
        public string Source { get => Model.Source; set { if (value == Source) return; MakeHistory("Source"); Model.Source = value; OnPropertyChanged("Source"); } }
        public string Description { get => Model.Description; set { if (value == Description) return; MakeHistory("Description"); Model.Description = value ; OnPropertyChanged("Description"); } }
        public string Flavour { get => Model.Flavour; set { if (value == Flavour) return; MakeHistory("Flavour"); Model.Flavour = value; OnPropertyChanged("Flavour"); } }
        public int HitDie { get => Model.HitDie; set { if (value == HitDie) return; MakeHistory("HitDie"); Model.HitDie = value; OnPropertyChanged("HitDie"); } }
        public int HitDieCount { get => Model.HitDieCount; set { if (value == HitDieCount) return; MakeHistory("HitDieCount"); Model.HitDieCount = value; OnPropertyChanged("HitDieCount"); } }
        public int HPFirstLevel { get => Model.HPFirstLevel; set { if (value == HPFirstLevel) return; MakeHistory("HPFirstLevel"); Model.HPFirstLevel = value; OnPropertyChanged("HPFirstLevel"); } }
        public int AverageHPPerLevel { get => Model.AverageHPPerLevel; set { if (value == AverageHPPerLevel) return; MakeHistory("AverageHPPerLevel"); Model.AverageHPPerLevel = value; OnPropertyChanged("AverageHPPerLevel"); } }
        public bool AvailableAtFirstLevel { get => Model.AvailableAtFirstLevel; set { if (value == AvailableAtFirstLevel) return; MakeHistory("AvailableAtFirstLevel"); Model.AvailableAtFirstLevel = value; OnPropertyChanged("AvailableAtFirstLevel"); } }
        public String MulticlassingCondition { get => Model.MulticlassingCondition; set { if (value == MulticlassingCondition) return; MakeHistory("MulticlassingCondition"); Model.MulticlassingCondition = value; OnPropertyChanged("MulticlassingCondition"); } }

        public ImageSource Image
        {
            get { if (Model != null && Model.ImageData != null) return ImageSource.FromStream(() => new MemoryStream(Model.ImageData)); return null; }
        }

        public Command ShowImage { get; private set; }
        public Command SaveImage { get; private set; }

        public override string GetPath(ClassDefinition obj)
        {
            return PortablePath.Combine(obj.Source, Context.Config.Classes_Directory);
        }
        public List<Description> Descriptions { get => Model.Descriptions; }
        public List<Feature> Features { get => Model.Features; }
        public List<Feature> MulticlassingFeatures { get => Model.MulticlassingFeatures; }
        public List<Feature> FirstClassFeatures { get => Model.FirstClassFeatures; }
        public List<int> MulticlassingSpellLevels { get => Model.MulticlassingSpellLevels; }
        public List<string> FeaturesToAddClassKeywordTo { get => Model.FeaturesToAddClassKeywordTo; }
        public List<string> SpellsToAddClassKeywordTo { get => Model.SpellsToAddClassKeywordTo; }
    }
}

