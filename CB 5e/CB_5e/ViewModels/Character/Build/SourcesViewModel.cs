using CB_5e.Helpers;
using CB_5e.Models;
using CB_5e.Services;
using CB_5e.Views;
using CB_5e.Views.Character;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character.Build
{
    public class Source: BaseViewModel
    {
        private string name;
        public string Name { get => name; set => SetProperty(ref name, value); }
        private bool active;
        public bool Active { get => active; set
            {
                SetProperty(ref active, value);
                OnPropertyChanged("ActiveColor");
            }
        }
        public Command OnSelect { get; set; }
        public Command ExcludeOthers { get; set; }
        public bool Plugin { get; set; }
        public Color ActiveColor { get => active ? Color.DarkBlue : Color.DarkGray; }
        public string Type { get => Plugin ? "House Rule" : "Source Book"; }
    }

    public class Option: ObservableRangeCollection<Source>
    {
        private string name;
        public string Name
        {
            get => name; set
            {
                name = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Name"));
            }
        }
    }

    public class SourcesViewModel : SubModel
    {
        public SourcesViewModel(PlayerModel parent) : base(parent, "Options")
        {
            Image = ImageSource.FromResource("CB_5e.images.sources.png");
            Sources.Name = "Source books";
            Plugins.Name = "Houserules";
            Options.Add(Sources);
            Options.Add(Plugins);
            Excluded = new HashSet<string>(Context.Player.ExcludedSources);
            ActivePlugins = new HashSet<string>(Context.Player.ActiveHouseRules);
            parent.PlayerChanged += Parent_PlayerChanged;
            OnApply = new Command(async () =>
            {
                IsBusy = true;
                MakeHistory();
                if (!Excluded.SetEquals(Context.Player.ExcludedSources))
                {
                    
                    Context.Player.ExcludedSources.Clear();
                    Context.Player.ExcludedSources.AddRange(Excluded);
                    Context.ExcludedSources.Clear();
                    Context.ExcludedSources.UnionWith(Context.Player.ExcludedSources);
                    LoadingProgress loader = new LoadingProgress(Context);
                    LoadingPage l = new LoadingPage(loader, false);
                    await Navigation.PushModalAsync(l);
                    var t = l.Cancel.Token;
                    try
                    {
                        await loader.Load(t).ConfigureAwait(false);
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Navigation.PopModalAsync(false);
                        });
                    }
                    catch (OperationCanceledException)
                    {
                    }
                }
                if (!ActivePlugins.SetEquals(Context.Player.ActiveHouseRules)) { 
                    Context.Player.ActiveHouseRules.Clear();
                    Context.Player.ActiveHouseRules.AddRange(ActivePlugins);
                    Context.Plugins.Load(Context.Player.ActiveHouseRules);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    OnApply.ChangeCanExecute();
                });
                FirePlayerChanged();
                IsBusy = false;
                Save();
            }, () => !ActivePlugins.SetEquals(Context.Player.ActiveHouseRules) || !Excluded.SetEquals(Context.Player.ExcludedSources));
            OnSelect = new Command((par) =>
            {
                if (par is Source s)
                {
                    if (s.Plugin)
                    {
                        if (ActivePlugins.Contains(s.Name))
                        {
                            s.Active = false;
                            ActivePlugins.Remove(s.Name);
                        }
                        else
                        {
                            s.Active = true;
                            ActivePlugins.Add(s.Name);
                        }
                    }
                    else
                    {
                        if (Excluded.Contains(s.Name))
                        {
                            s.Active = true;
                            Excluded.Remove(s.Name);
                        }
                        else
                        {
                            s.Active = false;
                            Excluded.Add(s.Name);
                        }
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        OnApply.ChangeCanExecute();
                    });
                }
            });

            ExcludeOthers = new Command((par) =>
            {
                if (par is Source s)
                {
                    if (s.Plugin)
                    {
                        ActivePlugins.UnionWith(Context.Plugins.available.Keys);
                        ActivePlugins.Remove(s.Name);
                        foreach (Source ss in Plugins) ss.Active = ActivePlugins.Contains(ss.Name);
                    }
                    else
                    {
                        Excluded.Clear();
                        Excluded.UnionWith(from f in PCLSourceManager.Sources where f.Name != s.Name select f.Name);
                        foreach (Source ss in Sources) ss.Active = !Excluded.Contains(ss.Name);
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        OnApply.ChangeCanExecute();
                    });
                }
            });
            Refresh = new Command(() =>
            {
                IsBusy = true;
                if (!Excluded.SetEquals(Context.Player.ExcludedSources))
                {
                    Excluded = new HashSet<string>(Context.Player.ExcludedSources);
                    UpdateSources();
                }
                if (!ActivePlugins.SetEquals(Context.Player.ActiveHouseRules))
                {
                    ActivePlugins = new HashSet<string>(Context.Player.ActiveHouseRules);
                    UpdatePlugins();
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    OnApply.ChangeCanExecute();
                });
                IsBusy = false;
            });
            UpdatePlugins();
            UpdateSources();
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            if (!Excluded.SetEquals(Context.Player.ExcludedSources))
            {
                Excluded = new HashSet<string>(Context.Player.ExcludedSources);
                UpdateSources();
            }
            if (!ActivePlugins.SetEquals(Context.Player.ActiveHouseRules))
            {
                ActivePlugins = new HashSet<string>(Context.Player.ActiveHouseRules);
                UpdatePlugins();
            }
        }

        public bool ShowRituals {
            get => Context.Player.AllRituals;
            set
            {
                MakeHistory("ShowRituals");
                Context.Player.AllRituals = value;
                OnPropertyChanged("ShowRituals");
                UpdateSpellcasting();
                Save();
            }
        }

        public bool Advancement
        {
            get => Context.Player.Advancement;
            set
            {
                MakeHistory("Advancement");
                Context.Player.Advancement = value;
                OnPropertyChanged("Advancement");
                Save();
                FirePlayerChanged();
            }
        }

        public Command OnApply { get; private set; }
        public HashSet<string> Excluded { get; private set; }
        public HashSet<string> ActivePlugins { get; private set; }

        public Option Sources { get; set; } = new Option();
        public Option Plugins { get; set; } = new Option();
        public ObservableRangeCollection<Option> Options { get; set; } = new ObservableRangeCollection<Option>();

        private string sourceSearch;

        public string SourceSearch
        {
            get => sourceSearch;
            set
            {
                SetProperty(ref sourceSearch, value);
                UpdateSources();
                UpdatePlugins();
            }
        }

        private void UpdateSources()
        {
            Sources.ReplaceRange(from s in PCLSourceManager.Sources
                                 where sourceSearch == null || sourceSearch == "" || Culture.CompareInfo.IndexOf(s.Name ?? "", sourceSearch, CompareOptions.IgnoreCase) >= 0
                                 select new Source()
                                 {
                                     Name = s.Name,
                                     Active = !Excluded.Contains(s.Name),
                                     OnSelect = OnSelect,
                                     ExcludeOthers = ExcludeOthers
                                 });
        }

        private void UpdatePlugins()
        {
            Plugins.ReplaceRange(from s in Context.Plugins.available.Keys
                                 where sourceSearch == null || sourceSearch == "" || Culture.CompareInfo.IndexOf(s ?? "", sourceSearch, CompareOptions.IgnoreCase) >= 0
                                 select new Source()
                                 {
                                     Name = s,
                                     Active = ActivePlugins.Contains(s),
                                     OnSelect = OnSelect,
                                     ExcludeOthers = ExcludeOthers,
                                     Plugin = true
                                 });
        }

        public Command OnSelect { get; private set; }
        public Command ExcludeOthers { get; private set; }
        public Command Refresh { get; private set; }
    }
}
