using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Character_Builder;
using Xamarin.Forms;
using System.Threading;
using CB_5e.Helpers;
using OGL.Features;
using OGL;
using PCLStorage;
using System.IO;

namespace CB_5e.ViewModels
{
    public class PlayerBuildModel : PlayerModel
    {
        public Mutex SaveLock = new Mutex();
        public PlayerBuildModel(BuilderContext context) : base(context)
        {
            PlayerChanged += PlayerBuildModel_PlayerChanged;
            Context.HistoryButtonChange += Player_HistoryButtonChange;
            Undo = new Command(() =>
            {
                Context.Undo();
                FirePlayerChanged();
                Save();
            }, () =>
            {
                return Context.CanUndo();
            });
            Redo = new Command(() =>
            {
                Context.Redo();
                FirePlayerChanged();
                Save();
            }, () =>
            {
                return Context.CanRedo();
            });
            UpdateRaceChoices();
        }

        private void PlayerBuildModel_PlayerChanged(object sender, EventArgs e)
        {
            UpdateRaceChoices();
        }

        public override void DoSave()
        {
            if (Context.Player != null && !ChildModel)
            {
                Player p = Context.Player;
                SaveLock.WaitOne();
                try
                {
                    if (p.FilePath == null) p.FilePath = App.Storage.CreateFolderAsync("Characters", CreationCollisionOption.OpenIfExists).Result.CreateFileAsync(p.Name + ".cb5", CreationCollisionOption.GenerateUniqueName).Result;
                        if (p.FilePath is IFile file)
                        {
                            Context.UnsavedChanges = 0;
                            using (Stream s = file.OpenAsync(FileAccess.ReadAndWrite).Result)
                            {
                                Player.Serializer.Serialize(s, p);
                                s.SetLength(s.Position);
                            }
                        }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error Saving", e);
                }
                SaveLock.ReleaseMutex();
            }
        }

        public override void MakeHistory(string h = null)
        {
            Context.MakeHistory(h);
        }

        public override void Save()
        {
            
        }

        private void Player_HistoryButtonChange(object sender, bool CanUndo, bool CanRedo)
        {
            Undo.ChangeCanExecute();
            Redo.ChangeCanExecute();
        }

        public ObservableRangeCollection<ChoiceViewModel> RaceChoices { get; set; } = new ObservableRangeCollection<ChoiceViewModel>();
        public void UpdateRaceChoices()
        {
            List<ChoiceViewModel> choices = new List<ChoiceViewModel>();
            choices.Add(new RaceChoice(this));
            List<String> parentraces = new List<string>();
            foreach (Feature f in Context.Player.GetFeatures().Where(f => f is SubRaceFeature)) parentraces.AddRange(((SubRaceFeature)f).Races);
            if (parentraces.Count > 0) choices.Add(new SubRaceChoice(this, parentraces));
            foreach (Feature f in Context.Player.GetRaceFeatures())
            {
                ChoiceViewModel c = ChoiceViewModel.GetChoice(this, f);
                if (c != null) choices.Add(c);
            }
            RaceChoices.ReplaceRange(choices);
        }

        public override void UpdateSpellcasting()
        {
            throw new NotImplementedException();
        }
    }
}
