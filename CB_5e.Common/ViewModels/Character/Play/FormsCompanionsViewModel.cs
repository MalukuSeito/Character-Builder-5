using CB_5e.Helpers;
using CB_5e.ViewModels.Character;
using CB_5e.Views;
using Character_Builder;
using OGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character.Play
{
    public class FormsCompanionsViewModel: SubModel
    {
        public PlayerModel Model { get; set; }

        FormsCompanionInfo FormsCompanion { get; set; }

        public Command OnPrepare { get; set; }
        public Command ShowInfo { get; set; }
        public Command Info { get; set; }

        public string ID { get => FormsCompanion.ID; }

        public FormsCompanionsViewModel(PlayerModel model, FormsCompanionInfo fci): base(model, fci.DisplayName)
        {
            Model = model;
            FormsCompanion = fci;
            OnPrepare = new Command((par) =>
            {
                if (par is MonsterViewModel mvm && FormsCompanion != null)
                {
                    if (!mvm.Selected)
                    {
                        if (Selected < Count || Count < 0)
                        {
                            mvm.Selected = true;
                            Model.MakeHistory();
                            Model.Context.Player.AddFormCompanion(FormsCompanion.ID, mvm.Monster);
                            Model.Save();
                        }
                    }
                    else
                    {
                        mvm.Selected = false;
                        Model.MakeHistory();
                        Model.Context.Player.RemoveFormCompanion(FormsCompanion.ID, mvm.Monster);
                        Model.Save();
                        if (mvm.BadChoice) Choices.Remove(mvm);
                        if (mvm.BadChoice) choices.Remove(mvm);
                    }
                    OnPropertyChanged("Count");
                    OnPropertyChanged("Selected");
                    OnPropertyChanged("SelectedInfo");
                }
            }, (par) => par is MonsterViewModel mvm);
            ShowInfo = new Command(async (par) =>
            {
                if (par is MonsterViewModel mvm)
                {
                    await Navigation.PushAsync(InfoPage.Show(mvm.Monster));
                }
            });

            Info = new Command(async (par) =>
            {
                await Navigation.PushAsync(InfoPage.Show(fci.Source));
            });
            ResetPrepared = new Command(() =>
            {
                IsBusy = true;
                Model.MakeHistory();
                Model.Context.Player.RemoveAllFormCompanion(FormsCompanion.ID);
                foreach (MonsterViewModel m in choices)
                {
                    m.Selected = false;
                }
                Model.Save();
                choices.RemoveAll(s => s.BadChoice);
                UpdateChoices();
                OnPropertyChanged("Count");
                OnPropertyChanged("Selected");
                OnPropertyChanged("SelectedInfo");
                IsBusy = false;
            });
            AddChoices();

        }

        public void Refresh(FormsCompanionInfo fci)
        {
            FormsCompanion = fci;
            AddChoices();
            OnPropertyChanged(null);
        }

        public void AddChoices()
        {
            choices.Clear();
            List<Monster> c = new List<Monster>(FormsCompanion.Choices);
            foreach (Monster m in FormsCompanion.AvailableOptions(Model.Context, Model.Context.Player.GetFinalAbilityScores()))
            {
                choices.Add(new MonsterViewModel(m)
                {
                    Selected = c.Remove(m),
                    Prepare = OnPrepare,
                    ShowInfo = ShowInfo
                });
            }
            foreach (Monster m in c)
            {
                choices.Add(new MonsterViewModel(m)
                {
                    Selected = true,
                    BadChoice = true,
                    Prepare = OnPrepare,
                    ShowInfo = ShowInfo
                });
            }
            choices.Sort();
            UpdateChoices();
        }

        public int Count { get => FormsCompanion.Count; }
        public int Selected { get => FormsCompanion.Choices.Count; }

        public string SelectedInfo { get => "Selected " + Selected + (Count >= 0 ? "/" + Count : ""); }
        public Command ResetPrepared { get; private set; }

        private List<MonsterViewModel> choices = new List<MonsterViewModel>();
        public ObservableRangeCollection<MonsterViewModel> Choices { get; private set; } = new ObservableRangeCollection<MonsterViewModel>();
        private string monsterSearch;
        public String MonsterSearch
        {
            get => monsterSearch; set
            {
                SetProperty(ref monsterSearch, value);
                UpdateChoices();
            }
        }
        public void UpdateChoices() => Choices.ReplaceRange(from f in choices where monsterSearch == null || monsterSearch == ""|| f.Monster.Matches(monsterSearch, false) orderby f.Name select f);
    }
}
