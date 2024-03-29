﻿using System;

using CB_5e.Models;
using CB_5e.ViewModels;

using Xamarin.Forms;
using CB_5e.Services;
using Character_Builder;
using System.Collections.Generic;
using PCLStorage;
using System.IO;
using System.Threading.Tasks;
using PluginDMG;
using CB_5e.ViewModels.Character;
using CB_5e.ViewModels.Character.Play;

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
            manager.Add(new OptionalClassFeatures());
            manager.Add(new RacialAbilityShift());
            manager.Add(new LanguageChoice());
            manager.Add(new SkillChoice());
            manager.Add(new ToolChoice());
            manager.Add(new NoFreeEquipment());
            manager.Add(new CustomBackground());
            manager.Add(new BackgroundFeat());
            manager.Add(new SpellPoints());
            manager.Add(new SingleLanguage());
            manager.Add(new PlaneTouchedWings());
            Context.Plugins = manager;

            if (App.AutoSaveDuringPlay)
            {
                Task.Run(async () =>
                {
                    if (Context.Player.FilePath is IFile file)
                    {
                        string name = file.Name;
                        IFolder back = await App.Storage.CreateFolderAsync("Backups", CreationCollisionOption.OpenIfExists).ConfigureAwait(false);
                        if (item.Folder != "--")
                        {
                            foreach (string f in item.Folder.Split('/'))
                            {
                                if (f != "")
                                {
                                    back = await back.CreateFolderAsync(f, CreationCollisionOption.OpenIfExists).ConfigureAwait(false);
                                }
                            }
                        }
                        IFile target = await back.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting).ConfigureAwait(false);
                        using (Stream fout = await target.OpenAsync(FileAccess.ReadAndWrite))
                        {
                            using (Stream fin = await file.OpenAsync(FileAccess.Read))
                            {
                                await fin.CopyToAsync(fout);
                            }
                        };
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
                    await Navigation.PushModalAsync(new NavigationPage(new FlexPage(model)));
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
