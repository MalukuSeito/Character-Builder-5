using Character_Builder;
using Character_Builder_Forms;
using Microsoft.VisualBasic;
using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
using OGL.Items;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Character_Builder_5
{
    public partial class Form1 : Form
    {
        private bool layouting = false;
        public String lastfile = "";
        private MouseEventArgs drag;
        private Dictionary<string, ToolStripMenuItem> plugins = new Dictionary<string, ToolStripMenuItem>();
        private class Drag
        {
            public Drag(int i, object v)
            {
                index = i;
                curindex = index;
                value = v;
            }
            public int curindex;
            public int index;
            public object value;
        }
        public Form1()
        {
            InitializeComponent();
            itemCategories.Items.AddRange(Program.Context.Section().ToArray<Category>());
            itemCategories.Items.AddRange(Program.Context.MagicSection().ToArray<MagicCategory>());
            itemCategories.Items.Add("Spells");
            itemCategories.Items.AddRange(Program.Context.FeatureSection().ToArray<string>());
            ArrayBox.Items.AddRange(Program.Context.Scores.GetArrays().ToArray<AbilityScoreArray>());
            racebox.MouseWheel += listbox_MouseWheel;
            subracebox.MouseWheel += listbox_MouseWheel;
            bonds.MouseWheel += listbox_MouseWheel;
            ideals.MouseWheel += listbox_MouseWheel;
            flaws.MouseWheel += listbox_MouseWheel;
            background.MouseWheel += listbox_MouseWheel;
            traits.MouseWheel += listbox_MouseWheel;
            traits2.MouseWheel += listbox_MouseWheel;
            journalentrybox.MouseWheel += listbox_MouseWheel;
            skillbox.MouseWheel += listbox_MouseWheel;
            HitDiceUsed.MouseWheel += listbox_MouseWheel;
            ResourcesBox.MouseWheel += listbox_MouseWheel;
            Features.MouseWheel += listbox_MouseWheel;
            activeConditions.MouseWheel += listbox_MouseWheel;
            availableConditions.MouseWheel += listbox_MouseWheel;
            actionBox.Controls.Clear();
            UpdateLayout();
            //pDFExporterToolStripMenuItem.DropDownItems.Clear();
            //bool first=true;
            //foreach (string s in Program.Context.Config.PDFExporters)
            //{
            //    ToolStripMenuItem p = new ToolStripMenuItem(Path.GetFileNameWithoutExtension(s));
            //    if (first)
            //    {
            //        first = false;
            //        p.Checked = true;
            //    }
            //    p.Name = s;
            //    p.Size = new System.Drawing.Size(152, 22);
            //    p.Click += new EventHandler(pdfexporter_click);
            //    pDFExporterToolStripMenuItem.DropDownItems.Add(p);
            //}

            configureHouserulesToolStripMenuItem.DropDownItems.Clear();
            if (Program.Context.Plugins.available.Count == 0) configureHouserulesToolStripMenuItem.Enabled = false;
            foreach (string s in Program.Context.Plugins.available.Keys)
            {
                ToolStripMenuItem p = new ToolStripMenuItem(s);
                plugins.Add(s, p);
                p.Name = s;
                p.Size = new System.Drawing.Size(152, 22);
                p.Click += pluginClick;
                configureHouserulesToolStripMenuItem.DropDownItems.Add(p);
            }
            PluginManager.PluginsChanged += PluginManager_PluginsChanged;
            BuildSources();
            portraitBox.AllowDrop = true;
            FactionInsignia.AllowDrop = true;
            sidePortrait.AllowDrop = true;
            AllowDrop = true;
            possequip.Items.Clear();
            possequip.Items.Add(EquipSlot.None);
            foreach (string s in Program.Context.Config.Slots) possequip.Items.Add(s);
            journalDate.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        }

        public void BuildSources()
        {
            sourcesToolStrip.DropDownItems.Clear();
            ToolStripMenuItem addall = new ToolStripMenuItem("Add all")
            {
                Name = "Add all",
                Size = new System.Drawing.Size(152, 22)
            };
            addall.Click += Addall_Click;
            sourcesToolStrip.DropDownItems.Add(addall);
            ToolStripMenuItem remall = new ToolStripMenuItem("Remove all")
            {
                Name = "Remove all",
                Size = new System.Drawing.Size(152, 22)
            };
            remall.Click += Remall_Click; ;
            sourcesToolStrip.DropDownItems.Add(remall);
            sourcesToolStrip.DropDownItems.Add(new ToolStripSeparator());
            foreach (string s in SourceManager.Sources)
            {
                ToolStripMenuItem p = new ToolStripMenuItem(s)
                {
                    Name = s,
                    Size = new System.Drawing.Size(152, 22)
                };
                p.Click += SourceClick;
                p.Checked = !Program.Context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase);
                sourcesToolStrip.DropDownItems.Add(p);
            }
            itemCategories.Items.Clear();
            itemCategories.Items.AddRange(Program.Context.Section().ToArray<Category>());
            itemCategories.Items.AddRange(Program.Context.MagicSection().ToArray<MagicCategory>());
            itemCategories.Items.Add("Spells");
            itemCategories.Items.AddRange(Program.Context.FeatureSection().ToArray<string>());
            ArrayBox.Items.Clear();
            ArrayBox.Items.AddRange(Program.Context.Scores.GetArrays().ToArray<AbilityScoreArray>());
        }

        private void Remall_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("Sources");
            Program.Context.Player.ExcludedSources.AddRange(SourceManager.Sources);
            Program.Context.ExcludedSources.Clear();
            Program.Context.ExcludedSources.UnionWith(Program.Context.Player.ExcludedSources);
            Program.ReloadData();
            UpdateLayout();
        }

        private void Addall_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("Sources");
            Program.Context.Player.ExcludedSources.Clear();
            Program.Context.ExcludedSources.Clear();
            Program.Context.ExcludedSources.UnionWith(Program.Context.Player.ExcludedSources);
            Program.ReloadData();
            UpdateLayout();
        }

        private void SourceClick(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem tsmi)
            {
                Program.Context.MakeHistory("Sources");
                if (tsmi.Checked) Program.Context.Player.ExcludedSources.Add(tsmi.Name);
                else Program.Context.Player.ExcludedSources.RemoveAll(s => StringComparer.OrdinalIgnoreCase.Equals(s, tsmi.Name));
                Program.Context.ExcludedSources.Clear();
                Program.Context.ExcludedSources.UnionWith(Program.Context.Player.ExcludedSources);
                Program.ReloadData();
                UpdateLayout();
            }
        }

        private void PluginManager_PluginsChanged(object sender, EventArgs e)
        {
            PluginManager manager = Program.Context.Plugins;
            foreach (ToolStripMenuItem t in plugins.Values) t.Checked = false;
            foreach (Character_Builder_Plugin.IPlugin p in manager.plugins) plugins[p.Name].Checked = true;
        }

        private void pluginClick(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem p)
            {
                Program.Context.MakeHistory(null);
                if (p.Checked) Program.Context.Player.ActiveHouseRules.RemoveAll(s => StringComparer.InvariantCultureIgnoreCase.Equals(s, p.Name));
                else Program.Context.Player.ActiveHouseRules.Add(p.Name);
                Program.Context.Plugins.Load(Program.Context.Player.ActiveHouseRules);
                UpdateLayout();
            }
        }

        //private void pdfexporter_click(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem s=(ToolStripMenuItem)sender;
        //    s.Checked = true;
        //    Config.PDFExporter = PlayerExtensions.Load(s.Name);
        //    foreach (ToolStripMenuItem p in pDFExporterToolStripMenuItem.DropDownItems)
        //    {
        //        if (p != s) p.Checked = false;
        //    }
        //}

        public void listbox_MouseWheel(object sender, MouseEventArgs e)
        {
            Control p = ((Control)sender).Parent;
            if (sender is ListBox && ((ListBox)sender).Height < ((ListBox)sender).ItemHeight * ((ListBox)sender).Items.Count)
            {
                ListBox lb = (ListBox)sender;
                if (e.Delta > 0 && lb.TopIndex > 0) return;
                if (e.Delta < 0 && lb.TopIndex + lb.Height / lb.ItemHeight < lb.Items.Count) return;
            }
            if (p is Panel && ((Panel)p).AutoScroll)
            {
                Point s = ((Panel)p).AutoScrollPosition;
                ((Panel)p).AutoScrollPosition = new Point(-s.X, -(s.Y + e.Delta));
            }
            else if (p.Parent is Panel && ((Panel)p.Parent).AutoScroll)
            {
                Point s = ((Panel)p.Parent).AutoScrollPosition;
                ((Panel)p.Parent).AutoScrollPosition = new Point(-s.X, -(s.Y + e.Delta));
            }
                // We have to go deeper
            else if (p.Parent.Parent is Panel && ((Panel)p.Parent.Parent).AutoScroll)
            {
                Point s = ((Panel)p.Parent.Parent).AutoScrollPosition;
                ((Panel)p.Parent.Parent).AutoScrollPosition = new Point(-s.X, -(s.Y + e.Delta));
            }
        }

        private void listItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listItems.SelectedItem != null)
            {
                if (listItems.SelectedItem is Item) Choice_DisplayItem(sender, e);
                if (listItems.SelectedItem is MagicProperty)
                {
                    Choice_DisplayMagicProperty(sender, e);
                    MagicProperty mp = (MagicProperty)listItems.SelectedItem;
                    addButton.Enabled = false;
                    addtoItemButton.Enabled = false;
                    if (mp.Base == null || mp.Base == "") addButton.Enabled = true;
                    else if (inventory2.SelectedItem != null && ((Possession)inventory2.SelectedItem).BaseItem != null && ((Possession)inventory2.SelectedItem).BaseItem != "")
                        addtoItemButton.Enabled = Utils.Fits(Program.Context, mp, Program.Context.GetItem(((Possession)inventory2.SelectedItem).BaseItem, null));
                }
                if (listItems.SelectedItem is Spell)
                {
                    Choice_DisplaySpellScroll(sender, e);
                    if (spellbookFeaturesBox.SelectedItem != null && spellbookFeaturesBox.SelectedItem is SpellcastingCapsule)
                        addspellbookButton.Enabled = Utils.Matches(Program.Context, ((Spell)listItems.SelectedItem), ((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature.PrepareableSpells, ((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature.SpellcastingID) && !Program.Context.Player.GetSpellcasting(((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature.SpellcastingID).GetSpellbook(Program.Context.Player, Program.Context).Contains((Spell)listItems.SelectedItem);
                }
                if (listItems.SelectedItem is Feature)
                {
                    Choice_DisplayFeature(sender, e);
                    addButton.Enabled = !Program.Context.Player.Boons.Contains(((Feature)listItems.SelectedItem).Name, ConfigManager.SourceInvariantComparer);
                }
            }
        }

        private void itemCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (itemCategories.SelectedItem != null)
            {
                listItems.Items.Clear();
                if (itemCategories.SelectedItem is Category)
                {
                    listItems.Items.AddRange(Program.Context.Subsection((Category)itemCategories.SelectedItem).ToArray<Item>());
                    //inventorySplit.Panel2Collapsed = true;
                    inventory2.Enabled = true;
                    addButton.Enabled = true;
                    buyButton.Enabled = true;
                    ItemCounter.Value = 1;
                    actionBox.Controls.Clear();
                    actionBox.Controls.Add(buyButton);
                    actionBox.Controls.Add(addButton);
                    actionBox.Controls.Add(ItemCounter);

                }
                else if (itemCategories.SelectedItem is MagicCategory)
                {
                    listItems.Items.AddRange(((MagicCategory)itemCategories.SelectedItem).SubSection(Program.Context.Search).ToArray<MagicProperty>());
                    //inventorySplit.Panel2Collapsed = false;
                    inventory2.Enabled = true;
                    actionBox.Controls.Clear();
                    addtoItemButton.Enabled = false;
                    addButton.Enabled = false;
                    actionBox.Controls.Add(addtoItemButton);
                    actionBox.Controls.Add(addButton);
                }
                else if (itemCategories.SelectedItem.ToString() == "Spells")
                {
                    listItems.Items.AddRange(Program.Context.SpellSubsection().ToArray<Spell>());
                   // inventorySplit.Panel2Collapsed = true;
                    inventory2.Enabled = true;
                    addspellbookButton.Enabled = false;
                    actionBox.Controls.Clear();
                    actionBox.Controls.Add(addspellbookButton);
                    actionBox.Controls.Add(spellbookFeaturesBox);
                    actionBox.Controls.Add(addScrollButton);
                }
                else
                {
                    listItems.Items.AddRange(Program.Context.FeatureSubsection(itemCategories.SelectedItem.ToString()).ToArray<Feature>());
                    //inventorySplit.Panel2Collapsed = true;
                    inventory2.Enabled = true;
                    actionBox.Controls.Clear();
                    addButton.Enabled = false;
                    actionBox.Controls.Add(addButton);
                }
            }
        }

        private void itemsearchbox_TextChanged(object sender, EventArgs e)
        {
            if (itemsearchbox.ForeColor == Color.LightGray) Program.Context.Search = "";
            else Program.Context.Search = itemsearchbox.Text;
            itemCategories.Items.Clear();
            listItems.Items.Clear();
            itemCategories.Items.AddRange(Program.Context.Section().ToArray<Category>());
            itemCategories.Items.AddRange(Program.Context.MagicSection().ToArray<MagicCategory>());
            itemCategories.Items.Add("Spells");
            itemCategories.Items.AddRange(Program.Context.FeatureSection().ToArray<string>());
        }

        private void itemsearchbox_Enter(object sender, EventArgs e)
        {
            if (itemsearchbox.ForeColor == Color.LightGray)
            {
                itemsearchbox.ForeColor = default(Color);
                itemsearchbox.Text = "";
            }
        }

        private void itemsearchbox_Leave(object sender, EventArgs e)
        {
            if (itemsearchbox.Text == "")
            {
                itemsearchbox.ForeColor = Color.LightGray;
                itemsearchbox.Text = "Search";
            }
        }
        public void Resetglobals()
        {
            displayElement.Navigate("about:blank");
        }
        public void UpdateLayout()
        {
            try
            {
                if (lastfile == "") Text = "Character Builder 5";
                else Text = "Character Builder 5 - " + lastfile;
                UpdateSideLayout();
                UpdateRaceLayout(false);
                UpdateBackgroundLayout(false);
                UpdatePersonal(false);
                UpdateScores(false);
                UpdateClass(false);
                UpdateSpellcastingOuter(false);
                UpdateEquipmentLayout(false);
                UpdateFormsCompanions();
                UpdateInPlayInner();
                UpdateJournal(false);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating Layout");
            }
        }

        public void UpdateJournal(bool updateside = true)
        {
            bool waslayouting = layouting;
            if (!layouting) layouting = true;
            if (updateside) UpdateSideLayout();
            advancementCheckpointsInsteadOfXPToolStripMenuItem.Checked = Program.Context.Player.Advancement;
            journalTab.SuspendLayout();
            int index = journalEntries.SelectedIndex;
            journalEntries.Items.Clear();
            int down = 0;
            int renown = 0;
            int t1tp = 0;
            int t2tp = 0;
            int t3tp = 0;
            int t4tp = 0;
            int magic = 0;
            foreach (JournalEntry je in Program.Context.Player.ComplexJournal)
            {
                down += je.Downtime;
                renown += je.Renown;
                magic += je.MagicItems;
                t1tp += je.T1TP;
                t2tp += je.T2TP;
                t3tp += je.T3TP;
                t4tp += je.T4TP;
                journalEntries.Items.Add(je);
            }
            if (index >= 0 && index < journalEntries.Items.Count) journalEntries.SelectedIndex = index;
            else journalEntries_SelectedIndexChanged(null, null);
            List<string> c = new List<string>();
            if (down != 0) c.Add(down + " Downtime");
            if (renown != 0) c.Add(renown + " Renown");
            if (magic != 0) c.Add(magic + " Magic Items");
            if (t1tp != 0) c.Add(t1tp + " Tier 1 TP");
            if (t2tp != 0) c.Add(t2tp + " Tier 2 TP");
            if (t3tp != 0) c.Add(t3tp + " Tier 3 TP");
            if (t4tp != 0) c.Add(t4tp + " Tier 4 TP");
            journalTotal.Text = "Total: " + String.Join(", ", c);
            journalTab.ResumeLayout();
            if (!waslayouting) layouting = false;
        }

        public void UpdateEquipmentLayout(bool updateside = true)
        {
            try
            {
                bool waslayouting = layouting;
                if (!layouting) layouting = true;
                if (updateside) UpdateSideLayout();
                equiptab.SuspendLayout();
                layouting = true;
                int iindex = inventory.SelectedIndex;
                inventory.Items.Clear();
                inventory2.Items.Clear();
                Possession[] pos = Program.Context.Player.GetItemsAndPossessions().ToArray<Possession>();
                inventory.Items.AddRange(pos);
                inventory.Items.AddRange((from b in Program.Context.Player.Boons select Program.Context.GetBoon(b, null)).ToArray<Feature>());
                inventory2.Items.AddRange(pos);
                if (iindex >= 0 && iindex < inventory.Items.Count) inventory.SelectedIndex = iindex;
                int index = 0;
                if (spellbookFeaturesBox.SelectedIndex > 0) index = spellbookFeaturesBox.SelectedIndex;
                UpdateInventoryOptions();
                spellbookFeaturesBox.Items.Clear();
                spellbookFeaturesBox.Items.AddRange((from sc in Program.Context.Player.GetFeatures() where sc is SpellcastingFeature && ((SpellcastingFeature)sc).Preparation == PreparationMode.Spellbook select new SpellcastingCapsule((SpellcastingFeature)sc)).ToArray<SpellcastingCapsule>());
                if (index >= 0 && index < spellbookFeaturesBox.Items.Count) spellbookFeaturesBox.SelectedIndex = index;
                equiptab.ResumeLayout();
                if (!waslayouting) layouting = false;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating Equipment Layout");
            }
        }
        public void UpdateInventoryOptions() {
            try
            {
                bool waslayouting = layouting;
                if (!layouting) layouting = true;
                equiptab.SuspendLayout();

                if (inventory.SelectedItem is Possession p)
                {
                    possequip.Enabled = true;
                    try
                    {
                        possequip.SelectedItem = p.Equipped;
                    }
                    catch (Exception)
                    {
                        p.Equipped = EquipSlot.None;
                        possequip.SelectedItem = EquipSlot.None;
                    }
                    attunedcheck.Enabled = true;
                    attunedcheck.Checked = p.Attuned;
                    highlightcheck.Enabled = true;
                    highlightcheck.Checked = p.Hightlight;
                    posscharges.Enabled = true;
                    posscharges.Value = p.ChargesUsed;
                    possweight.Enabled = true;
                    possweight.Value = (decimal)p.Weight;
                    possname.Text = p.Name;
                    possdescription.Text = p.Description;
                    poscounter.Value = (p.Count > 0 ? p.Count : 1);
                    magicproperties.Items.Clear();
                    magicproperties.Items.AddRange((from s in p.MagicProperties select Program.Context.GetMagic(s, null)).ToArray());
                    unpack.Enabled = (p.BaseItem != null && p.BaseItem != "" && p.Item is Pack);
                    splitstack.Enabled = true;
                    changecount.Enabled = true;
                    updateposs.Enabled = true;
                }
                else if (inventory.SelectedItem is Feature)
                {
                    possequip.SelectedIndex = -1;
                    possequip.Enabled = false;
                    attunedcheck.Enabled = false;
                    attunedcheck.Checked = false;
                    highlightcheck.Enabled = false;
                    highlightcheck.Checked = false;
                    posscharges.Value = 0;
                    posscharges.Enabled = false;
                    possweight.Value = -1;
                    possweight.Enabled = false;
                    possname.Text = "";
                    possdescription.Text = "";
                    poscounter.Value = 1;
                    magicproperties.Items.Clear();
                    unpack.Enabled = false;
                    splitstack.Enabled = false;
                    changecount.Enabled = false;
                    updateposs.Enabled = false;
                }
                else
                {
                    possequip.SelectedIndex = -1;
                    possequip.Enabled = false;
                    attunedcheck.Enabled = false;
                    attunedcheck.Checked = false;
                    highlightcheck.Enabled = false;
                    highlightcheck.Checked = false;
                    posscharges.Value = 0;
                    posscharges.Enabled = false;
                    possweight.Value = -1;
                    possweight.Enabled = false;
                    possname.Text = "";
                    possdescription.Text = "";
                    poscounter.Value = 1;
                    magicproperties.Items.Clear();
                    unpack.Enabled = false;
                    unpack.Enabled = false;
                    splitstack.Enabled = false;
                    changecount.Enabled = false;
                    updateposs.Enabled = false;
                }
                equiptab.ResumeLayout();
                if (!waslayouting) layouting = false;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating Inventory");
            }
        }
        public void UpdateSideLayout()
        {
            try
            {
                layouting = true;
                mainSpilt.Panel1.SuspendLayout();
                statisticsbox.SuspendLayout();
                undoToolStripMenuItem.Enabled = Program.Context.CanUndo();
                redoToolStripMenuItem.Enabled = Program.Context.CanRedo();
                AbilityScoreArray scores = Program.Context.Player.GetFinalAbilityScores();
                SideStrength.Text = "Str " + scores.Strength;
                SideDexterity.Text = "Dex " + scores.Dexterity;
                SideConstitution.Text = "Con " + scores.Constitution;
                SideIntelligence.Text = scores.Intelligence + " Int";
                SideWisdom.Text = scores.Wisdom + " Wis";
                SideCharisma.Text = scores.Charisma + " Cha";
                int speed = Program.Context.Player.GetSpeed();
                SideSpeed.Text = speed + " ft";
                int hp = Program.Context.Player.GetHitpointMax();
                Speed.Text = speed.ToString();
                SideMaxHP.Text = "HP: " + hp;
                MaxHP.Text = hp.ToString();
                int curhploss = Program.Context.Player.CurrentHPLoss;
                if (curhploss > 0) curhploss = 0;
                CurHP.Maximum = hp;
                CurHP.Value = hp + curhploss;
                bonusMaxHP.Minimum = -Math.Abs(CurHP.Maximum);
                sidePortrait.Image = Program.Context.Player.GetPortrait();
                int level = Program.Context.Player.GetLevel();
                SideName.Text = Program.Context.Player.Name + "\n" + String.Join(" | ", from pc in Program.Context.Player.Classes select pc.ToString(Program.Context, level)) + "\n(Level " + Program.Context.Player.GetLevel() + ")\n" + Program.Context.Player.GetRaceSubName();
                int ac = Program.Context.Player.GetAC();
                SideAC.Text = ac + " AC";
                AC.Text = ac.ToString();
                int init = Program.Context.Player.GetInitiative();
                SideInit.Text = "Init: " + plusMinus(init);
                Initiative.Text = plusMinus(init);
                double carry = Program.Context.Player.GetCarryCapacity();
                double weight = Program.Context.Player.GetWeight();
                currentweight.Text = weight.ToString("N") + " lb / " + carry.ToString("N0") + " lb";
                CurWeight.Text = weight.ToString("N");
                MaxWeight.Text = carry.ToString("N");
                Price money = Program.Context.Player.GetMoney();
                string moneytext = money.ToString();
                Money.Text = money.ToString();
                if (moneytext == "") moneytext = "Coinage";
                MoneyButton.Text = moneytext;
                PP.Value = (int)money.pp;
                GP.Value = (int)money.gp;
                EP.Value = (int)money.ep;
                SP.Value = (int)money.sp;
                CP.Value = (int)money.cp;
                statisticsbox.ResumeLayout(true);
                mainSpilt.Panel1.ResumeLayout(true);
                layouting = false;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating stats");
            }
        }
        public void UpdateInPlayInner()
        {
            try
            {
                layouting = true;
                showAllKnownRitualsToolStripMenuItem.Checked = Program.Context.Player.AllRituals;
                inspiration.Checked = Program.Context.Player.Inspiration;
                int prof = Program.Context.Player.GetProficiency();
                profval.Text = plusMinus(prof);
                AbilityScoreArray asa = Program.Context.Player.GetFinalAbilityScores(out AbilityScoreArray max);
                Ability SaveProf = Program.Context.Player.GetSaveProficiencies();
                AbilityScoreArray saveBonus = Program.Context.Player.GetSavingThrowsBoni();
                strval.Text = asa.Strength.ToString();
                strmod.Text = plusMinus(asa.StrMod);
                strmax.Text = max.Strength.ToString();
                strsave.Text = plusMinus(asa.StrMod + (SaveProf.HasFlag(Ability.Strength) ? prof : 0) + saveBonus.Strength);
                dexval.Text = asa.Dexterity.ToString();
                dexmod.Text = plusMinus(asa.DexMod);
                dexmax.Text = max.Dexterity.ToString();
                dexsave.Text = plusMinus(asa.DexMod + (SaveProf.HasFlag(Ability.Dexterity) ? prof : 0) + saveBonus.Dexterity);
                conval.Text = asa.Constitution.ToString();
                conmod.Text = plusMinus(asa.ConMod);
                conmax.Text = max.Constitution.ToString();
                consave.Text = plusMinus(asa.ConMod + (SaveProf.HasFlag(Ability.Constitution) ? prof : 0) + saveBonus.Constitution);
                intval.Text = asa.Intelligence.ToString();
                intmod.Text = plusMinus(asa.IntMod);
                intmax.Text = max.Intelligence.ToString();
                intsave.Text = plusMinus(asa.IntMod + (SaveProf.HasFlag(Ability.Intelligence) ? prof : 0) + saveBonus.Intelligence);
                wisval.Text = asa.Wisdom.ToString();
                wismod.Text = plusMinus(asa.WisMod);
                wismax.Text = max.Wisdom.ToString();
                wissave.Text = plusMinus(asa.WisMod + (SaveProf.HasFlag(Ability.Wisdom) ? prof : 0) + saveBonus.Wisdom);
                chaval.Text = asa.Charisma.ToString();
                chamod.Text = plusMinus(asa.ChaMod);
                chamax.Text = max.Charisma.ToString();
                chasave.Text = plusMinus(asa.ChaMod + (SaveProf.HasFlag(Ability.Charisma) ? prof : 0) + saveBonus.Charisma);
                skillbox.Items.Clear();
                skillbox.Items.AddRange(Program.Context.Player.GetSkills().ToArray());
                List<HitDie> hd = Program.Context.Player.GetHitDie();
                hd.Sort();
                HitDice.Text = String.Join(", ", from h in hd select h.Total());
                HitDiceUsed.Items.Clear();
                HitDiceUsed.Items.AddRange(hd.ToArray());
                TempHP.Value = Program.Context.Player.TempHP;
                bonusMaxHP.Value = Program.Context.Player.BonusMaxHP;
                if (Program.Context.Player.FailedDeathSaves < 0) Program.Context.Player.FailedDeathSaves = 0;
                if (Program.Context.Player.SuccessDeathSaves < 0) Program.Context.Player.SuccessDeathSaves = 0;
                if (Program.Context.Player.FailedDeathSaves > 3) Program.Context.Player.FailedDeathSaves = 3;
                if (Program.Context.Player.SuccessDeathSaves > 3) Program.Context.Player.SuccessDeathSaves = 3;
                DeathFail.Value = Program.Context.Player.FailedDeathSaves;
                DeathSuccess.Value = Program.Context.Player.SuccessDeathSaves;
                int resourceindex = ResourcesBox.SelectedIndex;
                List<ModifiedSpell> bonusspells = new List<ModifiedSpell>(Program.Context.Player.GetBonusSpells());
                foreach (ModifiedSpell mods in bonusspells)
                {
                    mods.used = Program.Context.Player.GetUsedResources(mods.getResourceID());
                    mods.displayShort = true;
                }
                ResourcesBox.Items.Clear();
                ResourcesBox.Items.AddRange(Program.Context.Player.GetResourceInfo(true).Values.ToArray());
                ResourcesBox.Items.AddRange(bonusspells.ToArray());
                if (resourceindex >= 0 && resourceindex < ResourcesBox.Items.Count) ResourcesBox.SelectedIndex = resourceindex;
                else resourceused.Enabled = false;
                int featindex = Features.SelectedIndex;
                Features.Items.Clear();
                Features.Items.AddRange((from f in Program.Context.Player.GetFeatures() where f.Name != "" && !f.Hidden select f).ToArray());
                if (featindex >= 0 && featindex < Features.Items.Count) Features.SelectedIndex = featindex;
                else hidefeature.Enabled = false;
                List<Condition> active = new List<Condition>(from c in Program.Context.Player.Conditions select Program.Context.GetCondition(c, null));
                List<Condition> possible = new List<Condition>(Program.Context.Conditions.Values);
                possible.RemoveAll(t => active.Contains(t));
                possible.Sort();
                active.Sort();
                activeConditions.Items.Clear();
                activeConditions.Items.AddRange(active.ToArray());
                availableConditions.Items.Clear();
                availableConditions.Items.AddRange(possible.ToArray());
                availableConditions.Items.Add("- Custom -");
                List<Feature> spellfeatures = new List<Feature>(from f in Program.Context.Player.GetFeatures() where f is SpellcastingFeature select f);
                List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>();
                foreach (Feature f in spellfeatures) if (f is SpellcastingFeature) spellcasts.Add((SpellcastingFeature)f);
                for (int i = 6; i < inplayflow.Controls.Count; i++)
                {
                    if (inplayflow.Controls[i] is GroupBox box)
                    {
                        string spellcasting = box.Name;
                        Spellcasting sc = Program.Context.Player.GetSpellcasting(spellcasting);
                        SpellcastingFeature sf = spellcasts.FirstOrDefault(f => f.SpellcastingID == spellcasting);
                        if (box.Controls[0] is Label)
                        {
                            Ability spellcastingability = (Ability)int.Parse(box.Controls[0].Name);
                            Control[] spells = box.Controls.Find("SpellsBox", true);
                            if (spells.Count() > 0)
                            {
                                ListBox spellbox = (ListBox)spells[0];
                                List<ModifiedSpell> modspells = new List<ModifiedSpell>();
                                modspells.AddRange(sc.GetLearned(Program.Context.Player, Program.Context));
                                modspells.AddRange(sc.GetPrepared(Program.Context.Player, Program.Context));
                                modspells.Sort();
                                if (sf?.Preparation == PreparationMode.ClassList)
                                {
                                    modspells.AddRange(sc.GetCLassListRituals(sf?.PrepareableSpells ?? "false", Program.Context.Player, Program.Context));
                                } else if (sf?.Preparation == PreparationMode.Spellbook)
                                {
                                    modspells.AddRange(sc.GetSpellbookRituals(Program.Context.Player, Program.Context));
                                }
                                foreach (ModifiedSpell ms in modspells)
                                {
                                    //if (ms.differentAbility == Ability.None) ms.differentAbility = spellcastingability;
                                    ms.AddAlwaysPreparedToName = false;
                                    ms.includeRecharge = false;
                                    ms.includeResources = false;
                                }
                                spellbox.Items.Clear();
                                spellbox.Items.AddRange(modspells.ToArray());
                            }
                            Control[] highlights = box.Controls.Find("SpellOnSheetLabel", true);
                            if (highlights.Count() > 0)
                            {
                                Label highlight = (Label)highlights[0];
                                highlight.Text = "Spell on Sheet: " + (sc.Highlight != null && sc.Highlight != "" ? sc.Highlight : "--");
                            }
                            List<SpellSlotInfo> ssi = Program.Context.Player.GetSpellSlotInfo(spellcasting);
                            Control[] slotboxes = box.Controls.Find("SpellSlotBox", true);
                            if (slotboxes.Count() > 0)
                            {
                                ListBox slotbox = (ListBox)slotboxes[0];
                                int sindex = slotbox.SelectedIndex;
                                slotbox.Items.Clear();
                                slotbox.Items.AddRange(ssi.ToArray());
                                if (sindex >= 0 && sindex < slotbox.Items.Count) slotbox.SelectedIndex = sindex;
                                else
                                {
                                    Control[] useds = box.Controls.Find("SpellSlotUsed", true);
                                    if (useds.Count() > 0)
                                    {
                                        ((NumericUpDown)useds[0]).Enabled = false;
                                    }
                                }
                            }
                            Control[] resets = box.Controls.Find("ResetSlots", true);
                            if (resets.Count() > 0)
                            {
                                ((Button)resets[0]).Enabled = (ssi.Count > 0);
                            }
                        }
                    }
                }
                List<object> profs = new List<object>();
                Proficiencies.Items.Clear();
                Proficiencies.Items.AddRange(Program.Context.Player.GetLanguages().ToArray());
                Proficiencies.Items.AddRange(Program.Context.Player.GetToolProficiencies().ToArray());
                Proficiencies.Items.AddRange(Program.Context.Player.GetToolKWProficiencies().ToArray());
                Proficiencies.Items.AddRange(Program.Context.Player.GetOtherProficiencies().ToArray());
                actionsBox.Items.Clear();
                actionsBox.Items.AddRange(Program.Context.Player.GetActions().ToArray());
                attacksBox.Items.Clear();
                foreach (Possession p in Program.Context.Player.GetItemsAndPossessions()) {
                    AttackInfo ai = Program.Context.Player.GetAttack(p, 0, false);
                    if (ai != null)
                    {
                        attacksBox.Items.Add(new AttackRow() { Possession = p, Attack = ai });
                    }
                }
                attacksLabel.Text = "Attacks (" + (Program.Context.Player.GetExtraAttacks() + 1) + " per action):";
                layouting = false;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating In-Play Layout");
            }
        }
        public void UpdateBackgroundLayout(bool updateside = true)
        {
            try
            {
                if (updateside) UpdateSideLayout();
                layouting = true;
                backtab.SuspendLayout();
                backtab.Controls.Clear();
                int level = Program.Context.Player.GetLevel();
                List<Control> backt = new List<Control>
                {
                    backgroundLabel,
                    background
                };
                background.Items.Clear();
                background.ForeColor = System.Drawing.SystemColors.WindowText;
                Background back = Program.Context.Player.Background;
                List<TableDescription> tables = Program.Context.Player.CollectTables();
                if (back == null)
                {
                    background.Items.AddRange(Program.Context.Backgrounds.Values.OrderBy(s => s.Name).ToArray<Background>());
                    background.Height = Program.Context.Backgrounds.Count() * background.ItemHeight + 10;
                }
                else
                {
                    background.Items.Add(back);
                    background.ForeColor = Config.SelectColor;
                    background.Height = background.ItemHeight + 10;
                    ControlAdder.AddControls(back, backt, level);
                    foreach (Feature f in Program.Context.Player.GetBackgroundFeatures(0, true).OrderBy(a => a.Level))
                    {
                        ControlAdder.AddControl(backt, level, f);
                    }
                }
                backt.Add(traitLabel);
                backt.Add(traits);
                backt.Add(trait2Label);
                backt.Add(traits2);
                traits.Items.Clear();
                traits.ForeColor = System.Drawing.SystemColors.WindowText;
                traits2.Items.Clear();
                traits2.ForeColor = System.Drawing.SystemColors.WindowText;
                if (Program.Context.Player.PersonalityTrait == null || Program.Context.Player.PersonalityTrait == "")
                {
                    if (back != null) traits.Items.AddRange(back.PersonalityTrait.ToArray<TableEntry>());
                    foreach (TableDescription td in tables) if (td.BackgroundOption.HasFlag(BackgroundOption.Trait)) traits.Items.AddRange(td.Entries.ToArray<TableEntry>());
                    traits.Items.Add("- Custom Personality Trait -");
                }
                else
                {
                    traits.ForeColor = Config.SelectColor;
                    traits.Items.Add(Program.Context.Player.PersonalityTrait);
                }
                traits.Height = traits.Items.Count * traits.ItemHeight + 10;

                if (Program.Context.Player.PersonalityTrait2 == null || Program.Context.Player.PersonalityTrait2 == "")
                {
                    if (back != null) traits2.Items.AddRange(back.PersonalityTrait.ToArray<TableEntry>());
                    traits2.Items.Add("- None -");
                    foreach (TableDescription td in tables) if (td.BackgroundOption.HasFlag(BackgroundOption.Trait)) traits2.Items.AddRange(td.Entries.ToArray<TableEntry>());
                    traits2.Items.Add("- Custom Personality Trait -");
                }
                else
                {
                    traits2.ForeColor = Config.SelectColor;
                    traits2.Items.Add(Program.Context.Player.PersonalityTrait2);
                }
                traits2.Height = traits2.Items.Count * traits2.ItemHeight + 10;

                backt.Add(ideallabel);
                backt.Add(ideals);
                ideals.Items.Clear();
                ideals.ForeColor = System.Drawing.SystemColors.WindowText;
                if (Program.Context.Player.Ideal == null || Program.Context.Player.Ideal == "")
                {
                    if (back != null) ideals.Items.AddRange(back.Ideal.ToArray<TableEntry>());
                    foreach (TableDescription td in tables) if (td.BackgroundOption.HasFlag(BackgroundOption.Ideal)) ideals.Items.AddRange(td.Entries.ToArray<TableEntry>());
                    ideals.Items.Add("- Custom Ideal -");
                }
                else
                {
                    ideals.ForeColor = Config.SelectColor;
                    ideals.Items.Add(Program.Context.Player.Ideal);
                }
                ideals.Height = ideals.Items.Count * ideals.ItemHeight + 10;

                backt.Add(bondlabel);
                backt.Add(bonds);
                bonds.Items.Clear();
                bonds.ForeColor = System.Drawing.SystemColors.WindowText;
                if (Program.Context.Player.Bond == null || Program.Context.Player.Bond == "")
                {
                    if (back != null) bonds.Items.AddRange(back.Bond.ToArray<TableEntry>());
                    foreach (TableDescription td in tables) if (td.BackgroundOption.HasFlag(BackgroundOption.Bond)) bonds.Items.AddRange(td.Entries.ToArray<TableEntry>());
                    bonds.Items.Add("- Custom Bond -");
                }
                else
                {
                    bonds.ForeColor = Config.SelectColor;
                    bonds.Items.Add(Program.Context.Player.Bond);
                }
                bonds.Height = bonds.Items.Count * bonds.ItemHeight + 10;

                backt.Add(flawlabel);
                backt.Add(flaws);
                flaws.Items.Clear();
                flaws.ForeColor = System.Drawing.SystemColors.WindowText;
                if (Program.Context.Player.Flaw == null || Program.Context.Player.Flaw == "")
                {
                    if (back != null) flaws.Items.AddRange(back.Flaw.ToArray<TableEntry>());
                    foreach (TableDescription td in tables) if (td.BackgroundOption.HasFlag(BackgroundOption.Flaw)) flaws.Items.AddRange(td.Entries.ToArray<TableEntry>());
                    flaws.Items.Add("- Custom Flaw -");
                }
                else
                {
                    flaws.ForeColor = Config.SelectColor;
                    flaws.Items.Add(Program.Context.Player.Flaw);
                }
                flaws.Height = flaws.Items.Count * flaws.ItemHeight + 10;



                backt.Reverse();
                foreach (Control c in backt) backtab.Controls.Add(c);
                backtab.ResumeLayout(true);
                layouting = false;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating Background");
            }
        }

        public void UpdateFormsCompanions()
        {
            string selected = null;
            if (FormsCompanionOptions.SelectedItem is FormsCompanionInfo fci)
            {
                selected = fci.ID;
            }
            FormsCompanionOptions.Items.Clear();
            FormsCompanionsBox.Items.Clear();
            int i = 0;
            var fcs = Program.Context.Player.GetFormsCompanionChoices();
            if (fcs.Count > 0 && Program.Context.MonstersSimple.Count == 0)
            {
                Program.Context.ImportMonsters(true);
                fcs = Program.Context.Player.GetFormsCompanionChoices();
            }
            foreach (FormsCompanionInfo fc in fcs )
            {
                FormsCompanionOptions.Items.Add(fc);
                if (StringComparer.Ordinal.Equals(fc.ID, selected)) FormsCompanionOptions.SelectedIndex = i;
                i++;
                foreach (Monster m in fc.AppliedChoices(Program.Context, Program.Context.Player.GetFinalAbilityScores()))
                {
                    if (m != null) FormsCompanionsBox.Items.Add(m);
                }
            }
            
        }

        public void UpdateSpellcastingOuter(bool updateside = true)
        {
            try
            {
                if (updateside) UpdateSideLayout();
                layouting = true;
                spellcontrol.SuspendLayout();
                inplayflow.SuspendLayout();
                List<Feature> spellfeatures = new List<Feature>(from f in Program.Context.Player.GetFeatures() where f is SpellcastingFeature || f is SpellChoiceFeature || f is ModifySpellChoiceFeature || f is IncreaseSpellChoiceAmountFeature select f);
                List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>();
                foreach (Feature f in spellfeatures) if (f is SpellcastingFeature) spellcasts.Add((SpellcastingFeature)f);
                int spindex = spellcontrol.SelectedIndex;
                spellcontrol.Controls.Clear();
                inplayflow.Controls.Clear();
                inplayflow.Controls.Add(statisticsbox);
                inplayflow.Controls.Add(skillsbox);
                inplayflow.Controls.Add(resourcebox);
                inplayflow.Controls.Add(featurebox);
                inplayflow.Controls.Add(conditionbox);
                inplayflow.Controls.Add(combatBox);
                inplayflow.Controls.Add(formsbox);
                foreach (SpellcastingFeature sf in spellcasts)
                {
                    if (sf.SpellcastingID == "MULTICLASS") continue;
                    TabPage tab = new TabPage(sf.DisplayName)
                    {
                        Name = sf.SpellcastingID
                    };
                    spellcontrol.Controls.Add(tab);
                    Control target = tab;
                    if (sf.Preparation == PreparationMode.ClassList || sf.Preparation == PreparationMode.Spellbook)
                    {
                        SplitContainer split = new SplitContainer()
                        {
                            Dock = DockStyle.Fill,
                            Orientation = Orientation.Horizontal,
                            Size = new System.Drawing.Size(100, 100),
                            SplitterDistance = 50,
                            IsSplitterFixed = true
                        };
                        target.Controls.Add(split);
                        target = split.Panel2;
                        SplitContainer split2 = new SplitContainer()
                        {
                            Dock = DockStyle.Fill,
                            Orientation = Orientation.Vertical,
                            Size = new System.Drawing.Size(100, 100),
                            SplitterDistance = 50,
                            IsSplitterFixed = true
                        };
                        split.Panel1.Controls.Add(split2);
                        split2.Panel1.Padding = new Padding(5);
                        split2.Panel2.Padding = new Padding(5);
                        GroupBox spellspreparedbox = new GroupBox()
                        {
                            Text = "Prepared Spells",
                            Name = sf.SpellcastingID + "=preparedbox",
                            Dock = DockStyle.Fill
                        };
                        ListBox spellsprepared = new ListBox()
                        {
                            Name = sf.SpellcastingID + "=prepared"
                        };
                        spellsprepared.SelectedIndexChanged += Choice_DisplaySpell;
                        spellsprepared.Leave += listbox_Deselect_on_Leave;
                        spellsprepared.DoubleClick += unprepare_Spell;
                        spellsprepared.Dock = DockStyle.Fill;
                        spellspreparedbox.Controls.Add(spellsprepared);
                        split2.Panel1.Controls.Add(spellspreparedbox);
                        ListBox spellsprepareable = new ListBox()
                        {
                            Dock = DockStyle.Fill,
                            Name = sf.SpellcastingID + "=prepareable"
                    };
                        spellsprepareable.SelectedIndexChanged += Choice_DisplaySpell;
                        spellsprepareable.Leave += listbox_Deselect_on_Leave;
                        spellsprepareable.DoubleClick += prepare_Spell;
                        GroupBox spellsprepareablebox = new GroupBox()
                        {
                            Text = (sf.Preparation == PreparationMode.ClassList ? "Available Spells" : "Spellbook"),
                            Name = sf.SpellcastingID + "=prepareablebox",
                            Dock = DockStyle.Fill
                        };
                        spellsprepareablebox.Controls.Add(spellsprepareable);
                        split2.Panel2.Controls.Add(spellsprepareablebox);
                    }
                    SplitContainer split3 = new SplitContainer()
                    {
                        Dock = DockStyle.Fill,
                        Orientation = Orientation.Horizontal,
                        Size = new System.Drawing.Size(100, 100),
                        SplitterDistance = 50,
                        IsSplitterFixed = true
                    };
                    target.Controls.Add(split3);
                    SplitContainer split4 = new SplitContainer()
                    {
                        Dock = DockStyle.Fill,
                        Orientation = Orientation.Vertical,
                        Size = new System.Drawing.Size(100, 100),
                        SplitterDistance = 50,
                        IsSplitterFixed = true
                    };
                    split3.Panel2.Controls.Add(split4);
                    split4.Panel1.Padding = new Padding(5);
                    split4.Panel2.Padding = new Padding(5);

                    split3.Panel1.Padding = new Padding(5);
                    GroupBox spellchoicesbox = new GroupBox()
                    {
                        Text = "Available Choices:",
                        Name = sf.SpellcastingID + "=choicesbox",
                        Dock = DockStyle.Fill
                    };
                    ListBox spellchoices = new ListBox()
                    {
                        Name = sf.SpellcastingID + "=choices",
                        Dock = DockStyle.Fill
                };
                    spellchoices.SelectedIndexChanged += Choice_DisplaySpellChoices;
                    spellchoicesbox.Controls.Add(spellchoices);
                    split3.Panel1.Controls.Add(spellchoicesbox);

                    GroupBox spellchosenbox = new GroupBox()
                    {
                        Text = "Selected Spells:",
                        Name = sf.SpellcastingID + "=chosenbox",
                        Dock = DockStyle.Fill
                    };
                    ListBox spellschosen = new ListBox()
                    {
                        Name = sf.SpellcastingID + "=chosen",
                        Dock = DockStyle.Fill
                    };
                    spellschosen.SelectedIndexChanged += Choice_DisplaySpell;
                    spellschosen.Leave += listbox_Deselect_on_Leave;
                    spellschosen.DoubleClick += deselect_Spell;
                    spellchosenbox.Controls.Add(spellschosen);
                    split4.Panel1.Controls.Add(spellchosenbox);
                    ListBox spelltochoose = new ListBox()
                    {
                        Dock = DockStyle.Fill
                    };
                    spelltochoose.SelectedIndexChanged += Choice_DisplaySpell;
                    spelltochoose.Leave += listbox_Deselect_on_Leave;
                    spelltochoose.DoubleClick += select_Spell;
                    GroupBox spelltochoosebox = new GroupBox()
                    {
                        Text = "Available Spells:",
                        Name = sf.SpellcastingID + "=choosebox",
                        Dock = DockStyle.Fill
                    };
                    spelltochoosebox.Controls.Add(spelltochoose);
                    split4.Panel2.Controls.Add(spelltochoosebox);
                    spelltochoose.Name = sf.SpellcastingID + "=choose";

                    GroupBox box = new GroupBox()
                    {
                        Name = sf.SpellcastingID,
                        Size = new System.Drawing.Size(230, 457),
                        Text = "Spellcasting - " + sf.DisplayName
                    };
                    Label attacksave = new Label()
                    {
                        AutoEllipsis = true,
                        Location = new Point(6, 16),
                        Name = ((int)sf.SpellcastingAbility).ToString(),
                        Size = new System.Drawing.Size(218, 13),
                        Text = Enum.GetName(typeof(Ability), sf.SpellcastingAbility) + ": " + plusMinus(Program.Context.Player.GetSpellAttack(sf.SpellcastingID, sf.SpellcastingAbility)) + " | DC " + Program.Context.Player.GetSpellSaveDC(sf.SpellcastingID, sf.SpellcastingAbility),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    box.Controls.Add(attacksave);

                    Button reset = new Button()
                    {
                        Enabled = false,
                        Location = new Point(12, 425),
                        Name = "ResetSlots",
                        Size = new System.Drawing.Size(50, 23),
                        Text = "Reset"
                    };
                    reset.Click += new EventHandler(button1_Click_2);
                    box.Controls.Add(reset);

                    NumericUpDown slotused = new NumericUpDown()
                    {
                        Enabled = false,
                        Location = new Point(12, 399),
                        Name = "SpellSlotUsed",
                        Size = new System.Drawing.Size(50, 20)
                    };
                    slotused.ValueChanged += new EventHandler(numericUpDown1_ValueChanged);
                    box.Controls.Add(slotused);

                    Label spellonsheet = new Label()
                    {
                        AutoEllipsis = true,
                        AutoSize = true,
                        Location = new Point(6, 363),
                        Name = "SpellOnSheetLabel",
                        Text = "Spell on Sheet: --",
                        Cursor = Cursors.Hand
                    };
                    spellonsheet.DoubleClick += new EventHandler(label_DoubleClick);
                    box.Controls.Add(spellonsheet);

                    ListBox spellslots = new ListBox()
                    {
                        ColumnWidth = 60,
                        FormattingEnabled = true,
                        Location = new Point(68, 382),
                        MultiColumn = true,
                        Name = "SpellSlotBox",
                        Size = new System.Drawing.Size(156, 69)
                    };
                    spellslots.SelectedIndexChanged += new EventHandler(listBox1_IndexChanged);
                    spellslots.DoubleClick += new EventHandler(listBox1_DoubleClick);
                    spellslots.MouseWheel += listbox_MouseWheel;
                    box.Controls.Add(spellslots);

                    Label label = new Label()
                    {
                        AutoSize = true,
                        Location = new Point(9, 382),
                        Size = new System.Drawing.Size(56, 13),
                        Text = "Slots Used:"
                    };
                    box.Controls.Add(label);

                    ListBox spells = new ListBox()
                    {
                        FormattingEnabled = true,
                        Location = new Point(6, 31),
                        Name = "SpellsBox",
                        Size = new System.Drawing.Size(218, 329)
                    };
                    spells.SelectedIndexChanged += new EventHandler(Choice_DisplayModifiedSpell);
                    spells.Leave += new EventHandler(listbox_Deselect_on_Leave);
                    spells.DoubleClick += new EventHandler(listBox2_DoubleClick);
                    spells.MouseWheel += listbox_MouseWheel;
                    box.Controls.Add(spells);

                    inplayflow.Controls.Add(box);

                }
                if (spindex < spellcontrol.TabCount && spindex >= 0) spellcontrol.SelectedIndex = spindex;
                spellcontrol.ResumeLayout();
                inplayflow.ResumeLayout();
                layouting = false;
                UpdateSpellcastingInner(false, spellfeatures);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while creating Spellcasting Layout");
            }
        }

        private void deselect_Spell(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            Control tab = spellcontrol.SelectedTab;
            if (lb.SelectedItem != null && lb.SelectedItem is Spell && tab != null)
            {
                Control[] choices = tab.Controls.Find(tab.Name + "=choices", true);
                ListBox choicebox = (ListBox)choices[0];
                if (choicebox != null && choicebox.SelectedItem != null && choicebox.SelectedItem is SpellChoiceCapsule && ((SpellChoiceCapsule)choicebox.SelectedItem).Spellchoicefeature != null)
                {
                    Program.Context.MakeHistory("");
                    SpellChoiceFeature scf = ((SpellChoiceCapsule)choicebox.SelectedItem).Spellchoicefeature;
                    string r = ((Spell)lb.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Spell)lb.SelectedItem).Source;
                    Program.Context.Player.GetSpellChoice(tab.Name, scf.UniqueID).Choices.RemoveAll(t => ConfigManager.SourceInvariantComparer.Equals(t, r));
                    //UpdateSpellChoices(choicebox);
                    UpdateSpellcastingInner();
                    UpdateFormsCompanions();
                    UpdateInPlayInner();
                }
            }
        }
        private void select_Spell(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            Control tab = spellcontrol.SelectedTab;
            if (lb.SelectedItem != null && lb.SelectedItem is Spell && tab != null)
            {
                Control[] choices = tab.Controls.Find(tab.Name + "=choices", true);
                ListBox choicebox = (ListBox)choices[0];
                if (choicebox != null && choicebox.SelectedItem != null && choicebox.SelectedItem is SpellChoiceCapsule && ((SpellChoiceCapsule)choicebox.SelectedItem).Spellchoicefeature != null)
                {
                    SpellChoiceFeature scf = ((SpellChoiceCapsule)choicebox.SelectedItem).Spellchoicefeature;
                    List<Feature> spellfeatures = new List<Feature>(from f in Program.Context.Player.GetFeatures() where f is SpellcastingFeature || f is SpellChoiceFeature || f is ModifySpellChoiceFeature || f is IncreaseSpellChoiceAmountFeature select f);
                    int amount = scf.Amount;
                    foreach (Feature f in spellfeatures) if (f is IncreaseSpellChoiceAmountFeature && ((IncreaseSpellChoiceAmountFeature)f).UniqueID == scf.UniqueID) amount += ((IncreaseSpellChoiceAmountFeature)f).Amount;
                    SpellChoice sc=Program.Context.Player.GetSpellChoice(tab.Name, scf.UniqueID);
                    if (sc.Choices.Count < amount)
                    {
                        Program.Context.MakeHistory("");
                        sc.Choices.Add(((Spell)lb.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Spell)lb.SelectedItem).Source);
                    }
                    //UpdateSpellChoices(choicebox);
                    UpdateSpellcastingInner(true, spellfeatures);
                    UpdateFormsCompanions();
                    UpdateInPlayInner();
                }
            }
        }

        private void Choice_DisplaySpellChoices(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            List<Feature> spellfeatures = new List<Feature>(from f in Program.Context.Player.GetFeatures() where f is SpellcastingFeature || f is SpellChoiceFeature || f is ModifySpellChoiceFeature || f is IncreaseSpellChoiceAmountFeature select f);
            if (lb.SelectedItem != null && lb.SelectedItem is SpellChoiceCapsule)
            {
                SpellChoiceFeature selected = ((SpellChoiceCapsule)lb.SelectedItem).Spellchoicefeature;
                if (selected != null)
                {
                    List<Feature> choicefeats = new List<Feature>() {selected};
                    foreach (Feature f in spellfeatures)
                    {
                        if (f is ModifySpellChoiceFeature && ((ModifySpellChoiceFeature)f).UniqueID == selected.UniqueID) choicefeats.Add(f);
                        if (f is IncreaseSpellChoiceAmountFeature && ((IncreaseSpellChoiceAmountFeature)f).UniqueID == selected.UniqueID) choicefeats.Add(f);
                    }
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(new FeatureContainer(choicefeats).ToHTML());
                    displayElement.Refresh();
                }
            }
            UpdateSpellChoices(lb, spellfeatures);

        }

        private void prepare_Spell(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem != null)
            {
                List<Feature> spellfeatures = new List<Feature>(from f in Program.Context.Player.GetFeatures() where f is SpellcastingFeature || f is SpellChoiceFeature || f is ModifySpellChoiceFeature || f is IncreaseSpellChoiceAmountFeature select f);
                SpellcastingFeature sf = null;
                foreach (Feature f in spellfeatures) if (f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcontrol.SelectedTab.Name) sf = (SpellcastingFeature)f;
                if (sf != null)
                {
                    Spellcasting sc = Program.Context.Player.GetSpellcasting(sf.SpellcastingID);
                    if (sc.GetPreparedList(Program.Context.Player, Program.Context).Count < Utils.AvailableToPrepare(Program.Context, sf, Program.Context.Player.GetClassLevel(sf.SpellcastingID)))
                    {
                        Program.Context.MakeHistory("");
                        sc.GetPreparedList(Program.Context.Player, Program.Context).Add(((Spell)lb.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Spell)lb.SelectedItem).Source);
                    }
                    UpdateSpellcastingInner(true, spellfeatures);
                    UpdateFormsCompanions();
                    UpdateInPlayInner();
                }
            }
        }

        private void unprepare_Spell(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem != null)
            {
                Spellcasting sc = Program.Context.Player.GetSpellcasting(spellcontrol.SelectedTab.Name);
                Program.Context.MakeHistory("");
                string r = ((Spell)lb.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Spell)lb.SelectedItem).Source;
                sc.GetPreparedList(Program.Context.Player, Program.Context).RemoveAll(s => ConfigManager.SourceInvariantComparer.Equals(s, r));
                UpdateSpellcastingInner();
                UpdateFormsCompanions();
                UpdateInPlayInner();
            }
        }
        public void UpdateSpellcastingInner(bool updateside = true, List<Feature> spellfeatures=null)
        {
            try
            {
                layouting = true;
                if (spellfeatures == null) spellfeatures = new List<Feature>(from f in Program.Context.Player.GetFeatures() where f is SpellcastingFeature || f is SpellChoiceFeature || f is ModifySpellChoiceFeature || f is IncreaseSpellChoiceAmountFeature select f);
                List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>();
                foreach (Feature f in spellfeatures) if (f is SpellcastingFeature) spellcasts.Add((SpellcastingFeature)f);
                if (updateside) UpdateSideLayout();
                foreach (SpellcastingFeature sf in spellcasts)
                {
                    Spellcasting sc = Program.Context.Player.GetSpellcasting(sf.SpellcastingID);
                    Control tab = null;
                    foreach (Control tp in spellcontrol.Controls) if (tp.Name == sf.SpellcastingID) tab = (Control)tp;
                    
                    if (tab != null)
                    {
                        int classlevel = Program.Context.Player.GetClassLevel(sf.SpellcastingID);
                        SpellChoiceCapsule bonusprepared = null;
                        if (sf.Preparation == PreparationMode.ClassList || sf.Preparation == PreparationMode.Spellbook)
                        {
                            Control[] prepare = tab.Controls.Find(sf.SpellcastingID + "=prepared", true);
                            if (prepare.Count() > 0)
                            {
                                ListBox prep = (ListBox)prepare[0];
                                Control[] preparebox = tab.Controls.Find(sf.SpellcastingID + "=preparedbox", true);
                                if (preparebox.Count() > 0)
                                {
                                    preparebox[0].Text = "Prepared Spells (" + sc.GetPreparedList(Program.Context.Player, Program.Context).Count + "/" + Utils.AvailableToPrepare(Program.Context, sf, classlevel) + ")";
                                }
                                List<Spell> preparedspells = new List<Spell>(sc.GetPrepared(Program.Context.Player, Program.Context));
                                //List<Spell> preparedspells = new List<Spell>(from s in sc.Prepared select Program.Context.GetSpell(s));
                                prep.Items.Clear();
                                prep.Items.AddRange(preparedspells.ToArray<Spell>());
                                if (sf.Preparation == PreparationMode.ClassList)
                                {
                                    Control[] prepareable = tab.Controls.Find(sf.SpellcastingID + "=prepareable", true);
                                    if (prepareable.Count() > 0)
                                    {
                                        ListBox prepable = (ListBox)prepareable[0];
                                        prepable.Items.Clear();
                                        List<Spell> prepableSpells = new List<Spell>(sc.GetAdditionalClassSpells(Program.Context.Player, Program.Context));
                                        prepableSpells.AddRange(Utils.FilterSpell(Program.Context, sf.PrepareableSpells, sf.SpellcastingID, classlevel));
                                        prepableSpells.Sort();
                                        prepable.Items.AddRange(prepableSpells.Where(s => !preparedspells.Exists(t => t.Name == s.Name && s.Source == t.Source)).ToArray<Spell>());
                                    }
                                }
                                else if (sf.Preparation == PreparationMode.Spellbook)
                                {
                                    Control[] prepareable = tab.Controls.Find(sf.SpellcastingID + "=prepareable", true);
                                    if (prepareable.Count() > 0)
                                    {
                                        ListBox prepable = (ListBox)prepareable[0];
                                        prepable.Items.Clear();
                                        prepable.Items.AddRange(sc.GetSpellbook(Program.Context.Player, Program.Context).Where(s => !preparedspells.Exists(t => t.Name == s.Name && s.Source == t.Source)).ToArray<Spell>());
                                    }
                                }
                            }
                        } else if (sc.GetPrepared(Program.Context.Player, Program.Context).Count() > 0) {
                            bonusprepared = new SpellChoiceCapsule(null)
                            {
                                CalculatedChoices = sc.GetPrepared(Program.Context.Player, Program.Context).ToList<Spell>()
                            };
                            bonusprepared.CalculatedAmount = bonusprepared.CalculatedChoices.Count;
                        }
                        Control[] choices = tab.Controls.Find(sf.SpellcastingID + "=choices", true);
                        if (choices.Count() > 0)
                        {
                            ListBox choice = (ListBox)choices[0];
                            int index = choice.SelectedIndex;
                            choice.Items.Clear();
                            List<SpellChoiceCapsule> scfs = new List<SpellChoiceCapsule>(from f in spellfeatures where f is SpellChoiceFeature && ((SpellChoiceFeature)f).SpellcastingID == sf.SpellcastingID select new SpellChoiceCapsule((SpellChoiceFeature)f));
                            foreach (SpellChoiceCapsule scf in scfs)
                            {
                                scf.CalculatedChoices = Program.Context.Player.GetSpellChoice(sf.SpellcastingID, scf.Spellchoicefeature.UniqueID).Choices.Select(t => Program.Context.GetSpell(t, scf.Spellchoicefeature.Source)).ToList();
                                scf.CalculatedAmount = scf.Spellchoicefeature.Amount;
                                foreach (Feature f in spellfeatures) if (f is IncreaseSpellChoiceAmountFeature && ((IncreaseSpellChoiceAmountFeature)f).UniqueID == scf.Spellchoicefeature.UniqueID) scf.CalculatedAmount += ((IncreaseSpellChoiceAmountFeature)f).Amount;
                            }
                            choice.Items.AddRange(scfs.ToArray<SpellChoiceCapsule>());
                            if (bonusprepared != null) choice.Items.Add(bonusprepared);
                            if (spellcontrol.SelectedTab == tab)
                            {
                                if (index >= 0 && index < choice.Items.Count) choice.SelectedIndex = index;
                                else UpdateSpellChoices(choice, spellfeatures);
                            }
                        }
                    }
                }
                layouting = false;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating Spellcasting Layout");
            }
        }

        private void UpdateSpellChoices(ListBox lb, List<Feature> spellfeatures=null)
        {
            try
            {
                if (lb.SelectedItem != null && lb.SelectedItem is SpellChoiceCapsule)
                {
                    bool waslayouting = layouting;
                    if (!layouting) layouting = true;
                    SpellChoiceFeature scf = ((SpellChoiceCapsule)lb.SelectedItem).Spellchoicefeature;
                    if (spellfeatures == null) spellfeatures = new List<Feature>(from f in Program.Context.Player.GetFeatures() where f is SpellcastingFeature || f is SpellChoiceFeature || f is ModifySpellChoiceFeature || f is IncreaseSpellChoiceAmountFeature select f);
                    SpellcastingFeature sf = null;
                    foreach (Feature f in spellfeatures) if (f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcontrol.SelectedTab.Name) sf = (SpellcastingFeature)f;
                    if (sf != null)
                    {
                        if (scf != null)
                        {
                            int classlevel = Program.Context.Player.GetClassLevel(sf.SpellcastingID);
                            List<Spell> available = new List<Spell>(Utils.FilterSpell(Program.Context, scf.AvailableSpellChoices, sf.SpellcastingID, classlevel));
                            List<Spell> chosen = new List<Spell>(Program.Context.Player.GetSpellChoice(sf.SpellcastingID, scf.UniqueID).Choices.Select(t => Program.Context.GetSpell(t, scf.Source)));
                            int amount = scf.Amount;
                            foreach (Feature f in spellfeatures)
                            {
                                if (f is ModifySpellChoiceFeature msf && msf.UniqueID == scf.UniqueID)
                                {
                                    if (msf.AdditionalSpellChoices != "false") available.AddRange(Utils.FilterSpell(Program.Context, msf.AdditionalSpellChoices, sf.SpellcastingID, classlevel));
                                    if (msf.AdditionalSpells != null && msf.AdditionalSpells.Count > 0) available.AddRange(Program.Context.Spells.Values.Where(s => msf.AdditionalSpells.FirstOrDefault(ss => StringComparer.InvariantCultureIgnoreCase.Equals(s.Name, ss)) != null));
                                }
                                if (f is IncreaseSpellChoiceAmountFeature && ((IncreaseSpellChoiceAmountFeature)f).UniqueID == scf.UniqueID) amount += ((IncreaseSpellChoiceAmountFeature)f).Amount;
                            }
                            available.Sort();
                            chosen.Sort();
                            Control tab = null;
                            foreach (Control tp in spellcontrol.Controls) if (tp.Name == sf.SpellcastingID) tab = (Control)tp;
                            if (tab != null)
                            {
                                available.RemoveAll(t => chosen.Exists(s => s.Name == t.Name && s.Source == t.Source));
                                Control[] chose = tab.Controls.Find(sf.SpellcastingID + "=chosen", true);
                                ListBox cho = (ListBox)chose[0];
                                cho.Items.Clear();
                                cho.Items.AddRange(chosen.ToArray<Spell>());

                                Control[] chosebox = tab.Controls.Find(sf.SpellcastingID + "=chosenbox", true);
                                chosebox[0].Text = "Selected Spells (" + chosen.Count + "/" + amount + ")";

                                Control[] choose = tab.Controls.Find(sf.SpellcastingID + "=choose", true);
                                ListBox choo = (ListBox)choose[0];
                                choo.Items.Clear();
                                choo.Items.AddRange(available.ToArray<Spell>());
                            }
                        } else
                        {
                            Control tab = null;
                            foreach (Control tp in spellcontrol.Controls) if (tp.Name == sf.SpellcastingID) tab = (Control)tp;
                            if (tab != null)
                            {
                                Control[] chose = tab.Controls.Find(sf.SpellcastingID + "=chosen", true);
                                ListBox cho = (ListBox)chose[0];
                                cho.Items.Clear();
                                cho.Items.AddRange(((SpellChoiceCapsule)lb.SelectedItem).CalculatedChoices.ToArray<Spell>());

                                Control[] chosebox = tab.Controls.Find(sf.SpellcastingID + "=chosenbox", true);
                                chosebox[0].Text = "Spells (" + ((SpellChoiceCapsule)lb.SelectedItem).CalculatedChoices.Count + ")";

                                Control[] choose = tab.Controls.Find(sf.SpellcastingID + "=choose", true);
                                ListBox choo = (ListBox)choose[0];
                                choo.Items.Clear();
                            }
                        }
                    }
                    if (!waslayouting) layouting = false;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating Spell Layout");
            }
        }
        public void UpdatePersonal(bool updateside = true)
        {
            try
            {
                if (updateside) UpdateSideLayout();
                layouting = true;
                portraitBox.Image = Program.Context.Player.GetPortrait();
                characterName.Text = Program.Context.Player.Name;
                XP.Minimum = Program.Context.Player.GetXP(true);
                XP.Value = Program.Context.Player.GetXP();
                XPLabel.Text = Program.Context.Player.Advancement ? "Checkpoints:" : "Experience:";
                Alignment.Text = Program.Context.Player.Alignment;
                PlayerName.Text = Program.Context.Player.PlayerName;
                DCI.Text = Program.Context.Player.DCI;
                XPtoUP.Value = Program.Context.Levels.XpToLevelUp(Program.Context.Player.GetXP(), Program.Context.Player.Advancement);
                Age.Value = Program.Context.Player.Age;
                HeightValue.Text = Program.Context.Player.Height;
                Weight.Value = Program.Context.Player.Weight;
                Eyes.Text = Program.Context.Player.Eyes;
                Skin.Text = Program.Context.Player.Skin;
                Hair.Text = Program.Context.Player.Hair;
                FactionName.Text = Program.Context.Player.FactionName;
                factionRank.Text = Program.Context.Player.FactionRank;
                FactionInsignia.Image = Program.Context.Player.GetFactionImage();
                Backstory.Text = Program.Context.Player.Backstory;
                Allies.Text = Program.Context.Player.Allies;
                journalentrybox.Items.Clear();
                journalentrybox.Items.AddRange((from s in Program.Context.Player.Journal select s.IndexOfAny(new char[] { '\r', '\n' }) == -1 ? s : s.Substring(0, s.IndexOfAny(new char[] { '\r', '\n' }))).ToArray());
                journalbox.Text = "";
                layouting = false;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating Personal Layout");
            }
        }
        public void UpdateAbilityFeatList()
        {
            try
            {
                if (AbilityFeatChoiceBox.SelectedItem == null)
                {
                    AbilityFeatBox.Items.Clear();
                }
                else
                {
                    AbilityScoreFeatFeature asff = ((AbilityFeatChoiceContainer)AbilityFeatChoiceBox.SelectedItem).ASFF;
                    List<string> taken = new List<string>(Program.Context.Player.GetFeatNames());
                    foreach (Feature f in Program.Context.Player.GetFeatures()) if (f is CollectionChoiceFeature && (((CollectionChoiceFeature)f).Collection == null || ((CollectionChoiceFeature)f).Collection == "")) taken.AddRange(((CollectionChoiceFeature)f).Choices(Program.Context.Player));
                    string ff = Program.Context.Player.GetAbilityFeatChoice(asff).Feat;
                    taken.RemoveAll(s => ConfigManager.SourceInvariantComparer.Equals(s, ff));
                    AbilityFeatBox.Items.Clear();
                    AbilityFeatBox.Items.Add("+1 Strength");
                    AbilityFeatBox.Items.Add("+1 Dexterity");
                    AbilityFeatBox.Items.Add("+1 Constitution");
                    AbilityFeatBox.Items.Add("+1 Intelligence");
                    AbilityFeatBox.Items.Add("+1 Wisdom");
                    AbilityFeatBox.Items.Add("+1 Charisma");
                    int level = Program.Context.Player.GetLevel();
                    AbilityFeatBox.Items.AddRange(Program.Context.GetFeatureCollection("").Where(f => !taken.Contains(f.Name) && f.Level <= level).ToArray<Feature>());
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating Abilities and Feats");
            }
        }
        public void UpdateClassesBox()
        {
            bool waslayouting = layouting;
            if (!layouting) layouting = true;
            classesBox.Items.Clear();
            hpSpinner.Enabled = false;
            if (classList.SelectedItem != null)
            {
                ClassInfo ci = (ClassInfo)classList.SelectedItem;
                var cls = Program.Context.GetClasses(ci.Level, Program.Context.Player).OrderBy(s => s.Name).ToArray<ClassDefinition>();
                classesBox.Items.AddRange(cls);
                if (ci.Class != null)
                {
                    hpSpinner.Minimum = 0;
                    hpSpinner.Maximum = ci.Class.HitDieCount * Math.Max(1, ci.Class.HitDie);
                    hpSpinner.Value = ci.Hp;
                    hpSpinner.Enabled = true;
                    //for( int i = 0; i < cls.Length; i++)
                    //{
                    //    if (cls[i] == ci.Class)
                    //    {
                    //        classesBox.SelectedIndex = i;
                    //        break;
                    //    }
                    //}
                }
            }
            if (!waslayouting) layouting = false;
        }
        public void UpdateClass(bool updateside = true)
        {
            try
            {
                if (updateside) UpdateSideLayout();
                layouting = true;
                ClassPanel.SuspendLayout();
                ClassPanel.Controls.Clear();
                List<Control> classt = new List<Control>();
                List<SubClassFeature> subclass = new List<SubClassFeature>(from Feature f in Program.Context.Player.GetFeatures() where f is SubClassFeature select (SubClassFeature)f);
                int index = classList.SelectedIndex;
                int top = classList.TopIndex;
                classList.Items.Clear();
                classList.Items.AddRange(Program.Context.Player.GetClassInfos().ToArray<ClassInfo>());
                if (index >= 0 && index < classList.Items.Count)
                {
                    classList.SelectedIndex = index;
                    int visibleItems = classList.ClientSize.Height / classList.ItemHeight;
                    if (top >= 0 && top < classList.Items.Count - visibleItems)
                    {
                        classList.TopIndex = top;
                    }
                    
                    //classList.TopIndex = Math.Max(index - (visibleItems / 2) + 1, 0);
                }
                else UpdateClassesBox();
                //List<ClassDefinition> classes = Program.Context.Player.getClassesByLevel();
                //List<int> hprolls = Program.Context.Player.getHProllsByLevel();
                /*for (int c = 0; c < classes.Count; c++)
                {
                    Label l=new Label();
                    l.AutoSize = true;
                    l.Dock = System.Windows.Forms.DockStyle.Top;
                    l.Name = "levellabel" + (c + 1);
                    l.Padding = new System.Windows.Forms.Padding(0, 8, 0, 3);
                    l.Text = "Level " + (c + 1) + ":";
                    classt.Add(l);
                    if (classes[c] == null)
                    {
                        ListBox clb = new ListBox();
                        clb.Dock = System.Windows.Forms.DockStyle.Top;
                        clb.FormattingEnabled = true;
                        clb.Name = "classBox" + (c + 1);
                        clb.SelectedIndexChanged += new System.EventHandler(this.Choice_DisplayClass);
                        clb.DoubleClick += new System.EventHandler(this.classBox1_DoubleClick);
                        clb.Leave += new System.EventHandler(this.listbox_Deselect_on_Leave);
                        clb.Items.AddRange(ClassDefinition.GetClasses(c+1).ToArray<ClassDefinition>());
                        clb.Height = clb.Items.Count * clb.ItemHeight + 10;
                        classt.Add(clb);
                    }
                    else
                    {
                        HPUpDown hud = new HPUpDown(classes[c].ToString() + " (Hitpoint Roll): ");
                        hud.Dock = System.Windows.Forms.DockStyle.Top;
                        hud.Maximum = classes[c].HitDie;
                        hud.Name = "hitpointspinner" + (c + 1);
                        hud.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                        hud.Value = hprolls[c];
                        hud.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
                        hud.DoubleClick += new System.EventHandler(this.numericUpDown1_DoubleClick);
                        classt.Add(hud);
                        if (subclass.Where(sc => sc.ParentClass == classes[c].Name).Count() > 0)
                        {
                            subclass.RemoveAll(sc => sc.ParentClass == classes[c].Name);*/
                foreach (SubClassFeature sc in subclass)
                {
                    Label scl = new Label()
                    {
                        AutoSize = true,
                        Dock = System.Windows.Forms.DockStyle.Top,
                        Name = "subclasslabel" + sc.ParentClass,
                        Padding = new System.Windows.Forms.Padding(0, 8, 0, 3),
                        Text = "Choose a Subclass for " + sc.ParentClass + ":"
                    };
                    classt.Add(scl);
                    ListBox scb = new ListBox()
                    {
                        Dock = System.Windows.Forms.DockStyle.Top,
                        Name = sc.ParentClass
                    };
                    scb.SelectedIndexChanged += new EventHandler(Choice_DisplaySubClass);
                    scb.DoubleClick += new EventHandler(subclassbox_DoubleClick);
                    scb.Leave += new EventHandler(listbox_Deselect_on_Leave);
                    scb.MouseWheel += listbox_MouseWheel;
                    SubClass scs = Program.Context.Player.GetSubclass(sc.ParentClass);
                    if (scs == null) scb.Items.AddRange(Program.Context.SubClassFor(sc.ParentClass).OrderBy(s=>s.Name).ToArray<SubClass>());
                    else
                    {
                        scb.ForeColor = Config.SelectColor;
                        scb.Items.Add(scs);
                    }
                    scb.Height = scb.Items.Count * scb.ItemHeight + 10;
                    classt.Add(scb);
                }
                /*}
            }
        }*/
                int level = Program.Context.Player.GetLevel();
                ControlAdder.AddClassControls(Program.Context.Player, classt, level);
                foreach (Feature f in Program.Context.Player.GetCommonFeaturesAndFeats())
                {
                    ControlAdder.AddControl(classt, level, f);
                }
                classt.Reverse();
                foreach (Control c in classt) ClassPanel.Controls.Add(c);
                layouting = false;
                ClassPanel.ResumeLayout(true);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating Class Layout");
            }
        }

        private void subclassbox_DoubleClick(object sender, EventArgs e)
        {
            ListBox l = (ListBox)sender;
            Program.Context.MakeHistory("");
            if (Program.Context.Player.GetSubclass(l.Name) == null) Program.Context.Player.AddSubclass(l.Name, ((SubClass)l.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((SubClass)l.SelectedItem).Source);
            else Program.Context.Player.RemoveSubclass(l.Name);
            UpdateLayout();
        }

        private void Choice_DisplaySubClass(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is SubClass)
            {
                SubClass selected = (SubClass)choicer.SelectedItem;
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                }
            }
        }
        public void UpdateScores(bool updateside = true)
        {
            try
            {
                if (updateside) UpdateSideLayout();
                layouting = true;
                Strength.Value = Program.Context.Player.BaseStrength;
                Dexterity.Value = Program.Context.Player.BaseDexterity;
                Constitution.Value = Program.Context.Player.BaseConstitution;
                Intelligence.Value = Program.Context.Player.BaseIntelligence;
                Wisdom.Value = Program.Context.Player.BaseWisdom;
                Charisma.Value = Program.Context.Player.BaseCharisma;
                AbilityScoreArray scores = Program.Context.Player.GetFinalAbilityScores(out AbilityScoreArray max);
                StrengthFinal.Text = "=" + scores.Strength;
                StrengthMod.Text = "(" + plusMinus(AbilityScores.GetMod(scores.Strength)) + ")";
                DexterityFinal.Text = "=" + scores.Dexterity;
                DexterityMod.Text = "(" + plusMinus(AbilityScores.GetMod(scores.Dexterity)) + ")";
                ConstitutionFinal.Text = "=" + scores.Constitution;
                ConstitutionMod.Text = "(" + plusMinus(AbilityScores.GetMod(scores.Constitution)) + ")";
                IntelligenceFinal.Text = "=" + scores.Intelligence;
                IntelligenceMod.Text = "(" + plusMinus(AbilityScores.GetMod(scores.Intelligence)) + ")";
                WisdomFinal.Text = "=" + scores.Wisdom;
                WisdomMod.Text = "(" + plusMinus(AbilityScores.GetMod(scores.Wisdom)) + ")";
                CharismaFinal.Text = "=" + scores.Charisma;
                CharismaMod.Text = "(" + plusMinus(AbilityScores.GetMod(scores.Charisma)) + ")";
                StrengthMax.Text = "| " + max.Strength;
                DexterityMax.Text = "| " + max.Dexterity;
                ConstitutionMax.Text = "| " + max.Constitution;
                IntelligenceMax.Text = "| " + max.Intelligence;
                WisdomMax.Text = "| " + max.Wisdom;
                CharismaMax.Text = "| " + max.Charisma;
                PointBuyRemaining.Text = "Points left: " + Utils.GetPointsRemaining(Program.Context.Player, Program.Context);
                int index = AbilityFeatChoiceBox.SelectedIndex;
                AbilityFeatChoiceBox.Items.Clear();
                AbilityFeatChoiceBox.Items.AddRange((from asff in Program.Context.Player.GetAbilityIncreases() select new AbilityFeatChoiceContainer(Program.Context.Player, asff)).ToArray<AbilityFeatChoiceContainer>());
                if (index >= 0 && index < AbilityFeatChoiceBox.Items.Count) AbilityFeatChoiceBox.SelectedIndex = index;
                //splitContainer4.Panel1.SuspendLayout();
                //splitContainer4.Panel1.Controls.Clear();
                /*for (int c = increases-1; c >= 0; c--)
                {
                    RadioButton rb = new RadioButton();
                    rb.AutoSize = true;
                    rb.Dock = System.Windows.Forms.DockStyle.Top;
                    rb.Name = "AblilityFeat"+c;
                    rb.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
                    rb.Text = Program.Context.Player.AbilityFeatChoices[c].ToString();
                    rb.CheckedChanged += new System.EventHandler(this.AblilityFeat1_CheckedChanged);
                    if (c == abilityFeatSelected) rb.Checked = true;
                    splitContainer4.Panel1.Controls.Add(rb);
                }*/
                //splitContainer4.Panel1.ResumeLayout(true);
                layouting = false;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating Ability Score Layout");
            }
        }
        public void UpdateRaceLayout(bool updateside=true)
        {
            try
            {
                if (updateside) UpdateSideLayout();
                layouting = true;
                racetab.SuspendLayout();
                racetab.Controls.Clear();
                int level = Program.Context.Player.GetLevel();
                List<Control> racet = new List<Control>
                {
                    racelabel,
                    racebox
                };
                racebox.Items.Clear();
                racebox.ForeColor = System.Drawing.SystemColors.WindowText;
                Race rac = Program.Context.Player.Race;
                
                

                List<String> parentraces = new List<string>();
                foreach (Feature f in Program.Context.Player.GetFeatures().Where(f => f is SubRaceFeature)) parentraces.AddRange(((SubRaceFeature)f).Races);
                if (parentraces.Count > 0)
                {
                    racet.Add(subracelabel);
                    racet.Add(subracebox);
                    SubRace subrac = Program.Context.Player.SubRace;
                    subracebox.Items.Clear();
                    subracebox.ForeColor = System.Drawing.SystemColors.WindowText;
                    if (subrac == null) subracebox.Items.AddRange(Program.Context.SubRaceFor(parentraces).OrderBy(s => s.Name).ToArray<SubRace>());
                    else
                    {
                        subracebox.Items.Add(subrac);
                        subracebox.ForeColor = Config.SelectColor;
                        //subrac.AddControls(racet);
                        //foreach (Feature f in Program.Context.Player.getSubRaceFeatures()) f.AddControl(racet);
                    }
                    subracebox.Height = subracebox.Items.Count * subracebox.ItemHeight + 10;
                }
                if (rac == null) racebox.Items.AddRange(Program.Context.Races.Values.OrderBy(s => s.Name).ToArray<Race>());
                else
                {
                    racebox.Items.Add(rac);
                    racebox.ForeColor = Config.SelectColor;
                    ControlAdder.AddControls(rac, racet, level);
                    foreach (Feature f in Program.Context.Player.GetRaceFeatures(0, true).OrderBy(a => a.Level))
                    {
                        ControlAdder.AddControl(racet, level, f);
                    }
                }
                racebox.Height = racebox.Items.Count * racebox.ItemHeight + 10;
                racet.Reverse();
                foreach (Control c in racet) racetab.Controls.Add(c);
                racetab.ResumeLayout(true);
                layouting = false;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Internal Error while updating Race Layout");
            }
        }

        private void background_SelectedIndexChanged(object sender, EventArgs e)
        {
            Background selected = (Background)background.SelectedItem;
            if (selected != null)
            {
                displayElement.Navigate("about:blank");
                displayElement.Document.OpenNew(true);
                displayElement.Document.Write(selected.ToHTML());
                displayElement.Refresh();
            }
            
        }

        private void background_DoubleClick(object sender, EventArgs e)
        {
            Background selected = (Background)background.SelectedItem;
            if (selected != null)
            {
                Program.Context.MakeHistory("");
                if (Program.Context.Player.Background == null) Program.Context.Player.Background = selected;
                else Program.Context.Player.Background = null;
                UpdateLayout();
            }
        }

        private void traits_DoubleClick(object sender, EventArgs e)
        {
            if (traits.SelectedItem != null) {
                Program.Context.MakeHistory("");
                if (Program.Context.Player.PersonalityTrait == null || Program.Context.Player.PersonalityTrait == "")
                {
                    if (traits.SelectedIndex == traits.Items.Count - 1) Program.Context.Player.PersonalityTrait = Interaction.InputBox("Custom Personality Trait:", "CB 5");
                    else Program.Context.Player.PersonalityTrait = traits.SelectedItem.ToString();
                }
                else Program.Context.Player.PersonalityTrait = "";
                UpdateBackgroundLayout();
            }
        }

        private void traits2_DoubleClick(object sender, EventArgs e)
        {
            if (traits2.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
                if (Program.Context.Player.PersonalityTrait2 == null || Program.Context.Player.PersonalityTrait2 == "")
                {
                    if (traits2.SelectedIndex == traits2.Items.Count - 1) Program.Context.Player.PersonalityTrait2 = Interaction.InputBox("Custom Personality Trait:", "CB 5");
                    else Program.Context.Player.PersonalityTrait2 = traits2.SelectedItem.ToString();
                }
                else Program.Context.Player.PersonalityTrait2 = "";
                UpdateBackgroundLayout();
            }
        }

        private void ideals_DoubleClick(object sender, EventArgs e)
        {
            if (ideals.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
                if (Program.Context.Player.Ideal == null || Program.Context.Player.Ideal == "")
                {
                    if (ideals.SelectedIndex == ideals.Items.Count - 1) Program.Context.Player.Ideal = Interaction.InputBox("Custom Ideal:", "CB 5");
                    else Program.Context.Player.Ideal = ideals.SelectedItem.ToString();
                }
                else Program.Context.Player.Ideal = "";
                UpdateBackgroundLayout();
            }
        }

        private void bonds_DoubleClick(object sender, EventArgs e)
        {
            if (bonds.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
                if (Program.Context.Player.Bond == null || Program.Context.Player.Bond == "")
                {
                    if (bonds.SelectedIndex == bonds.Items.Count - 1) Program.Context.Player.Bond = Interaction.InputBox("Custom Bond:", "CB 5");
                    else Program.Context.Player.Bond = bonds.SelectedItem.ToString();
                }
                else Program.Context.Player.Bond = "";
                UpdateBackgroundLayout();
            }
        }

        private void flaws_DoubleClick(object sender, EventArgs e)
        {
            if (flaws.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
                if (Program.Context.Player.Flaw == null || Program.Context.Player.Flaw == "")
                {
                    if (flaws.SelectedIndex == flaws.Items.Count - 1) Program.Context.Player.Flaw = Interaction.InputBox("Custom Flaw:", "CB 5");
                    else Program.Context.Player.Flaw = flaws.SelectedItem.ToString();
                }
                else Program.Context.Player.Flaw = "";
                UpdateBackgroundLayout();
            }
        }

        public void listbox_Deselect_on_Leave(object sender, EventArgs e)
        {
            ((ListBox)sender).SelectedItem = null;
        }

        public void ChoiceCustom_DoubleClick(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
                Choice c = Program.Context.Player.GetChoice(choicer.Name);
                if (c == null || c.Value == "")
                {
                    bool old = ConfigManager.AlwaysShowSource;
                    ConfigManager.AlwaysShowSource = true;
                    if (choicer.SelectedIndex == choicer.Items.Count - 1) Program.Context.Player.SetChoice(choicer.Name,Interaction.InputBox("Custom Entry:", "CB 5"));
                    else Program.Context.Player.SetChoice(choicer.Name,choicer.SelectedItem.ToString());
                    ConfigManager.AlwaysShowSource = old;
                }
                else Program.Context.Player.RemoveChoice(choicer.Name);
                Program.MainWindow.UpdateLayout();
            }
        }
        public void Choice_DoubleClick(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
                Choice c = Program.Context.Player.GetChoice(choicer.Name);
                bool old = ConfigManager.AlwaysShowSource;
                ConfigManager.AlwaysShowSource = true;
                if (c == null || c.Value == "") Program.Context.Player.SetChoice(choicer.Name, choicer.SelectedItem.ToString());
                else Program.Context.Player.RemoveChoice(choicer.Name);
                ConfigManager.AlwaysShowSource = old;
                Program.MainWindow.UpdateLayout();
            }
        }

        public void Choice_DisplayFeature(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is Feature)
            {
                Feature selected = (Feature)choicer.SelectedItem;
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                }
            }
        }
        public void Choice_DisplayCondition(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is Condition)
            {
                Condition selected = (Condition)choicer.SelectedItem;
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                }
            }
        }
        public void Choice_DisplaySpell(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is Spell)
            {
                Spell selected = (Spell)choicer.SelectedItem;
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                }
            }
        }
        public void Choice_DisplaySpellScroll(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is Spell)
            {
                Scroll selected = new Scroll((Spell)choicer.SelectedItem);
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                }
            }
        }
        public void Choice_DisplayModifiedSpell(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is ModifiedSpell)
            {
                GroupBox box = (GroupBox)choicer.Parent;
                Ability spellcastingability = (Ability)int.Parse(box.Controls[0].Name);
                ModifiedSpell selected = (ModifiedSpell)choicer.SelectedItem;
                selected.Modifikations.AddRange(from f in Program.Context.Player.GetFeatures() where f is SpellModifyFeature && Utils.Matches(Program.Context, selected, ((SpellModifyFeature)f).Spells, null) select f);
                selected.Modifikations = selected.Modifikations.Distinct().ToList();
                selected.Info = Program.Context.Player.GetAttack(selected, (selected.differentAbility == Ability.None?spellcastingability:selected.differentAbility));
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                }
            }
        }

        public void Choice_DisplayMagicProperty(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is MagicProperty)
            {
                MagicProperty selected = (MagicProperty)choicer.SelectedItem;
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                }
            }
        }
        
        public void Choice_DisplayItem(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is Item)
            {
                Item selected = (Item)choicer.SelectedItem;
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                }
            }
        }
        public void Choice_DisplaySkill(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is Skill)
            {
                Skill selected = (Skill)choicer.SelectedItem;
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                }
            }
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is SkillInfo)
            {
                Skill selected = ((SkillInfo)choicer.SelectedItem).Skill;
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                    skillabilitybox.SelectedIndex = abilitytoindex(((SkillInfo)choicer.SelectedItem).Base);
                }
            }
        }
        public static int abilitytoindex(Ability a)
        {
            if (a.HasFlag(Ability.Strength)) return 0;
            if (a.HasFlag(Ability.Dexterity)) return 1;
            if (a.HasFlag(Ability.Constitution)) return 2;
            if (a.HasFlag(Ability.Intelligence)) return 3;
            if (a.HasFlag(Ability.Wisdom)) return 4;
            if (a.HasFlag(Ability.Charisma)) return 5;
            return -1;
        }
        public static Ability indextoability(int i)
        {
            if (i == 0) return Ability.Strength;
            if (i == 1) return Ability.Dexterity;
            if (i == 2) return Ability.Constitution;
            if (i == 3) return Ability.Intelligence;
            if (i == 4) return Ability.Wisdom;
            if (i == 5) return Ability.Charisma;
            return Ability.None;
        }
        public void Choice_DisplayLanguage(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is Language)
            {
                Language selected = (Language)choicer.SelectedItem;
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Context.UnsavedChanges == 0 || MessageBox.Show(Program.Context.UnsavedChanges + " unsaved changes will be lost. Continue?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OpenFileDialog od = new OpenFileDialog()
                {
                    Filter = "CB5 XML|*.cb5",
                    Title = "Open a Player File"
                };
                if (Properties.Settings.Default.LastCB5Folder != null && Properties.Settings.Default.LastCB5Folder != "") od.InitialDirectory = Properties.Settings.Default.LastCB5Folder;
                if (od.ShowDialog() == DialogResult.OK && od.FileName != "")
                {
                    Properties.Settings.Default.LastCB5Folder = Path.GetDirectoryName(od.FileName);
                    Properties.Settings.Default.Save();
                    try
                    {
                        using (FileStream fs = (FileStream)od.OpenFile())
                        {
                            Player newP = PlayerExtensions.Load(Program.Context, fs);
                            Program.Context.UndoBuffer = new LinkedList<Player>();
                            Program.Context.RedoBuffer = new LinkedList<Player>();
                            Program.Context.UnsavedChanges = 0;
                            Program.Context.Player = newP;

                        }
                        lastfile = od.FileName;
                        Program.Resetglobals();
                        UpdateLayout();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error opening "+od.FileName);
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastfile == "")
            {
                SaveFileDialog od = new SaveFileDialog()
                {
                    Filter = "CB5 XML|*.cb5",
                    Title = "Save a Player File"
                };
                if (Properties.Settings.Default.LastCB5Folder != null && Properties.Settings.Default.LastCB5Folder != "") od.InitialDirectory = Properties.Settings.Default.LastCB5Folder;
                if (od.ShowDialog() == DialogResult.OK && od.FileName != "")
                {
                    Properties.Settings.Default.LastCB5Folder = Path.GetDirectoryName(od.FileName);
                    Properties.Settings.Default.Save();
                    try
                    {
                        lastfile = od.FileName;
                        using (FileStream fs = (FileStream)od.OpenFile()) Program.Context.Player.Save(fs);
                        Program.Context.UnsavedChanges = 0;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error saving " + od.FileName);
                    }
                }
            }
            else
            {
                try
                {
                    using (FileStream fs = new FileStream(lastfile, FileMode.Truncate))
                    {
                        Program.Context.Player.Save(fs);
                        Program.Context.UnsavedChanges = 0;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error saving " + lastfile);
                }
            }
            UpdateLayout();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Context.UnsavedChanges == 0 || MessageBox.Show(Program.Context.UnsavedChanges + " unsaved changes will be lost. Continue?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lastfile = "";
                Player p = new Player();
                if (Properties.Settings.Default.DCI != null && Properties.Settings.Default.DCI != "") p.DCI = Properties.Settings.Default.DCI;
                if (Properties.Settings.Default.PlayerName != null && Properties.Settings.Default.PlayerName != "") p.PlayerName = Properties.Settings.Default.PlayerName;
                if (Properties.Settings.Default.EnabledSourcebooks != null && Properties.Settings.Default.EnabledSourcebooks.Count != 0)
                {
                    p.ExcludedSources = SourceManager.Sources.Where(s => !Properties.Settings.Default.EnabledSourcebooks.Contains(s)).ToList();
                }
                Program.Context.Player = p;
                Program.Context.UndoBuffer = new LinkedList<Player>();
                Program.Context.RedoBuffer = new LinkedList<Player>();
                Program.Context.UnsavedChanges = 0;
                Program.Resetglobals();
                UpdateLayout();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog od = new SaveFileDialog()
            {
                Filter = "CB5 XML|*.cb5",
                Title = "Save a Player File"
            };
            if (Properties.Settings.Default.LastCB5Folder != null && Properties.Settings.Default.LastCB5Folder != "") od.InitialDirectory = Properties.Settings.Default.LastCB5Folder;
            if (od.ShowDialog() == DialogResult.OK && od.FileName != "")
            {
                Properties.Settings.Default.LastCB5Folder = Path.GetDirectoryName(od.FileName);
                Properties.Settings.Default.Save();
                try
                {
                    using (FileStream fs = (FileStream)od.OpenFile()) Program.Context.Player.Save(fs);
                    Program.Context.UnsavedChanges = 0;
                    lastfile = od.FileName;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error saving " + od.FileName);
                }
            }
            UpdateLayout();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
                Application.Exit();
        }

        private void exportPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Export(lastfile).ShowDialog();
            //SaveFileDialog od = new SaveFileDialog();
            //if (Properties.Settings.Default.LastPDFFolder != null && Properties.Settings.Default.LastPDFFolder != "") od.InitialDirectory = Properties.Settings.Default.LastPDFFolder;
            //if (lastfile != null && lastfile != "")
            //{
            //    if (Properties.Settings.Default.LastPDFFolder == null || Properties.Settings.Default.LastPDFFolder == "") od.InitialDirectory = Path.GetDirectoryName(lastfile);
            //    od.FileName = Path.GetFileNameWithoutExtension(lastfile) + ".pdf";
            //}
            //od.Filter = "PDF|*.pdf";
            //od.Title = "Save a PDF File";
            //if (od.ShowDialog() == DialogResult.OK && od.FileName != "")
            //{
            //    Properties.Settings.Default.LastPDFFolder = Path.GetDirectoryName(od.FileName);
            //    Properties.Settings.Default.Save();
            //    try
            //    {
            //        using (FileStream fs = (FileStream)od.OpenFile())
            //        {
            //            PDFForms pdf = new PDFForms()
            //            {
            //                //IncludeActions = includeActionsInPDFToolStripMenuItem.Checked,
            //                //IncludeLog = PDFjournal.Checked,
            //                //IncludeResources = includeResourcesInSheetToolStripMenuItem.Checked,
            //                //IncludeSpellbook = PDFspellbook.Checked,
            //                //PreserveEdit = preservePDFFormsToolStripMenuItem.Checked,
            //                //IncludeMonsters = toolStripMenuItem1.Checked,
            //                OutStream = fs
            //            };
            //            await Config.PDFExporter.Export(Program.Context, pdf);
            //        }
            //        if (MessageBox.Show("PDF exported to: " + od.FileName + " Do you want to open it?", "CB5", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //        {
            //            Process.Start(od.FileName);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error exporting PDF to " + od.FileName);
            //    }
            //}
        }
        //private void defaultPDFToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Config.PDFExporter = PlayerExtensions.Load("DefaultPDF.xml");
        //    defaultPDFToolStripMenuItem.Checked = true;
        //    alternateToolStripMenuItem.Checked = false;
        //}

        //private void alternateToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Config.PDFExporter = PlayerExtensions.Load("AlternatePDF.xml");
        //    alternateToolStripMenuItem.Checked = true;
        //    defaultPDFToolStripMenuItem.Checked = false;
        //}

        private void racebox_DoubleClick(object sender, EventArgs e)
        {
            Race selected = (Race)racebox.SelectedItem;
            if (selected != null)
            {
                Program.Context.MakeHistory("");
                if (Program.Context.Player.Race == null) Program.Context.Player.Race = selected;
                else Program.Context.Player.Race = null;
                UpdateLayout();
            }
        }

        private void racebox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Race selected = (Race)racebox.SelectedItem;
            if (selected != null)
            {
                displayElement.Navigate("about:blank");
                displayElement.Document.OpenNew(true);
                displayElement.Document.Write(selected.ToHTML());
                displayElement.Refresh();
            }
        }

        private void subracebox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SubRace selected = (SubRace)subracebox.SelectedItem;
            if (selected != null)
            {
                displayElement.Navigate("about:blank");
                displayElement.Document.OpenNew(true);
                displayElement.Document.Write(selected.ToHTML());
                displayElement.Refresh();
            }
        }

        private void subracebox_DoubleClick(object sender, EventArgs e)
        {
            SubRace selected = (SubRace)subracebox.SelectedItem;
            if (selected != null)
            {
                Program.Context.MakeHistory("");
                if (Program.Context.Player.SubRace == null) Program.Context.Player.SubRace = selected;
                else Program.Context.Player.SubRace = null;
                UpdateLayout();
            }
        }

        private void characterName_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Name");
                Program.Context.Player.Name = characterName.Text;
                UpdateSideLayout();
            }
        }

        private void removePortrait_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("");
            Program.Context.Player.Portrait = null;
            UpdatePersonal();
        }

        private void choosePortrait_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Choose a Portrait"
            };
            List<String> extensions=new List<string>();
            foreach (string s in (from c in ImageCodecInfo.GetImageEncoders() select c.FilenameExtension)) extensions.AddRange(s.Split(';'));
            ofd.Filter = "Image Files | *." + String.Join(";", extensions);
            if (Properties.Settings.Default.LastImageFolder != null && Properties.Settings.Default.LastImageFolder != "") ofd.InitialDirectory = Properties.Settings.Default.LastImageFolder;
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName != "")
            {
                Properties.Settings.Default.LastImageFolder = Path.GetDirectoryName(ofd.FileName);
                Properties.Settings.Default.Save();
                try
                {
                    Program.Context.MakeHistory("");
                    Program.Context.Player.SetPortrait(new Bitmap(ofd.FileName));
                    UpdatePersonal();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error loading file " + ofd.FileName);
                }
            }
        }

        private void Alignment_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("");
                Program.Context.Player.Alignment = Alignment.Text;
            }
        }

        private void XP_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("XP");
                Decimal xp=Decimal.Round(XP.Value);
                if (XP.Value == xp)
                {
                    Program.Context.Player.SetXP((int)XP.Value);
                    UpdateLayout();
                }
                else if (XP.Value > xp) XP.Value = xp + Program.Context.Levels.XpToLevelUp((int)xp, Program.Context.Player.Advancement);
                else XP.Value = Math.Max(XP.Minimum, xp - Program.Context.Levels.XpToLevelDown((int)xp, Program.Context.Player.Advancement));
            }
        }

        private void PlayerName_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Playername");
                Program.Context.Player.PlayerName = PlayerName.Text;
            }
        }

        private void Age_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Age");
                Program.Context.Player.Age = (int)Age.Value;
            }
        }

        private void Weight_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Weight");
                Program.Context.Player.Weight = (int)Weight.Value;
            }
        }

        private void Height_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Height");
                Program.Context.Player.Height = HeightValue.Text;
            }
        }

        private void Eyes_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Eyes");
                Program.Context.Player.Eyes = Eyes.Text;
            }
        }

        private void Skin_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Skin");
                Program.Context.Player.Skin = Skin.Text;
            }
        }

        private void Hair_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Hair");
                Program.Context.Player.Hair = Hair.Text;
            }
        }

        private void FactionName_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Factionname");
                Program.Context.Player.FactionName = FactionName.Text;
            }
        }

        private void FactionBlank_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("");
            Program.Context.Player.FactionImage = null;
            UpdatePersonal();
        }

        private void FactionChoose_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Choose a Insignia"
            };
            List<String> extensions = new List<string>();
            foreach (string s in (from c in ImageCodecInfo.GetImageEncoders() select c.FilenameExtension)) extensions.AddRange(s.Split(';'));
            ofd.Filter = "Image Files | *." + String.Join(";", extensions);
            if (Properties.Settings.Default.LastImageFolder != null && Properties.Settings.Default.LastImageFolder != "") ofd.InitialDirectory = Properties.Settings.Default.LastImageFolder;
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName != "")
            {
                Properties.Settings.Default.LastImageFolder = Path.GetDirectoryName(ofd.FileName);
                Properties.Settings.Default.Save();
                try
                {
                    Program.Context.MakeHistory("");
                    Program.Context.Player.SetFactionImage(new Bitmap(ofd.FileName));
                    UpdatePersonal();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error loading file " + ofd.FileName);
                }
            }
        }

        private void Backstory_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Background");
                Program.Context.Player.Backstory = Backstory.Text;
            }
        }

        private void Allies_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Allies");
                Program.Context.Player.Allies = Allies.Text;
            }
        }
        public static string plusMinus(int x)
        {
            if (x < 0) return x.ToString();
            return "+" + x;
        }

        private void Score_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Score"+((Control)sender).Name);
                Program.Context.Player.BaseStrength = (int)Strength.Value;
                Program.Context.Player.BaseDexterity = (int)Dexterity.Value;
                Program.Context.Player.BaseConstitution = (int)Constitution.Value;
                Program.Context.Player.BaseIntelligence = (int)Intelligence.Value;
                Program.Context.Player.BaseWisdom = (int)Wisdom.Value;
                Program.Context.Player.BaseCharisma = (int)Charisma.Value;
                UpdateLayout();
            }
        }

        private void ArrayBox_DoubleClick(object sender, EventArgs e)
        {
            if (ArrayBox.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
                AbilityScoreArray a = (AbilityScoreArray)ArrayBox.SelectedItem;
                Program.Context.Player.BaseStrength = a.Strength;
                Program.Context.Player.BaseDexterity = a.Dexterity;
                Program.Context.Player.BaseConstitution = a.Constitution;
                Program.Context.Player.BaseIntelligence = a.Intelligence;
                Program.Context.Player.BaseWisdom = a.Wisdom;
                Program.Context.Player.BaseCharisma = a.Charisma;
                UpdateScores();
                ArrayBox.SelectedItem = null;
            }
        }

        private void Strength_MouseDown(object sender, MouseEventArgs e)
        {
            ((Control)sender).DoDragDrop(((NumericUpDown)sender).Name, DragDropEffects.Move);
        }

        private void Strength_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string s = (string)e.Data.GetData(DataFormats.StringFormat);
                if (s != ((Control)sender).Name) if (s == "Strength" || s == "Dexterity" || s == "Constitution" || s == "Intelligence" || s == "Wisdom" || s == "Charisma")
                    {
                        Program.Context.MakeHistory("");
                        NumericUpDown n=((NumericUpDown)sender);
                        int temp = 0;
                        if (s == "Strength")
                        {
                            temp = Program.Context.Player.BaseStrength;
                            Program.Context.Player.BaseStrength = (int)n.Value;
                        }
                        if (s == "Dexterity")
                        {
                            temp = Program.Context.Player.BaseDexterity;
                            Program.Context.Player.BaseDexterity = (int)n.Value;
                        }
                        if (s == "Constitution")
                        {
                            temp = Program.Context.Player.BaseConstitution;
                            Program.Context.Player.BaseConstitution = (int)n.Value;
                        }
                        if (s == "Intelligence")
                        {
                            temp = Program.Context.Player.BaseIntelligence;
                            Program.Context.Player.BaseIntelligence = (int)n.Value;
                        }
                        if (s == "Wisdom")
                        {
                            temp = Program.Context.Player.BaseWisdom;
                            Program.Context.Player.BaseWisdom = (int)n.Value;
                        }
                        if (s == "Charisma")
                        {
                            temp = Program.Context.Player.BaseCharisma;
                            Program.Context.Player.BaseCharisma = (int)n.Value;
                        }
                        if (n.Name == "Strength") Program.Context.Player.BaseStrength = temp;
                        if (n.Name == "Dexterity") Program.Context.Player.BaseDexterity = temp;
                        if (n.Name == "Constitution") Program.Context.Player.BaseConstitution = temp;
                        if (n.Name == "Intelligence") Program.Context.Player.BaseIntelligence = temp;
                        if (n.Name == "Wisdom") Program.Context.Player.BaseWisdom = temp;
                        if (n.Name == "Charisma") Program.Context.Player.BaseCharisma = temp;
                        UpdateScores();
                        UpdateSpellcastingInner();
                    }
                    else e.Effect = DragDropEffects.None;
                else e.Effect = DragDropEffects.None;
            }
            else e.Effect = DragDropEffects.None;
        }

        private void Strength_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string s = (string)e.Data.GetData(DataFormats.StringFormat);
                if (s != ((Control)sender).Name) if (s == "Strength" || s == "Dexterity" || s == "Constitution" || s == "Intelligence" || s == "Wisdom" || s == "Charisma") e.Effect = DragDropEffects.Move;
                    else e.Effect = DragDropEffects.None;
                else e.Effect = DragDropEffects.None;
            }
            else e.Effect = DragDropEffects.None;
        }

        private void AbilityFeatBox_DoubleClick(object sender, EventArgs e)
        {
            object o = AbilityFeatBox.SelectedItem;
            if (AbilityFeatChoiceBox.SelectedItem==null) return;
            if (o == null) return;
            AbilityScoreFeatFeature asff = ((AbilityFeatChoiceContainer)AbilityFeatChoiceBox.SelectedItem).ASFF;
            AbilityFeatChoice afc = Program.Context.Player.GetAbilityFeatChoice(asff);
            if (o is string)
            {
                Program.Context.MakeHistory("");
                Ability ab = Ability.None;
                if (((string)o) == "+1 Strength") ab = Ability.Strength;
                else if (((string)o) == "+1 Dexterity") ab = Ability.Dexterity;
                else if (((string)o) == "+1 Constitution") ab = Ability.Constitution;
                else if (((string)o) == "+1 Intelligence") ab = Ability.Intelligence;
                else if (((string)o) == "+1 Wisdom") ab = Ability.Wisdom;
                else if (((string)o) == "+1 Charisma") ab = Ability.Charisma;
                afc.Feat = null;
                if (afc.Ability2 != Ability.None)
                {
                    afc.Ability1 = ab;
                    afc.Ability2 = Ability.None;
                }
                else if (afc.Ability1 != Ability.None) afc.Ability2 = ab;
                else afc.Ability1 = ab;
                UpdateLayout();
            }
            else if (o is Feature)
            {
                Program.Context.MakeHistory("");
                afc.Ability1 = Ability.None;
                afc.Ability2 = Ability.None;
                afc.Feat = ((Feature)o).Name + " " + ConfigManager.SourceSeperator + " " + ((Feature)o).Source;
                UpdateLayout();
            }
        }
       /* private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            int level = 0;
            HPUpDown l = (HPUpDown)sender;
            if (l != null)
            {
                level = int.Parse(l.Name.TrimStart("hitponser".ToCharArray()));
                Program.Context.Player.setHPRoll(level, (int)l.Value);
                UpdateLayout();
            }
        }

        private void numericUpDown1_DoubleClick(object sender, EventArgs e)
        {
            int level = 0;
            //HPUpDown l = (HPUpDown)sender;
            if (l != null)
            {
                level = int.Parse(l.Name.TrimStart("hitponser".ToCharArray()));
                Program.Context.Player.DeleteClass(level);
                UpdateLayout();
            }
        }*/

        private void Choice_DisplayClass(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is ClassDefinition)
            {
                ClassDefinition selected = (ClassDefinition)choicer.SelectedItem;
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                }
            }
        }

        private void classBox1_DoubleClick(object sender, EventArgs e)
        {
            int level = 0;
            ListBox l = (ListBox)sender;
            if (l != null && l.SelectedItem!=null)
            {
                Program.Context.MakeHistory("");
                level = int.Parse(l.Name.TrimStart("classBox".ToCharArray()));
                //Program.Context.Player.AddClass((ClassDefinition)l.SelectedItem, level);
                UpdateLayout();
            }
        }

        private void AbilityFeatChoiceBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAbilityFeatList();
            if (AbilityFeatChoiceBox.SelectedItem is AbilityFeatChoiceContainer)
            {
                AbilityFeatChoice afc = Program.Context.Player.GetAbilityFeatChoice(((AbilityFeatChoiceContainer)AbilityFeatChoiceBox.SelectedItem).ASFF);
                if (afc != null && afc.Feat != "")
                {
                    List<Feature> feats = Program.Context.GetFeatureCollection("");
                    Feature selected = feats.Find(f => string.Equals(f.Name + " " + ConfigManager.SourceSeperator + " " + f.Source, afc.Feat, StringComparison.InvariantCultureIgnoreCase));
                    if (selected == null) selected = feats.Find(f => ConfigManager.SourceInvariantComparer.Equals(f.Name + " " + ConfigManager.SourceSeperator + " " + f.Source, afc.Feat));
                    if (selected != null)
                    {
                        displayElement.Navigate("about:blank");
                        displayElement.Document.OpenNew(true);
                        displayElement.Document.Write(selected.ToHTML());
                        displayElement.Refresh();
                    }
                }
            }
        }

        private void classList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateClassesBox();
            if (classList.SelectedItem != null)
            {
                ClassInfo ci = (ClassInfo)classList.SelectedItem;
                if (ci.Class != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(ci.Class.ToHTML());
                    displayElement.Refresh();
                }
            }
        }

        private void classesBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (classList.SelectedItem != null && classesBox.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
                ClassInfo ci = (ClassInfo)classList.SelectedItem;
                ClassDefinition cur=Program.Context.Player.GetClass(ci.Level);
                if (cur == (ClassDefinition)classesBox.SelectedItem) return;
                if (cur != null) Program.Context.Player.DeleteClass(ci.Level);
                Program.Context.Player.AddClass((ClassDefinition)classesBox.SelectedItem, ci.Level);
                if (classList.SelectedIndex < classList.Items.Count - 1) classList.SelectedIndex++;
                UpdateLayout();
            }
        }

        private void hpSpinner_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            if (classList.SelectedItem != null)
            {
                ClassInfo ci = (ClassInfo)classList.SelectedItem;
                Program.Context.MakeHistory("HP"+ci.Level);
                Program.Context.Player.SetHPRoll(ci.Level, (int)hpSpinner.Value);
                UpdateLayout();
            }
        }

        private void classList_DoubleClick(object sender, EventArgs e)
        {
            if (classList.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
               ClassInfo ci = (ClassInfo)classList.SelectedItem;
               Program.Context.Player.DeleteClass(ci.Level);
               UpdateLayout();
            }
        }

        private void AbilityFeatChoiceBox_DoubleClick(object sender, EventArgs e)
        {
            if (classList.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
                AbilityFeatChoice afc = Program.Context.Player.GetAbilityFeatChoice(((AbilityFeatChoiceContainer)AbilityFeatChoiceBox.SelectedItem).ASFF);
                afc.Ability1 = Ability.None;
                afc.Ability2 = Ability.None;
                afc.Feat = "";
                UpdateLayout();
            }
        }

        private void ideals_Enter(object sender, EventArgs e)
        {

        }

        private void addspellbookButton_Click(object sender, EventArgs e)
        {
            if (spellbookFeaturesBox.SelectedItem != null && listItems.SelectedItem != null && listItems.SelectedItem is Spell)
            {
                Program.Context.MakeHistory("");
                SpellcastingFeature sc = ((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature;
                Program.Context.Player.GetSpellcasting(sc.SpellcastingID).GetAdditionalList(Program.Context.Player, Program.Context).Add(((Spell)listItems.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Spell)listItems.SelectedItem).Source);
                UpdateLayout();
            }
        }

        private void addtoItemButton_Click(object sender, EventArgs e)
        {
            if (inventory2.SelectedItem != null && listItems.SelectedItem != null && listItems.SelectedItem is MagicProperty)
            {
                Program.Context.MakeHistory("");
                Possession p = (Possession)inventory2.SelectedItem;
                int stack = 1;
                if (p.Item != null) stack = Math.Max(1, p.Item.StackSize);
                if (p.Count > stack)
                {
                    p.Count -= stack;
                    p = new Possession(p, (MagicProperty)listItems.SelectedItem);
                }
                else
                {
                    p.MagicProperties.Add(((MagicProperty)listItems.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((MagicProperty)listItems.SelectedItem).Source);
                }
                Program.Context.Player.AddPossession(p);
                UpdateEquipmentLayout();
            }
        }

        private void addScrollButton_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItem != null && listItems.SelectedItem is Spell)
            {
                Program.Context.MakeHistory("");
                Program.Context.Player.Items.Add(((Spell)listItems.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Spell)listItems.SelectedItem).Source);
                UpdateEquipmentLayout();
                UpdateFormsCompanions();
            }
        }

        private void buyButton_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItem != null && listItems.SelectedItem is Item)
            {
                Program.Context.MakeHistory("");
                Item i=(Item)listItems.SelectedItem;
                int count = (int)ItemCounter.Value;
                for (int c = 0; c < count; c++)
                    Program.Context.Player.Items.Add(i.Name + " " + ConfigManager.SourceSeperator + " " + i.Source);
                //Program.Context.Player.Pay(new Price(i.Price,count));
                if (count > 1)
                {
                    Program.Context.Player.ComplexJournal.Add(new JournalEntry(i.ToString() + " x " + count, new Price(i.Price, count)));
                } else Program.Context.Player.ComplexJournal.Add(new JournalEntry(i.ToString(), new Price(i.Price, count)));
                UpdateLayout();
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
                if (listItems.SelectedItem is Item)
                {
                    int count=(int)ItemCounter.Value;
                    for (int c = 0; c < count; c++ )
                        Program.Context.Player.Items.Add(((Item)listItems.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Item)listItems.SelectedItem).Source);
                }
                if (listItems.SelectedItem is MagicProperty && (((MagicProperty)listItems.SelectedItem).Base == null || ((MagicProperty)listItems.SelectedItem).Base == "")) 
                    Program.Context.Player.Possessions.Add(new Possession(Program.Context, (Item)null,(MagicProperty)listItems.SelectedItem));
                if (listItems.SelectedItem is Feature)
                {
                    Program.Context.Player.Boons.Add(((Feature)listItems.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Feature)listItems.SelectedItem).Source);
                    addButton.Enabled = false;
                }
                UpdateLayout();
            }
        }

        private void inventory2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (inventory2.SelectedItem != null)
            {
                if (listItems.SelectedItem != null && listItems.SelectedItem is MagicProperty)
                {
                    Possession p = (Possession)inventory2.SelectedItem;
                    MagicProperty mp = (MagicProperty)listItems.SelectedItem;
                    if (mp.Base == null || mp.Base == "")
                    {
                        addButton.Enabled = true;
                        addtoItemButton.Enabled = false;
                    }
                    else
                    {
                        addButton.Enabled = false;
                        addtoItemButton.Enabled = Utils.Fits(Program.Context, mp, Program.Context.GetItem(p.BaseItem, null));
                    }
                }
                displayElement.Navigate("about:blank");
                displayElement.Document.OpenNew(true);
                displayElement.Document.Write(new DisplayPossession((Possession)inventory2.SelectedItem, Program.Context.Player).ToHTML());
                displayElement.Refresh();
            }
        }

        private void spellbookFeaturesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (spellbookFeaturesBox.SelectedItem != null && listItems.SelectedItem != null && listItems.SelectedItem is Spell)
            {
                addspellbookButton.Enabled = Utils.Matches(Program.Context, ((Spell)listItems.SelectedItem), ((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature.PrepareableSpells, ((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature.SpellcastingID) && !Program.Context.Player.GetSpellcasting(((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature.SpellcastingID).GetSpellbook(Program.Context.Player, Program.Context).Contains((Spell)listItems.SelectedItem);
            }
        }

        private void inventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (inventory.SelectedItem is Possession)
            {
                displayElement.Navigate("about:blank");
                displayElement.Document.OpenNew(true);
                displayElement.Document.Write(new DisplayPossession((Possession)inventory.SelectedItem, Program.Context.Player).ToHTML());
                displayElement.Refresh();
            }
            else if (inventory.SelectedItem is Feature)
            {
                displayElement.Navigate("about:blank");
                displayElement.Document.OpenNew(true);
                displayElement.Document.Write(((Feature)inventory.SelectedItem).ToHTML());
                displayElement.Refresh();
            }
            UpdateInventoryOptions();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (inventory.SelectedItem is Possession && magicproperties.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
                string mp = ((MagicProperty)magicproperties.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((MagicProperty)magicproperties.SelectedItem).Source;
                int index = ((Possession)inventory.SelectedItem).MagicProperties.FindIndex(m => ConfigManager.SourceInvariantComparer.Equals(e, mp));
                if (index >= 0) ((Possession)inventory.SelectedItem).MagicProperties.RemoveAt(index);
                UpdateLayout();
            }
        }

        private void removepossesion_Click(object sender, EventArgs e)
        {
            if (inventory.SelectedItem is Possession)
            {
                Program.Context.MakeHistory("");
                Program.Context.Player.RemovePossessionAndItems((Possession)inventory.SelectedItem);
                UpdateLayout();
            } else if (inventory.SelectedItem is Feature)
            {
                Program.Context.MakeHistory("");
                Program.Context.Player.RemoveBoon(inventory.SelectedItem as Feature);
                UpdateLayout();
            }
        }

        private void changecount_Click(object sender, EventArgs e)
        {
            if (inventory.SelectedItem is Possession)
            {
                Program.Context.MakeHistory("");
                Program.Context.Player.ChangePossessionAmountAndAddRemoveItemsAccordingly((Possession)inventory.SelectedItem,(int)poscounter.Value);
                InventoryRefresh();
            }
        }
        private void InventoryRefresh()
        {
            int count = inventory.Items.Count;
            inventory.SuspendLayout();
            inventory2.SuspendLayout();
            for (int i = 0; i < count; i++)
            {
                inventory.Items[i] = inventory.Items[i];
                inventory2.Items[i] = inventory2.Items[i];
            }
            inventory.ResumeLayout();
            inventory2.ResumeLayout();
        }

        private void splitstack_Click(object sender, EventArgs e)
        {
            if (inventory.SelectedItem is Possession)
            {
                Program.Context.MakeHistory("");
                Possession p = (Possession)inventory.SelectedItem;
                Possession newp = new Possession(p);
                if ((int)poscounter.Value >= p.Count) return;
                int stacksize = 1;
                if (p.Item != null) stacksize = Math.Max(1, p.Item.StackSize);
                newp.Count = p.Count - (int)poscounter.Value;
                if (((int)poscounter.Value) % stacksize != 0 && p.Item != null) Program.Context.Player.Items.Add(p.BaseItem);
                p.Count=(int)poscounter.Value;
                Program.Context.Player.AddPossession(p);
                if (newp.Count > 0) Program.Context.Player.AddPossession(newp);
                UpdateLayout();
            }
        }

        private void updateposs_Click(object sender, EventArgs e)
        {
            if (inventory.SelectedItem is Possession)
            {
                Program.Context.MakeHistory("");
                Possession p = (Possession)inventory.SelectedItem;
                p.Name = possname.Text;
                p.Description = possdescription.Text;
                Program.Context.Player.AddPossession(p);
                InventoryRefresh();
            }
        }

        private void newposs_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("");
            Possession p = new Possession(Program.Context, possname.Text, possdescription.Text, (int)poscounter.Value, (double)possweight.Value)
            {
                Name = possname.Text,
                Description = possdescription.Text
            };
            Program.Context.Player.AddPossession(p);
            UpdateLayout();
        }

        private void possequip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                if (inventory.SelectedItem is Possession)
                {
                    string es = (string)possequip.SelectedItem;
                    Program.Context.MakeHistory("");
                    foreach (Possession pos in Program.Context.Player.Possessions)
                    {
                        if (string.Equals(pos.Equipped, es, StringComparison.InvariantCultureIgnoreCase)) pos.Equipped = EquipSlot.None;
                    }
                    Possession p = (Possession)inventory.SelectedItem;
                    p.Equipped = es;
                    Program.Context.Player.AddPossession(p);
                    UpdateEquipmentLayout();
                }
            }
        }

        private void attunedcheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                if (inventory.SelectedItem is Possession)
                {
                    Program.Context.MakeHistory("");
                    Possession p = (Possession)inventory.SelectedItem;
                    p.Attuned = attunedcheck.Checked;
                    Program.Context.Player.AddPossession(p);
                    UpdateLayout();
                }
            }
        }

        private void posscharges_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                if (inventory.SelectedItem is Possession)
                {
                    Program.Context.MakeHistory("");
                    Possession p = (Possession)inventory.SelectedItem;
                    p.ChargesUsed = (int)posscharges.Value;
                    Program.Context.Player.AddPossession(p);
                    InventoryRefresh();
                }
            }
        }

        private void possweight_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                if (inventory.SelectedItem is Possession)
                {
                    Program.Context.MakeHistory("");
                    Possession p = (Possession)inventory.SelectedItem;
                    p.Weight = (double)possweight.Value;
                    Program.Context.Player.AddPossession(p);
                    InventoryRefresh();
                }
            }
        }

        private void highlightcheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                if (inventory.SelectedItem is Possession)
                {
                    Program.Context.MakeHistory("");
                    Possession p = (Possession)inventory.SelectedItem;
                    p.Hightlight = highlightcheck.Checked;
                    Program.Context.Player.AddPossession(p);
                    UpdateLayout();
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            splitContainer11.Panel2Collapsed = !splitContainer11.Panel2Collapsed;
        }

        private void PP_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Money" + ((Control)sender).Name);
                Program.Context.Player.SetMoney((int)CP.Value, (int)SP.Value, (int)EP.Value, (int)GP.Value, (int)PP.Value);
                UpdateSideLayout();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Context.Undo();
            UpdateLayout();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Context.Redo();
            UpdateLayout();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.Context.UnsavedChanges == 0 || MessageBox.Show(Program.Context.UnsavedChanges + " unsaved changes will be lost. Continue?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else e.Cancel = true;
        }

        private void inspiration_CheckedChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("");
                Program.Context.Player.Inspiration = inspiration.Checked;
            }
        }

        private void skillabilitybox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (skillabilitybox.SelectedItem != null && skillbox.SelectedItem != null)
                skillablityresult.Text = "= " + plusMinus(Program.Context.Player.GetSkill(((SkillInfo)skillbox.SelectedItem).Skill, indextoability(skillabilitybox.SelectedIndex)));
        }

        private void hdreset_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("");
            Program.Context.Player.UsedHitDice = new List<int>();
            UpdateInPlayInner();
        }

        private void HitDiceUsed_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void HitDiceUsed_DoubleClick(object sender, EventArgs e)
        {
            if (HitDiceUsed.SelectedItem is HitDie hd)
            {
                if (hd.Used < hd.Count)
                {
                    Program.Context.MakeHistory("");
                    Program.Context.Player.UseHitDie(hd.Dice);
                    UpdateInPlayInner();
                }
            }
        }

        private void CurHP_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("CurHP");
                Program.Context.Player.CurrentHPLoss = (int)CurHP.Value - Program.Context.Player.GetHitpointMax();
            }
        }

        private void TempHP_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("TempHP");
                Program.Context.Player.TempHP = (int)TempHP.Value;
            }
        }

        private void DeathSuccess_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("DeathSuccess");
                Program.Context.Player.SuccessDeathSaves = (int)DeathSuccess.Value;
            }
        }

        private void DeathFail_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("DeathFail");
                Program.Context.Player.FailedDeathSaves = (int)DeathFail.Value;
            }
        }

        private void ResourcesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ResourcesBox.SelectedItem != null)
            {
                if (ResourcesBox.SelectedItem is ResourceInfo selected)
                {
                    if (selected != null)
                    {
                        layouting = true;
                        resourceused.Enabled = true;
                        if (selected.Max > 0) resourceused.Maximum = selected.Max;
                        else resourceused.Maximum = 10000;
                        resourceused.Value = selected.Used;
                        layouting = false;
                        displayElement.Navigate("about:blank");
                        displayElement.Document.OpenNew(true);
                        displayElement.Document.Write(Program.Context.Player.GetResourceFeatures(selected.ResourceID).ToHTML());
                        displayElement.Refresh();
                    }
                }
                else if (ResourcesBox.SelectedItem is ModifiedSpell ms)
                {
                    if (ms != null)
                    {
                        layouting = true;
                        ms.Info = Program.Context.Player.GetAttack(ms, ms.differentAbility);
                        ms.Modifikations.AddRange(from f in Program.Context.Player.GetFeatures() where f is SpellModifyFeature && Utils.Matches(Program.Context, ms, ((SpellModifyFeature)f).Spells, null) select f);
                        ms.Modifikations = ms.Modifikations.Distinct().ToList();
                        if ((ms.Level > 0 && ms.RechargeModifier < RechargeModifier.AtWill) || (ms.Level == 0 && ms.RechargeModifier != RechargeModifier.Unmodified && ms.RechargeModifier < RechargeModifier.AtWill))
                        {
                            resourceused.Enabled = true;
                            resourceused.Maximum = ms.count;
                            resourceused.Value = 0;
                            resourceused.Value = Program.Context.Player.GetUsedResources(ms.getResourceID());
                        }
                        else resourceused.Enabled = false;
                        layouting = false;
                        displayElement.Navigate("about:blank");
                        displayElement.Document.OpenNew(true);
                        displayElement.Document.Write(ms.ToHTML());
                        displayElement.Refresh();

                    }
                }
            }
            else resourceused.Enabled = false;
        }

        private void resourceused_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting && ResourcesBox.SelectedItem != null)
            {
                if (ResourcesBox.SelectedItem is ResourceInfo)
                {
                    Program.Context.MakeHistory("Resource" + ((ResourceInfo)ResourcesBox.SelectedItem).ResourceID);
                    Program.Context.Player.SetUsedResources(((ResourceInfo)ResourcesBox.SelectedItem).ResourceID, (int)resourceused.Value);
                    UpdateInPlayInner();
                }
                else if (ResourcesBox.SelectedItem is ModifiedSpell)
                {
                    Program.Context.MakeHistory("Resource" + ((ModifiedSpell)ResourcesBox.SelectedItem).getResourceID());
                    Program.Context.Player.SetUsedResources(((ModifiedSpell)ResourcesBox.SelectedItem).getResourceID(), (int)resourceused.Value);
                    UpdateInPlayInner();
                }
            }
        }

        private void shortrest_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("ShortRest");
            foreach (ResourceInfo r in Program.Context.Player.GetResourceInfo(true).Values)
            {
                if (r.Recharge >= RechargeModifier.ShortRest) Program.Context.Player.SetUsedResources(r.ResourceID, 0);
            }
            foreach (ModifiedSpell ms in Program.Context.Player.GetBonusSpells())
            {
                if (ms.RechargeModifier >= RechargeModifier.ShortRest) Program.Context.Player.SetUsedResources(ms.getResourceID(), 0);
            }
            UpdateInPlayInner();
        }

        private void longrest_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("LongRest");
            foreach (ResourceInfo r in Program.Context.Player.GetResourceInfo(true).Values)
            {
                if (r.Recharge >= RechargeModifier.LongRest) Program.Context.Player.SetUsedResources(r.ResourceID, 0);
            }
            foreach (ModifiedSpell ms in Program.Context.Player.GetBonusSpells())
            {
                if (ms.RechargeModifier >= RechargeModifier.LongRest) Program.Context.Player.SetUsedResources(ms.getResourceID(), 0);
            }
            UpdateInPlayInner();
        }

        private void ResourcesBox_DoubleClick(object sender, EventArgs e)
        {
            if (ResourcesBox.SelectedItem != null)
            {
                if (ResourcesBox.SelectedItem is ResourceInfo selected)
                {
                    if (selected.Used < selected.Max)
                    {
                        Program.Context.MakeHistory("Resource" + selected.ResourceID);
                        Program.Context.Player.SetUsedResources(selected.ResourceID, selected.Used + 1);
                        UpdateInPlayInner();
                    }
                }
                else if (ResourcesBox.SelectedItem is ModifiedSpell ms)
                {
                    if (ms.used < ms.count)
                    {
                        Program.Context.MakeHistory("Resource" + ms.getResourceID());
                        Program.Context.Player.SetUsedResources(ms.getResourceID(), ms.used + 1);
                        UpdateInPlayInner();
                    }
                }
            }
        }

        private void Features_DoubleClick(object sender, EventArgs e)
        {
            if (Features.SelectedItem != null && Features.SelectedItem is Feature) {
                Program.Context.MakeHistory("");
                Feature f = (Feature)Features.SelectedItem;
                if (Program.Context.Player.HiddenFeatures.RemoveAll(s => StringComparer.OrdinalIgnoreCase.Equals(s, f.Name)) == 0) Program.Context.Player.HiddenFeatures.Add(f.Name);
                UpdateInPlayInner();
            }
        }

        private void Features_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Features.SelectedItem != null && Features.SelectedItem is Feature)
            {
                layouting = true;
                hidefeature.Enabled = true;
                Feature f = (Feature)Features.SelectedItem;
                hidefeature.Checked = Program.Context.Player.HiddenFeatures.Exists(s => StringComparer.OrdinalIgnoreCase.Equals(s, f.Name) || StringComparer.OrdinalIgnoreCase.Equals(s, f.Name + "/" + f.Level));
                layouting = false;
                Choice_DisplayFeature(sender, e);
            }
            else hidefeature.Enabled = false;
        }

        private void hidefeature_CheckedChanged(object sender, EventArgs e)
        {
            if (!layouting && Features.SelectedItem != null && Features.SelectedItem is Feature)
            {
                Program.Context.MakeHistory("");
                Feature f = (Feature)Features.SelectedItem;
                if (hidefeature.Checked) Program.Context.Player.HiddenFeatures.Add(f.Name + "/" + f.Level);
                else Program.Context.Player.HiddenFeatures.RemoveAll(s => StringComparer.OrdinalIgnoreCase.Equals(s, f.Name) || StringComparer.OrdinalIgnoreCase.Equals(s, f.Name + "/" + f.Level));
                //UpdateInPlayInner();
            }
        }

        private void availableConditions_DoubleClick(object sender, EventArgs e)
        {
            if (availableConditions.SelectedItem != null)
            {
                Program.Context.MakeHistory("");
                if (availableConditions.SelectedItem is Condition)
                {
                    Program.Context.Player.Conditions.Add(((Condition)availableConditions.SelectedItem).Name);
                }
                else
                {
                    string c = Interaction.InputBox("Custom Condition:", "CB 5");
                    if (c != null && c != "") Program.Context.Player.Conditions.Add(c);
                }
                UpdateInPlayInner();
            }
        }

        private void activeConditions_DoubleClick(object sender, EventArgs e)
        {
            if (activeConditions.SelectedItem != null && activeConditions.SelectedItem is Condition)
            {
                Program.Context.MakeHistory("");
                Program.Context.Player.Conditions.RemoveAll(t => StringComparer.InvariantCultureIgnoreCase.Equals(t, ((Condition)activeConditions.SelectedItem).Name));
                UpdateInPlayInner();
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem != null && lb.SelectedItem is ModifiedSpell)
            {
                Program.Context.MakeHistory("");
                Program.Context.Player.GetSpellcasting(lb.Parent.Name).Highlight = ((ModifiedSpell)lb.SelectedItem).Name;
                UpdateInPlayInner();
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem != null && lb.SelectedItem is SpellSlotInfo)
            {
                SpellSlotInfo ssi = (SpellSlotInfo)lb.SelectedItem;
                Program.Context.MakeHistory("");
                if (ssi.Used < ssi.Slots) Program.Context.Player.SetSpellSlot(ssi.SpellcastingID, ssi.Level, ssi.Used + 1);
                UpdateInPlayInner();
            }
        }

        private void label_DoubleClick(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("");
            Program.Context.Player.GetSpellcasting(((Control)sender).Parent.Name).Highlight = null;
            UpdateInPlayInner();
        }

        private void listBox1_IndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem != null && lb.SelectedItem is SpellSlotInfo)
            {
                layouting = true;
                Control[] useds = lb.Parent.Controls.Find("SpellSlotUsed", true);
                SpellSlotInfo ssi = (SpellSlotInfo)lb.SelectedItem;
                if (useds.Count() > 0)
                {
                    NumericUpDown used = (NumericUpDown)useds[0];
                    used.Enabled = true;
                    used.Minimum = 0;
                    used.Maximum = ssi.Slots;
                    used.Value = ssi.Used;
                }
                layouting = false;
            }
            else
            {
                layouting = true;
                Control[] useds = lb.Parent.Controls.Find("SpellSlotUsed", true);
                if (useds.Count() > 0)
                {
                    NumericUpDown used = (NumericUpDown)useds[0];
                    used.Enabled = false;
                    used.Minimum = 0;
                    used.Maximum = 1;
                    used.Value = 0;
                }
                layouting = false;
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("");
            Program.Context.Player.ResetSpellSlots(((Control)sender).Parent.Name);
            UpdateInPlayInner();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting) {
                NumericUpDown nup = (NumericUpDown)sender;
                Control[] slotboxes = nup.Parent.Controls.Find("SpellSlotBox", true);
                if (slotboxes.Count() > 0)
                {
                    ListBox lb = (ListBox)slotboxes[0];
                    if (lb.SelectedItem != null && lb.SelectedItem is SpellSlotInfo)
                    {
                        SpellSlotInfo ssi = (SpellSlotInfo)lb.SelectedItem;
                        Program.Context.MakeHistory("Spellslots" + ssi.SpellcastingID);
                        Program.Context.Player.SetSpellSlot(ssi.SpellcastingID, ssi.Level, (int)nup.Value);
                        UpdateInPlayInner();
                    }
                }
            }
        }

        private void unpack_Click(object sender, EventArgs e)
        {
            if (inventory.SelectedItem is Possession p)
            {
                if (p.BaseItem != null && p.BaseItem != "" && p.Item is Pack)
                {
                    Program.Context.MakeHistory("");
                    for (int i = 0; i < p.Count; i++)
                        Program.Context.Player.Items.AddRange(((Pack)p.Item).Contents);
                    Program.Context.Player.RemovePossessionAndItems(p);
                    UpdateLayout();
                }
            }
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("BonusMaxHP");
                Program.Context.Player.BonusMaxHP = (int)bonusMaxHP.Value;
                UpdateSideLayout();
            }
        }
        static string BuildCommandLineFromArgs(params string[] args)
        {
            if (args == null)
                return null;
            string result = "";

            if (Environment.OSVersion.Platform == PlatformID.Unix
                ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                foreach (string arg in args)
                {
                    result += (result.Length > 0 ? " " : "")
                        + arg
                            .Replace(@" ", @"\ ")
                            .Replace("\t", "\\\t")
                            .Replace(@"\", @"\\")
                            .Replace(@"""", @"\""")
                            .Replace(@"<", @"\<")
                            .Replace(@">", @"\>")
                            .Replace(@"|", @"\|")
                            .Replace(@"@", @"\@")
                            .Replace(@"&", @"\&");
                }
            }
            else //Windows family
            {
                bool enclosedInApo, wasApo;
                string subResult;
                foreach (string arg in args)
                {
                    enclosedInApo = arg.LastIndexOfAny(
                        new char[] { ' ', '\t', '|', '@', '^', '<', '>', '&' }) >= 0;
                    wasApo = enclosedInApo;
                    subResult = "";
                    for (int i = arg.Length - 1; i >= 0; i--)
                    {
                        switch (arg[i])
                        {
                            case '"':
                                subResult = @"\""" + subResult;
                                wasApo = true;
                                break;
                            case '\\':
                                subResult = (wasApo ? @"\\" : @"\") + subResult;
                                break;
                            default:
                                subResult = arg[i] + subResult;
                                wasApo = false;
                                break;
                        }
                    }
                    result += (result.Length > 0 ? " " : "")
                        + (enclosedInApo ? "\"" + subResult + "\"" : subResult);
                }
            }

            return result;
        }
        private void makeDefaultEditorForcb5FilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.IsRunAsAdmin())
            {
                if (Program.Context.UnsavedChanges == 0 || MessageBox.Show("The application needs to restart for that.\n" + Program.Context.UnsavedChanges + " unsaved changes will be lost. Continue?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Launch itself as administrator
                    ProcessStartInfo proc = new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        WorkingDirectory = Environment.CurrentDirectory,
                        FileName = Application.ExecutablePath,
                        Verb = "runas",
                        Arguments = BuildCommandLineFromArgs(new string[] { lastfile, "register" })
                    };
                    try
                    {
                        Process.Start(proc);
                    }
                    catch
                    {
                        // The user refused the elevation.
                        // Do nothing and return directly ...
                        return;
                    }
                }
                Application.Exit();  // Quit itself
            }
            else
            {
                Program.Register();
            }
        }

        private void portraitBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                String s = (String)e.Data.GetData(DataFormats.StringFormat);
                if (s.StartsWith("data:"))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (file.Count()==1) e.Effect = DragDropEffects.Copy;
                else e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void portraitBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                Program.Context.MakeHistory("");
                Program.Context.Player.SetPortrait((Bitmap)e.Data.GetData(DataFormats.Bitmap));
                UpdatePersonal();
            }
            else if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                String s = (String)e.Data.GetData(DataFormats.StringFormat);
                Match m = Regex.Match(s, @"data:\s*;\s*base64\s*,\s*(?<data>.+)");
                if (m.Success)
                {
                    try
                    {
                        Program.Context.MakeHistory("");
                        Program.Context.Player.Portrait = Convert.FromBase64String(m.Groups["data"].Value);
                        UpdatePersonal();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error loading drag/drop object ");
                    }
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    Program.Context.MakeHistory("");
                    string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (file.Count() == 1) Program.Context.Player.SetPortrait(new Bitmap(file[0]));
                    UpdatePersonal();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error loading drag/drop object ");
                }
            }
        }

        private void portraitBox_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(Program.Context.Player.Portrait, DragDropEffects.Copy);
        }

        private void FactionInsignia_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                Program.Context.MakeHistory("");
                Program.Context.Player.SetFactionImage((Bitmap)e.Data.GetData(DataFormats.Bitmap));
                UpdatePersonal();
            }
            else if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                String s = (String)e.Data.GetData(DataFormats.StringFormat);
                Match m = Regex.Match(s, @"data:\s*;\s*base64\s*,\s*(?<data>.+)");
                if (m.Success)
                {
                    try
                    {
                        Program.Context.MakeHistory("");
                        Program.Context.Player.FactionImage = Convert.FromBase64String(m.Groups["data"].Value);
                        UpdatePersonal();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error loading drag/drop object ");
                    }
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    Program.Context.MakeHistory("");
                    string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (file.Count() == 1) Program.Context.Player.SetFactionImage(new Bitmap(file[0]));
                    UpdatePersonal();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error loading drag/drop object ");
                }
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (file.Count() == 1 && Path.GetExtension(file[0]).ToLowerInvariant() == ".cb5")
                {
                    e.Effect = DragDropEffects.Link;
                }
                else e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (file.Count() == 1)
                {
                    try
                    {
                        using (FileStream fs = new FileStream(file[0], FileMode.Open))
                        {
                            Player p = PlayerExtensions.Load(Program.Context, fs);
                            if (p != null)
                            {
                                if (Program.Context.UnsavedChanges == 0 || MessageBox.Show(Program.Context.UnsavedChanges + " unsaved changes will be lost. Continue?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    Program.Context.UndoBuffer = new LinkedList<Player>();
                                    Program.Context.RedoBuffer = new LinkedList<Player>();
                                    Program.Context.UnsavedChanges = 0;
                                    Program.Context.Player = p;
                                    UpdateLayout();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error loading drag/drop object "+String.Join(", ",file));
                    }
                }
                else e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void journalentrybox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool waslayouting = layouting;
            layouting = true;
            if (journalentrybox.SelectedIndex >= 0 && journalentrybox.SelectedIndex < Program.Context.Player.Journal.Count)
                journalbox.Text = Program.Context.Player.Journal[journalentrybox.SelectedIndex];
            if (!waslayouting) layouting = false;
        }

        private void newentrybotton_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("");
            Program.Context.Player.Journal.Add(journalbox.Text != "" ? journalbox.Text : "-New Entry-");
            UpdatePersonal();
        }

        private void journalbox_TextChanged(object sender, EventArgs e)
        {
            if (!layouting && journalentrybox.SelectedIndex >= 0 && journalentrybox.SelectedIndex < Program.Context.Player.Journal.Count)
            {
                Program.Context.MakeHistory("JournalEntry" + journalentrybox.SelectedIndex);
                Program.Context.Player.Journal[journalentrybox.SelectedIndex] = journalbox.Text;
                int index = journalbox.Text.IndexOfAny(new char[] { '\r', '\n' });
                journalentrybox.Items[journalentrybox.SelectedIndex] = index == -1 ? journalbox.Text : journalbox.Text.Substring(0, index);
            }
        }

        private void removejournalentry_Click(object sender, EventArgs e)
        {
            if (journalentrybox.SelectedIndex >= 0 && journalentrybox.SelectedIndex < Program.Context.Player.Journal.Count)
            {
                Program.Context.MakeHistory("");
                Program.Context.Player.Journal.RemoveAt(journalentrybox.SelectedIndex);
                UpdatePersonal();
            }
        }

        private void Display_Generic(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null)
            {
                if (choicer.SelectedItem is IXML selected)
                {
                    if (selected != null)
                    {
                        displayElement.Navigate("about:blank");
                        displayElement.Document.OpenNew(true);
                        displayElement.Document.Write(selected.ToHTML());
                        displayElement.Refresh();
                    }
                }
            }
        }

        private void splitContainer12_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void journalEntries_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool waslayouting = layouting;
            if (!layouting) layouting = true;
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                journalTitle.Text = je.Title;
                journalTime.Text = je.Time;
                journalText.Text = je.Text;
                journalSession.Text = je.Session;
                journalDM.Text = je.DM;
                journalCP.Value = je.CP;
                journalSP.Value = je.SP;
                journalGP.Value = je.GP;
                journalEP.Value = je.EP;
                journalPP.Value = je.PP;
                journalXP.Value = je.XP;
                journalAP.Value = je.AP;
                journalT1TP.Value = je.T1TP;
                journalT2TP.Value = je.T2TP;
                journalT3TP.Value = je.T3TP;
                journalT4TP.Value = je.T4TP;
                journalMagic.Value = je.MagicItems;
                journalDowntime.Value = je.Downtime;
                journalRenown.Value = je.Renown;
                journalInSheet.Checked = je.InSheet;
                journalDate.Value = je.Added;
                journalTitle.Enabled = true;
                journalTime.Enabled = true;
                journalText.Enabled = true;
                journalDM.Enabled = true;
                journalSession.Enabled = true;
                journalCP.Enabled = true;
                journalSP.Enabled = true;
                journalGP.Enabled = true;
                journalEP.Enabled = true;
                journalPP.Enabled = true;
                journalXP.Enabled = true;
                journalAP.Enabled = true;
                journalT1TP.Enabled = true;
                journalT2TP.Enabled = true;
                journalT3TP.Enabled = true;
                journalT4TP.Enabled = true;
                journalMagic.Enabled = true;
                journalDowntime.Enabled = true;
                journalRenown.Enabled = true;
                journalInSheet.Enabled = true;
                journalDate.Enabled = true;
                removeJournalButton.Enabled = true;
            } else
            {
                journalTitle.Text = "";
                journalTime.Text = "";
                journalText.Text = "";
                journalSession.Text = "";
                journalDM.Text = "";
                journalCP.Value = 0;
                journalSP.Value = 0;
                journalGP.Value = 0;
                journalEP.Value = 0;
                journalPP.Value = 0;
                journalXP.Value = 0;
                journalAP.Value = 0;
                journalT1TP.Value = 0;
                journalT2TP.Value = 0;
                journalT3TP.Value = 0;
                journalT4TP.Value = 0;
                journalMagic.Value = 0;
                journalDowntime.Value = 0;
                journalRenown.Value = 0;
                journalDate.Value = DateTime.Now;
                journalInSheet.Checked = false;
                journalTitle.Enabled = false;
                journalTime.Enabled = false;
                journalText.Enabled = false;
                journalDM.Enabled = false;
                journalSession.Enabled = false;
                journalCP.Enabled = false;
                journalSP.Enabled = false;
                journalGP.Enabled = false;
                journalEP.Enabled = false;
                journalPP.Enabled = false;
                journalXP.Enabled = false;
                journalAP.Enabled = false;
                journalT1TP.Enabled = false;
                journalT2TP.Enabled = false;
                journalT3TP.Enabled = false;
                journalT4TP.Enabled = false;
                journalMagic.Enabled = false;
                journalDowntime.Enabled = false;
                journalRenown.Enabled = false;
                journalInSheet.Enabled = false;
                journalDate.Enabled = false;
                removeJournalButton.Enabled = false;
            }
            if (!waslayouting) layouting = false;

        }

        private void journalTitle_TextChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalTitle");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.Title = journalTitle.Text;
                journalEntries.DisplayMember = "Title";
                journalEntries.DisplayMember = "";
            }
        }

        private void journalText_TextChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalText");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.Text = journalText.Text;
            }
        }

        private void journalTime_TextChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalTime");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.Time = journalTime.Text;
            }
        }

        private void journalXP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalXP");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.XP = (int)journalXP.Value;
                UpdateLayout();
            }
        }

        private void journalPP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalPP");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.PP = (int)journalPP.Value;
                UpdateJournal();
            }
        }

        private void journalGP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalGP");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.GP = (int)journalGP.Value;
                UpdateJournal();
            }
        }

        private void journalEP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalEP");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.EP = (int)journalEP.Value;
                UpdateJournal();
            }
        }

        private void journalSP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalSP");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.SP = (int)journalSP.Value;
                UpdateJournal();
            }
        }

        private void journalCP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalCP");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.CP = (int)journalCP.Value;
                UpdateJournal();
            }
        }

        private void newJournalEntry_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory();
            Program.Context.Player.ComplexJournal.Add(new JournalEntry());
            UpdateJournal();
        }

        private void removeJournalButton_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory();
            if (journalEntries.SelectedItem is JournalEntry)
            {
                Program.Context.Player.ComplexJournal.RemoveAt(journalEntries.SelectedIndex);
                UpdateJournal();
            }
        }

        private void ideals_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null)
            {
                Feature selected = new Feature("Text", choicer.SelectedItem.ToString());
                displayElement.Navigate("about:blank");
                displayElement.Document.OpenNew(true);
                displayElement.Document.Write(selected.ToHTML());
                displayElement.Refresh();
            }
        }

        private void showDescriptionToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            ConfigManager.Description = showDescriptionToolStripMenuItem.Checked;
        }

        private void magicproperties_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) drag = e;
        }

        private void magicproperties_MouseUp(object sender, MouseEventArgs e)
        {
            drag = null;
        }

        private void magicproperties_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag == null || magicproperties.SelectedItem == null) return;
            Point point = magicproperties.PointToClient(new Point(e.X, e.Y));
            if ((e.X - drag.X) * (e.X - drag.X) + (e.Y - drag.Y) * (e.Y - drag.Y) < 6) return;
            magicproperties.DoDragDrop(new Drag(magicproperties.SelectedIndex, magicproperties.SelectedItem), DragDropEffects.Move);
            drag = null;
        }

        private void magicproperties_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point point = magicproperties.PointToClient(new Point(e.X, e.Y));
            if (point.Y < 0 || point.Y > magicproperties.Size.Height) return;
            int index = magicproperties.IndexFromPoint(point);
            if (index < 0) index = magicproperties.Items.Count - 1;
            Drag data = (Drag)e.Data.GetData(typeof(Drag));
            magicproperties.Items.RemoveAt(data.curindex);
            magicproperties.Items.Insert(index, data.value);
            data.curindex = index;
        }

        private void magicproperties_DragLeave(object sender, EventArgs e)
        {
            UpdateInventoryOptions();
        }

        private void magicproperties_DragDrop(object sender, DragEventArgs e)
        {
            Point point = magicproperties.PointToClient(new Point(e.X, e.Y));
            int index = magicproperties.IndexFromPoint(point);
            if (index < 0) index = magicproperties.Items.Count - 1;
            Drag data = (Drag)e.Data.GetData(typeof(Drag));
            if (data == null)
                return;
            Program.Context.MakeHistory(null);
            if (inventory.SelectedItem is Possession p)
            {
                string prop = p.MagicProperties[data.index];
                p.MagicProperties.RemoveAt(data.index);
                p.MagicProperties.Insert(index, prop);
            }
            UpdateLayout();
        }

        private void alawaysShowTheSourcebookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigManager.AlwaysShowSource = alawaysShowTheSourcebookToolStripMenuItem.Checked = !alawaysShowTheSourcebookToolStripMenuItem.Checked;
            UpdateLayout();
        }

        private void DCI_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("DCI");
                Program.Context.Player.DCI = DCI.Text;
            }
        }

        private void journalSession_TextChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalSession");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.Session = journalSession.Text;
            }
        }

        private void journalDM_TextChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalDM");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.DM = journalDM.Text;
            }
        }

        private void journalRenown_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalRenown");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.Renown = (int)journalRenown.Value;
                UpdateJournal();
            }
        }

        private void journalDowntime_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("journalDowntime");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.Downtime = (int)journalDowntime.Value;
                UpdateJournal();
            }
        }

        private void journalMagic_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("journalMagic");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.MagicItems = (int)journalMagic.Value;
                UpdateJournal();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("journalInclude");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.InSheet = journalInSheet.Checked;
                UpdateJournal();
            }
        }

        private void Renown_ValueChanged(object sender, EventArgs e)
        {

        }

        //private void PDFjournal_Click(object sender, EventArgs e)
        //{
        //    PDFjournal.Checked = !PDFjournal.Checked;
        //}

        //private void PDFspellbook_Click(object sender, EventArgs e)
        //{
        //    PDFspellbook.Checked = !PDFspellbook.Checked;
        //}

        private void reladDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.ReloadData();
            UpdateLayout();
        }

        private void showErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Errorlog.Show();
        }

        private void showAllKnownRitualsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Context.Player.AllRituals = !Program.Context.Player.AllRituals;
            UpdateLayout();
        }

        private void actionsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actionsBox.SelectedItem is ActionInfo ai)
            {
                Feature selected = ai.Feature;
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.ToHTML());
                    displayElement.Refresh();
                }
            }
        }

        //private void includeActionsInPDFToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    includeActionsInPDFToolStripMenuItem.Checked = !includeActionsInPDFToolStripMenuItem.Checked;
        //}

        private void factionRank_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Program.Context.MakeHistory("Factionrank");
                Program.Context.Player.FactionRank = factionRank.Text;
            }
        }

        private void FormsCompanionOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormsCompanionOptions.SelectedItem is FormsCompanionInfo fci)
            {
                if (fci.Source != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(fci.Source.ToHTML());
                    displayElement.Refresh();
                }
                FormsCompanionsSelected.Items.Clear();
                foreach (Monster m in fci.AppliedChoices(Program.Context, Program.Context.Player.GetFinalAbilityScores())) if (m != null) FormsCompanionsSelected.Items.Add(m);
                FormsCompanionsAvailable.Items.Clear();
                foreach (Monster m in fci.AvailableOptions(Program.Context, Program.Context.Player.GetFinalAbilityScores()).Where(m=>!fci.Choices.Exists(mm=>StringComparer.OrdinalIgnoreCase.Equals(m.Name, mm.Name)))) FormsCompanionsAvailable.Items.Add(m);
                if (fci.Count < 0) FormsCompanionsCounter.Text = "Selected: " + fci.Choices.Count;
                else FormsCompanionsCounter.Text = "Selected: " + fci.Choices.Count + " / " + fci.Count;
            }
        }

        private void FormsCompanionsSelected_DoubleClick(object sender, EventArgs e)
        {
            if (FormsCompanionOptions.SelectedItem is FormsCompanionInfo fci)
                if (FormsCompanionsSelected.SelectedItem is Monster m)
                    Program.Context.Player.RemoveFormCompanion(fci.ID, m);
            UpdateFormsCompanions();
        }

        private void FormsCompanionsAvailable_DoubleClick(object sender, EventArgs e)
        {
            if (FormsCompanionOptions.SelectedItem is FormsCompanionInfo fci)
                if (fci.Count < 0 || fci.Choices.Count < fci.Count)
                    if (FormsCompanionsAvailable.SelectedItem is Monster m)
                        Program.Context.Player.AddFormCompanion(fci.ID, m);
            UpdateFormsCompanions();
        }

        //private void toolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    toolStripMenuItem1.Checked = !toolStripMenuItem1.Checked;
        //}

        private void advancementCheckpointsInsteadOfXPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Context.MakeHistory("");
            Program.Context.Player.Advancement = !advancementCheckpointsInsteadOfXPToolStripMenuItem.Checked;
            UpdateLayout();
        }

        private void journalAP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalAP");
            if (journalEntries.SelectedItem is JournalEntry je)
            {
                je.AP = (int)journalAP.Value;
                UpdateLayout();
            }
        }

        private void journalT1TP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalT1TP");
            if (journalEntries.SelectedItem is JournalEntry je)
            {
                je.T1TP = (int)journalT1TP.Value;
                UpdateJournal();
            }
        }

        private void journalT2TP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalT2TP");
            if (journalEntries.SelectedItem is JournalEntry je)
            {
                je.T2TP = (int)journalT2TP.Value;
                UpdateJournal();
            }
        }

        private void journalT3TP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalT3TP");
            if (journalEntries.SelectedItem is JournalEntry je)
            {
                je.T3TP = (int)journalT3TP.Value;
                UpdateJournal();
            }
        }

        private void journalT4TP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalT4TP");
            if (journalEntries.SelectedItem is JournalEntry je)
            {
                je.T4TP = (int)journalT4TP.Value;
                UpdateJournal();
            }
        }

        private void attacksBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (attacksBox.SelectedItem is AttackRow row && row.Possession != null)
            {
                displayElement.Navigate("about:blank");
                displayElement.Document.OpenNew(true);
                displayElement.Document.Write(new DisplayPossession(row.Possession, Program.Context.Player).ToHTML());
                displayElement.Refresh();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Program.Context.MakeHistory("JournalDate");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.Added = journalDate.Value;
                UpdateJournal();
            }
        }

        private void saveCurrentPlayerDCIAsDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DCI = Program.Context.Player.DCI;
            Properties.Settings.Default.PlayerName = Program.Context.Player.PlayerName;
            Properties.Settings.Default.Save();
        }

        private void saveCurrentSourcesAsDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.EnabledSourcebooks == null) Properties.Settings.Default.EnabledSourcebooks = new System.Collections.Specialized.StringCollection();
            Properties.Settings.Default.EnabledSourcebooks.Clear();
            Properties.Settings.Default.EnabledSourcebooks.AddRange(SourceManager.Sources.Where(s => !Program.Context.Player.ExcludedSources.Contains(s)).ToArray());
            Properties.Settings.Default.Save();
        }
    }
}