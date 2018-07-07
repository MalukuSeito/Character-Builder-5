﻿using CB_5e.Helpers;
using CB_5e.Services;
using CB_5e.ViewModels;
using CB_5e.ViewModels.Modify;
using CB_5e.Views.Modify.Features;
using OGL;
using OGL.Common;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Modify
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompendiumFeatsPage : ContentPage
    {
        public static CultureInfo Culture = CultureInfo.InvariantCulture;
        private OGLContext Context = new OGLContext();
        private string category = null;
        public CompendiumFeatsPage(string cat)
        {
            InitializeComponent();
            category = cat;
            Refresh = new Command(async () =>
            {
                Entries.ReplaceRange(new List<FeatureContainer>());
                if (IsBusy) return;
                IsBusy = true;
                await PCLSourceManager.InitAsync();
                ConfigManager config = await Context.LoadConfigAsync(await PCLSourceManager.Data.GetFileAsync("Config.xml").ConfigureAwait(false)).ConfigureAwait(false);
                DependencyService.Get<IHTMLService>().Reset(config);
                await PCLImport.ImportStandaloneFeaturesAsync(Context);
                UpdateEntries();
                IsBusy = false;
            });
            Title = "Features";
            BindingContext = this;
        }

        public Command Refresh { get; private set; }

        private string search;

        public string Search {
            get => search;
            set
            {
                search = value;
                OnPropertyChanged("Search");
                if (!IsBusy) UpdateEntries();
            }
        }


        public void UpdateEntries()
        {
            Entries.ReplaceRange(new List<FeatureContainer>());
            if (category != null && Context.FeatureContainers.ContainsKey(category)) Entries.ReplaceRange(from r in Context.FeatureContainers[category]
                                 where search == null 
                                 || search == "" 
                                 || Culture.CompareInfo.IndexOf(r.Name ?? "", search, CompareOptions.IgnoreCase) >= 0 
                                 || Culture.CompareInfo.IndexOf(r.Source ?? "", search, CompareOptions.IgnoreCase) >= 0 
                                 orderby r.Name, r.Source select r);
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is IXML obj) await Navigation.PushAsync(InfoPage.Show(obj));
        }

        public ObservableRangeCollection<FeatureContainer> Entries { get; set; } = new ObservableRangeCollection<FeatureContainer>();

        protected override void OnAppearing()
        {
            if (Entries.Count == 0) Refresh.Execute(null);
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is FeatureContainer obj)
            {
                if (IsBusy) return;
                IsBusy = true;
                await Navigation.PushModalAsync(MakePage(new FeatureContainerEditModel(obj, Context)));
                Entries.Clear();
                IsBusy = false;
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            await Navigation.PushModalAsync(MakePage(new FeatureContainerEditModel(new FeatureContainer() { Source = Context.Config.DefaultSource, category = category }, Context)));
            Entries.Clear();
            IsBusy = false;
        }

        private Page MakePage(FeatureContainerEditModel m)
        {
            TabbedPage t = new TabbedPage();
            t.Children.Add(new NavigationPage(new EditFeatureContainer(m)) { Title = "Edit", Icon = Device.RuntimePlatform == Device.iOS ? "save.png" : null });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "Features")) { Title = "Features", Icon = Device.RuntimePlatform == Device.iOS ? "wallet_app.png" : null });
            return t;
        }
    }
}