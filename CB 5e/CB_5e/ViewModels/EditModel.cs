using CB_5e.Services;
using CB_5e.Views;
using OGL;
using OGL.Common;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public abstract class EditModel<T> : BaseViewModel where T: IOGLElement<T>
    {
        public LinkedList<T> UndoBuffer = new LinkedList<T>();
        public LinkedList<T> RedoBuffer = new LinkedList<T>();
        private string lastid = null;
        public int UnsavedChanges;
        private static int MaxBuffer = 100;
        private bool overwrite = false;
        public bool Overwrite
        {
            get => overwrite;
            set =>SetProperty(ref overwrite, value);
        }

        public T Model { get; private set; }
        public Command Undo { get; private set; }
        public Command Redo { get; private set; }
        public INavigation Navigation { get; set; }
        public object Program { get; private set; }
        public Command Save { get; private set; }
        public Command Preview { get; private set; }
        public OGLContext Context { get; private set; }
        public Color Accent { get => Color.Accent; }
        public bool TrackChanges { get; set; }

        public EditModel(T obj, OGLContext context)
        {
            Context = context;
            Model = obj;
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
                    ModelChanged();
                    OnPropertyChanged();
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
                    ModelChanged();
                    OnPropertyChanged();
                }
            },
            () => RedoBuffer.Count > 0);
            Save = new Command(async () =>
            {
                if (IsBusy) return;
                IsBusy = true;
                Overwrite = !await SaveAsync(Overwrite);
                IsBusy = false;
            }, () => Model.Name != null && Model.Name != "" && UnsavedChanges > 0);
            Preview = new Command(async () =>
            {
                await Navigation.PushAsync(InfoPage.Show(Model));
            });
        }

        public async Task<bool> SaveAsync(bool overwrite)
        {
            string iname = Model.Name.Replace(ConfigManager.SourceSeperator, '-') + ".xml";
            IFolder target = await PCLSourceManager.GetFolder(PCLSourceManager.Data, GetPath(Model), CreationCollisionOption.OpenIfExists).ConfigureAwait(false);
            ExistenceCheckResult res = await target.CheckExistsAsync(iname).ConfigureAwait(false);
            if (!overwrite && res == ExistenceCheckResult.FileExists && (Model.FileName == null || !Model.FileName.Equals(PortablePath.Combine(target.Path, iname))))
            {
                return false;
            }
            else
            {
                IFile file = await target.CreateFileAsync(iname, CreationCollisionOption.OpenIfExists).ConfigureAwait(false);
                using (Stream t = await file.OpenAsync(FileAccess.ReadAndWrite).ConfigureAwait(false))
                {
                    Model.Write(t);
                    t.SetLength(t.Position);
                }
                UnsavedChanges = 0;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Save.ChangeCanExecute();
                });
                Model.FileName = file.Path;
                return true;
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
                if (UndoBuffer.Count > MaxBuffer) UndoBuffer.RemoveFirst();
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
        public abstract void ModelChanged();
        public abstract string GetPath(T obj);

    }
}
