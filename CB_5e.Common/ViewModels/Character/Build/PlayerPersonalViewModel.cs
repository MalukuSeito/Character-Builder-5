using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CB_5e.Helpers;
using CB_5e.Views;

namespace CB_5e.ViewModels.Character.Build
{
    public class PlayerPersonalViewModel : SubModel
    {
        public PlayerPersonalViewModel(PlayerModel parent) : base(parent, "Personal")
        {
            ShowImagePortrait = new Command(async () =>
            {
                await Navigation.PushAsync(new ImageEditor(Portrait, Context.Player.Portrait, SavePortrait, "Portrait"));
            });
            SavePortrait = new Command(async (par) =>
            {
                MakeHistory();
                Context.Player.Portrait = par as byte[];
                OnPropertyChanged("Portrait");
                await Navigation.PopAsync();
            });
            ShowImageFaction = new Command(async () =>
            {
                await Navigation.PushAsync(new ImageEditor(FactionImage, Context.Player.FactionImage, SaveFaction, "Faction Insignia"));
            });
            SaveFaction = new Command(async (par) =>
            {
                MakeHistory();
                Context.Player.FactionImage = par as byte[];
                OnPropertyChanged("FactionImage");
                await Navigation.PopAsync();
            });
        }

        public string CharacterName
        {
            get => Context.Player.Name;
            set
            {
                if (value == Name) return;
                MakeHistory("Name");
                Context.Player.Name = value;
                FirePropertyChanged("Name");
                FirePropertyChanged("CharacterName");
                FirePropertyChanged("PlayerName");
                Save();
            }
        }

        public string Alignment
        {
            get => Context.Player.Alignment;
            set
            {
                if (value == null) return;
                if (value == Alignment) return;
                MakeHistory("Alignment");
                Context.Player.Alignment = value;
                FirePropertyChanged("Alignment");
                Save();
            }
        }

        public int XP
        {
            get => Context.Player.GetXP();
            set
            {
                if (value == XP) return;
                if (value < Context.Player.GetXP(true)) value = Context.Player.GetXP(true);
                MakeHistory("XP");
                Context.Player.SetXP(value);
                FirePlayerChanged();
                Save();
            }
        }

        public int XPToLevel
        {
            get => Context.Levels.XpToLevelUp(XP);
        }

        public string Player
        {
            get => Context.Player.PlayerName;
            set
            {
                if (value == Player) return;
                MakeHistory("PlayerName");
                Context.Player.PlayerName = value;
                FirePropertyChanged("Player");
                Save();
            }
        }

        public string DCI
        {
            get => Context.Player.DCI;
            set
            {
                if (value == DCI) return;
                MakeHistory("DCI");
                Context.Player.DCI = value;
                FirePropertyChanged("DCI");
                Save();
            }
        }

        public int Age
        {
            get => Context.Player.Age;
            set
            {
                if (value == Age) return;
                MakeHistory("Age");
                Context.Player.Age = value;
                FirePropertyChanged("Age");
                Save();
            }
        }

        public int Weight
        {
            get => Context.Player.Weight;
            set
            {
                if (value == Weight) return;
                MakeHistory("Weight");
                Context.Player.Weight = value;
                FirePropertyChanged("Weight");
                Save();
            }
        }

        public string Height
        {
            get => Context.Player.Height;
            set
            {
                if (value == Height) return;
                MakeHistory("Height");
                Context.Player.Height = value;
                FirePropertyChanged("Height");
                Save();
            }
        }

        public string Eyes
        {
            get => Context.Player.Eyes;
            set
            {
                if (value == Eyes) return;
                MakeHistory("Eyes");
                Context.Player.Eyes = value;
                FirePropertyChanged("Eyes");
                Save();
            }
        }

        public string Skin
        {
            get => Context.Player.Skin;
            set
            {
                if (value == Skin) return;
                MakeHistory("Skin");
                Context.Player.Skin = value;
                FirePropertyChanged("Skin");
                Save();
            }
        }

        public string Hair
        {
            get => Context.Player.Hair;
            set
            {
                if (value == Hair) return;
                MakeHistory("Hair");
                Context.Player.Hair = value;
                FirePropertyChanged("Hair");
                Save();
            }
        }

        public string Faction
        {
            get => Context.Player.FactionName;
            set
            {
                if (value == Faction) return;
                MakeHistory("Faction");
                Context.Player.FactionName = value;
                FirePropertyChanged("Faction");
                Save();
            }
        }

        public ImageSource Portrait
        {
            get { if (Context.Player != null && Context.Player.Portrait != null) return ImageSource.FromStream(() => new MemoryStream(Context.Player.Portrait)); return null; }
        }

        public ImageSource FactionImage
        {
            get { if (Context.Player != null && Context.Player.FactionImage != null) return ImageSource.FromStream(() => new MemoryStream(Context.Player.FactionImage)); return null; }
        }

        public string Backstory
        {
            get => Context.Player.Backstory;
            set
            {
                if (value == Backstory) return;
                MakeHistory("Backstory");
                Context.Player.Backstory = value;
                FirePropertyChanged("Backstory");
                Save();
            }
        }

        public string Allies
        {
            get => Context.Player.Allies;
            set
            {
                if (value == Allies) return;
                MakeHistory("Allies");
                Context.Player.Allies = value;
                FirePropertyChanged("Allies");
                Save();
            }
        }
        public ObservableRangeCollection<string> Alignments {get; set;} = new ObservableRangeCollection<string>(new List<string>() {
            "Lawful good",
            "Neutral good",
            "Chaotic good",
            "Lawful neutral",
            "Neutral",
            "Chaotic neutral",
            "Lawful evil",
            "Neutral evil",
            "Chaotic evil"});
        public Command SavePortrait { get; private set; }
        public Command ShowImagePortrait { get; private set; }
        public Command ShowImageFaction { get; private set; }
        public Command SaveFaction { get; private set; }
    }
}
