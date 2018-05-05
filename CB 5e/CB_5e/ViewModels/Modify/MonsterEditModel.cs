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
using OGL.Keywords;
using OGL.Monsters;

namespace CB_5e.ViewModels.Modify
{
    public class MonsterEditModel : EditModel<Monster>
    {
        
        public MonsterEditModel(Monster cond, OGLContext context): base(cond, context)
        {
            
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

        public ImageSource Image
        {
            get { if (Model != null && Model.ImageData != null) return ImageSource.FromStream(() => new MemoryStream(Model.ImageData)); return null; }
        }

        public Command ShowImage { get; private set; }
        public Command SaveImage { get; private set; }

        public override string GetPath(Monster obj)
        {
            return PortablePath.Combine(obj.Source, Context.Config.Monster_Directory);
        }

        public List<Description> Descriptions { get => Model.Descriptions; }

        public string Size
        {
            get => Model.Size.ToString();
            set
            {
                if (value == Size || value == null) return;
                MakeHistory("Size");
                if (Enum.TryParse(value, out OGL.Base.Size a)) Model.Size = a;
                else Model.Size = OGL.Base.Size.Medium;
                OnPropertyChanged("Size");
            }
        }

        public List<string> Sizes { get; set; } = Enum.GetNames(typeof(OGL.Base.Size)).ToList();


        public List<Keyword> Keywords { get => Model.Keywords; }

        public string Alignment { get => Model.Alignment; set { if (value == Alignment) return; MakeHistory("Alignment"); Model.Alignment = value; OnPropertyChanged("Alignment"); } }
        public int AC { get => Model.AC; set { if (value == AC) return; MakeHistory("AC"); Model.AC = value; OnPropertyChanged("AC"); } }
        public string ACText { get => Model.ACText; set { if (value == ACText) return; MakeHistory("ACText"); Model.ACText = value; OnPropertyChanged("ACText"); } }
        public int HP { get => Model.HP; set { if (value == HP) return; MakeHistory("HP"); Model.HP = value; OnPropertyChanged("HP"); } }
        public string HPRoll { get => Model.HPRoll; set { if (value == HPRoll) return; MakeHistory("HPRoll"); Model.HPRoll = value; OnPropertyChanged("HPRoll"); } }
        public List<String> Speed { get => Model.Speeds; }
        public int Strength { get => Model.Strength; set { if (value == Strength) return; MakeHistory("Strength"); Model.Strength = value; OnPropertyChanged("Strength"); } }
        public int Dexterity { get => Model.Dexterity; set { if (value == Dexterity) return; MakeHistory("Dexterity"); Model.Dexterity = value; OnPropertyChanged("Dexterity"); } }
        public int Constitution { get => Model.Constitution; set { if (value == Constitution) return; MakeHistory("Constitution"); Model.Constitution = value; OnPropertyChanged("Constitution"); } }
        public int Intelligence { get => Model.Intelligence; set { if (value == Intelligence) return; MakeHistory("Intelligence"); Model.Intelligence = value; OnPropertyChanged("Intelligence"); } }
        public int Wisdom { get => Model.Wisdom; set { if (value == Wisdom) return; MakeHistory("Wisdom"); Model.Wisdom = value; OnPropertyChanged("Wisdom"); } }
        public int Charisma { get => Model.Charisma; set { if (value == Charisma) return; MakeHistory("Charisma"); Model.Charisma = value; OnPropertyChanged("Charisma"); } }
        public List<String> Resistances { get => Model.Resistances; }
        public List<String> Vulnerabilities { get => Model.Vulnerablities; }
        public List<String> Immunities { get => Model.Immunities; }
        public List<String> ConditionImmunities { get => Model.ConditionImmunities; }
        public List<String> Senses { get => Model.Senses; }
        public int PassivePerception { get => Model.PassivePerception; set { if (value == PassivePerception) return; MakeHistory("PassivePerception"); Model.PassivePerception = value; OnPropertyChanged("PassivePerception"); OnPropertyChanged("PassivePerceptionValue"); } }
        public int PassivePerceptionValue { get => Model.PassivePerception + 20; set { if (value == PassivePerceptionValue) return; MakeHistory("PassivePerception"); Model.PassivePerception = value - 20; OnPropertyChanged("PassivePerception"); OnPropertyChanged("PassivePerceptionValue"); } }
        public List<String> Languages { get => Model.Languages; }
        public decimal CR { get => Model.CR; set { if (value == CR) return; MakeHistory("CR"); Model.CR = value; OnPropertyChanged("CR"); OnPropertyChanged("CRValue"); } }
        public int CRValue {
            get {
                return CR <= 0 ? 0 :
                    CR <= 0.125m ? 1 :
                    CR <= 0.25m ? 2 :
                    CR <= 0.5m ? 3 :
                    (int)Math.Ceiling(CR + 3);
            }
            set {
                if (value == CRValue) return;
                MakeHistory("CR");
                Model.CR = value == 0 ? 0m :
                    value == 1 ? 0.125m :
                    value == 2 ? 0.25m :
                    value == 3 ? 0.5m :
                    value - 3;
                OnPropertyChanged("CR");
                OnPropertyChanged("CRValue");
            }
        }
        public int XP { get => Model.XP; set { if (value == XP) return; MakeHistory("XP"); Model.XP = value; OnPropertyChanged("XP"); OnPropertyChanged("XPValue"); } }
        public int XPValue { get => Model.XP/100; set { if (value == XPValue) return; MakeHistory("XP"); Model.XP = value*100; OnPropertyChanged("XP"); OnPropertyChanged("XPValue"); } }
        public List<MonsterTrait> Traits { get => Model.Traits; }
        public List<MonsterTrait> Actions { get => Model.Actions; }
        public List<MonsterTrait> Reactions { get => Model.Reactions; }
        public List<MonsterTrait> LegendaryActions { get => Model.LegendaryActions; }
        public List<MonsterSaveBonus> SaveBonus { get => Model.SaveBonus; }
        public List<MonsterSkillBonus> SkillBonus { get => Model.SkillBonus; }


        public List<String> Spells { get => Model.Spells; }
        public List<int> Slots { get => Model.Slots; }

    }
}
