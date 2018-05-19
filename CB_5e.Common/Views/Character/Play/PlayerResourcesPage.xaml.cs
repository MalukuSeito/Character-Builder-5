using CB_5e.ViewModels;
using CB_5e.ViewModels.Character.Play;
using Character_Builder;
using OGL.Features;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Character.Play
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerResourcesPage : ContentPage
    {
        PlayerResourcesViewModel Model { get; set; }
        public PlayerResourcesPage(PlayerResourcesViewModel model)
        {
            BindingContext = Model = model;
            model.Navigation = Navigation;
            InitializeComponent();
        }

        private async void ResourceInfo(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is ResourceViewModel rs)
            {
                if (rs.Value is ModifiedSpell ms)
                {
                    ms.Info = Model.Context.Player.GetAttack(ms, ms.differentAbility);
                    ms.Modifikations.AddRange(from f in Model.Context.Player.GetFeatures() where f is SpellModifyFeature && Utils.Matches(Model.Context, ms, ((SpellModifyFeature)f).Spells, null) select f);
                    ms.Modifikations = ms.Modifikations.Distinct().ToList();
                    await Navigation.PushAsync(InfoPage.Show(ms));
                }
                if (rs.Value is ResourceInfo r)
                {
                    await Navigation.PushAsync(InfoPage.Show(Model.Context.Player.GetResourceFeatures(r.ResourceID)));
                }
            }
        }
    }
}