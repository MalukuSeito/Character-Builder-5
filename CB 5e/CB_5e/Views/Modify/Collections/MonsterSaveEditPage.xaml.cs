using CB_5e.ViewModels;
using CB_5e.ViewModels.Modify.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Modify.Collections
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonsterSaveEditPage : ContentPage
    {
        public MonsterSaveViewModel Model { get; private set; }
        public MonsterSaveEditPage(MonsterSaveViewModel model)
        {
            BindingContext = Model = model;
            InitializeComponent();
        }
        protected override void OnDisappearing()
        {
            Model?.Refresh();
            base.OnDisappearing();

        }

    }
}