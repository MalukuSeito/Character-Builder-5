using System;

using CB_5e.Models;

using Xamarin.Forms;
using CB_5e.ViewModels;
using System.IO;
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
            if (source is FileInfo file) Name = Title = file.Name;
            if (source is DirectoryInfo f) Name = Title = f.Name;
            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Name = string.Join("_", Name.Split(ConfigManager.InvalidChars));
                if (Name == null || Name == "")
                {
                    return;
                }
                if (source is FileInfo file)
                {
                    string s = Path.Combine(file.Directory.FullName, Name);
                    int i = 0;
                    while (File.Exists(s)) s = Path.Combine(file.Directory.FullName, Name.Contains(".") ? Name.Replace(".", " (" + ++i + ").") : Name + " (" + ++i + ")");
                    file.MoveTo(s);
                }
                if (source is DirectoryInfo f)
                {
                    string s = Path.Combine(f.Parent.FullName, Name);
                    int i = 0;
                    while (Directory.Exists(s)) s = Path.Combine(f.Parent.FullName, Name + " (" + ++i + ")");
                    Directory.Move(f.FullName, s);
                }
                model.ExecuteLoadItemsCommand();
            }).Forget();
            await Navigation.PopAsync();
            
        }
    }
}