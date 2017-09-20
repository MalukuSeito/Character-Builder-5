using CB_5e.ViewModels.Modify;
using CB_5e.ViewModels.Modify.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Modify.Features
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditIntFeature : ContentPage
    {
        public string Property { get; private set; }
        public int Value
        {
            get => (int)Model.GetType().GetRuntimeProperty(Property).GetValue(Model);
            set
            {
                Model.GetType().GetRuntimeProperty(Property).SetValue(Model, value);
                OnPropertyChanged("Value");
                OnPropertyChanged("StepperValue");
            }
        }

        public int StepperValue
        {
            get => Value / Stepsize;
            set => Value = value * Stepsize;
        }
        private int Stepsize = 1;

        public string Header { get; set; }
        public EditIntFeature(IEditModel parent, string title, string header, string property, int stepsize = 1)
        {
            InitializeComponent();
            Model = parent;
            parent.PropertyChanged += Parent_PropertyChanged;
            Property = property;
            Title = title;
            Stepsize = stepsize;
            Header = header;
            BindingContext = this;
        }

        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == Property)
            {
                OnPropertyChanged("Value");
                OnPropertyChanged("StepperValue");
            }
        }

        public IEditModel Model { get; set; }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Model.SaveAsync(true);
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            Task.Run(async () =>
            {
                await Model.SaveAsync(true);
                await Navigation.PopModalAsync();
            });
            return true;
        }
    }
}