using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL;
using Xamarin.Forms;
using CB_5e.Services;
using PCLStorage;
using System.IO;

namespace CB_5e.ViewModels.Modify
{
    public class LevelEditModel : BaseViewModel, IEditModel
    {
        public Command Undo { get; set; }

        public Command Redo { get; set; }

        public bool TrackChanges { get; set; }

        public OGLContext Context { get; set; }

        public int UnsavedChanges { get; set; }

        public INavigation Navigation { get; set; }

        public Level Model {
            get {
                return Context.Levels;
            }
            set
            {
                Context.Levels = value;
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

        public LinkedList<Level> UndoBuffer = new LinkedList<Level>();
        public LinkedList<Level> RedoBuffer = new LinkedList<Level>();
        private string lastid = null;

        public Command Save { get; private set; }
        public void FireModelChanged() => ModelChanged?.Invoke(this, Model);
        public event EventHandler<Level> ModelChanged;

        public LevelEditModel (OGLContext context) {
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
            IFile file = await PCLSourceManager.Data.GetFileAsync(Context.Config.Levels).ConfigureAwait(false);
            using (Stream fs = await file.OpenAsync(FileAccess.ReadAndWrite).ConfigureAwait(false)) {
                Level.Serializer.Serialize(fs, Model);
                fs.SetLength(fs.Position);
            }
            UnsavedChanges = 0;
            Device.BeginInvokeOnMainThread(() =>
            {
                Save.ChangeCanExecute();
            });
        }

        public List<int> Experience { get => Model.Experience; }
        public List<int> Proficiency { get => Model.Proficiency; }
    }
}
