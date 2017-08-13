using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class PlayerPersonalViewModel : SubModel
    {
        public PlayerPersonalViewModel(PlayerModel parent) : base(parent, "Personal")
        {
        }

        public string CharacterName
        {
            get => Context.Player.Name;
            set
            {
                if (value == Name) return;
                Context.Player.Name = value;
                FirePropertyChanged("Name");
                FirePropertyChanged("CharacterName");
                FirePropertyChanged("PlayerName");
            }
        }

        public string Alignment
        {
            get => Context.Player.Alignment;
            set
            {
                if (value == Alignment) return;
                Context.Player.Alignment = value;
                FirePropertyChanged("Alignment");
            }
        }

        public int XP
        {
            get => Context.Player.GetXP();
            set
            {
                if (value == XP) return;
                if (value < Context.Player.GetXP(true)) value = Context.Player.GetXP(true);
                Context.Player.SetXP(value);
                FirePlayerChanged();
            }
        }

        public int XPToLevel
        {
            get=> Context.Levels.XpToLevelUp(XP);
        }

        public string Player
        {
            get => Context.Player.PlayerName;
            set
            {
                if (value == Player) return;
                Context.Player.PlayerName = value;
                FirePropertyChanged("Player");
            }
        }

        public string DCI
        {
            get => Context.Player.DCI;
            set
            {
                if (value == DCI) return;
                Context.Player.DCI = value;
                FirePropertyChanged("DCI");
            }
        }

        public int Age
        {
            get => Context.Player.Age;
            set
            {
                if (value == Age) return;
                Context.Player.Age = value;
                FirePropertyChanged("Age");
            }
        }

        public int Weight
        {
            get => Context.Player.Weight;
            set
            {
                if (value == Weight) return;
                Context.Player.Weight = value;
                FirePropertyChanged("Weight");
            }
        }

        public string Height
        {
            get => Context.Player.Height;
            set
            {
                if (value == Height) return;
                Context.Player.Height = value;
                FirePropertyChanged("Height");
            }
        }

        public string Eyes
        {
            get => Context.Player.Eyes;
            set
            {
                if (value == Eyes) return;
                Context.Player.Eyes = value;
                FirePropertyChanged("Eyes");
            }
        }

        public string Skin
        {
            get => Context.Player.Skin;
            set
            {
                if (value == Skin) return;
                Context.Player.Skin = value;
                FirePropertyChanged("Skin");
            }
        }

        public string Hair
        {
            get => Context.Player.Hair;
            set
            {
                if (value == Hair) return;
                Context.Player.Hair = value;
                FirePropertyChanged("Hair");
            }
        }

        public string Faction
        {
            get => Context.Player.FactionName;
            set
            {
                if (value == Faction) return;
                Context.Player.FactionName = value;
                FirePropertyChanged("Faction");
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
                Context.Player.Backstory = value;
                FirePropertyChanged("Backstory");
            }
        }

        public string Allies
        {
            get => Context.Player.Allies;
            set
            {
                if (value == Allies) return;
                Context.Player.Allies = value;
                FirePropertyChanged("Allies");
            }
        }
    }
}
