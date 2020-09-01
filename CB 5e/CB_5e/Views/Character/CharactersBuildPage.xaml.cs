using System;

using CB_5e.Models;
using CB_5e.ViewModels;

using Xamarin.Forms;
using Character_Builder;
using PluginDMG;
using System.Collections.Generic;
using System.Threading.Tasks;
using PCLStorage;
using System.IO;
using CB_5e.ViewModels.Character;
using CB_5e.ViewModels.Character.Build;

namespace CB_5e.Views.Character
{
    public partial class CharactersBuildPage : ContentPage
    {
        CharactersViewModel viewModel;

        public CharactersBuildPage()
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
            manager.Add(new SpellPoints());
            manager.Add(new SingleLanguage());
            manager.Add(new CustomBackground());
            manager.Add(new NoFreeEquipment());
            Context.Plugins = manager;

            //Task.Run(async () =>
            //     {
            //         if (Context.Player.FilePath is IFile file)
            //         {
            //             string name = file.Name;
            //             IFile target = await (await App.Storage.CreateFolderAsync("Backups", CreationCollisionOption.OpenIfExists).ConfigureAwait(false)).CreateFileAsync(name, CreationCollisionOption.ReplaceExisting).ConfigureAwait(false);
            //             using (Stream fout = await target.OpenAsync(FileAccess.ReadAndWrite))
            //             {
            //                 using (Stream fin = await file.OpenAsync(FileAccess.Read))
            //                 {
            //                     await fin.CopyToAsync(fout);
            //                 }
            //             };
            //         }
            //     }).Forget();
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
                PlayerBuildModel model = new PlayerBuildModel(Context);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync(false);
                    await Navigation.PushModalAsync(new NavigationPage(new FlexPage(model)));
                });
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                IsBusy = false;
                (sender as ListView).SelectedItem = null;
            }
            // Manually deselect item

        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            viewModel.Items.Clear();
            BuilderContext Context = new BuilderContext(new Player()
            {
                Name = "New Player"
            });
            PluginManager manager = new PluginManager();
            manager.Add(new NoFreeEquipment());
            manager.Add(new CustomBackground());
            manager.Add(new SpellPoints());
            manager.Add(new SingleLanguage());
            Context.Plugins = manager;
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
                PlayerBuildModel model = new PlayerBuildModel(Context);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync(false);
                    await Navigation.PushModalAsync(new NavigationPage(new FlexPage(model)));
                });
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                IsBusy = false;
            }
            // Manually deselect item
        }
    }
}
