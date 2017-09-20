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
    public partial class EditExpression : ContentPage
    {
        public string Property { get; private set; }
        public string Text
        {
            get => (string)Model.GetType().GetRuntimeProperty(Property).GetValue(Model);
            set {
                Model.GetType().GetRuntimeProperty(Property).SetValue(Model, value.Replace('\n', ' '));
                OnPropertyChanged("Text");
            }
        }
        public Color Accent { get => Color.Accent; }
        public string Notes { get; set; }
        public string Header { get; set; }

        public EditExpression(IEditModel parent, string title, string header, string property, string notes)
        {
            InitializeComponent();
            Model = parent;
            parent.PropertyChanged += Parent_PropertyChanged;
            Property = property;
            Notes = notes;
            Title = title;
            Header = header;
            BindingContext = this;
        }

        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == Property) OnPropertyChanged("Text");
        }

        public IEditModel Model { get; private set; }

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