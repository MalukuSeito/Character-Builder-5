using CB_5e.ViewModels;
using CB_5e.ViewModels.Character;
using OGL.Items;
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
    public partial class JournalEntryPage : ContentPage
    {
        public JournalViewModel Model { get; set; }
        public JournalEntryPage(JournalViewModel model)
        {
            InitializeComponent();
            BindingContext = Model = model;
        }
        protected override void OnDisappearing()
        {
            if (Model.IsChanged)
            {
                if (Model.IsNew) Model.Context.Context.Player.ComplexJournal.Add(Model.Journal);
                Model.IsChanged = false;
                Model.Context.Save();
                if (Model.XPChanged) Model.Context.FirePlayerChanged();
                else
                {
                    if (Model.MoneyChanged) Model.Context.MoneyChanged();
                    if (Model.StatsChanged) Model.Context.StatsChanged();
                    Model.Context.RefreshJournal.Execute(null);
                }
            }
            base.OnDisappearing();
        }
    }
}