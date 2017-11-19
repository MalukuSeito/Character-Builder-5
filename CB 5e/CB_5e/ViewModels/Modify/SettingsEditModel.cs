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
using OGL.Features;
using System.Globalization;

namespace CB_5e.ViewModels.Modify
{
    public class SettingsEditModel : BaseViewModel, IEditModel
    {
        public Command Undo { get; set; }

        public Command Redo { get; set; }

        public bool TrackChanges { get; set; }

        public OGLContext Context { get; set; }

        public int UnsavedChanges { get; set; }

        public INavigation Navigation { get; set; }

        public ConfigManager Model {
            get {
                return Context.Config;
            }
            set
            {
                Context.Config = value;
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

        public LinkedList<ConfigManager> UndoBuffer = new LinkedList<ConfigManager>();
        public LinkedList<ConfigManager> RedoBuffer = new LinkedList<ConfigManager>();
        private string lastid = null;

        public Command Save { get; private set; }
        public void FireModelChanged() => ModelChanged?.Invoke(this, Model);
        public event EventHandler<ConfigManager> ModelChanged;

        public SettingsEditModel(OGLContext context) {
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
            IFile file = await PCLSourceManager.Data.GetFileAsync("Config.xml").ConfigureAwait(false);
            using (Stream fs = await file.OpenAsync(FileAccess.ReadAndWrite).ConfigureAwait(false)) {
                ConfigManager.Serializer.Serialize(fs, Model);
                fs.SetLength(fs.Position);
            }
            UnsavedChanges = 0;
            Device.BeginInvokeOnMainThread(() =>
            {
                Save.ChangeCanExecute();
            });
        }

        public List<string> PDFExporters { get => Model.PDFExporters; }
        public List<string> EqiupmentSlots { get => Model.Slots; }
        public List<Feature> CommonFeatures { get => Model.FeaturesForAll; }
        public List<Feature> MulticlassingFeatures { get => Model.FeaturesForMulticlassing; }
        public string DefaultSource { get => Model.DefaultSource; set { if (value == DefaultSource) return; MakeHistory("DefaultSource"); Model.DefaultSource = value; OnPropertyChanged("DefaultSource"); } }
        public string WeightOfACoin
        {
            get => Model.WeightOfACoin.ToString();
            set
            {
                double parsed = 0;
                if (value == "" || value == "-" || double.TryParse(value, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, null, out parsed))
                {
                    if (Model.WeightOfACoin == parsed) return;
                    MakeHistory("WeightOfACoin");
                    Model.WeightOfACoin = parsed;
                    if (value != "" && value != "-") OnPropertyChanged("WeightOfACoin");
                }
                if (value != "" && value != "-") OnPropertyChanged("WeightOfACoin");
            }
        }
    }
}
