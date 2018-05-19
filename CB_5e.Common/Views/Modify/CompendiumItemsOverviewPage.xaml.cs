﻿using CB_5e.Helpers;
using CB_5e.Services;
using OGL;
using OGL.Common;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Modify
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompendiumItemsOverviewPage : ContentPage
    {
        private OGLContext Context = new OGLContext();
        public CompendiumItemsOverviewPage()
        {
            InitializeComponent();
            Refresh = new Command(async () =>
            {
                Entries.ReplaceRange(new List<string>());
                if (IsBusy) return;
                IsBusy = true;
                await PCLSourceManager.InitAsync();
                ConfigManager config = await Context.LoadConfigAsync(new FileInfo(Path.Combine(PCLSourceManager.Data.FullName, "Config.xml"))).ConfigureAwait(false);
                DependencyService.Get<IHTMLService>().Reset(config);
                Entries.Add("Items");
                foreach (string s in await PCLImport.EnumerateCategories(Context, Context.Config.Items_Directory)) Entries.Add(s);
                IsBusy = false;
            });

            Title = "Item Categories";
            BindingContext = this;
        }

        public Command Refresh { get; private set; }

       

        public ObservableRangeCollection<string> Entries { get; set; } = new ObservableRangeCollection<string>();

        protected override void OnAppearing()
        {
            if (Entries.Count == 0) Refresh.Execute(null);
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
                await Navigation.PushAsync(new CustomTextEntryPage("New Item Category", new Command(async (par) =>
                {
                    if (par is string s)
                    {
                        String iname = string.Join("_", s.Split(ConfigManager.InvalidChars));
                        string b = Path.Combine(PCLSourceManager.Data.FullName, Context.Config.DefaultSource ?? "No Source");
                        if ((sender as MenuItem).BindingContext is string t) b = b = Path.Combine(b, t, iname);
                        else b = Path.Combine(b, Context.Config.Items_Directory, iname);
                        Directory.CreateDirectory(b);
                        Refresh.Execute(null);
                    }
                })));
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is string)
            {
                await Navigation.PushAsync(new CompendiumItemsPage(e.SelectedItem as string));
                (sender as ListView).SelectedItem = null;
            }
        }
    }
}