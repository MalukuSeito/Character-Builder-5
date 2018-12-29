using CB_5e.Services;
using CB_5e.Views;
using Character_Builder;
using OGL;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character
{
    public class PlayerPDFViewModel : SubModel
    {
        private const string NONE = "- NONE -";
        private const string CUSTOM = "- Custom -";

        private string preset = CUSTOM;
        private string sheet = NONE;
        private string spell = NONE;
        private string log = NONE;
        private string spellbook = NONE;
        private string action = NONE;
        private string monster = NONE;
        public string Preset
        {
            get => preset ?? CUSTOM;
            set
            {
                SetProperty(ref preset, value == CUSTOM ? null : value, onChanged: () =>
                {
                    if (preset == null)
                    {
                        if (Application.Current.Properties.TryGetValue("CustomSheet", out object CustomSheet)) sheet = CustomSheet as string;
                        else sheet = null;
                        if (Application.Current.Properties.TryGetValue("CustomSpellSheet", out object CustomSpellSheet)) spell = CustomSpellSheet as string;
                        else spell = null;
                        if (Application.Current.Properties.TryGetValue("CustomLogSheet", out object CustomLogSheet)) log = CustomLogSheet as string;
                        else log = null;
                        if (Application.Current.Properties.TryGetValue("CustomSpellbook", out object CustomSpellbook)) spellbook = CustomSpellbook as string;
                        else spellbook = null;
                        if (Application.Current.Properties.TryGetValue("CustomActionsSheet", out object CustomActionsSheet)) action = CustomActionsSheet as string;
                        else action = null;
                        if (Application.Current.Properties.TryGetValue("CustomMonsterSheet", out object CustomMonsterSheet)) monster = CustomMonsterSheet as string;
                        else monster = null;
                    }
                    else if(pdfs.ContainsKey(preset))
                    {
                        PDF p = pdfs[preset];
                        sheet = p.SheetName ?? (p.File != null && p.File != "" ? Path.GetFileName(p.File) : null);
                        spell = p.SpellName ?? (p.SpellFile != null && p.SpellFile != "" ? Path.GetFileName(p.SpellFile) : null);
                        log = p.LogName ?? (p.LogFile != null && p.LogFile != "" ? Path.GetFileName(p.LogFile) : null);
                        spellbook = p.SpellbookName ?? (p.SpellbookFile != null && p.SpellbookFile != "" ? Path.GetFileName(p.SpellbookFile) : null);
                        action = p.ActionsName ?? (p.ActionsFile != null && p.ActionsFile != "" ? Path.GetFileName(p.ActionsFile) : null);
                        monster = p.MonstersName ?? (p.MonstersFile != null && p.MonstersFile != "" ? Path.GetFileName(p.MonstersFile) : null);
                    }
                    OnPropertyChanged("Spell");
                    OnPropertyChanged("Sheet");
                    OnPropertyChanged("Log");
                    OnPropertyChanged("Spellbook");
                    OnPropertyChanged("Action");
                    OnPropertyChanged("Monster");
                    OnPropertyChanged("SheetPreview");
                    OnPropertyChanged("SpellPreview");
                    OnPropertyChanged("LogPreview");
                    OnPropertyChanged("SpellbookPreview");
                    OnPropertyChanged("ActionsPreview");
                    OnPropertyChanged("MonsterPreview");
                    OnPropertyChanged("MagicBook");
                    OnPropertyChanged("MagicFeaturesEnabled");
                    OnPropertyChanged("MagicFeatures");
                    OnPropertyChanged("ActionFeatures");
                    OnPropertyChanged("ActionFeaturesEnabled");
                    OnPropertyChanged("ButtonEnabled");
                });
            }
        }
        
        public string Spell { get => spell ?? NONE; set => SetProperty(ref spell, value == NONE ? null : value, onChanged: () => { OnPropertyChanged("SpellPreview"); preset = null; OnPropertyChanged("Preset"); }); }
        public string Sheet { get => sheet ?? NONE; set => SetProperty(ref sheet, value == NONE ? null : value, onChanged: () => { OnPropertyChanged("SheetPreview"); preset = null; OnPropertyChanged("Preset"); OnPropertyChanged("ButtonEnabled"); }); }
        public string Log { get => log ?? NONE; set => SetProperty(ref log, value == NONE ? null : value, onChanged: () => { OnPropertyChanged("LogPreview"); preset = null; OnPropertyChanged("Preset"); }); }
        public string Spellbook { get => spellbook ?? NONE; set => SetProperty(ref spellbook, value == NONE ? null : value, onChanged: () => { OnPropertyChanged("SpellbookPreview"); preset = null; OnPropertyChanged("Preset"); OnPropertyChanged("MagicBook"); OnPropertyChanged("MagicFeaturesEnabled"); OnPropertyChanged("MagicFeatures"); }); }
        public string Action { get => action ?? NONE; set => SetProperty(ref action, value == NONE ? null : value, onChanged: () => { OnPropertyChanged("ActionsPreview"); preset = null; OnPropertyChanged("Preset"); OnPropertyChanged("ActionFeatures"); OnPropertyChanged("ActionFeaturesEnabled"); }); }
        public string Monster { get => monster ?? NONE; set => SetProperty(ref monster, value == NONE ? null : value, onChanged: () => { OnPropertyChanged("MonsterPreview"); preset = null; OnPropertyChanged("Preset"); }); }

        public ImageSource SheetPreview => sheet != null && charsheets.ContainsKey(sheet) ? charsheets[sheet].SheetPreview.ToSource() : ImageSource.FromResource("CB_5e.images.1e233a34-374f-4348-a588-793e0bc793b5.png");
        public ImageSource SpellPreview => spell != null && spellsheets.ContainsKey(spell) ? spellsheets[spell].SpellPreview.ToSource() : ImageSource.FromResource("CB_5e.images.1e233a34-374f-4348-a588-793e0bc793b5.png");
        public ImageSource LogPreview => log != null && logsheets.ContainsKey(log) ? logsheets[log].LogPreview.ToSource() : ImageSource.FromResource("CB_5e.images.1e233a34-374f-4348-a588-793e0bc793b5.png");
        public ImageSource SpellbookPreview => spellbook != null && spellbooks.ContainsKey(spellbook) ? spellbooks[spellbook].SpellbookPreview.ToSource() : ImageSource.FromResource("CB_5e.images.1e233a34-374f-4348-a588-793e0bc793b5.png");
        public ImageSource ActionsPreview => action != null && actionsheets.ContainsKey(action) ? actionsheets[action].ActionsPreview.ToSource() : ImageSource.FromResource("CB_5e.images.1e233a34-374f-4348-a588-793e0bc793b5.png");
        public ImageSource MonsterPreview => monster != null && monstersheets.ContainsKey(monster) ? monstersheets[monster].MonstersPreview.ToSource() : ImageSource.FromResource("CB_5e.images.1e233a34-374f-4348-a588-793e0bc793b5.png");

        private bool? resources;
        private bool? forms;
        private bool? duplex;
        private bool? duplexWhite;
        private bool? swap;
        private bool? magicFeatures;
        private bool? actionFeatures;
        private bool? countMagic;
        private bool? magicBook;
        private int? acpFormat;

        public bool Resources { get => resources ?? Default("PDFUsedResources", false); set => SetProperty(ref resources, value); }
        public bool Forms { get => forms ?? Default("PDFPreserveForms", false); set => SetProperty(ref forms, value); }
        public bool Duplex { get => duplex ?? Default("PDFDuplex", false); set => SetProperty(ref duplex, value); }
        public bool DuplexWhite {
            get => duplexWhite ?? Default("PDFDuplexWhite", false);
            set { if (Duplex) SetProperty(ref duplexWhite, value); }
        }
        public bool Swap { get => swap ?? Default("PDFSwapScoreMod", true); set => SetProperty(ref swap, value); }
        public bool MagicFeatures
        {
            get => !MagicFeaturesEnabled ? true : magicFeatures ?? Default("PDFSheetAttunementOnUse", false);
            set { if (MagicFeaturesEnabled) SetProperty(ref magicFeatures, value, onChanged: () => { OnPropertyChanged("MagicBookEnabled"); OnPropertyChanged("MagicBook"); }); }
        }
        public bool ActionFeatures
        {
            get => ActionFeaturesEnabled ? actionFeatures ?? Default("PDFSheetActions", false) : true;
            set { if (ActionFeaturesEnabled) SetProperty(ref actionFeatures, value); }
        }
        public bool CountMagic { get => countMagic ?? Default("PDFLogMagicItems", false); set => SetProperty(ref countMagic, value); }
        public bool MagicBook
        {
            get => !MagicBookEnabled ? spellbook != null : magicBook ?? Default("PDFSpellbookMagicItems", false);
            set { if (MagicBookEnabled) SetProperty(ref magicBook, value); }
        }
        public int ACPFormat { get => acpFormat ?? Default("PDFACPFormat", 3); set => SetProperty(ref acpFormat, value >= 0 ? value: 0); }

        public List<string> ACPFormats => PDFBase.APFormats.Select(s => PDF.APFormat(Context, s, 14) + " + 4 AP = " + PDF.APFormat(Context, s, 18)).ToList();

        public bool MagicFeaturesEnabled => spellbook != null;
        public bool ActionFeaturesEnabled => action != null;
        public bool MagicBookEnabled => MagicFeaturesEnabled && MagicFeatures;
        public bool ButtonEnabled => sheet != null;

        public List<string> Presets => new string[] { CUSTOM }.Concat(pdfs.Keys.OrderBy(s => s)).ToList();
        public List<string> SpellSheets => new string[] { NONE }.Concat(spellsheets.Keys.OrderBy(s => s)).ToList();
        public List<string> Sheets => new string[] { NONE }.Concat(charsheets.Keys.OrderBy(s => s)).ToList();
        public List<string> LogSheets => new string[] { NONE }.Concat(logsheets.Keys.OrderBy(s => s)).ToList();
        public List<string> Spellbooks => new string[] { NONE }.Concat(spellbooks.Keys.OrderBy(s => s)).ToList();
        public List<string> ActionSheets => new string[] { NONE }.Concat(actionsheets.Keys.OrderBy(s => s)).ToList();
        public List<string> MonsterSheets => new string[] { NONE }.Concat(monstersheets.Keys.OrderBy(s => s)).ToList();

        private Dictionary<string, PDF> pdfs = new Dictionary<string, PDF>();
        private Dictionary<string, PDF> charsheets = new Dictionary<string, PDF>();
        private Dictionary<string, PDF> spellsheets = new Dictionary<string, PDF>();
        private Dictionary<string, PDF> logsheets = new Dictionary<string, PDF>();
        private Dictionary<string, PDF> spellbooks = new Dictionary<string, PDF>();
        private Dictionary<string, PDF> actionsheets = new Dictionary<string, PDF>();
        private Dictionary<string, PDF> monstersheets = new Dictionary<string, PDF>();


        public PlayerPDFViewModel(PlayerModel parent) : base(parent, "PDF Export")
        {
            Image = ImageSource.FromResource("CB_5e.images.export.png");
        }


        public async static Task<PDF> Load(IFile file)
        {
            using (Stream s = await file.OpenAsync(PCLStorage.FileAccess.Read))
            {
                PDF p = (PDF)PDF.Serializer.Deserialize(s);
                p.File = PCLImport.MakeRelativeFile(p.File);
                p.SpellFile = PCLImport.MakeRelativeFile(p.SpellFile);
                p.LogFile = PCLImport.MakeRelativeFile(p.LogFile);
                p.SpellbookFile = PCLImport.MakeRelativeFile(p.SpellbookFile);
                p.ActionsFile = PCLImport.MakeRelativeFile(p.ActionsFile);
                p.ActionsFile2 = PCLImport.MakeRelativeFile(p.ActionsFile2);
                p.MonstersFile = PCLImport.MakeRelativeFile(p.MonstersFile);
                return p;
            }
        }

        public async Task Load()
        {
            foreach (String s in Context.Config.PDF)
            {
                try
                {
                    PDF p = await Load(await PCLSourceManager.Data.GetFileAsync(s));
                    if (p.Name != "" && p.Name != null && !"- Custom -".Equals(p.Name) && !"<Save New>".Equals(p.Name)) pdfs[p.Name] = p;
                    if (p.File != null && p.File != "" && (!charsheets.ContainsKey(p.SheetName ?? Path.GetFileName(p.File)) || charsheets[p.SheetName ?? Path.GetFileName(p.File)].SheetPreview == null)) charsheets[p.SheetName ?? Path.GetFileName(p.File)] = p;
                    if (p.SpellFile != null && p.SpellFile != "" && (!spellsheets.ContainsKey(p.SpellName ?? Path.GetFileName(p.SpellFile)) || spellsheets[p.SpellName ?? Path.GetFileName(p.SpellFile)].SpellPreview == null)) spellsheets[p.SpellName ?? Path.GetFileName(p.SpellFile)] = p;
                    if (p.LogFile != null && p.LogFile != "" && (!logsheets.ContainsKey(p.LogName ?? Path.GetFileName(p.LogFile)) || logsheets[p.LogName ?? Path.GetFileName(p.LogFile)].LogPreview == null)) logsheets[p.LogName ?? Path.GetFileName(p.LogFile)] = p;
                    if (p.SpellbookFile != null && p.SpellbookFile != "" && (!spellbooks.ContainsKey(p.SpellbookName ?? Path.GetFileName(p.SpellbookFile)) || spellbooks[p.SpellbookName ?? Path.GetFileName(p.SpellbookFile)].SpellbookPreview == null)) spellbooks[p.SpellbookName ?? Path.GetFileName(p.SpellbookFile)] = p;
                    if (p.ActionsFile != null && p.ActionsFile != "" && (!actionsheets.ContainsKey(p.ActionsName ?? Path.GetFileName(p.ActionsFile)) || actionsheets[p.ActionsName ?? Path.GetFileName(p.ActionsFile)].ActionsPreview == null)) actionsheets[p.ActionsName ?? Path.GetFileName(p.ActionsFile)] = p;
                    if (p.MonstersFile != null && p.MonstersFile != "" && (!monstersheets.ContainsKey(p.MonstersName ?? Path.GetFileName(p.MonstersFile)) || monstersheets[p.MonstersName ?? Path.GetFileName(p.MonstersFile)].MonstersPreview == null)) monstersheets[p.MonstersName ?? Path.GetFileName(p.MonstersFile)] = p;
                }
                catch (Exception e)
                {
                    ConfigManager.LogError(e);
                }
                OnPropertyChanged("Presets");
                OnPropertyChanged("SpellSheets");
                OnPropertyChanged("Sheets");
                OnPropertyChanged("LogSheets");
                OnPropertyChanged("Spellbooks");
                OnPropertyChanged("ActionSheets");
                OnPropertyChanged("MonsterSheets");
            }
            preset = CUSTOM;
            if (Application.Current.Properties.TryGetValue("LastPDFPreset", out object val))
            {
                if (val is string s && (s == CUSTOM || pdfs.ContainsKey(s))) Preset = s;
                else Preset = Presets.Skip(1).FirstOrDefault();
            }
            else Preset = Presets.Skip(1).FirstOrDefault();
        }

        private  bool Default(string prop, bool def)
        {
            Application.Current.Properties.TryGetValue(prop, out object val);
            return val as bool? ?? def;
        }
        private int Default(string prop, int def)
        {
            Application.Current.Properties.TryGetValue(prop, out object val);
            return val as int? ?? def;
        }

        public Task SaveConfig()
        {
            Application.Current.Properties["LastPDFPreset"] = Preset ?? "- Custom -";
            if (preset == null)
            {
                Application.Current.Properties["CustomSheet"] = sheet;
                Application.Current.Properties["CustomSpellSheet"] = spell;
                Application.Current.Properties["CustomLogSheet"] = log;
                Application.Current.Properties["CustomSpellbook"] = spellbook;
                Application.Current.Properties["CustomActionsSheet"] = action;
                Application.Current.Properties["CustomMonsterSheet"] = monster;
            }

            Application.Current.Properties["PDFUsedResources"] = resources;
            Application.Current.Properties["PDFPreserveForms"] = forms;
            Application.Current.Properties["PDFDuplex"] = duplex;
            if (Duplex) Application.Current.Properties["PDFDuplexWhite"] = duplexWhite;
            Application.Current.Properties["PDFACPFormat"] = acpFormat;
            Application.Current.Properties["PDFSwapScoreMod"] = swap;
            if (MagicFeaturesEnabled) Application.Current.Properties["PDFSheetAttunementOnUse"] = magicFeatures;
            if (ActionFeaturesEnabled) Application.Current.Properties["PDFSheetActions"] = actionFeatures;
            Application.Current.Properties["PDFLogMagicItems"] = countMagic;
            if (MagicBookEnabled) Application.Current.Properties["PDFSpellbookMagicItems"] = magicBook;
            return Application.Current.SavePropertiesAsync();
        }

        public PDF Exporter => preset == null ? MakeCustom() : pdfs[preset];

        private PDF MakeCustom()
        {
            PDF res = new PDF();
            res.Name = null;
            if (sheet != null && charsheets.ContainsKey(sheet))
            {
                PDF other = charsheets[sheet];
                res.SheetName = other.SheetName;
                res.SheetPreview = other.SheetPreview;
                res.File = other.File;
                res.Fields = other.Fields;
            }
            if (spell != null && spellsheets.ContainsKey(spell))
            {
                PDF other = spellsheets[spell];
                res.SpellName = other.SpellName;
                res.SpellPreview = other.SpellPreview;
                res.SpellFile = other.SpellFile;
                res.SpellFields = other.SpellFields;
            }
            if (log != null && logsheets.ContainsKey(log))
            {
                PDF other = logsheets[log];
                res.LogName = other.LogName;
                res.LogPreview = other.LogPreview;
                res.LogFile = other.LogFile;
                res.LogFields = other.LogFields;
            }
            if (action != null && actionsheets.ContainsKey(action))
            {
                PDF other = actionsheets[action];
                res.ActionsName = other.ActionsName;
                res.ActionsPreview = other.ActionsPreview;
                res.ActionsFile = other.ActionsFile;
                res.ActionsFile2 = other.ActionsFile2;
                res.ActionsFields = other.ActionsFields;
                res.ActionsFields2 = other.ActionsFields2;
            }
            if (spellbook != null && spellbooks.ContainsKey(spellbook))
            {
                PDF other = spellbooks[spellbook];
                res.SpellbookName = other.SpellbookName;
                res.SpellbookPreview = other.SpellbookPreview;
                res.SpellbookFile = other.SpellbookFile;
                res.SpellbookFields = other.SpellbookFields;
            }
            if (monster != null && monstersheets.ContainsKey(monster))
            {
                PDF other = monstersheets[monster];
                res.MonstersName = other.MonstersName;
                res.MonstersPreview = other.MonstersPreview;
                res.MonstersFile = other.MonstersFile;
                res.MonsterFields = other.MonsterFields;
            }
            return res;
        }

        public void Set(IPDFService pdf)
        {
            pdf.IncludeResources = Resources;
            pdf.PreserveEdit = Forms;
            pdf.Duplex = Duplex;
            pdf.DuplexWhite = DuplexWhite;
            pdf.APFormat = PDFBase.APFormats[ACPFormat >= 0 ? ACPFormat : 0];
            pdf.AutoExcludeActions = !ActionFeatures;
            pdf.ForceAttunedAndOnUseItemsOnSheet = MagicFeatures;
            pdf.ForceAttunedItemsInSpellbook = MagicBook;
            pdf.IgnoreMagicItems = !CountMagic;
            pdf.IncludeActions = action != null;
            pdf.IncludeLog = log != null;
            pdf.IncludeMonsters = monster != null;
            pdf.IncludeSpellbook = spellbook != null;
            pdf.SwapScoreAndMod = Swap;
        }

        private string name;
        public string NewName { get => name; set => SetProperty(ref name, value); }

        public async Task SaveCustom()
        {
            PDF p = MakeCustom();
            if (NewName == null) return;
            string name = NewName;
            if (name != null && name != "" && !name.Equals("Config", StringComparison.OrdinalIgnoreCase) && !name.Equals("Levels", StringComparison.OrdinalIgnoreCase) && !name.Equals("AbilityScores", StringComparison.OrdinalIgnoreCase))
            {
                p.Name = name;
                string iname = string.Join("_", name.Split(ConfigManager.InvalidChars));
                IFolder f = PCLSourceManager.Data;

                using (Stream fs = await (await f.CreateFileAsync(iname + ".xml", CreationCollisionOption.OpenIfExists)).OpenAsync(FileAccess.ReadAndWrite))
                {
                    PDF.Serializer.Serialize(fs, p);
                    fs.SetLength(fs.Position);
                }
                if (!Context.Config.PDF.Contains(iname + ".xml", StringComparer.OrdinalIgnoreCase))
                {
                    Context.Config.PDF.Add(iname + ".xml");
                    Context.Config.PDFExporters.Add(iname + ".xml");
                    IFile file = await PCLSourceManager.Data.GetFileAsync("Config.xml").ConfigureAwait(false);
                    using (Stream fs = await file.OpenAsync(FileAccess.ReadAndWrite).ConfigureAwait(false))
                    {
                        ConfigManager.Serializer.Serialize(fs, Context.Config);
                        fs.SetLength(fs.Position);
                    }
                }
                pdfs[p.Name] = p;
                preset = p.Name;
                OnPropertyChanged("Presets");
                OnPropertyChanged("Preset");
                NewName = null;
            }
        }
    }
}
