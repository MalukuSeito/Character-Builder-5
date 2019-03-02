using Character_Builder;
using Character_Builder_Forms;
using Microsoft.VisualBasic;
using OGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using OGL.Base;
using OGL.Items;
using OGL.Spells;
using OGL.Features;

namespace Character_Builder_5
{


    public partial class Export : Form
    {

        [DllImport("shlwapi.dll", EntryPoint = "PathRelativePathTo")]
        private static extern bool PathRelativePathTo(StringBuilder lpszDst,
              string from, UInt32 attrFrom,
              string to, UInt32 attrTo);

        public static string GetRelativePath(string from, string to)
        {
            StringBuilder builder = new StringBuilder(1024);
            bool result = PathRelativePathTo(builder, from, 0, to, 0);
            return builder.ToString();
        }

        private const string NONE = "- NONE -";
        private const string CUSTOM = "- Custom -";
        private const string SAVE = "<Save New>";

        private Dictionary<string, PDF> charsheets = new Dictionary<string, PDF>();
        private Dictionary<string, PDF> spellsheets = new Dictionary<string, PDF>();
        private Dictionary<string, PDF> logsheets = new Dictionary<string, PDF>();
        private Dictionary<string, PDF> spellbooks = new Dictionary<string, PDF>();
        private Dictionary<string, PDF> actionsheets = new Dictionary<string, PDF>();
        private Dictionary<string, PDF> monstersheets = new Dictionary<string, PDF>();
        private Dictionary<string, PDF> pdfs = new Dictionary<string, PDF>();

        private string lastfile;
        private bool internalEvent = false; 

        public Export(string lastfile)
        {
            this.lastfile = lastfile;
            InitializeComponent();
            foreach (String s in Program.Context.Config.PDFExporters)
            {
                try
                {
                    PDF p = PlayerExtensions.Load(s);
                    if (p.Name != "" && p.Name != null && !CUSTOM.Equals(p.Name) && !SAVE.Equals(p.Name)) pdfs[p.Name] = p;
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
            }
            Preset.Items.Add(CUSTOM);
            foreach (string s in pdfs.Keys.OrderBy(s => s)) Preset.Items.Add(s);
            Preset.Items.Add(SAVE);
            SpellBox.Items.Add(NONE);
            LogBox.Items.Add(NONE);
            SpellbookBox.Items.Add(NONE);
            ActionsBox.Items.Add(NONE);
            MonsterBox.Items.Add(NONE);
            SheetBox.Items.AddRange(charsheets.Keys.OrderBy(s => s).ToArray());
            SpellBox.Items.AddRange(spellsheets.Keys.OrderBy(s => s).ToArray());
            LogBox.Items.AddRange(logsheets.Keys.OrderBy(s => s).ToArray());
            SpellbookBox.Items.AddRange(spellbooks.Keys.OrderBy(s => s).ToArray());
            ActionsBox.Items.AddRange(actionsheets.Keys.OrderBy(s => s).ToArray());
            MonsterBox.Items.AddRange(monstersheets.Keys.OrderBy(s => s).ToArray());

            foreach (string s in PDFBase.APFormats)
            {
                ACPBox.Items.Add(PDF.APFormat(Program.Context, s, 14) + " + 4 AP = " + PDF.APFormat(Program.Context, s, 18));
            }
            string last = Properties.Settings.Default.LastPDFPreset;
            if (pdfs.ContainsKey(last) || CUSTOM.Equals(last)) Preset.SelectedItem = last;
            else button1.Enabled = false;
            ResourcesBox.Checked = Properties.Settings.Default.PDFUsedResources;
            PreserveBox.Checked = Properties.Settings.Default.PDFPreserveForms;
            DuplexBox.Checked = Properties.Settings.Default.PDFDuplex;
            DuplexWhiteBox.Checked = Properties.Settings.Default.PDFDuplexWhite;
            ACPBox.SelectedIndex = Properties.Settings.Default.PDFACPFormat;
            SwapScoreModBox.Checked = Properties.Settings.Default.PDFSwapScoreMod;
            IgnoreMagicItemsBox.Checked = !Properties.Settings.Default.PDFLogMagicItems;
            MundaneItemBox.Checked = Properties.Settings.Default.PDFSpellbookMundaneItems;
            BonusSpellsResources.SelectedIndex = Properties.Settings.Default.PDFBonusSpellResources;
            EquipmentKeywords.Checked = Properties.Settings.Default.PDFEquipmentKeywords;
            EquipmentStats.Checked = Properties.Settings.Default.PDFEquipmentStats;
            FeatureTitle.Checked = Properties.Settings.Default.PDFFeatureTitlesOnly;

            BuildAttacks();

        }

        private void BuildAttacks()
        {
            List<Possession> equip = new List<Possession>();
            foreach (Possession pos in Program.Context.Player.GetItemsAndPossessions())
            {
                if (pos.Count > 0 && pos.BaseItem != null && pos.BaseItem != "")
                {
                    Item i = Program.Context.GetItem(pos.BaseItem, null);
                    if (pos.Equipped != EquipSlot.None || i is Weapon || i is Armor || i is Shield) equip.Add(pos);
                }
            }
            equip.Sort(delegate (Possession t1, Possession t2)
            {
                if (t1.Hightlight && !t2.Hightlight) return -1;
                else if (t2.Hightlight && !t1.Hightlight) return 1;
                else
                {
                    if (!string.Equals(t1.Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase) && string.Equals(t2.Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase)) return -1;
                    else if (!string.Equals(t2.Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase) && string.Equals(t1.Equipped, EquipSlot.None, StringComparison.OrdinalIgnoreCase)) return 1;
                    else return (t1.ToString().CompareTo(t2.ToString()));
                }

            });

            List<KeyValuePair<string, AttackInfo>> attackinfos = new List<KeyValuePair<string, AttackInfo>>();
            List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>(from f in Program.Context.Player.GetFeatures() where f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID != "MULTICLASS" select (SpellcastingFeature)f);
            List<KeyValuePair<string, AttackInfo>> addattackinfos = new List<KeyValuePair<string, AttackInfo>>();
            foreach (SpellcastingFeature scf in spellcasts)
            {
                Spellcasting sc = Program.Context.Player.GetSpellcasting(scf.SpellcastingID);
                foreach (Spell s in sc.GetLearned(Program.Context.Player, Program.Context))
                {
                    if (sc.Highlight != null && sc.Highlight != "" && s.Name.ToLowerInvariant() == sc.Highlight.ToLowerInvariant())
                    {
                        AttackInfo ai = Program.Context.Player.GetAttack(s, scf.SpellcastingAbility);
                        if (ai != null && ai.Damage != null && ai.Damage != "") attackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                    }
                    else
                    {
                        AttackInfo ai = Program.Context.Player.GetAttack(s, scf.SpellcastingAbility);
                        if (ai != null && ai.Damage != null && ai.Damage != "") addattackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                    }
                }
                //No prepared Cantrips, so whatever
                //foreach (Spell s in sc.GetPrepared(Program.Context.Player, Program.Context))
                //{
                //    if (sc.Highlight != null && sc.Highlight != "" && s.Name.ToLowerInvariant() == sc.Highlight.ToLowerInvariant())
                //    {
                //        AttackInfo ai = Program.Context.Player.GetAttack(s, scf.SpellcastingAbility);
                //        if (ai != null && ai.Damage != null && ai.Damage != "") attackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                //    }
                //    else
                //    {
                //        AttackInfo ai = Program.Context.Player.GetAttack(s, scf.SpellcastingAbility);
                //        if (ai != null && ai.Damage != null && ai.Damage != "") addattackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                //    }
                //}
                //foreach (Spell s in sc.GetAdditionalClassSpells(Program.Context.Player, Program.Context))
                //{
                //    if (sc.Highlight != null && sc.Highlight != "" && s.Name.ToLowerInvariant() == sc.Highlight.ToLowerInvariant())
                //    {
                //        AttackInfo ai = Program.Context.Player.GetAttack(s, scf.SpellcastingAbility);
                //        if (ai != null && ai.Damage != null && ai.Damage != "") attackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                //    }
                //    else
                //    {
                //        AttackInfo ai = Program.Context.Player.GetAttack(s, scf.SpellcastingAbility);
                //        if (ai != null && ai.Damage != null && ai.Damage != "") addattackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                //    }
                //}
            }
            foreach (Possession pos in equip)
            {
                AttackInfo ai = Program.Context.Player.GetAttack(pos);
                if (ai != null) attackinfos.Add(new KeyValuePair<string, AttackInfo>(pos.ToString(), ai));
            }
            foreach (ModifiedSpell s in Program.Context.Player.GetBonusSpells(false))
            {
                if (Utils.Matches(Program.Context, s, "Attack or Save", null))
                {
                    AttackInfo ai = Program.Context.Player.GetAttack(s, s.differentAbility);
                    if (ai != null && ai.Damage != null && ai.Damage != "") attackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                }
            }
            attackinfos.AddRange(addattackinfos);
            //attackinfos.OrderBy((a, b) =>
            //{
            //    int oa = Program.Context.Player.AttackOrder.FindIndex(s => StringComparer.OrdinalIgnoreCase.Equals(a.Key, s));
            //    int ob = Program.Context.Player.AttackOrder.FindIndex(s => StringComparer.OrdinalIgnoreCase.Equals(b.Key, s));
            //    if (oa < 0) oa = int.MaxValue;
            //    if (ob < 0) ob = int.MaxValue;
            //    return oa.CompareTo(ob);
            //});

            AttackOrder.Items.Clear();
            foreach (var a in attackinfos.OrderBy(a=> { int i = Program.Context.Player.AttackOrder.FindIndex(s => StringComparer.OrdinalIgnoreCase.Equals(a.Key, s)); return i < 0 ? int.MaxValue : i; })) AttackOrder.Items.Add(a);
        }

        private void SpellbookBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            internalEvent = true;
            Preset.SelectedItem = CUSTOM;
            internalEvent = false;
            if (SpellbookBox.Text == null)
            {
                AttunedSheetBox.Enabled = false;
                AttunedSheetBox.CheckState = CheckState.Indeterminate;
            } else
            {
                AttunedSheetBox.Enabled = true;
                AttunedSheetBox.CheckState = Properties.Settings.Default.PDFSheetAttunementOnUse ? CheckState.Checked : CheckState.Unchecked;
            }
            if (SpellbookBox.SelectedItem is string && spellbooks.ContainsKey(SpellbookBox.SelectedItem as string)) SpellbookPreview.Image = LoadImage(spellbooks[SpellbookBox.SelectedItem as string].SpellbookPreview);
            else SpellbookPreview.Image = null;
        }

        private void ActionsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            internalEvent = true;
            Preset.SelectedItem = CUSTOM;
            internalEvent = false;
            if (ActionsBox.Text == null)
            {
                IgnoreActionsBox.Enabled = false;
                IgnoreActionsBox.CheckState = CheckState.Indeterminate;
            }
            else
            {
                IgnoreActionsBox.Enabled = true;
                IgnoreActionsBox.CheckState = Properties.Settings.Default.PDFSheetActions ? CheckState.Unchecked : CheckState.Checked;
            }
            if (ActionsBox.SelectedItem is string && actionsheets.ContainsKey(ActionsBox.SelectedItem as string)) ActionsPreview.Image = LoadImage(actionsheets[ActionsBox.SelectedItem as string].ActionsPreview);
            else ActionsPreview.Image = null;
        }

        private void AttunedSheetBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AttunedSheetBox.CheckState == CheckState.Checked)
            {
                AttunementSpellbookBox.Enabled = true;
                AttunementSpellbookBox.CheckState = Properties.Settings.Default.PDFSpellbookMagicItems ? CheckState.Checked : CheckState.Unchecked;
            }
            else
            {
                AttunementSpellbookBox.Enabled = false;
                AttunementSpellbookBox.CheckState = CheckState.Indeterminate;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            DuplexWhiteBox.Enabled = DuplexBox.Checked;
        }

        private void Preset_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = Preset.SelectedItem != null;
            if (internalEvent) return;
            if (CUSTOM.Equals(Preset.SelectedItem))
            {
                if (charsheets.ContainsKey(Properties.Settings.Default.CustomSheet)) SheetBox.SelectedItem = Properties.Settings.Default.CustomSheet;
                else SheetBox.SelectedItem = NONE;
                if (spellsheets.ContainsKey(Properties.Settings.Default.CustomSpellSheet)) SpellBox.SelectedItem = Properties.Settings.Default.CustomSpellSheet;
                else SpellBox.SelectedItem = NONE;
                if (logsheets.ContainsKey(Properties.Settings.Default.CustomLogSheet)) LogBox.SelectedItem = Properties.Settings.Default.CustomLogSheet;
                else LogBox.SelectedItem = NONE;
                if (spellbooks.ContainsKey(Properties.Settings.Default.CustomSpellbook)) SpellbookBox.SelectedItem = Properties.Settings.Default.CustomSpellbook;
                else SpellbookBox.SelectedItem = NONE;
                if (actionsheets.ContainsKey(Properties.Settings.Default.CustomActionsSheet)) ActionsBox.SelectedItem = Properties.Settings.Default.CustomActionsSheet;
                else ActionsBox.SelectedItem = NONE;
                if (monstersheets.ContainsKey(Properties.Settings.Default.CustomMonsterSheet)) MonsterBox.SelectedItem = Properties.Settings.Default.CustomMonsterSheet;
                else MonsterBox.SelectedItem = NONE;
            }
            else if (SAVE.Equals(Preset.SelectedItem))
            {
                PDF p = MakeCustom();
                string name = Interaction.InputBox("Name:", "Save Preset");
                if (name != null && name != "" && !name.Equals("Config", StringComparison.InvariantCultureIgnoreCase) && !name.Equals("Levels", StringComparison.InvariantCultureIgnoreCase) && !name.Equals("AbilityScores", StringComparison.InvariantCultureIgnoreCase))
                {
                    p.Name = name;
                    string iname = string.Join("_", name.Split(ConfigManager.InvalidChars));
                    String path = Path.Combine(Application.StartupPath, iname + ".xml");
                    p.File = MakeRelative(p.File, path);
                    p.SpellbookFile = MakeRelative(p.SpellbookFile, path);
                    p.SpellFile = MakeRelative(p.SpellFile, path);
                    p.ActionsFile = MakeRelative(p.ActionsFile, path);
                    p.LogFile = MakeRelative(p.LogFile, path);
                    p.MonstersFile = MakeRelative(p.MonstersFile, path);
                    p.ActionsFile2 = MakeRelative(p.ActionsFile2, path);

                    using (FileStream fs = File.OpenWrite(path))
                    {
                        PDF.Serializer.Serialize(fs, p);
                    }
                    if (!Program.Context.Config.PDF.Contains(iname + ".xml", StringComparer.InvariantCultureIgnoreCase))
                    {
                        Program.Context.Config.PDF.Add(iname + ".xml");
                        Program.Context.Config.PDFExporters.Add(path);
                        Program.Context.Config.Save(Path.Combine(Application.StartupPath, "Config.xml"));
                        Preset.Items.Add(p.Name);
                    }
                    pdfs[p.Name] = p;
                    internalEvent = true;
                    Preset.SelectedItem = p.Name;
                    internalEvent = false;
                }
                else
                {
                    internalEvent = true;
                    Preset.SelectedItem = CUSTOM;
                    internalEvent = false;
                }
            } else
            {
                PDF p = pdfs[Preset.SelectedItem as string];
                SheetBox.SelectedItem = p.SheetName ?? (p.File != null && p.File != "" ? Path.GetFileName(p.File) : NONE);
                SpellBox.SelectedItem = p.SpellName ?? (p.SpellFile != null && p.SpellFile != "" ? Path.GetFileName(p.SpellFile) : NONE);
                LogBox.SelectedItem = p.LogName ?? (p.LogFile != null && p.LogFile != "" ? Path.GetFileName(p.LogFile) : NONE);
                SpellbookBox.SelectedItem = p.SpellbookName ?? (p.SpellbookFile != null && p.SpellbookFile != "" ? Path.GetFileName(p.SpellbookFile) : NONE);
                ActionsBox.SelectedItem = p.ActionsName ?? (p.ActionsFile != null && p.ActionsFile != "" ? Path.GetFileName(p.ActionsFile) : NONE);
                MonsterBox.SelectedItem = p.MonstersName ?? (p.MonstersFile != null && p.MonstersFile != "" ? Path.GetFileName(p.MonstersFile) : NONE);
                internalEvent = true;
                Preset.SelectedItem = p.Name;
                internalEvent = false;
            }
        }

        private string MakeRelative(string file, string path)
        {
            if (file.IsSubPathOf(Application.StartupPath))
            {
                return GetRelativePath(path, file).TrimStart('.', '\\');
            }
            return file;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (Preset.SelectedItem == null) return;
            Properties.Settings.Default.LastPDFPreset = Preset.SelectedItem.ToString();
            PDF exporter;
            if (CUSTOM.Equals(Preset.SelectedItem))
            {
                Properties.Settings.Default.CustomSheet = SheetBox.SelectedItem as string;
                Properties.Settings.Default.CustomSpellSheet = SpellBox.SelectedItem as string;
                Properties.Settings.Default.CustomLogSheet = LogBox.SelectedItem as string;
                Properties.Settings.Default.CustomSpellbook = SpellbookBox.SelectedItem as string;
                Properties.Settings.Default.CustomActionsSheet = ActionsBox.SelectedItem as string;
                Properties.Settings.Default.CustomMonsterSheet = MonsterBox.SelectedItem as string;
                exporter = MakeCustom();
            }
            else
            {
                exporter = pdfs[Preset.SelectedItem as string];
            }
            Properties.Settings.Default.PDFUsedResources = ResourcesBox.Checked;
            Properties.Settings.Default.PDFPreserveForms = PreserveBox.Checked;
            Properties.Settings.Default.PDFDuplex = DuplexBox.Checked;
            Properties.Settings.Default.PDFDuplexWhite = DuplexWhiteBox.Checked;
            Properties.Settings.Default.PDFACPFormat = ACPBox.SelectedIndex;
            Properties.Settings.Default.PDFSwapScoreMod = SwapScoreModBox.Checked;
            if (AttunedSheetBox.Enabled) Properties.Settings.Default.PDFSheetAttunementOnUse = AttunedSheetBox.Checked;
            if (IgnoreActionsBox.Enabled) Properties.Settings.Default.PDFSheetActions = !IgnoreActionsBox.Checked;
            Properties.Settings.Default.PDFLogMagicItems = !IgnoreMagicItemsBox.Checked;
            if (AttunementSpellbookBox.Enabled) Properties.Settings.Default.PDFSpellbookMagicItems = AttunementSpellbookBox.Checked;
            Properties.Settings.Default.PDFSpellbookMundaneItems = MundaneItemBox.Checked;
            Properties.Settings.Default.PDFBonusSpellResources = BonusSpellsResources.SelectedIndex;
            Properties.Settings.Default.PDFEquipmentKeywords = EquipmentKeywords.Checked;
            Properties.Settings.Default.PDFEquipmentStats = EquipmentStats.Checked;
            Properties.Settings.Default.PDFFeatureTitlesOnly = FeatureTitle.Checked;
            Properties.Settings.Default.Save();


            SaveFileDialog od = new SaveFileDialog();
            if (Properties.Settings.Default.LastPDFFolder != null && Properties.Settings.Default.LastPDFFolder != "") od.InitialDirectory = Properties.Settings.Default.LastPDFFolder;
            if (lastfile != null && lastfile != "")
            {
                if (Properties.Settings.Default.LastPDFFolder == null || Properties.Settings.Default.LastPDFFolder == "") od.InitialDirectory = Path.GetDirectoryName(lastfile);
                od.FileName = Path.GetFileNameWithoutExtension(lastfile) + ".pdf";
            }
            od.Filter = "PDF|*.pdf";
            od.Title = "Save a PDF File";
            if (od.ShowDialog() == DialogResult.OK && od.FileName != "")
            {
                Properties.Settings.Default.LastPDFFolder = Path.GetDirectoryName(od.FileName);
                Properties.Settings.Default.Save();
                try
                {
                    using (FileStream fs = (FileStream)od.OpenFile())
                    {
                        PDFForms pdf = new PDFForms()
                        {
                            IncludeResources = ResourcesBox.Checked,
                            PreserveEdit = PreserveBox.Checked,
                            Duplex = DuplexBox.Checked,
                            DuplexWhite = DuplexWhiteBox.Checked,
                            APFormat = PDFBase.APFormats[ACPBox.SelectedIndex >= 0 ? ACPBox.SelectedIndex : 0],
                            AutoExcludeActions = IgnoreActionsBox.Checked,
                            ForceAttunedAndOnUseItemsOnSheet = AttunedSheetBox.Checked,
                            ForceAttunedItemsInSpellbook = AttunementSpellbookBox.Checked,
                            MundaneEquipmentInSpellbook = MundaneItemBox.Checked,
                            IgnoreMagicItems = IgnoreMagicItemsBox.Checked,
                            IncludeActions = ActionsBox.SelectedItem != null && !NONE.Equals(ActionsBox.SelectedItem),
                            IncludeLog = LogBox.SelectedItem != null && !NONE.Equals(LogBox.SelectedItem),
                            IncludeMonsters = MonsterBox.SelectedItem != null && !NONE.Equals(MonsterBox.SelectedItem),
                            IncludeSpellbook = SpellbookBox.SelectedItem != null && !NONE.Equals(SpellbookBox.SelectedItem),
                            SwapScoreAndMod = SwapScoreModBox.Checked,
                            BonusSpellsAreResources = BonusSpellsResources.SelectedIndex,
                            EquipmentKeywords = EquipmentKeywords.Checked,
                            EquipmentStats = EquipmentStats.Checked,
                            OnlyFeatureTitles = FeatureTitle.Checked,
                            OutStream = fs
                        };
                        await exporter.Export(Program.Context, pdf);
                        Hide();
                    }
                    if (MessageBox.Show("PDF exported to: " + od.FileName + " Do you want to open it?", "CB5", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Process.Start(od.FileName);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error exporting PDF to " + od.FileName);
                    button1.Enabled = true;
                }
            }

        }

        private PDF MakeCustom()
        {
            PDF res = new PDF();
            res.Name = null;
            if (SheetBox.SelectedItem is string && charsheets.ContainsKey(SheetBox.SelectedItem as string))
            {
                PDF other = charsheets[SheetBox.SelectedItem as string];
                res.SheetName = other.SheetName;
                res.SheetPreview = other.SheetPreview;
                res.File = other.File;
                res.Fields = other.Fields;
            }
            if (SpellBox.SelectedItem is string && spellsheets.ContainsKey(SpellBox.SelectedItem as string))
            {
                PDF other = spellsheets[SpellBox.SelectedItem as string];
                res.SpellName = other.SpellName;
                res.SpellPreview = other.SpellPreview;
                res.SpellFile = other.SpellFile;
                res.SpellFields = other.SpellFields;
            }
            if (LogBox.SelectedItem is string && logsheets.ContainsKey(LogBox.SelectedItem as string))
            {
                PDF other = logsheets[LogBox.SelectedItem as string];
                res.LogName = other.LogName;
                res.LogPreview = other.LogPreview;
                res.LogFile = other.LogFile;
                res.LogFields = other.LogFields;
            }
            if (ActionsBox.SelectedItem is string && actionsheets.ContainsKey(ActionsBox.SelectedItem as string))
            {
                PDF other = actionsheets[ActionsBox.SelectedItem as string];
                res.ActionsName = other.ActionsName;
                res.ActionsPreview = other.ActionsPreview;
                res.ActionsFile = other.ActionsFile;
                res.ActionsFile2 = other.ActionsFile2;
                res.ActionsFields = other.ActionsFields;
                res.ActionsFields2 = other.ActionsFields2;
            }
            if (SpellbookBox.SelectedItem is string && spellbooks.ContainsKey(SpellbookBox.SelectedItem as string))
            {
                PDF other = spellbooks[SpellbookBox.SelectedItem as string];
                res.SpellbookName = other.SpellbookName;
                res.SpellbookPreview = other.SpellbookPreview;
                res.SpellbookFile = other.SpellbookFile;
                res.SpellbookFields = other.SpellbookFields;
            }
            if (MonsterBox.SelectedItem is string && monstersheets.ContainsKey(MonsterBox.SelectedItem as string))
            {
                PDF other = monstersheets[MonsterBox.SelectedItem as string];
                res.MonstersName = other.MonstersName;
                res.MonstersPreview = other.MonstersPreview;
                res.MonstersFile = other.MonstersFile;
                res.MonsterFields = other.MonsterFields;
            }
            return res;
        }

        private void SheetBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            internalEvent = true;
            Preset.SelectedItem = CUSTOM;
            internalEvent = false;
            if (charsheets.ContainsKey(SheetBox.SelectedItem as string)) SheetPreview.Image = LoadImage(charsheets[SheetBox.SelectedItem as string].SheetPreview);
            else SheetPreview.Image = null;
        }

        private Image LoadImage(byte[] sheetPreview)
        {
            if (sheetPreview == null) return null;
            try
            {
                using (MemoryStream ms = new MemoryStream(sheetPreview)) return new Bitmap(ms);
            }
            catch (Exception e)
            {
                ConfigManager.LogError(e);
            }
            return null;
        }

        private void SpellBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            internalEvent = true;
            Preset.SelectedItem = CUSTOM;
            internalEvent = false;
            if (SpellBox.SelectedItem is string && spellsheets.ContainsKey(SpellBox.SelectedItem as string)) SpellPreview.Image = LoadImage(spellsheets[SpellBox.SelectedItem as string].SpellPreview);
            else SpellPreview.Image = null;
        }

        private void LogBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            internalEvent = true;
            Preset.SelectedItem = CUSTOM;
            internalEvent = false;
            if (LogBox.SelectedItem is string && logsheets.ContainsKey(LogBox.SelectedItem as string)) LogPreview.Image = LoadImage(logsheets[LogBox.SelectedItem as string].LogPreview);
            else LogPreview.Image = null;
        }

        private void MonsterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            internalEvent = true;
            Preset.SelectedItem = CUSTOM;
            internalEvent = false;
            if (MonsterBox.SelectedItem is string && monstersheets.ContainsKey(MonsterBox.SelectedItem as string)) MonsterPreview.Image = LoadImage(monstersheets[MonsterBox.SelectedItem as string].MonstersPreview);
            else MonsterPreview.Image = null;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (AttackOrder.SelectedIndex >= 1)
            {
                var o = AttackOrder.Items[AttackOrder.SelectedIndex];
                AttackOrder.Items[AttackOrder.SelectedIndex] = AttackOrder.Items[AttackOrder.SelectedIndex - 1];
                AttackOrder.Items[AttackOrder.SelectedIndex - 1] = o;
                SaveOrder();
                AttackOrder.SelectedIndex--;
            }
        }

        private void SaveOrder()
        {
            Program.Context.MakeHistory("AttackOrder");
            Program.Context.Player.AttackOrder.Clear();
            foreach (object o in AttackOrder.Items)
            {
                if (o is KeyValuePair<string, AttackInfo> kp) Program.Context.Player.AttackOrder.Add(kp.Key);
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (AttackOrder.SelectedIndex >= 0 && AttackOrder.SelectedIndex < AttackOrder.Items.Count - 1)
            {
                var o = AttackOrder.Items[AttackOrder.SelectedIndex];
                AttackOrder.Items[AttackOrder.SelectedIndex] = AttackOrder.Items[AttackOrder.SelectedIndex + 1];
                AttackOrder.Items[AttackOrder.SelectedIndex + 1] = o;
                SaveOrder();
                AttackOrder.SelectedIndex++;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("AttackOrder");
            Program.Context.Player.AttackOrder.Clear();
            BuildAttacks();
        }
    }
}
