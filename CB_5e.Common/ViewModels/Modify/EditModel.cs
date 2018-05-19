﻿using CB_5e.Services;
using CB_5e.Views;
using OGL;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Modify
{
    public interface IEditModel: INotifyPropertyChanged
    {
        Command Undo { get; }
        Command Redo { get; }
        Command Save { get; }
        bool TrackChanges { get; set; }
        OGLContext Context { get; }
        void MakeHistory(string id = "");
        int UnsavedChanges { get; }
        Task<bool> SaveAsync(bool overwrite);
        INavigation Navigation { get; set; }
    }

    public abstract class EditModel<T> : BaseViewModel, IEditModel where T: IOGLElement<T>
    {
        public LinkedList<T> UndoBuffer = new LinkedList<T>();
        public LinkedList<T> RedoBuffer = new LinkedList<T>();
        private string lastid = null;
        public int UnsavedChanges { get; set; }
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
        public Command Save { get; private set; }
        public Command Preview { get; private set; }
        public OGLContext Context { get; private set; }
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
            string target = Path.Combine(PCLSourceManager.Data.FullName, GetPath(Model));
            Directory.CreateDirectory(target);
            string f = Path.Combine(target, iname);
            if (!overwrite && File.Exists(f) && (Model.FileName == null || !Model.FileName.Equals(f)))
            {
                return false;
            }
            else
            {
                using (Stream t = new FileStream(f, FileMode.OpenOrCreate))
                {
                    await Task.Run(() => Model.Write(t)).ConfigureAwait(false);
                    t.SetLength(t.Position);
                }
                UnsavedChanges = 0;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Save.ChangeCanExecute();
                });
                Model.FileName = f;
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
        public abstract string GetPath(T obj);
        public void FireModelChanged() => ModelChanged?.Invoke(this, Model);
        public event EventHandler<T> ModelChanged;

    }
}
