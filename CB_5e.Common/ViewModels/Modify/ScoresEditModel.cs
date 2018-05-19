using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL;
using Xamarin.Forms;
using CB_5e.Services;
using System.IO;

namespace CB_5e.ViewModels.Modify
{
    public class ScoresEditModel : BaseViewModel, IEditModel
    {
        public Command Undo { get; set; }

        public Command Redo { get; set; }

        public bool TrackChanges { get; set; }

        public OGLContext Context { get; set; }

        public int UnsavedChanges { get; set; }

        public INavigation Navigation { get; set; }

        public AbilityScores Model {
            get {
                return Context.Scores;
            }
            set
            {
                Context.Scores = value;
            }
        }


        public void MakeHistory(string id = "")
        {
            if (!TrackChanges) return;
            if (id == null) id = "";
            if (id == "" || id != lastid)
            {
                UndoBuffer.AddLast(Model.Clone());
                RedoBuffer.Clear();
                if (UndoBuffer.Count > 100) UndoBuffer.RemoveFirst();
                UnsavedChanges++;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Save.ChangeCanExecute();
                    Undo.ChangeCanExecute();
                    Redo.ChangeCanExecute();
                });
            }
            lastid = id;
        }

        public async Task<bool> SaveAsync(bool overwrite)
        {
            await SaveAsync();
            return true;
        }

        public LinkedList<AbilityScores> UndoBuffer = new LinkedList<AbilityScores>();
        public LinkedList<AbilityScores> RedoBuffer = new LinkedList<AbilityScores>();
        private string lastid = null;

        public Command Save { get; private set; }
        public void FireModelChanged() => ModelChanged?.Invoke(this, Model);
        public event EventHandler<AbilityScores> ModelChanged;

        public ScoresEditModel(OGLContext context) {
            Context = context;
            Undo = new Command(() =>
            {
                if (UndoBuffer.Count > 0)
                {
                    lastid = "";
                    RedoBuffer.AddLast(Model);
                    Model = UndoBuffer.Last.Value;
                    UndoBuffer.RemoveLast();
                    if (UnsavedChanges > 0) UnsavedChanges--;
                    Undo.ChangeCanExecute();
                    Redo.ChangeCanExecute();
                    Save.ChangeCanExecute();
                    bool old = TrackChanges;
                    TrackChanges = false;
                    FireModelChanged();
                    OnPropertyChanged("");
                    TrackChanges = old;
                }
            },
            () => UndoBuffer.Count > 0);
            Redo = new Command(() =>
            {
                if (RedoBuffer.Count > 0)
                {
                    lastid = "";
                    UndoBuffer.AddLast(Model);
                    Model = RedoBuffer.Last.Value;
                    RedoBuffer.RemoveLast();
                    UnsavedChanges++;
                    Undo.ChangeCanExecute();
                    Redo.ChangeCanExecute();
                    Save.ChangeCanExecute();
                    bool old = TrackChanges;
                    FireModelChanged();
                    TrackChanges = false;
                    OnPropertyChanged("");
                    TrackChanges = old;
                }
            },
            () => RedoBuffer.Count > 0);
            Save = new Command(async () =>
            {
                if (IsBusy) return;
                IsBusy = true;
                await SaveAsync();
                IsBusy = false;
            }, () => UnsavedChanges > 0);
        }

        public async Task SaveAsync()
        {
            using (Stream fs = new FileStream(Path.Combine(PCLSourceManager.Data.FullName, Context.Config.AbilityScores), FileMode.OpenOrCreate))
            {
                await Task.Run(() => AbilityScores.Serializer.Serialize(fs, Model)).ConfigureAwait(false);
                fs.SetLength(fs.Position);
            }
            UnsavedChanges = 0;
            Device.BeginInvokeOnMainThread(() =>
            {
                Save.ChangeCanExecute();
            });
        }

        public List<int> PointBuyCost { get => Model.PointBuyCost; }
        public List<string> Arrays { get => Model.Arrays; }
        public int DefaultMax { get => Model.DefaultMax; set { if (value == DefaultMax) return; MakeHistory("DefaultMax"); Model.DefaultMax = value; OnPropertyChanged("DefaultMax"); } }
        public int PointBuyPoints { get => Model.PointBuyPoints; set { if (value == PointBuyPoints) return; MakeHistory("PointBuyPoints"); Model.PointBuyPoints = value; OnPropertyChanged("PointBuyPoints"); } }
        public int PointBuyMinScore { get => Model.PointBuyMinScore; set { if (value == PointBuyMinScore) return; MakeHistory("PointBuyMinScore"); Model.PointBuyMinScore = value; OnPropertyChanged("PointBuyMinScore"); } }
        public int PointBuyMaxScore { get => Model.PointBuyMaxScore; set { if (value == PointBuyMaxScore) return; MakeHistory("PointBuyMaxScore"); Model.PointBuyMaxScore = value; OnPropertyChanged("PointBuyMaxScore"); } }
    }
}
