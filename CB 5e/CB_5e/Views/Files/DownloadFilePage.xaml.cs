using System;

using CB_5e.Models;

using Xamarin.Forms;
using PCLStorage;
using CB_5e.ViewModels;
using OGL;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.ComponentModel;
using CB_5e.ViewModels.Files;

namespace CB_5e.Views.Files
{
    public partial class DownloadFilePage : ContentPage, INotifyPropertyChanged
    {
        public string URL
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                if (value.Length > 0)
                {
                    string r = value;
                    if (r.EndsWith("/")) r = r.Substring(0, r.Length - 1);
                    if (r.Contains("/"))
                    {
                        Name = r.Substring(r.LastIndexOf('/') + 1);
                        OnPropertyChanged("Name");
                    }
                }
            }
        }
        private string _url;
        public string Name { get; set; }
        private FilesViewModel model;

        public DownloadFilePage(FilesViewModel model)
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
                UriBuilder u = new UriBuilder(URL);
                HttpClientHandler aHandler = new HttpClientHandler()
                {
                    ClientCertificateOptions = ClientCertificateOption.Automatic
                };
                HttpClient aClient = new HttpClient(aHandler);
                aClient.DefaultRequestHeaders.ExpectContinue = false;
                HttpResponseMessage response = await aClient.GetAsync(u.Uri);
                if (response.IsSuccessStatusCode)
                {
                    IFile f = await model.Folder.CreateFileAsync(Name, CreationCollisionOption.GenerateUniqueName);
                    using (Stream s = await f.OpenAsync(FileAccess.ReadAndWrite)) 
                        await response.Content.CopyToAsync(s);
                    await model.ExecuteLoadItemsCommand();
                }
            }).Forget();
            await DisplayAlert("Background Download", "Download started and will continue in the Background", "OK");
            await Navigation.PopAsync();

        }
    }
}