using CB_5e.Helpers;
using CB_5e.ViewModels;
using Character_Builder;
using OGL.Common;
using OGL.Features;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JournalPage : ContentPage
	{
        public PlayerJournalViewModel Model { get; private set; }

        public JournalPage(PlayerJournalViewModel model)
		{
            BindingContext = Model = model;
            InitializeComponent ();
        }


        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new JournalEntryPage(new JournalViewModel(Model)));
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is JournalViewModel je) await Navigation.PushAsync(new JournalEntryPage(je));
            ((ListView)sender).SelectedItem = null;
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            if ((sender as Xamarin.Forms.MenuItem).BindingContext is JournalViewModel obj)
            {
                Model.MakeHistory();
                Model.Context.Player.ComplexJournal.Remove(obj.Journal);
                Model.Save();
                Model.FirePlayerChanged();
            }
        }
    }
}