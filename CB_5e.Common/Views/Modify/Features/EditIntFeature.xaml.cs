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
using OGL;
using OGL.Keywords;

namespace CB_5e.Views.Modify.Features
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditIntFeature : ContentPage, IFeatureEditModel
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
        public EditIntFeature(IFeatureEditModel parent, string title, string header, string property, int stepsize = 1)
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
            OnPropertyChanged(e.PropertyName);
        }

        public IFeatureEditModel Model { get; set; }

        public List<Keyword> Keywords => throw new NotImplementedException();

        public string Name { get => Model.Name; set => Model.Name = value; }
        public string Text { get => Model.Text; set => Model.Text = value; }
        public string Prerequisite { get => Model.Prerequisite; set => Model.Prerequisite = value; }
        public int Level { get => Model.Level; set => Model.Level = value; }
        public bool Hidden { get => Model.Hidden; set => Model.Hidden = value; }
        public bool Sheet { get => Model.Sheet; set => Model.Sheet = value; }
        public bool NoPreview { get => Model.NoPreview; set => Model.NoPreview = value; }
        public bool Preview { get => Model.Preview; set => Model.Preview = value; }
        public string Action { get => Model.Action; set => Model.Action = value; }
        public List<string> Actions { get => Model.Actions; set => Model.Actions = value; }

        public Command Undo => Model.Undo;

        public Command Redo => Model.Redo;

        public Command Save => Model.Save;

        public bool TrackChanges { get => Model.TrackChanges; set => Model.TrackChanges = value; }

        public OGLContext Context => Model.Context;

        public int UnsavedChanges => Model.UnsavedChanges;

        INavigation IEditModel.Navigation { get => Model.Navigation; set => Model.Navigation = value; }

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

        public void MakeHistory(string id = "")
        {
            Model.MakeHistory(id);
        }

        public Task<bool> SaveAsync(bool overwrite)
        {
            return Model.SaveAsync(overwrite);
        }
    }
}