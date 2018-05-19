using System;

using CB_5e.Models;

using Xamarin.Forms;
using CB_5e.ViewModels;
using OGL;
using System.Threading.Tasks;
using System.IO;
using CB_5e.ViewModels.Files;

namespace CB_5e.Views.Files
{


    public partial class NewFolderPage : ContentPage
    {
        public string Name { get; set; }
        private FilesViewModel model;

        public NewFolderPage(FilesViewModel model)
        {
            this.model = model;
            InitializeComponent();
            BindingContext = this;
            Title = model.Title;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                Name = string.Join("_", Name.Split(ConfigManager.InvalidChars));
                if (Name == null || Name == "")
                {
                    return;
                }

                Directory.CreateDirectory(Path.Combine(model.Folder.FullName, Name));
                model.ExecuteLoadItemsCommand();
            }).Forget();
            await Navigation.PopAsync();
            
        }
    }
}