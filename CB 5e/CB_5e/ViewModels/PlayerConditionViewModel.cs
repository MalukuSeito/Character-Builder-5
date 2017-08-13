using CB_5e.Helpers;
using OGL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class PlayerConditionViewModel : SubModel
    {
        public PlayerConditionViewModel(PlayerModel parent) : base(parent, "Conditions")
        {

            AddCustomCondition = new Command(() =>
            {
                if (CustomCondition != null && CustomCondition != "")
                {
                    MakeHistory();
                    Context.Player.Conditions.Add(CustomCondition);
                    UpdateCondtions();
                    Save();
                }
            }, () => CustomCondition != null && CustomCondition != "");
            AddCondition = new Command((par) =>
            {
                if (par is OGL.Condition p)
                {
                    MakeHistory();
                    Context.Player.Conditions.Add(p.Name + " " + ConfigManager.SourceSeperator + " " + p.Source);
                    UpdateCondtions();
                    Save();
                }
            });
            RemoveCondition = new Command((par) =>
            {
                if (par is OGL.Condition p)
                {
                    MakeHistory();
                    Context.Player.Conditions.RemoveAll(s => ConfigManager.SourceInvariantComparer.Equals(s, p.Name + " " + ConfigManager.SourceSeperator + " " + p.Source));
                    UpdateCondtions();
                    Save();
                }
            });
            ResetConditions = new Command((par) =>
            {
                ConditionsBusy = true;
                MakeHistory();
                Context.Player.Conditions.Clear();
                UpdateCondtions();
                Save();
                ConditionsBusy = false;
            });
            parent.PlayerChanged += Parent_PlayerChanged;
            UpdateAllConditions();
            UpdateCondtions();
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            UpdateAllConditions();
            UpdateCondtions();
        }
        private string customCondition;
        public string CustomCondition
        {
            get => customCondition; set
            {
                SetProperty(ref customCondition, value);
                AddCustomCondition.ChangeCanExecute();
            }
        }


        private string condtionSearch;
        public ObservableRangeCollection<OGL.Condition> ActiveConditions { get; set; } = new ObservableRangeCollection<OGL.Condition>();
        public ObservableRangeCollection<OGL.Condition> AllConditions { get; set; } = new ObservableRangeCollection<OGL.Condition>();
        public string ConditionSearch
        {
            get => condtionSearch;
            set
            {
                SetProperty(ref condtionSearch, value);
                UpdateAllConditions();
            }
        }

        public Command AddCustomCondition { get; private set; }
        public Command AddCondition { get; private set; }
        public Command RemoveCondition { get; private set; }
        public Command ResetConditions { get; private set; }
        private bool cbusy;
        public bool ConditionsBusy { get => cbusy; private set => SetProperty(ref cbusy, value); }

        public void UpdateAllConditions() => AllConditions.ReplaceRange(from c in Context.Conditions.Values where condtionSearch == null || condtionSearch == "" || Culture.CompareInfo.IndexOf(c.Description, condtionSearch, CompareOptions.IgnoreCase) >= 0 || Culture.CompareInfo.IndexOf(c.Name, condtionSearch, CompareOptions.IgnoreCase) >= 0 orderby c.Name select c);
        public void UpdateCondtions() => ActiveConditions.ReplaceRange(from c in Context.Player.Conditions orderby c select Context.GetCondition(c, null));
    }
}
