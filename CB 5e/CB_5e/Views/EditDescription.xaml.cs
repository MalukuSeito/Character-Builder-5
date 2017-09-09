using CB_5e.ViewModels;
using OGL.Descriptions;
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
    public partial class EditDescription : ContentPage
    {
        public DescriptionViewModel Model { get; private set; }
        public bool TrackChanges { get; private set; } = false;
        public IEditModel Edit { get; private set; }
        public bool Changed { get; private set; } = false;

        public String Name {
            get => Model.Description.Name; set
            {
                Model.Description.Name = value;
                if (TrackChanges) Changed = true;
                OnPropertyChanged("Name");
                Model.Refresh();
            }
        }

        public String Text
        {
            get => Model.Description.Text; set
            {
                Model.Description.Text = value;
                if (TrackChanges) Changed = true;
                OnPropertyChanged("Text");
            }
        }

        public EditDescription(DescriptionViewModel model, IEditModel editModel)
        {
            Model = model;
            BindingContext = this;
            TrackChanges = false;
            InitializeComponent();
            TrackChanges = true;
            Edit = editModel;
        }
       
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (Changed) Edit.MakeHistory("");
            Changed = false;
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            if (Changed) Edit.MakeHistory("");
            Changed = false;
            return base.OnBackButtonPressed();
        }
    }
}