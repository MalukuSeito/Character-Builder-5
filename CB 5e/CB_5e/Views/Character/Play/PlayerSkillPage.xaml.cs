using CB_5e.ViewModels;
using CB_5e.ViewModels.Character.Play;
using Character_Builder;
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
    public partial class PlayerSkillPage : ContentPage
    {
        public PlayerSkillPage(PlayerSkillViewModel model)
        {
            BindingContext = model;
            model.Navigation = Navigation;
            InitializeComponent();
        }
        private async void SkillInfo(object sender, EventArgs e)
        {
            if ((sender as Xamarin.Forms.MenuItem).BindingContext is SkillInfo obj) await Navigation.PushAsync(InfoPage.Show(obj.Skill));
        }
    }
}