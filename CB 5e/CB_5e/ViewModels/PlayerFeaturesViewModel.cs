using CB_5e.Helpers;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels
{
    public class PlayerFeaturesViewModel : SubModel
    {
        public PlayerFeaturesViewModel(PlayerModel parent) : base(parent, "Features & Traits")
        {
            parent.PlayerChanged += Parent_PlayerChanged;
            features = (from f in Context.Player.GetFeatures() where f.Name != "" && !f.Hidden select f).ToList();
            UpdateFeatures();
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            features = (from f in Context.Player.GetFeatures() where f.Name != "" && !f.Hidden select f).ToList();
            UpdateFeatures();
        }
        private string featureSearch;
        public string FeatureSearch
        {
            get => featureSearch;
            set
            {
                SetProperty(ref featureSearch, value);
                UpdateFeatures();
            }
        }
        private List<Feature> features;
        public ObservableRangeCollection<Feature> Features { get; set; } = new ObservableRangeCollection<Feature>();

        public void UpdateFeatures() => Features.ReplaceRange(from f in features where featureSearch == null || featureSearch == ""
            || Culture.CompareInfo.IndexOf(f.Name ?? "", featureSearch, CompareOptions.IgnoreCase) >= 0
            || Culture.CompareInfo.IndexOf(f.Text ?? "", featureSearch, CompareOptions.IgnoreCase) >= 0 orderby f.Name select f);
    }
}
