using CB_5e.ViewModels;
using CB_5e.ViewModels.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Character
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerNotesPage : ContentPage
    {
        public PlayerNotesViewModel Model { get; private set; }
        public PlayerNotesPage(PlayerNotesViewModel model)
        {
            BindingContext = Model = model;
            InitializeComponent();
        }
        private void MenuItem_Clicked_1(object sender, EventArgs e)
        {
            if ((sender as Xamarin.Forms.MenuItem).BindingContext is string obj)
            {
                Model.MakeHistory();
                Model.Context.Player.Journal.Remove(obj);
                Model.Save();
                Model.RefreshNotes.Execute(null);
            }
        }
    }
}