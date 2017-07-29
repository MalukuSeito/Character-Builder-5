using CB_5e.Helpers;
using CB_5e.Views;
using Character_Builder;
using OGL;
using OGL.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class ItemViewModel: BaseViewModel
    {

        public ItemViewModel(PlayerViewModel context): this (context, new Possession(context.Context, "", 1))
        {
            IsNew = true;
        }

        public ItemViewModel(PlayerViewModel context, Possession possession)
        {
            Context = context;
            Value = possession;
            BaseItemInfo = new Command(async () =>
            {

                await Context.ShopNavigation.PushAsync(InfoPage.Show(Value.Item));
            }, () => Value.BaseItem != null);

            ShowInfo = new Command(async (par) =>
            {
                if (par is MagicViewModel m)
                    await Context.ShopNavigation.PushAsync(InfoPage.Show(m.Magic));
            });
            OnDelete = new Command((par) =>
            {
                if (par is MagicViewModel m)
                {
                    Value.MagicProperties.RemoveAll(s => ConfigManager.SourceInvariantComparer.Equals(s, m.Magic.Name + " " + ConfigManager.SourceSeperator + " " + m.Magic.Source));
                    Magic.ReplaceRange(new List<MagicViewModel>());
                    UpdateMagic();
                    IsChanged = true;
                    Context.FirePlayerChanged();
                }
            });
            UpdateMagic();
        }
        private static CultureInfo culture = CultureInfo.InvariantCulture;
        public bool IsNew { get; set; }
        private bool changed = false;
        public bool IsChanged {
            get => changed;
            set
            {
                if (changed != value)
                {
                    changed = value;
                    if (changed)
                    {
                        Context.MakeHistory();
                        if (!IsNew) Context.Context.Player.AddPossession(Value);
                    }
                }
            }
        }

        public PlayerViewModel Context { get; set; }
        public Possession Value { get; set; }

        public string Name {
            get => Value.Name;
            set {
                if (value != Name)
                {
                    IsChanged = true;
                    Value.Name = value;
                }
                OnPropertyChanged("Name");
            }
        }
        public string Base { get => SourceInvariantComparer.NoSource(Value.BaseItem); }
        public Color Accent { get => Color.Accent; }
        public Command BaseItemInfo { get; set; }

        public string Description
        {
            get => Value.Description;
            set {
                if (value != Description)
                {
                    IsChanged = true;
                    Value.Description = value;
                }
                OnPropertyChanged("Description");
            }
        }

        public int Count
        {
            get => Value.Count;
            set
            {
                if (value != Count)
                {
                    IsChanged = true;
                    if (value < 1) value = 1;
                    Value.Count = value;
                }
                OnPropertyChanged("Count");
            }
        }

        public double Weight
        {
            get => Value.Weight;
            set
            {
                if (value != Weight)
                {
                    IsChanged = true;
                    if (value < 0) value = -1;
                    Value.Weight = value;
                }
                OnPropertyChanged("Weight");
            }
        }

        public Color CustomWeightColor
        {
            get => Value.Weight < 0 ? Color.LightGray : Color.Accent;
        }

        public Color CustomWeightColorNew
        {
            get => Value.Weight < 0 ? Color.LightGray : Color.Default;
        }

        public string Equipped
        {
            get => Value.Equipped ?? EquipSlot.None;
            set
            {
                if (value != Equipped)
                {
                    IsChanged = true;
                    foreach (Possession pos in Context.Context.Player.Possessions)
                    {
                        if (string.Equals(pos.Equipped, value, StringComparison.OrdinalIgnoreCase)) pos.Equipped = EquipSlot.None;
                    }
                    Value.Equipped = value;
                    Context.FirePlayerChanged();
                }
                OnPropertyChanged("Equipped");
            }
        }

        public List<string> EquipSlots
        {
            get
            {
                List<string> result = new List<string>()
                {
                    EquipSlot.None
                };
                result.AddRange(Context.Context.Config.Slots);
                if (!result.Contains(Value.Equipped)) result.Add(Value.Equipped);
                return result;
            }
        }
        public int Charges
        {
            get => Value.ChargesUsed;
            set
            {
                if (value != Charges)
                {
                    IsChanged = true;
                    if (value < 0) value = 0;
                    Value.ChargesUsed = value;
                }
                OnPropertyChanged("Charges");
            }
        }
        public bool Attuned
        {
            get => Value.Attuned;
            set
            {
                if (value != Attuned)
                {
                    IsChanged = true;
                    Value.Attuned = value;
                    Context.FirePlayerChanged();
                }
                OnPropertyChanged("Attuned");
            }
        }
        public bool RollsOnSheet
        {
            get => Value.Hightlight;
            set
            {
                if (value != RollsOnSheet)
                {
                    IsChanged = true;
                    Value.Hightlight = value;
                }
                OnPropertyChanged("Hightlight");
            }
        }
        private string magicsearch;
        public string MagicSearch
        {
            get => magicsearch;
            set
            {
                SetProperty(ref magicsearch, value);
                UpdateMagic();
            }
        }
        public ObservableRangeCollection<MagicViewModel> Magic { get; set; } = new ObservableRangeCollection<MagicViewModel>();
        public void UpdateMagic() => Magic.ReplaceRange(from m in Value.Magic
                                                        where magicsearch == null || magicsearch == "" || culture.CompareInfo.IndexOf(m.Name, magicsearch, CompareOptions.IgnoreCase) >= 0 || culture.CompareInfo.IndexOf(m.Description, magicsearch, CompareOptions.IgnoreCase) >= 0
                                                        select new MagicViewModel(m)
                                                        {
                                                            Delete = OnDelete,
                                                            ShowInfo = ShowInfo
                                                        });
        public Command OnDelete { get; private set; }
        public Command ShowInfo { get; private set; }
        private int split = 1;
        public int Split {
            get => split;
            set
            {
                if (value < 1) value = 1;
                if (value >= Count) value = Count - 1;
                SetProperty(ref split, value);
            }

        }
    }

    public class MagicViewModel
    {
        public MagicViewModel (MagicProperty m)
        {
            Magic = m;
        }
        public string Name { get => Magic.Name; }
        public MagicProperty Magic { get; private set; }
        public Command Delete { get; set; }
        public Command ShowInfo { get; set; }
    }
}
