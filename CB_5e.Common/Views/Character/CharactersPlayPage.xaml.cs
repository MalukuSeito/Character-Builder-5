using System;

using CB_5e.Models;
using CB_5e.ViewModels;

using Xamarin.Forms;
using CB_5e.Services;
using Character_Builder;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PluginDMG;
using CB_5e.ViewModels.Character;
using CB_5e.ViewModels.Character.Play;
using Character_Builder_Base;

namespace CB_5e.Views.Character
{
    public partial class CharactersPlayPage : ContentPage
    {
        CharactersViewModel viewModel;

        public CharactersPlayPage()
        {
            InitializeComponent();

            BindingContext = viewModel = CharactersViewModel.Instance;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (IsBusy) return;
            IsBusy = true;
            //viewModel.Items.Clear();
            var item = args.SelectedItem as Models.Character;
            if (item == null)
                return;
            BuilderContext Context = new BuilderContext(item.Player);
            PluginManager manager = new PluginManager();
            manager.Add(new NoFreeEquipment());
            manager.Add(new SpellPoints());
            manager.Add(new SingleLanguage());
            Context.Plugins = manager;

            if (App.AutoSaveDuringPlay)
            {
                Task.Run(() =>
                {
                    if (Context.Player.FilePath is FileInfo file)
                    {
                        string name = file.Name;
                        string tt = Path.Combine(App.Storage.FullName, "Backups");
                        Directory.CreateDirectory(tt);
                        file.CopyTo(Path.Combine(tt, file.Name), true);
                    }
                }).Forget();
            }
            Context.UndoBuffer = new LinkedList<Player>();
            Context.RedoBuffer = new LinkedList<Player>();
            Context.UnsavedChanges = 0;
            ItemsListView.SelectedItem = null;
            LoadingProgress loader = new LoadingProgress(Context);
            LoadingPage l = new LoadingPage(loader);
            await Navigation.PushModalAsync(l);
            var t = l.Cancel.Token;
            try
            {
                await loader.Load(t).ConfigureAwait(false);
                t.ThrowIfCancellationRequested();
                PlayerViewModel model = new PlayerViewModel(Context);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync(false);
                    await Navigation.PushModalAsync(new NavigationPage(new FlowPage(model)));
                });
            }
            catch (OperationCanceledException) {
            }
            finally
            {
                IsBusy = false;
                (sender as ListView).SelectedItem = null;
            }
            // Manually deselect item

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
