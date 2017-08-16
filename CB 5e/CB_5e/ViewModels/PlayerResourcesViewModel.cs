using CB_5e.Helpers;
using Character_Builder;
using OGL.Base;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class PlayerResourcesViewModel : SubModel
    {
        public PlayerResourcesViewModel(PlayerModel parent) : base(parent, "Resources")
        {
            DeselectResource = new Command(async () =>
            {
                ResourceBusy = true;
                SelectedResource = null;
                Resources.ReplaceRange(new List<ResourceViewModel>());
                resources.Clear();
                resources.AddRange(from r in Context.Player.GetResourceInfo(true).Values select new ResourceViewModel(r, this));
                resources.AddRange(from r in Context.Player.GetBonusSpells(false) select new ResourceViewModel(r, this));
                UpdateResources();
                await Task.Delay(500); //Stupid Refereshindicator
                ResourceBusy = false;

            });
            LongRest = new Command(() =>
            {
                MakeHistory("LongRest");
                foreach (ResourceInfo r in Context.Player.GetResourceInfo(true).Values)
                {
                    if (r.Recharge >= RechargeModifier.LongRest) Context.Player.SetUsedResources(r.ResourceID, 0);
                }
                foreach (ModifiedSpell ms in Context.Player.GetBonusSpells())
                {
                    if (ms.RechargeModifier >= RechargeModifier.LongRest) Context.Player.SetUsedResources(ms.getResourceID(), 0);
                }
                DeselectResource.Execute(null);
            });
            ShortRest = new Command(() =>
            {
                MakeHistory("ShortRest");
                foreach (ResourceInfo r in Context.Player.GetResourceInfo(true).Values)
                {
                    if (r.Recharge >= RechargeModifier.ShortRest) Context.Player.SetUsedResources(r.ResourceID, 0);
                }
                foreach (ModifiedSpell ms in Context.Player.GetBonusSpells())
                {
                    if (ms.RechargeModifier >= RechargeModifier.ShortRest) Context.Player.SetUsedResources(ms.getResourceID(), 0);
                }
                DeselectResource.Execute(null);
            });
            resources = new List<ResourceViewModel>();
            resources.AddRange(from r in Context.Player.GetResourceInfo(true).Values select new ResourceViewModel(r, this));
            resources.AddRange(from r in Context.Player.GetBonusSpells(false) select new ResourceViewModel(r, this));
            UpdateResources();
            parent.PlayerChanged += Parent_PlayerChanged;
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            resources.Clear();
            resources.AddRange(from r in Context.Player.GetResourceInfo(true).Values select new ResourceViewModel(r, this));
            resources.AddRange(from r in Context.Player.GetBonusSpells(false) select new ResourceViewModel(r, this));
            UpdateResources();
        }

        private string resourceSearch;
        public string ResourceSearch
        {
            get => resourceSearch;
            set
            {
                SetProperty(ref resourceSearch, value);
                UpdateResources();
            }
        }
        private List<ResourceViewModel> resources;
        public ObservableRangeCollection<ResourceViewModel> Resources { get; set; } = new ObservableRangeCollection<ResourceViewModel>();

        public void UpdateResources() => Resources.ReplaceRange(from r in resources where resourceSearch == null || resourceSearch == ""
             || Culture.CompareInfo.IndexOf(r.Name ?? "", resourceSearch, CompareOptions.IgnoreCase) >= 0 orderby r.ToString() select r);

        private ResourceViewModel selectedResource;
        public ResourceViewModel SelectedResource { get => selectedResource; set
            {
                SetProperty(ref selectedResource, value);
                if (value != null && value.IsChangeable)
                {
                    currentResourceValue = value.Used;
                    OnPropertyChanged("CurrentResourceValue");
                }
                else {
                    ResourceViewModel.last = null;
                    currentResourceValue = 0;
                    OnPropertyChanged("CurrentResourceValue");
                }
            }
        }
        private int currentResourceValue;
        public int CurrentResourceValue { get => currentResourceValue; set {
                if (value >= 0 && value != currentResourceValue && selectedResource != null && selectedResource.IsChangeable)
                {
                    ResourceViewModel r = selectedResource;
                    MakeHistory("Resource" + r.ResourceID);
                    if (r.Max > 0 && value > r.Max) value = r.Max;
                    r.Used = value;
                    Context.Player.SetUsedResources(r.ResourceID, value);
                    Save();
                }
                SetProperty(ref currentResourceValue, value);
            } }

        public void UpdateUsed()
        {
            currentResourceValue = selectedResource.Used;
            OnPropertyChanged("CurrentResourceValue");
        }
        public bool ResourceBusy { get => resourceBusy; set => SetProperty(ref resourceBusy, value); }
        public Command DeselectResource { get; private set; }
        public Command LongRest { get; private set; }
        public Command ShortRest { get; private set; }

        bool resourceBusy = false;
    }
}
