using System;

using CB_5e.Models;

using Xamarin.Forms;
using PCLStorage;
using CB_5e.ViewModels;
using OGL;
using System.Threading.Tasks;
using CB_5e.ViewModels.Files;

namespace CB_5e.Views.Files
{
    public partial class RenamePage : ContentPage
    {
        public string Name { get; set; }
        private FilesViewModel model;
        private object source;

        public RenamePage(FilesViewModel model, object source)
        {
            this.model = model;
            this.source = source;
            InitializeComponent();
            Title = model.Title;
            if (source is IFile file) Name = Title = file.Name;
            if (source is IFolder f) Name = Title = f.Name;
            BindingContext = this;
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
                if (source is IFile file)
                {
                    await file.RenameAsync(Name, NameCollisionOption.GenerateUniqueName);
                }
                if (source is IFolder f)
                {
                    await FileBrowser.Move(f, await model.Folder.CreateFolderAsync(Name, CreationCollisionOption.OpenIfExists).ConfigureAwait(false));
                }
                await model.ExecuteLoadItemsCommand();
            }).Forget();
            await Navigation.PopAsync();
            
        }
    }
}