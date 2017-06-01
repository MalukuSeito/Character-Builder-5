using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Character_Builder_5
{
    public partial class Form1 : Form
    {
        private bool layouting = false;
        public String lastfile = "";
        public Form1()
        {
            InitializeComponent();
            itemCategories.Items.AddRange(Item.Section().ToArray<Category>());
            itemCategories.Items.AddRange(MagicProperty.Section().ToArray<MagicCategory>());
            itemCategories.Items.Add("Spells");
            itemCategories.Items.AddRange(FeatureCollection.Section().ToArray<string>());
            ArrayBox.Items.AddRange(AbilityScores.GetArrays().ToArray<AbilityScoreArray>());
            racebox.MouseWheel += listbox_MouseWheel;
            subracebox.MouseWheel += listbox_MouseWheel;
            bonds.MouseWheel += listbox_MouseWheel;
            ideals.MouseWheel += listbox_MouseWheel;
            flaws.MouseWheel += listbox_MouseWheel;
            background.MouseWheel += listbox_MouseWheel;
            traits.MouseWheel += listbox_MouseWheel;
            journalentrybox.MouseWheel += listbox_MouseWheel;
            skillbox.MouseWheel += listbox_MouseWheel;
            HitDiceUsed.MouseWheel += listbox_MouseWheel;
            ResourcesBox.MouseWheel += listbox_MouseWheel;
            Features.MouseWheel += listbox_MouseWheel;
            activeConditions.MouseWheel += listbox_MouseWheel;
            availableConditions.MouseWheel += listbox_MouseWheel;
            actionBox.Controls.Clear();
            UpdateLayout();
            pDFExporterToolStripMenuItem.DropDownItems.Clear();
            bool first=true;
            foreach (string s in ConfigManager.PDFExporters)
            {
                ToolStripMenuItem p = new ToolStripMenuItem(Path.GetFileNameWithoutExtension(s));
                if (first)
                {
                    first = false;
                    p.Checked = true;
                }
                p.Name = s;
                p.Size = new System.Drawing.Size(152, 22);
                p.Click += new System.EventHandler(this.pdfexporter_click);
                pDFExporterToolStripMenuItem.DropDownItems.Add(p);
            }
            portraitBox.AllowDrop = true;
            FactionInsignia.AllowDrop = true;
            sidePortrait.AllowDrop = true;
            AllowDrop = true;
            possequip.Items.Clear();
            possequip.Items.Add(EquipSlot.None);
            foreach (string s in ConfigManager.loaded.Slots) possequip.Items.Add(s);
        }

        private void pdfexporter_click(object sender, EventArgs e)
        {
            ToolStripMenuItem s=(ToolStripMenuItem)sender;
            s.Checked = true;
            Config.PDFExporter = PDF.Load(s.Name);
            foreach (ToolStripMenuItem p in pDFExporterToolStripMenuItem.DropDownItems)
            {
                if (p != s) p.Checked = false;
            }
        }

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
                        addtoItemButton.Enabled = Utils.fits(mp, Item.Get(((Possession)inventory2.SelectedItem).BaseItem, null));
                }
                if (listItems.SelectedItem is Spell)
                {
                    Choice_DisplaySpellScroll(sender, e);
                    if (spellbookFeaturesBox.SelectedItem != null && spellbookFeaturesBox.SelectedItem is SpellcastingCapsule)
                        addspellbookButton.Enabled = Utils.matches(((Spell)listItems.SelectedItem), ((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature.PrepareableSpells, ((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature.SpellcastingID) && !Player.current.getSpellcasting(((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature.SpellcastingID).getSpellbook().Contains((Spell)listItems.SelectedItem);
                }
                if (listItems.SelectedItem is Feature)
                {
                    Choice_DisplayFeature(sender, e);
                    addButton.Enabled = !Player.current.Boons.Contains(((Feature)listItems.SelectedItem).Name, ConfigManager.SourceInvariantComparer);
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
                    listItems.Items.AddRange(Item.Subsection((Category)itemCategories.SelectedItem).ToArray<Item>());
                    //inventorySplit.Panel2Collapsed = true;
                    inventory2.Enabled = false;
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
                    listItems.Items.AddRange(((MagicCategory)itemCategories.SelectedItem).Contents.ToArray<MagicProperty>());
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
                    listItems.Items.AddRange(Spell.Subsection().ToArray<Spell>());
                   // inventorySplit.Panel2Collapsed = true;
                    inventory2.Enabled = false;
                    addspellbookButton.Enabled = false;
                    actionBox.Controls.Clear();
                    actionBox.Controls.Add(addspellbookButton);
                    actionBox.Controls.Add(spellbookFeaturesBox);
                    actionBox.Controls.Add(addScrollButton);
                }
                else
                {
                    listItems.Items.AddRange(FeatureCollection.Subsection(itemCategories.SelectedItem.ToString()).ToArray<Feature>());
                    //inventorySplit.Panel2Collapsed = true;
                    inventory2.Enabled = false;
                    actionBox.Controls.Clear();
                    addButton.Enabled = false;
                    actionBox.Controls.Add(addButton);
                }
            }
        }

        private void itemsearchbox_TextChanged(object sender, EventArgs e)
        {
            if (itemsearchbox.ForeColor == Color.LightGray) Item.Search = "";
            else Item.Search = itemsearchbox.Text;
            itemCategories.Items.Clear();
            listItems.Items.Clear();
            itemCategories.Items.AddRange(Item.Section().ToArray<Category>());
            itemCategories.Items.AddRange(MagicProperty.Section().ToArray<MagicCategory>());
            itemCategories.Items.Add("Spells");
            itemCategories.Items.AddRange(FeatureCollection.Section().ToArray<string>());
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
        public void resetglobals()
        {
            displayElement.Navigate("about:blank");
        }
        public void UpdateLayout()
        {
            try
            {
                if (lastfile == "") this.Text = "Character Builder 5";
                else this.Text = "Character Builder 5 - " + lastfile;
                UpdateSideLayout();
                UpdateRaceLayout(false);
                UpdateBackgroundLayout(false);
                UpdatePersonal(false);
                UpdateScores(false);
                UpdateClass(false);
                UpdateSpellcastingOuter(false);
                UpdateEquipmentLayout(false);
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
            journalTab.SuspendLayout();
            int index = journalEntries.SelectedIndex;
            journalEntries.Items.Clear();
            foreach (JournalEntry je in Player.current.ComplexJournal) journalEntries.Items.Add(je);
            if (index >= 0 && index < journalEntries.Items.Count) journalEntries.SelectedIndex = index;
            else journalEntries_SelectedIndexChanged(null, null);
            
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
                Possession[] pos = Player.current.getItemsAndPossessions().ToArray<Possession>();
                inventory.Items.AddRange(pos);
                inventory2.Items.AddRange(pos);
                if (iindex >= 0 && iindex < inventory.Items.Count) inventory.SelectedIndex = iindex;
                int index = 0;
                if (spellbookFeaturesBox.SelectedIndex > 0) index = spellbookFeaturesBox.SelectedIndex;
                UpdateInventoryOptions();
                spellbookFeaturesBox.Items.Clear();
                spellbookFeaturesBox.Items.AddRange((from sc in Player.current.getFeatures() where sc is SpellcastingFeature && ((SpellcastingFeature)sc).Preparation == PreparationMode.Spellbook select new SpellcastingCapsule((SpellcastingFeature)sc)).ToArray<SpellcastingCapsule>());
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
                if (inventory.SelectedItem == null)
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
                }
                else
                {
                    Possession p = (Possession)inventory.SelectedItem;
                    possequip.Enabled = true;
                    try
                    {
                        possequip.SelectedItem = p.Equipped;
                    } catch (Exception)
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
                    magicproperties.Items.AddRange((from s in p.MagicProperties select MagicProperty.Get(s, null)).ToArray());
                    unpack.Enabled = (p.BaseItem != null && p.BaseItem != "" && p.Item is Pack);
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
                undoToolStripMenuItem.Enabled = Player.CanUndo();
                redoToolStripMenuItem.Enabled = Player.CanRedo();
                AbilityScoreArray scores = Player.current.getFinalAbilityScores();
                SideStrength.Text = "Str " + scores.Strength;
                SideDexterity.Text = "Dex " + scores.Dexterity;
                SideConstitution.Text = "Con " + scores.Constitution;
                SideIntelligence.Text = scores.Intelligence + " Int";
                SideWisdom.Text = scores.Wisdom + " Wis";
                SideCharisma.Text = scores.Charisma + " Cha";
                int speed = Player.current.getSpeed();
                SideSpeed.Text = speed + " ft";
                int hp = Player.current.getHitpointMax();
                Speed.Text = speed.ToString();
                SideMaxHP.Text = "HP: " + hp;
                MaxHP.Text = hp.ToString();
                int curhploss = Player.current.CurrentHPLoss;
                if (curhploss > 0) curhploss = 0;
                CurHP.Maximum = hp;
                CurHP.Value = hp + curhploss;
                sidePortrait.Image = Player.current.Portrait;
                SideName.Text = Player.current.Name + "\n" + String.Join(" | ", Player.current.Classes) + "\n(Level " + Player.current.getLevel() + ")\n" + Player.current.getRaceSubName();
                int ac = Player.current.getAC();
                SideAC.Text = ac + " AC";
                AC.Text = ac.ToString();
                int init = Player.current.getInitiative();
                SideInit.Text = "Init: " + plusMinus(init);
                Initiative.Text = plusMinus(init);
                double weight = Player.current.getWeight();
                currentweight.Text = weight.ToString("N") + " lb / " + scores.Strength * 15 + " lb";
                CurWeight.Text = weight.ToString("N");
                MaxWeight.Text = (scores.Strength * 15).ToString();
                Price money = Player.current.getMoney();
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
                inspiration.Checked = Player.current.Inspiration;
                int prof = Player.current.getProficiency();
                profval.Text = plusMinus(prof);
                AbilityScoreArray max = null;
                AbilityScoreArray asa = Player.current.getFinalAbilityScores(out max);
                Ability SaveProf = Player.current.getSaveProficiencies();
                AbilityScoreArray saveBonus = Player.current.getSavingThrowsBoni();
                strval.Text = asa.Strength.ToString();
                strmod.Text = plusMinus(asa.strmod);
                strmax.Text = max.Strength.ToString();
                strsave.Text = plusMinus(asa.strmod + (SaveProf.HasFlag(Ability.Strength) ? prof : 0) + saveBonus.Strength);
                dexval.Text = asa.Dexterity.ToString();
                dexmod.Text = plusMinus(asa.dexmod);
                dexmax.Text = max.Dexterity.ToString();
                dexsave.Text = plusMinus(asa.dexmod + (SaveProf.HasFlag(Ability.Dexterity) ? prof : 0) + saveBonus.Dexterity);
                conval.Text = asa.Constitution.ToString();
                conmod.Text = plusMinus(asa.conmod);
                conmax.Text = max.Constitution.ToString();
                consave.Text = plusMinus(asa.conmod + (SaveProf.HasFlag(Ability.Constitution) ? prof : 0) + saveBonus.Constitution);
                intval.Text = asa.Intelligence.ToString();
                intmod.Text = plusMinus(asa.intmod);
                intmax.Text = max.Intelligence.ToString();
                intsave.Text = plusMinus(asa.intmod + (SaveProf.HasFlag(Ability.Intelligence) ? prof : 0) + saveBonus.Intelligence);
                wisval.Text = asa.Wisdom.ToString();
                wismod.Text = plusMinus(asa.wismod);
                wismax.Text = max.Wisdom.ToString();
                wissave.Text = plusMinus(asa.wismod + (SaveProf.HasFlag(Ability.Wisdom) ? prof : 0) + saveBonus.Wisdom);
                chaval.Text = asa.Charisma.ToString();
                chamod.Text = plusMinus(asa.chamod);
                chamax.Text = max.Charisma.ToString();
                chasave.Text = plusMinus(asa.chamod + (SaveProf.HasFlag(Ability.Charisma) ? prof : 0) + saveBonus.Charisma);
                skillbox.Items.Clear();
                skillbox.Items.AddRange(Player.current.getSkills().ToArray());
                List<HitDie> hd = Player.current.getHitDie();
                hd.Sort();
                HitDice.Text = String.Join(", ", from h in hd select h.Total());
                HitDiceUsed.Items.Clear();
                HitDiceUsed.Items.AddRange(hd.ToArray());
                TempHP.Value = Player.current.TempHP;
                bonusMaxHP.Value = Player.current.BonusMaxHP;
                if (Player.current.FailedDeathSaves < 0) Player.current.FailedDeathSaves = 0;
                if (Player.current.SuccessDeathSaves < 0) Player.current.SuccessDeathSaves = 0;
                if (Player.current.FailedDeathSaves > 3) Player.current.FailedDeathSaves = 3;
                if (Player.current.SuccessDeathSaves > 3) Player.current.SuccessDeathSaves = 3;
                DeathFail.Value = Player.current.FailedDeathSaves;
                DeathSuccess.Value = Player.current.SuccessDeathSaves;
                int resourceindex = ResourcesBox.SelectedIndex;
                List<ModifiedSpell> bonusspells = new List<ModifiedSpell>(Player.current.getBonusSpells());
                foreach (ModifiedSpell mods in bonusspells)
                {
                    if (Player.current.getUsedResources(mods.Name) > 0) mods.used = true;
                    mods.displayShort = true;
                }
                ResourcesBox.Items.Clear();
                ResourcesBox.Items.AddRange(Player.current.getResourceInfo(true).Values.ToArray());
                ResourcesBox.Items.AddRange(bonusspells.ToArray());
                if (resourceindex >= 0 && resourceindex < ResourcesBox.Items.Count) ResourcesBox.SelectedIndex = resourceindex;
                else resourceused.Enabled = false;
                int featindex = Features.SelectedIndex;
                Features.Items.Clear();
                Features.Items.AddRange((from f in Player.current.getFeatures() where f.Name != "" && !f.Hidden select f).ToArray());
                if (featindex >= 0 && featindex < Features.Items.Count) Features.SelectedIndex = featindex;
                else hidefeature.Enabled = false;
                List<Condition> active = new List<Condition>(from c in Player.current.Conditions select Condition.Get(c, null));
                List<Condition> possible = new List<Condition>(Condition.conditions.Values);
                possible.RemoveAll(t => active.Contains(t));
                possible.Sort();
                active.Sort();
                activeConditions.Items.Clear();
                activeConditions.Items.AddRange(active.ToArray());
                availableConditions.Items.Clear();
                availableConditions.Items.AddRange(possible.ToArray());
                availableConditions.Items.Add("- Custom -");
                for (int i = 4; i < inplayflow.Controls.Count; i++)
                {
                    if (inplayflow.Controls[i] is GroupBox)
                    {
                        GroupBox box = (GroupBox)inplayflow.Controls[i];
                        string spellcasting = box.Name;
                        Spellcasting sc = Player.current.getSpellcasting(spellcasting);
                        if (box.Controls[0] is Label)
                        {
                            Ability spellcastingability = (Ability)int.Parse(box.Controls[0].Name);
                            Control[] spells = box.Controls.Find("SpellsBox", true);
                            if (spells.Count() > 0)
                            {
                                ListBox spellbox = (ListBox)spells[0];
                                List<ModifiedSpell> modspells = new List<ModifiedSpell>();
                                modspells.AddRange(sc.getLearned());
                                modspells.AddRange(sc.getPrepared());
                                foreach (ModifiedSpell ms in modspells)
                                {
                                    //if (ms.differentAbility == Ability.None) ms.differentAbility = spellcastingability;
                                    ms.AddAlwaysPreparedToName = false;
                                    ms.includeRecharge = false;
                                    ms.includeResources = false;
                                }
                                modspells.Sort();
                                spellbox.Items.Clear();
                                spellbox.Items.AddRange(modspells.ToArray());
                            }
                            Control[] highlights = box.Controls.Find("SpellOnSheetLabel", true);
                            if (highlights.Count() > 0)
                            {
                                Label highlight = (Label)highlights[0];
                                highlight.Text = "Spell on Sheet: " + (sc.Highlight != null && sc.Highlight != "" ? sc.Highlight : "--");
                            }
                            List<SpellSlotInfo> ssi = Player.current.getSpellSlotInfo(spellcasting);
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
                Proficiencies.Items.AddRange(Player.current.getLanguages().ToArray());
                Proficiencies.Items.AddRange(Player.current.getToolProficiencies().ToArray());
                Proficiencies.Items.AddRange(Player.current.getToolKWProficiencies().ToArray());
                Proficiencies.Items.AddRange(Player.current.getOtherProficiencies().ToArray());
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
                int level = Player.current.getLevel();
                List<Control> backt = new List<Control>();
                backt.Add(backgroundLabel);
                backt.Add(background);
                background.Items.Clear();
                background.ForeColor = System.Drawing.SystemColors.WindowText;
                Background back = Player.current.Background;
                if (back == null)
                {
                    background.Items.AddRange(Background.backgrounds.Values.ToArray<Background>());
                    background.Height = Background.backgrounds.Count() * background.ItemHeight + 10;
                }
                else
                {
                    background.Items.Add(back);
                    background.ForeColor = Config.SelectColor;
                    background.Height = background.ItemHeight + 10;
                    ControlAdder.AddControls(back, backt, level);
                    foreach (Feature f in Player.current.getBackgroundFeatures().OrderBy(a => a.Level))
                    {
                        ControlAdder.AddControl(backt, level, f);
                    }
                }
                backt.Add(traitLabel);
                backt.Add(traits);
                traits.Items.Clear();
                traits.ForeColor = System.Drawing.SystemColors.WindowText;
                if (Player.current.PersonalityTrait == null || Player.current.PersonalityTrait == "")
                {
                    if (back != null) traits.Items.AddRange(back.PersonalityTrait.ToArray<TableEntry>());
                    traits.Items.Add("- Custom Personality Trait -");
                }
                else
                {
                    traits.ForeColor = Config.SelectColor;
                    traits.Items.Add(Player.current.PersonalityTrait);
                }
                traits.Height = traits.Items.Count * traits.ItemHeight + 10;

                backt.Add(ideallabel);
                backt.Add(ideals);
                ideals.Items.Clear();
                ideals.ForeColor = System.Drawing.SystemColors.WindowText;
                if (Player.current.Ideal == null || Player.current.Ideal == "")
                {
                    if (back != null) ideals.Items.AddRange(back.Ideal.ToArray<TableEntry>());
                    ideals.Items.Add("- Custom Ideal -");
                }
                else
                {
                    ideals.ForeColor = Config.SelectColor;
                    ideals.Items.Add(Player.current.Ideal);
                }
                ideals.Height = ideals.Items.Count * ideals.ItemHeight + 10;

                backt.Add(bondlabel);
                backt.Add(bonds);
                bonds.Items.Clear();
                bonds.ForeColor = System.Drawing.SystemColors.WindowText;
                if (Player.current.Bond == null || Player.current.Bond == "")
                {
                    if (back != null) bonds.Items.AddRange(back.Bond.ToArray<TableEntry>());
                    bonds.Items.Add("- Custom Bond -");
                }
                else
                {
                    bonds.ForeColor = Config.SelectColor;
                    bonds.Items.Add(Player.current.Bond);
                }
                bonds.Height = bonds.Items.Count * bonds.ItemHeight + 10;

                backt.Add(flawlabel);
                backt.Add(flaws);
                flaws.Items.Clear();
                flaws.ForeColor = System.Drawing.SystemColors.WindowText;
                if (Player.current.Flaw == null || Player.current.Flaw == "")
                {
                    if (back != null) flaws.Items.AddRange(back.Flaw.ToArray<TableEntry>());
                    flaws.Items.Add("- Custom Flaw -");
                }
                else
                {
                    flaws.ForeColor = Config.SelectColor;
                    flaws.Items.Add(Player.current.Flaw);
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
        public void UpdateSpellcastingOuter(bool updateside = true)
        {
            try
            {
                if (updateside) UpdateSideLayout();
                layouting = true;
                spellcontrol.SuspendLayout();
                inplayflow.SuspendLayout();
                List<Feature> spellfeatures = new List<Feature>(from f in Player.current.getFeatures() where f is SpellcastingFeature || f is SpellChoiceFeature || f is ModifySpellChoiceFeature || f is IncreaseSpellChoiceAmountFeature select f);
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
                foreach (SpellcastingFeature sf in spellcasts)
                {
                    if (sf.SpellcastingID == "MULTICLASS") continue;
                    TabPage tab = new TabPage(sf.DisplayName);
                    tab.Name = sf.SpellcastingID;
                    spellcontrol.Controls.Add(tab);
                    Control target = tab;
                    if (sf.Preparation == PreparationMode.ClassList || sf.Preparation == PreparationMode.Spellbook)
                    {
                        SplitContainer split = new SplitContainer();
                        split.Dock = DockStyle.Fill;
                        split.Orientation = Orientation.Horizontal;
                        split.Size = new System.Drawing.Size(100, 100);
                        split.SplitterDistance = 50;
                        split.IsSplitterFixed = true;
                        target.Controls.Add(split);
                        target = split.Panel2;
                        SplitContainer split2 = new SplitContainer();
                        split2.Dock = DockStyle.Fill;
                        split2.Orientation = Orientation.Vertical;
                        split2.Size = new System.Drawing.Size(100, 100);
                        split2.SplitterDistance = 50;
                        split2.IsSplitterFixed = true;
                        split.Panel1.Controls.Add(split2);
                        split2.Panel1.Padding = new Padding(5);
                        split2.Panel2.Padding = new Padding(5);
                        GroupBox spellspreparedbox = new GroupBox();
                        spellspreparedbox.Text = "Prepared Spells";
                        spellspreparedbox.Name = sf.SpellcastingID + "=preparedbox";
                        spellspreparedbox.Dock = DockStyle.Fill;
                        ListBox spellsprepared = new ListBox();
                        spellsprepared.Name = sf.SpellcastingID + "=prepared";
                        spellsprepared.SelectedIndexChanged += Choice_DisplaySpell;
                        spellsprepared.Leave += listbox_Deselect_on_Leave;
                        spellsprepared.DoubleClick += unprepare_Spell;
                        spellsprepared.Dock = DockStyle.Fill;
                        spellspreparedbox.Controls.Add(spellsprepared);
                        split2.Panel1.Controls.Add(spellspreparedbox);
                        ListBox spellsprepareable = new ListBox();
                        spellsprepareable.SelectedIndexChanged += Choice_DisplaySpell;
                        spellsprepareable.Leave += listbox_Deselect_on_Leave;
                        spellsprepareable.DoubleClick += prepare_Spell;
                        spellsprepareable.Dock = DockStyle.Fill;
                        GroupBox spellsprepareablebox = new GroupBox();
                        spellsprepareablebox.Text = (sf.Preparation == PreparationMode.ClassList ? "Available Spells" : "Spellbook");
                        spellsprepareablebox.Name = sf.SpellcastingID + "=prepareablebox";
                        spellsprepareablebox.Dock = DockStyle.Fill;
                        spellsprepareablebox.Controls.Add(spellsprepareable);
                        split2.Panel2.Controls.Add(spellsprepareablebox);
                        spellsprepareable.Name = sf.SpellcastingID + "=prepareable";
                    }
                    SplitContainer split3 = new SplitContainer();
                    split3.Dock = DockStyle.Fill;
                    split3.Orientation = Orientation.Horizontal;
                    split3.Size = new System.Drawing.Size(100, 100);
                    split3.SplitterDistance = 50;
                    split3.IsSplitterFixed = true;
                    target.Controls.Add(split3);
                    SplitContainer split4 = new SplitContainer();
                    split4.Dock = DockStyle.Fill;
                    split4.Orientation = Orientation.Vertical;
                    split4.Size = new System.Drawing.Size(100, 100);
                    split4.SplitterDistance = 50;
                    split4.IsSplitterFixed = true;
                    split3.Panel2.Controls.Add(split4);
                    split4.Panel1.Padding = new Padding(5);
                    split4.Panel2.Padding = new Padding(5);

                    split3.Panel1.Padding = new Padding(5);
                    GroupBox spellchoicesbox = new GroupBox();
                    spellchoicesbox.Text = "Available Choices:";
                    spellchoicesbox.Name = sf.SpellcastingID + "=choicesbox";
                    spellchoicesbox.Dock = DockStyle.Fill;
                    ListBox spellchoices = new ListBox();
                    spellchoices.Name = sf.SpellcastingID + "=choices";
                    spellchoices.SelectedIndexChanged += Choice_DisplaySpellChoices;
                    spellchoices.Dock = DockStyle.Fill;
                    spellchoicesbox.Controls.Add(spellchoices);
                    split3.Panel1.Controls.Add(spellchoicesbox);

                    GroupBox spellchosenbox = new GroupBox();
                    spellchosenbox.Text = "Selected Spells:";
                    spellchosenbox.Name = sf.SpellcastingID + "=chosenbox";
                    spellchosenbox.Dock = DockStyle.Fill;
                    ListBox spellschosen = new ListBox();
                    spellschosen.Name = sf.SpellcastingID + "=chosen";
                    spellschosen.SelectedIndexChanged += Choice_DisplaySpell;
                    spellschosen.Leave += listbox_Deselect_on_Leave;
                    spellschosen.DoubleClick += deselect_Spell;
                    spellschosen.Dock = DockStyle.Fill;
                    spellchosenbox.Controls.Add(spellschosen);
                    split4.Panel1.Controls.Add(spellchosenbox);
                    ListBox spelltochoose = new ListBox();
                    spelltochoose.SelectedIndexChanged += Choice_DisplaySpell;
                    spelltochoose.Leave += listbox_Deselect_on_Leave;
                    spelltochoose.DoubleClick += select_Spell;
                    spelltochoose.Dock = DockStyle.Fill;
                    GroupBox spelltochoosebox = new GroupBox();
                    spelltochoosebox.Text = "Available Spells:";
                    spelltochoosebox.Name = sf.SpellcastingID + "=choosebox";
                    spelltochoosebox.Dock = DockStyle.Fill;
                    spelltochoosebox.Controls.Add(spelltochoose);
                    split4.Panel2.Controls.Add(spelltochoosebox);
                    spelltochoose.Name = sf.SpellcastingID + "=choose";

                    GroupBox box = new System.Windows.Forms.GroupBox();
                    box.Name = sf.SpellcastingID;
                    box.Size = new System.Drawing.Size(230, 457);
                    box.Text = "Spellcasting - " + sf.DisplayName;

                    Label attacksave = new Label();
                    attacksave.AutoEllipsis = true;
                    attacksave.Location = new System.Drawing.Point(6, 16);
                    attacksave.Name = ((int)sf.SpellcastingAbility).ToString();
                    attacksave.Size = new System.Drawing.Size(218, 13);
                    attacksave.Text = Enum.GetName(typeof(Ability), sf.SpellcastingAbility) + ": " + plusMinus(Player.current.getSpellAttack(sf.SpellcastingID, sf.SpellcastingAbility)) + " | DC " + Player.current.getSpellSaveDC(sf.SpellcastingID, sf.SpellcastingAbility);
                    attacksave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    box.Controls.Add(attacksave);

                    Button reset = new System.Windows.Forms.Button();
                    reset.Enabled = false;
                    reset.Location = new System.Drawing.Point(12, 425);
                    reset.Name = "ResetSlots";
                    reset.Size = new System.Drawing.Size(50, 23);
                    reset.Text = "Reset";
                    reset.Click += new System.EventHandler(this.button1_Click_2);
                    box.Controls.Add(reset);

                    NumericUpDown slotused = new NumericUpDown();
                    slotused.Enabled = false;
                    slotused.Location = new System.Drawing.Point(12, 399);
                    slotused.Name = "SpellSlotUsed";
                    slotused.Size = new System.Drawing.Size(50, 20);
                    slotused.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
                    box.Controls.Add(slotused);

                    Label spellonsheet = new Label();
                    spellonsheet.AutoEllipsis = true;
                    spellonsheet.AutoSize = true;
                    spellonsheet.Location = new System.Drawing.Point(6, 363);
                    spellonsheet.Name = "SpellOnSheetLabel";
                    spellonsheet.Text = "Spell on Sheet: --";
                    spellonsheet.Cursor = Cursors.Hand;
                    spellonsheet.DoubleClick += new System.EventHandler(this.label_DoubleClick);
                    box.Controls.Add(spellonsheet);

                    ListBox spellslots = new ListBox();
                    spellslots.ColumnWidth = 60;
                    spellslots.FormattingEnabled = true;
                    spellslots.Location = new System.Drawing.Point(68, 382);
                    spellslots.MultiColumn = true;
                    spellslots.Name = "SpellSlotBox";
                    spellslots.Size = new System.Drawing.Size(156, 69);
                    spellslots.SelectedIndexChanged += new System.EventHandler(this.listBox1_IndexChanged);
                    spellslots.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
                    spellslots.MouseWheel += listbox_MouseWheel;
                    box.Controls.Add(spellslots);

                    Label label = new Label();
                    label.AutoSize = true;
                    label.Location = new System.Drawing.Point(9, 382);
                    label.Size = new System.Drawing.Size(56, 13);
                    label.Text = "Slots Used:";
                    box.Controls.Add(label);

                    ListBox spells = new ListBox();
                    spells.FormattingEnabled = true;
                    spells.Location = new System.Drawing.Point(6, 31);
                    spells.Name = "SpellsBox";
                    spells.Size = new System.Drawing.Size(218, 329);
                    spells.SelectedIndexChanged += new System.EventHandler(this.Choice_DisplayModifiedSpell);
                    spells.Leave += new System.EventHandler(this.listbox_Deselect_on_Leave);
                    spells.DoubleClick += new System.EventHandler(this.listBox2_DoubleClick);
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
                if (choicebox != null && choicebox.SelectedItem != null && choicebox.SelectedItem is SpellChoiceCapsule)
                {
                    Player.MakeHistory("");
                    SpellChoiceFeature scf = ((SpellChoiceCapsule)choicebox.SelectedItem).Spellchoicefeature;
                    string r = ((Spell)lb.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Spell)lb.SelectedItem).Source;
                    Player.current.getSpellChoice(tab.Name, scf.UniqueID).Choices.RemoveAll(t => ConfigManager.SourceInvariantComparer.Equals(t, r));
                    //UpdateSpellChoices(choicebox);
                    UpdateSpellcastingInner();
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
                if (choicebox != null && choicebox.SelectedItem != null && choicebox.SelectedItem is SpellChoiceCapsule)
                {
                    SpellChoiceFeature scf = ((SpellChoiceCapsule)choicebox.SelectedItem).Spellchoicefeature;
                    List<Feature> spellfeatures = new List<Feature>(from f in Player.current.getFeatures() where f is SpellcastingFeature || f is SpellChoiceFeature || f is ModifySpellChoiceFeature || f is IncreaseSpellChoiceAmountFeature select f);
                    int amount = scf.Amount;
                    foreach (Feature f in spellfeatures) if (f is IncreaseSpellChoiceAmountFeature && ((IncreaseSpellChoiceAmountFeature)f).UniqueID == scf.UniqueID) amount += ((IncreaseSpellChoiceAmountFeature)f).Amount;
                    SpellChoice sc=Player.current.getSpellChoice(tab.Name, scf.UniqueID);
                    if (sc.Choices.Count < amount)
                    {
                        Player.MakeHistory("");
                        sc.Choices.Add(((Spell)lb.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Spell)lb.SelectedItem).Source);
                    }
                    //UpdateSpellChoices(choicebox);
                    UpdateSpellcastingInner(true, spellfeatures);
                    UpdateInPlayInner();
                }
            }
        }

        private void Choice_DisplaySpellChoices(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            List<Feature> spellfeatures = new List<Feature>(from f in Player.current.getFeatures() where f is SpellcastingFeature || f is SpellChoiceFeature || f is ModifySpellChoiceFeature || f is IncreaseSpellChoiceAmountFeature select f);
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
                    displayElement.Document.Write(new FeatureContainer(choicefeats).toHTML());
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
                List<Feature> spellfeatures = new List<Feature>(from f in Player.current.getFeatures() where f is SpellcastingFeature || f is SpellChoiceFeature || f is ModifySpellChoiceFeature || f is IncreaseSpellChoiceAmountFeature select f);
                SpellcastingFeature sf = null;
                foreach (Feature f in spellfeatures) if (f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcontrol.SelectedTab.Name) sf = (SpellcastingFeature)f;
                if (sf != null)
                {
                    Spellcasting sc = Player.current.getSpellcasting(sf.SpellcastingID);
                    if (sc.getPreparedList().Count < Utils.availableToPrepare(sf, Player.current.getClassLevel(sf.SpellcastingID)))
                    {
                        Player.MakeHistory("");
                        sc.getPreparedList().Add(((Spell)lb.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Spell)lb.SelectedItem).Source);
                    }
                    UpdateSpellcastingInner(true, spellfeatures);
                    UpdateInPlayInner();
                }
            }
        }

        private void unprepare_Spell(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem != null)
            {
                Spellcasting sc = Player.current.getSpellcasting(spellcontrol.SelectedTab.Name);
                Player.MakeHistory("");
                string r = ((Spell)lb.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Spell)lb.SelectedItem).Source;
                sc.getPreparedList().RemoveAll(s => ConfigManager.SourceInvariantComparer.Equals(s, r));
                UpdateSpellcastingInner();
                UpdateInPlayInner();
            }
        }
        public void UpdateSpellcastingInner(bool updateside = true, List<Feature> spellfeatures=null)
        {
            try
            {
                layouting = true;
                if (spellfeatures == null) spellfeatures = new List<Feature>(from f in Player.current.getFeatures() where f is SpellcastingFeature || f is SpellChoiceFeature || f is ModifySpellChoiceFeature || f is IncreaseSpellChoiceAmountFeature select f);
                List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>();
                foreach (Feature f in spellfeatures) if (f is SpellcastingFeature) spellcasts.Add((SpellcastingFeature)f);
                if (updateside) UpdateSideLayout();
                foreach (SpellcastingFeature sf in spellcasts)
                {
                    Spellcasting sc = Player.current.getSpellcasting(sf.SpellcastingID);
                    Control tab = null;
                    foreach (Control tp in spellcontrol.Controls) if (tp.Name == sf.SpellcastingID) tab = (Control)tp;
                    if (tab != null)
                    {
                        int classlevel = Player.current.getClassLevel(sf.SpellcastingID);
                        if (sf.Preparation == PreparationMode.ClassList || sf.Preparation == PreparationMode.Spellbook)
                        {
                            Control[] prepare = tab.Controls.Find(sf.SpellcastingID + "=prepared", true);
                            if (prepare.Count() > 0)
                            {
                                ListBox prep = (ListBox)prepare[0];
                                Control[] preparebox = tab.Controls.Find(sf.SpellcastingID + "=preparedbox", true);
                                if (preparebox.Count() > 0)
                                {
                                    preparebox[0].Text = "Prepared Spells (" + sc.getPreparedList().Count + "/" + Utils.availableToPrepare(sf, classlevel) + ")";
                                }
                                List<Spell> preparedspells = new List<Spell>(sc.getPrepared());
                                //List<Spell> preparedspells = new List<Spell>(from s in sc.Prepared select Spell.Get(s));
                                prep.Items.Clear();
                                prep.Items.AddRange(preparedspells.ToArray<Spell>());
                                if (sf.Preparation == PreparationMode.ClassList)
                                {
                                    Control[] prepareable = tab.Controls.Find(sf.SpellcastingID + "=prepareable", true);
                                    if (prepareable.Count() > 0)
                                    {
                                        ListBox prepable = (ListBox)prepareable[0];
                                        prepable.Items.Clear();
                                        List<Spell> prepableSpells = new List<Spell>(sc.getAdditionalClassSpells());
                                        prepableSpells.AddRange(Utils.filterSpell(sf.PrepareableSpells, sf.SpellcastingID, classlevel));
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
                                        prepable.Items.AddRange(sc.getSpellbook().Where(s => !preparedspells.Exists(t => t.Name == s.Name && s.Source == t.Source)).ToArray<Spell>());
                                    }
                                }
                            }
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
                                scf.CalculatedChoices = Player.current.getSpellChoice(sf.SpellcastingID, scf.Spellchoicefeature.UniqueID).Choices;
                                scf.CalculatedAmount = scf.Spellchoicefeature.Amount;
                                foreach (Feature f in spellfeatures) if (f is IncreaseSpellChoiceAmountFeature && ((IncreaseSpellChoiceAmountFeature)f).UniqueID == scf.Spellchoicefeature.UniqueID) scf.CalculatedAmount += ((IncreaseSpellChoiceAmountFeature)f).Amount;
                            }

                            choice.Items.AddRange(scfs.ToArray<SpellChoiceCapsule>());
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
                    if (spellfeatures == null) spellfeatures = new List<Feature>(from f in Player.current.getFeatures() where f is SpellcastingFeature || f is SpellChoiceFeature || f is ModifySpellChoiceFeature || f is IncreaseSpellChoiceAmountFeature select f);
                    SpellcastingFeature sf = null;
                    foreach (Feature f in spellfeatures) if (f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID == spellcontrol.SelectedTab.Name) sf = (SpellcastingFeature)f;
                    if (sf != null)
                    {
                        int classlevel = Player.current.getClassLevel(sf.SpellcastingID);
                        List<Spell> available = new List<Spell>(Utils.filterSpell(scf.AvailableSpellChoices, sf.SpellcastingID, classlevel));
                        List<Spell> chosen = new List<Spell>(Player.current.getSpellChoice(sf.SpellcastingID, scf.UniqueID).Choices.Select(t => Spell.Get(t, scf.Source)));
                        int amount = scf.Amount;
                        foreach (Feature f in spellfeatures)
                        {
                            if (f is ModifySpellChoiceFeature && ((ModifySpellChoiceFeature)f).UniqueID == scf.UniqueID)
                                available.AddRange(Utils.filterSpell(((ModifySpellChoiceFeature)f).AdditionalSpellChoices, sf.SpellcastingID, classlevel));
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
                portraitBox.Image = Player.current.Portrait;
                characterName.Text = Player.current.Name;
                XP.Minimum = Player.current.getXP(true);
                XP.Value = Player.current.getXP();
                Alignment.Text = Player.current.Alignment;
                PlayerName.Text = Player.current.PlayerName;
                XPtoUP.Value = Level.XpToLevelUp(Player.current.getXP());
                Age.Value = Player.current.Age;
                HeightValue.Text = Player.current.Height;
                Weight.Value = Player.current.Weight;
                Eyes.Text = Player.current.Eyes;
                Skin.Text = Player.current.Skin;
                Hair.Text = Player.current.Hair;
                FactionName.Text = Player.current.FactionName;
                FactionInsignia.Image = Player.current.FactionImage;
                Backstory.Text = Player.current.Backstory;
                Allies.Text = Player.current.Allies;
                journalentrybox.Items.Clear();
                journalentrybox.Items.AddRange((from s in Player.current.Journal select s.IndexOfAny(new char[] { '\r', '\n' }) == -1 ? s : s.Substring(0, s.IndexOfAny(new char[] { '\r', '\n' }))).ToArray());
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
                    List<string> taken = new List<string>(Player.current.getFeatNames());
                    foreach (Feature f in Player.current.getFeatures()) if (f is CollectionChoiceFeature && (((CollectionChoiceFeature)f).Collection == null || ((CollectionChoiceFeature)f).Collection == "")) taken.AddRange(((CollectionChoiceFeature)f).Choices(Player.current));
                    taken.RemoveAll(s => s == Player.current.getAbilityFeatChoice(asff).Feat);
                    AbilityFeatBox.Items.Clear();
                    AbilityFeatBox.Items.Add("+1 Strength");
                    AbilityFeatBox.Items.Add("+1 Dexterity");
                    AbilityFeatBox.Items.Add("+1 Constitution");
                    AbilityFeatBox.Items.Add("+1 Intelligence");
                    AbilityFeatBox.Items.Add("+1 Wisdom");
                    AbilityFeatBox.Items.Add("+1 Charisma");
                    int level = Player.current.getLevel();
                    AbilityFeatBox.Items.AddRange(FeatureCollection.Get("").Where(f => !taken.Contains(f.Name) && f.Level <= level).ToArray<Feature>());
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
                classesBox.Items.AddRange(ClassDefinition.GetClasses(ci.Level, Player.current).ToArray<ClassDefinition>());
                if (ci.Class != null)
                {
                    hpSpinner.Minimum = 0;
                    hpSpinner.Maximum = ci.Class.HitDie;
                    hpSpinner.Value = ci.Hp;
                    hpSpinner.Enabled = true;
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
                List<SubClassFeature> subclass = new List<SubClassFeature>(from Feature f in Player.current.getFeatures() where f is SubClassFeature select (SubClassFeature)f);
                int index = classList.SelectedIndex;
                int top = classList.TopIndex;
                classList.Items.Clear();
                classList.Items.AddRange(Player.current.getClassInfos().ToArray<ClassInfo>());
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
                //List<ClassDefinition> classes = Player.current.getClassesByLevel();
                //List<int> hprolls = Player.current.getHProllsByLevel();
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
                    Label scl = new Label();
                    scl.AutoSize = true;
                    scl.Dock = System.Windows.Forms.DockStyle.Top;
                    scl.Name = "subclasslabel" + sc.ParentClass;
                    scl.Padding = new System.Windows.Forms.Padding(0, 8, 0, 3);
                    scl.Text = "Choose a Subclass for " + sc.ParentClass + ":";
                    classt.Add(scl);
                    ListBox scb = new ListBox();
                    scb.Dock = System.Windows.Forms.DockStyle.Top;
                    scb.Name = sc.ParentClass;
                    scb.SelectedIndexChanged += new System.EventHandler(this.Choice_DisplaySubClass);
                    scb.DoubleClick += new System.EventHandler(this.subclassbox_DoubleClick);
                    scb.Leave += new System.EventHandler(this.listbox_Deselect_on_Leave);
                    scb.MouseWheel += listbox_MouseWheel;
                    SubClass scs = Player.current.getSubclass(sc.ParentClass);
                    if (scs == null) scb.Items.AddRange(SubClass.For(sc.ParentClass).ToArray<SubClass>());
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
                int level = Player.current.getLevel();
                ControlAdder.AddClassControls(Player.current, classt, level);
                foreach (Feature f in Player.current.getCommonFeaturesAndFeats())
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
            Player.MakeHistory("");
            if (Player.current.getSubclass(l.Name) == null) Player.current.AddSubclass(l.Name, ((SubClass)l.SelectedItem).Name);
            else Player.current.RemoveSubclass(l.Name);
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
                    displayElement.Document.Write(selected.toHTML());
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
                Strength.Value = Player.current.BaseStrength;
                Dexterity.Value = Player.current.BaseDexterity;
                Constitution.Value = Player.current.BaseConstitution;
                Intelligence.Value = Player.current.BaseIntelligence;
                Wisdom.Value = Player.current.BaseWisdom;
                Charisma.Value = Player.current.BaseCharisma;
                AbilityScoreArray max;
                AbilityScoreArray scores = Player.current.getFinalAbilityScores(out max);
                StrengthFinal.Text = "=" + scores.Strength;
                StrengthMod.Text = "(" + plusMinus(AbilityScores.getMod(scores.Strength)) + ")";
                DexterityFinal.Text = "=" + scores.Dexterity;
                DexterityMod.Text = "(" + plusMinus(AbilityScores.getMod(scores.Dexterity)) + ")";
                ConstitutionFinal.Text = "=" + scores.Constitution;
                ConstitutionMod.Text = "(" + plusMinus(AbilityScores.getMod(scores.Constitution)) + ")";
                IntelligenceFinal.Text = "=" + scores.Intelligence;
                IntelligenceMod.Text = "(" + plusMinus(AbilityScores.getMod(scores.Intelligence)) + ")";
                WisdomFinal.Text = "=" + scores.Wisdom;
                WisdomMod.Text = "(" + plusMinus(AbilityScores.getMod(scores.Wisdom)) + ")";
                CharismaFinal.Text = "=" + scores.Charisma;
                CharismaMod.Text = "(" + plusMinus(AbilityScores.getMod(scores.Charisma)) + ")";
                StrengthMax.Text = "| " + max.Strength;
                DexterityMax.Text = "| " + max.Dexterity;
                ConstitutionMax.Text = "| " + max.Constitution;
                IntelligenceMax.Text = "| " + max.Intelligence;
                WisdomMax.Text = "| " + max.Wisdom;
                CharismaMax.Text = "| " + max.Charisma;
                PointBuyRemaining.Text = "Points left: " + Utils.getPointsRemaining(Player.current);
                int index = AbilityFeatChoiceBox.SelectedIndex;
                AbilityFeatChoiceBox.Items.Clear();
                AbilityFeatChoiceBox.Items.AddRange((from asff in Player.current.getAbilityIncreases() select new AbilityFeatChoiceContainer(asff)).ToArray<AbilityFeatChoiceContainer>());
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
                    rb.Text = Player.current.AbilityFeatChoices[c].ToString();
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
                int level = Player.current.getLevel();
                List<Control> racet = new List<Control>();
                racet.Add(racelabel);
                racet.Add(racebox);
                racebox.Items.Clear();
                racebox.ForeColor = System.Drawing.SystemColors.WindowText;
                Race rac = Player.current.Race;
                
                

                List<String> parentraces = new List<string>();
                foreach (Feature f in Player.current.getFeatures().Where<Feature>(f => f is SubRaceFeature)) parentraces.AddRange(((SubRaceFeature)f).Races);
                if (parentraces.Count > 0)
                {
                    racet.Add(subracelabel);
                    racet.Add(subracebox);
                    SubRace subrac = Player.current.SubRace;
                    subracebox.Items.Clear();
                    subracebox.ForeColor = System.Drawing.SystemColors.WindowText;
                    if (subrac == null) subracebox.Items.AddRange(SubRace.For(parentraces).ToArray<SubRace>());
                    else
                    {
                        subracebox.Items.Add(subrac);
                        subracebox.ForeColor = Config.SelectColor;
                        //subrac.AddControls(racet);
                        //foreach (Feature f in Player.current.getSubRaceFeatures()) f.AddControl(racet);
                    }
                    subracebox.Height = subracebox.Items.Count * subracebox.ItemHeight + 10;
                }
                if (rac == null) racebox.Items.AddRange(Race.races.Values.ToArray<Race>());
                else
                {
                    racebox.Items.Add(rac);
                    racebox.ForeColor = Config.SelectColor;
                    ControlAdder.AddControls(rac, racet, level);
                    foreach (Feature f in Player.current.getRaceFeatures().OrderBy(a => a.Level))
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
                displayElement.Document.Write(selected.toHTML());
                displayElement.Refresh();
            }
            
        }

        private void background_DoubleClick(object sender, EventArgs e)
        {
            Background selected = (Background)background.SelectedItem;
            if (selected != null)
            {
                Player.MakeHistory("");
                if (Player.current.Background == null) Player.current.Background = selected;
                else Player.current.Background = null;
                UpdateBackgroundLayout();
            }
        }

        private void traits_DoubleClick(object sender, EventArgs e)
        {
            if (traits.SelectedItem != null) {
                Player.MakeHistory("");
                if (Player.current.PersonalityTrait == null || Player.current.PersonalityTrait == "")
                {
                    if (traits.SelectedIndex == traits.Items.Count - 1) Player.current.PersonalityTrait = Interaction.InputBox("Custom Personality Trait:", "CB 5");
                    else Player.current.PersonalityTrait = traits.SelectedItem.ToString();
                }
                else Player.current.PersonalityTrait = "";
                UpdateBackgroundLayout();
            }
        }

        private void ideals_DoubleClick(object sender, EventArgs e)
        {
            if (ideals.SelectedItem != null)
            {
                Player.MakeHistory("");
                if (Player.current.Ideal == null || Player.current.Ideal == "")
                {
                    if (ideals.SelectedIndex == ideals.Items.Count - 1) Player.current.Ideal = Interaction.InputBox("Custom Ideal:", "CB 5");
                    else Player.current.Ideal = ideals.SelectedItem.ToString();
                }
                else Player.current.Ideal = "";
                UpdateBackgroundLayout();
            }
        }

        private void bonds_DoubleClick(object sender, EventArgs e)
        {
            if (bonds.SelectedItem != null)
            {
                Player.MakeHistory("");
                if (Player.current.Bond == null || Player.current.Bond == "")
                {
                    if (bonds.SelectedIndex == bonds.Items.Count - 1) Player.current.Bond = Interaction.InputBox("Custom Bond:", "CB 5");
                    else Player.current.Bond = bonds.SelectedItem.ToString();
                }
                else Player.current.Bond = "";
                UpdateBackgroundLayout();
            }
        }

        private void flaws_DoubleClick(object sender, EventArgs e)
        {
            if (flaws.SelectedItem != null)
            {
                Player.MakeHistory("");
                if (Player.current.Flaw == null || Player.current.Flaw == "")
                {
                    if (flaws.SelectedIndex == flaws.Items.Count - 1) Player.current.Flaw = Interaction.InputBox("Custom Flaw:", "CB 5");
                    else Player.current.Flaw = flaws.SelectedItem.ToString();
                }
                else Player.current.Flaw = "";
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
                Player.MakeHistory("");
                Choice c = Player.current.getChoice(choicer.Name);
                if (c == null || c.Value == "")
                {
                    bool old = ConfigManager.AlwaysShowSource;
                    ConfigManager.AlwaysShowSource = true;
                    if (choicer.SelectedIndex == choicer.Items.Count - 1) Player.current.setChoice(choicer.Name,Interaction.InputBox("Custom Entry:", "CB 5"));
                    else Player.current.setChoice(choicer.Name,choicer.SelectedItem.ToString());
                    ConfigManager.AlwaysShowSource = old;
                }
                else Player.current.RemoveChoice(choicer.Name);
                Program.MainWindow.UpdateLayout();
            }
        }
        public void Choice_DoubleClick(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null)
            {
                Player.MakeHistory("");
                Choice c = Player.current.getChoice(choicer.Name);
                bool old = ConfigManager.AlwaysShowSource;
                ConfigManager.AlwaysShowSource = true;
                if (c == null || c.Value == "") Player.current.setChoice(choicer.Name, choicer.SelectedItem.ToString());
                else Player.current.RemoveChoice(choicer.Name);
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
                    displayElement.Document.Write(selected.toHTML());
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
                    displayElement.Document.Write(selected.toHTML());
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
                    displayElement.Document.Write(selected.toHTML());
                    displayElement.Refresh();
                }
            }
        }
        public void Choice_DisplaySpellScroll(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null && choicer.SelectedItem is Spell)
            {
                Scroll selected = new Character_Builder_5.Scroll((Spell)choicer.SelectedItem);
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.toHTML());
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
                selected.Modifikations.AddRange(from f in Player.current.getFeatures() where f is SpellModifyFeature && Utils.matches(selected, ((SpellModifyFeature)f).Spells, null) select f);
                List<Feature> ls =
                selected.Modifikations = selected.Modifikations.Distinct().ToList();
                selected.Info = Player.current.getAttack(selected, (selected.differentAbility == Ability.None?spellcastingability:selected.differentAbility));
                if (selected != null)
                {
                    displayElement.Navigate("about:blank");
                    displayElement.Document.OpenNew(true);
                    displayElement.Document.Write(selected.toHTML());
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
                    displayElement.Document.Write(selected.toHTML());
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
                    displayElement.Document.Write(selected.toHTML());
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
                    displayElement.Document.Write(selected.toHTML());
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
                    displayElement.Document.Write(selected.toHTML());
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
                    displayElement.Document.Write(selected.toHTML());
                    displayElement.Refresh();
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Player.UnsavedChanges == 0 || MessageBox.Show(Player.UnsavedChanges + " unsaved changes will be lost. Continue?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OpenFileDialog od = new OpenFileDialog();
                od.Filter = "CB5 XML|*.cb5";
                od.Title = "Open a Player File";
                od.ShowDialog();
                if (od.FileName != "")
                {
                    try
                    {
                        using (FileStream fs = (FileStream)od.OpenFile())
                        {
                            Player newP = Player.Load(fs);
                            Player.UndoBuffer = new LinkedList<Player>();
                            Player.RedoBuffer = new LinkedList<Player>();
                            Player.UnsavedChanges = 0;
                            Player.current = newP;

                        }
                        lastfile = od.FileName;
                        Program.resetglobals();
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
                SaveFileDialog od = new SaveFileDialog();
                od.Filter = "CB5 XML|*.cb5";
                od.Title = "Save a Player File";
                od.ShowDialog();
                if (od.FileName != "")
                {
                    try
                    {
                        lastfile = od.FileName;
                        using (FileStream fs = (FileStream)od.OpenFile()) Player.current.Save(fs);
                        Player.UnsavedChanges = 0;
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
                        Player.current.Save(fs);
                        Player.UnsavedChanges = 0;
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
            if (Player.UnsavedChanges == 0 || MessageBox.Show(Player.UnsavedChanges + " unsaved changes will be lost. Continue?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lastfile = "";
                Player.current = new Player();
                Player.UndoBuffer = new LinkedList<Player>();
                Player.RedoBuffer = new LinkedList<Player>();
                Player.UnsavedChanges = 0;
                Program.resetglobals();
                UpdateLayout();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog od = new SaveFileDialog();
            od.Filter = "CB5 XML|*.cb5";
            od.Title = "Save a Player File";
            od.ShowDialog();
            if (od.FileName != "")
            {
                try
                {
                    lastfile = od.FileName;
                    using (FileStream fs = (FileStream)od.OpenFile()) Player.current.Save(fs);
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
            SaveFileDialog od = new SaveFileDialog();
            od.Filter = "PDF|*.pdf";
            od.Title = "Save a PDF File";
            od.ShowDialog();
            if (od.FileName != "")
            {
                try
                {
                    using (FileStream fs = (FileStream)od.OpenFile())
                    {
                        Config.PDFExporter.export(fs, preservePDFFormsToolStripMenuItem.Checked, includeResourcesInSheetToolStripMenuItem.Checked);
                    }
                    MessageBox.Show("PDF exported to: " + od.FileName, "CB5");
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error exporting PDF to " + od.FileName);
                }
            }
        }
        private void defaultPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.PDFExporter = PDF.Load("DefaultPDF.xml");
            defaultPDFToolStripMenuItem.Checked = true;
            alternateToolStripMenuItem.Checked = false;
        }

        private void alternateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.PDFExporter = PDF.Load("AlternatePDF.xml");
            alternateToolStripMenuItem.Checked = true;
            defaultPDFToolStripMenuItem.Checked = false;
        }

        private void racebox_DoubleClick(object sender, EventArgs e)
        {
            Race selected = (Race)racebox.SelectedItem;
            if (selected != null)
            {
                Player.MakeHistory("");
                if (Player.current.Race == null) Player.current.Race = selected;
                else Player.current.Race = null;
                UpdateRaceLayout();
            }
        }

        private void racebox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Race selected = (Race)racebox.SelectedItem;
            if (selected != null)
            {
                displayElement.Navigate("about:blank");
                displayElement.Document.OpenNew(true);
                displayElement.Document.Write(selected.toHTML());
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
                displayElement.Document.Write(selected.toHTML());
                displayElement.Refresh();
            }
        }

        private void subracebox_DoubleClick(object sender, EventArgs e)
        {
            SubRace selected = (SubRace)subracebox.SelectedItem;
            if (selected != null)
            {
                Player.MakeHistory("");
                if (Player.current.SubRace == null) Player.current.SubRace = selected;
                else Player.current.SubRace = null;
                UpdateRaceLayout();
            }
        }

        private void characterName_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("Name");
                Player.current.Name = characterName.Text;
                UpdateSideLayout();
            }
        }

        private void removePortrait_Click(object sender, EventArgs e)
        {
            Player.MakeHistory("");
            Player.current.Portrait = null;
            UpdatePersonal();
        }

        private void choosePortrait_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose a Portrait";
            List<String> extensions=new List<string>();
            foreach (string s in (from c in ImageCodecInfo.GetImageEncoders() select c.FilenameExtension)) extensions.AddRange(s.Split(';'));
            ofd.Filter = "Image Files | *." + String.Join(";", extensions);
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                try
                {
                    Player.MakeHistory("");
                    Player.current.Portrait = new Bitmap(ofd.FileName);
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
                Player.MakeHistory("");
                Player.current.Alignment = Alignment.Text;
            }
        }

        private void XP_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("XP");
                Decimal xp=Decimal.Round(XP.Value);
                if (XP.Value == xp)
                {
                    Player.current.setXP((int)XP.Value);
                    UpdateLayout();
                }
                else if (XP.Value > xp) XP.Value = xp + Level.XpToLevelUp((int)xp);
                else XP.Value = Math.Max(XP.Minimum, xp - Level.XpToLevelDown((int)xp));
            }
        }

        private void PlayerName_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("Playername");
                Player.current.PlayerName = PlayerName.Text;
            }
        }

        private void Age_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("Age");
                Player.current.Age = (int)Age.Value;
            }
        }

        private void Weight_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("Weight");
                Player.current.Weight = (int)Weight.Value;
            }
        }

        private void Height_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("Height");
                Player.current.Height = HeightValue.Text;
            }
        }

        private void Eyes_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("Eyes");
                Player.current.Eyes = Eyes.Text;
            }
        }

        private void Skin_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("Skin");
                Player.current.Skin = Skin.Text;
            }
        }

        private void Hair_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("Hair");
                Player.current.Hair = Hair.Text;
            }
        }

        private void FactionName_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("Factionname");
                Player.current.FactionName = FactionName.Text;
            }
        }

        private void FactionBlank_Click(object sender, EventArgs e)
        {
            Player.MakeHistory("");
            Player.current.FactionImage = null;
            UpdatePersonal();
        }

        private void FactionChoose_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose a Insignia";
            List<String> extensions = new List<string>();
            foreach (string s in (from c in ImageCodecInfo.GetImageEncoders() select c.FilenameExtension)) extensions.AddRange(s.Split(';'));
            ofd.Filter = "Image Files | *." + String.Join(";", extensions);
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                try
                {
                    Player.MakeHistory("");
                    Player.current.FactionImage = new Bitmap(ofd.FileName);
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
                Player.MakeHistory("Background");
                Player.current.Backstory = Backstory.Text;
            }
        }

        private void Allies_TextChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("Allies");
                Player.current.Allies = Allies.Text;
            }
        }
        public static String plusMinus(int x)
        {
            if (x < 0) return x.ToString();
            return "+" + x;
        }

        private void Score_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("Score"+((Control)sender).Name);
                Player.current.BaseStrength = (int)Strength.Value;
                Player.current.BaseDexterity = (int)Dexterity.Value;
                Player.current.BaseConstitution = (int)Constitution.Value;
                Player.current.BaseIntelligence = (int)Intelligence.Value;
                Player.current.BaseWisdom = (int)Wisdom.Value;
                Player.current.BaseCharisma = (int)Charisma.Value;
                UpdateLayout();
            }
        }

        private void ArrayBox_DoubleClick(object sender, EventArgs e)
        {
            if (ArrayBox.SelectedItem != null)
            {
                Player.MakeHistory("");
                AbilityScoreArray a = (AbilityScoreArray)ArrayBox.SelectedItem;
                Player.current.BaseStrength = a.Strength;
                Player.current.BaseDexterity = a.Dexterity;
                Player.current.BaseConstitution = a.Constitution;
                Player.current.BaseIntelligence = a.Intelligence;
                Player.current.BaseWisdom = a.Wisdom;
                Player.current.BaseCharisma = a.Charisma;
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
                        Player.MakeHistory("");
                        NumericUpDown n=((NumericUpDown)sender);
                        int temp = 0;
                        if (s == "Strength")
                        {
                            temp = Player.current.BaseStrength;
                            Player.current.BaseStrength = (int)n.Value;
                        }
                        if (s == "Dexterity")
                        {
                            temp = Player.current.BaseDexterity;
                            Player.current.BaseDexterity = (int)n.Value;
                        }
                        if (s == "Constitution")
                        {
                            temp = Player.current.BaseConstitution;
                            Player.current.BaseConstitution = (int)n.Value;
                        }
                        if (s == "Intelligence")
                        {
                            temp = Player.current.BaseIntelligence;
                            Player.current.BaseIntelligence = (int)n.Value;
                        }
                        if (s == "Wisdom")
                        {
                            temp = Player.current.BaseWisdom;
                            Player.current.BaseWisdom = (int)n.Value;
                        }
                        if (s == "Charisma")
                        {
                            temp = Player.current.BaseCharisma;
                            Player.current.BaseCharisma = (int)n.Value;
                        }
                        if (n.Name == "Strength") Player.current.BaseStrength = temp;
                        if (n.Name == "Dexterity") Player.current.BaseDexterity = temp;
                        if (n.Name == "Constitution") Player.current.BaseConstitution = temp;
                        if (n.Name == "Intelligence") Player.current.BaseIntelligence = temp;
                        if (n.Name == "Wisdom") Player.current.BaseWisdom = temp;
                        if (n.Name == "Charisma") Player.current.BaseCharisma = temp;
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
            AbilityFeatChoice afc = Player.current.getAbilityFeatChoice(asff);
            if (o is string)
            {
                Player.MakeHistory("");
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
                Player.MakeHistory("");
                afc.Ability1 = Ability.None;
                afc.Ability2 = Ability.None;
                afc.Feat = ((Feature)o).Name;
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
                Player.current.setHPRoll(level, (int)l.Value);
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
                Player.current.DeleteClass(level);
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
                    displayElement.Document.Write(selected.toHTML());
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
                Player.MakeHistory("");
                level = int.Parse(l.Name.TrimStart("classBox".ToCharArray()));
                //Player.current.AddClass((ClassDefinition)l.SelectedItem, level);
                UpdateLayout();
            }
        }

        private void AbilityFeatChoiceBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAbilityFeatList();
        }

        private void classList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateClassesBox();
        }

        private void classesBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (classList.SelectedItem != null && classesBox.SelectedItem != null)
            {
                Player.MakeHistory("");
                ClassInfo ci = (ClassInfo)classList.SelectedItem;
                ClassDefinition cur=Player.current.GetClass(ci.Level);
                if (cur == (ClassDefinition)classesBox.SelectedItem) return;
                if (cur != null) Player.current.DeleteClass(ci.Level);
                Player.current.AddClass((ClassDefinition)classesBox.SelectedItem, ci.Level);
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
                Player.MakeHistory("HP"+ci.Level);
                Player.current.setHPRoll(ci.Level, (int)hpSpinner.Value);
                UpdateLayout();
            }
        }

        private void classList_DoubleClick(object sender, EventArgs e)
        {
            if (classList.SelectedItem != null)
            {
                Player.MakeHistory("");
               ClassInfo ci = (ClassInfo)classList.SelectedItem;
               Player.current.DeleteClass(ci.Level);
               UpdateLayout();
            }
        }

        private void AbilityFeatChoiceBox_DoubleClick(object sender, EventArgs e)
        {
            if (classList.SelectedItem != null)
            {
                Player.MakeHistory("");
                AbilityFeatChoice afc = Player.current.getAbilityFeatChoice(((AbilityFeatChoiceContainer)AbilityFeatChoiceBox.SelectedItem).ASFF);
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
                Player.MakeHistory("");
                SpellcastingFeature sc = ((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature;
                Player.current.getSpellcasting(sc.SpellcastingID).getAdditionalList().Add(((Spell)listItems.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Spell)listItems.SelectedItem).Source);
                UpdateLayout();
            }
        }

        private void addtoItemButton_Click(object sender, EventArgs e)
        {
            if (inventory2.SelectedItem != null && listItems.SelectedItem != null && listItems.SelectedItem is MagicProperty)
            {
                Player.MakeHistory("");
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
                Player.current.AddPossession(p);
                UpdateEquipmentLayout();
            }
        }

        private void addScrollButton_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItem != null && listItems.SelectedItem is Spell)
            {
                Player.MakeHistory("");
                Player.current.Items.Add(((Spell)listItems.SelectedItem).Name);
                UpdateEquipmentLayout();
            }
        }

        private void buyButton_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItem != null && listItems.SelectedItem is Item)
            {
                Player.MakeHistory("");
                Item i=(Item)listItems.SelectedItem;
                int count = (int)ItemCounter.Value;
                for (int c = 0; c < count; c++)
                    Player.current.Items.Add(i.Name);
                //Player.current.Pay(new Price(i.Price,count));
                if (count > 1)
                {
                    Player.current.ComplexJournal.Add(new JournalEntry(i.ToString() + " x " + count, new Price(i.Price, count)));
                } else Player.current.ComplexJournal.Add(new JournalEntry(i.ToString(), new Price(i.Price, count)));
                UpdateLayout();
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItem != null)
            {
                Player.MakeHistory("");
                if (listItems.SelectedItem is Item)
                {
                    int count=(int)ItemCounter.Value;
                    for (int c = 0; c < count; c++ )
                        Player.current.Items.Add(((Item)listItems.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Item)listItems.SelectedItem).Source);
                }
                if (listItems.SelectedItem is MagicProperty && (((MagicProperty)listItems.SelectedItem).Base == null || ((MagicProperty)listItems.SelectedItem).Base == "")) 
                    Player.current.Possessions.Add(new Possession((Item)null,(MagicProperty)listItems.SelectedItem));
                if (listItems.SelectedItem is Feature)
                {
                    Player.current.Boons.Add(((Feature)listItems.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((Feature)listItems.SelectedItem).Source);
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
                        addtoItemButton.Enabled = Utils.fits(mp, Item.Get(p.BaseItem, null));
                    }
                }
                displayElement.Navigate("about:blank");
                displayElement.Document.OpenNew(true);
                displayElement.Document.Write(((Possession)inventory2.SelectedItem).toHTML());
                displayElement.Refresh();
            }
        }

        private void spellbookFeaturesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (spellbookFeaturesBox.SelectedItem != null && listItems.SelectedItem != null && listItems.SelectedItem is Spell)
            {
                addspellbookButton.Enabled = Utils.matches(((Spell)listItems.SelectedItem), ((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature.PrepareableSpells, ((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature.SpellcastingID) && !Player.current.getSpellcasting(((SpellcastingCapsule)spellbookFeaturesBox.SelectedItem).Spellcastingfeature.SpellcastingID).getSpellbook().Contains((Spell)listItems.SelectedItem);
            }
        }

        private void inventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (inventory.SelectedItem != null)
            {
                displayElement.Navigate("about:blank");
                displayElement.Document.OpenNew(true);
                displayElement.Document.Write(((Possession)inventory.SelectedItem).toHTML());
                displayElement.Refresh();
            }
            UpdateInventoryOptions();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (inventory.SelectedItem != null && magicproperties.SelectedItem != null)
            {
                Player.MakeHistory("");
                string mp = ((MagicProperty)magicproperties.SelectedItem).Name + " " + ConfigManager.SourceSeperator + " " + ((MagicProperty)magicproperties.SelectedItem).Source;
                int index = ((Possession)inventory.SelectedItem).MagicProperties.FindIndex(m => ConfigManager.SourceInvariantComparer.Equals(e, mp));
                if (index >= 0) ((Possession)inventory.SelectedItem).MagicProperties.RemoveAt(index);
                UpdateLayout();
            }
        }

        private void removepossesion_Click(object sender, EventArgs e)
        {
            if (inventory.SelectedItem != null)
            {
                Player.MakeHistory("");
                Player.current.removePossessionAndItems((Possession)inventory.SelectedItem);
                UpdateLayout();
            }
        }

        private void changecount_Click(object sender, EventArgs e)
        {
            if (inventory.SelectedItem != null)
            {
                Player.MakeHistory("");
                Player.current.changePossessionAmountAndAddRemoveItemsAccordingly((Possession)inventory.SelectedItem,(int)poscounter.Value);
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
            if (inventory.SelectedItem != null)
            {
                Player.MakeHistory("");
                Possession p = (Possession)inventory.SelectedItem;
                Possession newp = new Possession(p);
                if ((int)poscounter.Value >= p.Count) return;
                int stacksize = 1;
                if (p.Item != null) stacksize = Math.Max(1, p.Item.StackSize);
                newp.Count = p.Count - (int)poscounter.Value;
                if (((int)poscounter.Value) % stacksize != 0 && p.Item != null) Player.current.Items.Add(p.BaseItem);
                p.Count=(int)poscounter.Value;
                Player.current.AddPossession(p);
                if (newp.Count > 0) Player.current.AddPossession(newp);
                UpdateLayout();
            }
        }

        private void updateposs_Click(object sender, EventArgs e)
        {
            if (inventory.SelectedItem != null)
            {
                Player.MakeHistory("");
                Possession p = (Possession)inventory.SelectedItem;
                p.Name = possname.Text;
                p.Description = possdescription.Text;
                Player.current.AddPossession(p);
                InventoryRefresh();
            }
        }

        private void newposs_Click(object sender, EventArgs e)
        {
            Player.MakeHistory("");
            Possession p = new Possession(possname.Text, possdescription.Text, (int)poscounter.Value, (double)possweight.Value);
            p.Name = possname.Text;
            p.Description = possdescription.Text;
            Player.current.AddPossession(p);
            UpdateLayout();
        }

        private void possequip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                if (inventory.SelectedItem != null)
                {
                    string es = (string)possequip.SelectedItem;
                    Player.MakeHistory("");
                    foreach (Possession pos in Player.current.Possessions)
                    {
                        if (string.Equals(pos.Equipped, es, StringComparison.InvariantCultureIgnoreCase)) pos.Equipped = EquipSlot.None;
                    }
                    Possession p = (Possession)inventory.SelectedItem;
                    p.Equipped = es;
                    Player.current.AddPossession(p);
                    UpdateEquipmentLayout();
                }
            }
        }

        private void attunedcheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                if (inventory.SelectedItem != null)
                {
                    Player.MakeHistory("");
                    Possession p = (Possession)inventory.SelectedItem;
                    p.Attuned = attunedcheck.Checked;
                    Player.current.AddPossession(p);
                    UpdateLayout();
                }
            }
        }

        private void posscharges_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                if (inventory.SelectedItem != null)
                {
                    Player.MakeHistory("");
                    Possession p = (Possession)inventory.SelectedItem;
                    p.ChargesUsed = (int)posscharges.Value;
                    Player.current.AddPossession(p);
                    InventoryRefresh();
                }
            }
        }

        private void possweight_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                if (inventory.SelectedItem != null)
                {
                    Player.MakeHistory("");
                    Possession p = (Possession)inventory.SelectedItem;
                    p.Weight = (double)possweight.Value;
                    Player.current.AddPossession(p);
                    InventoryRefresh();
                }
            }
        }

        private void highlightcheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                if (inventory.SelectedItem != null)
                {
                    Player.MakeHistory("");
                    Possession p = (Possession)inventory.SelectedItem;
                    p.Hightlight = highlightcheck.Checked;
                    Player.current.AddPossession(p);
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
                Player.MakeHistory("Money" + ((Control)sender).Name);
                Player.current.setMoney((int)CP.Value, (int)SP.Value, (int)EP.Value, (int)GP.Value, (int)PP.Value);
                UpdateSideLayout();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Player.Undo();
            UpdateLayout();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Player.Redo();
            UpdateLayout();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Player.UnsavedChanges == 0 || MessageBox.Show(Player.UnsavedChanges + " unsaved changes will be lost. Continue?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else e.Cancel = true;
        }

        private void inspiration_CheckedChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("");
                Player.current.Inspiration = inspiration.Checked;
            }
        }

        private void skillabilitybox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (skillabilitybox.SelectedItem != null && skillbox.SelectedItem != null)
                skillablityresult.Text = "= " + plusMinus(Player.current.getSkill(((SkillInfo)skillbox.SelectedItem).Skill, indextoability(skillabilitybox.SelectedIndex)));
        }

        private void hdreset_Click(object sender, EventArgs e)
        {
            Player.MakeHistory("");
            Player.current.UsedHitDice = new List<int>();
            UpdateInPlayInner();
        }

        private void HitDiceUsed_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void HitDiceUsed_DoubleClick(object sender, EventArgs e)
        {
            if (HitDiceUsed.SelectedItem != null)
            {
                HitDie hd = (HitDie)HitDiceUsed.SelectedItem;
                if (hd.Used < hd.Count)
                {
                    Player.MakeHistory("");
                    Player.current.UseHitDie(hd.Dice);
                    UpdateInPlayInner();
                }
            }
        }

        private void CurHP_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("CurHP");
                Player.current.CurrentHPLoss = (int)CurHP.Value - Player.current.getHitpointMax();
            }
        }

        private void TempHP_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("TempHP");
                Player.current.TempHP = (int)TempHP.Value;
            }
        }

        private void DeathSuccess_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("DeathSuccess");
                Player.current.SuccessDeathSaves = (int)DeathSuccess.Value;
            }
        }

        private void DeathFail_ValueChanged(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("DeathFail");
                Player.current.FailedDeathSaves = (int)DeathFail.Value;
            }
        }

        private void ResourcesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ResourcesBox.SelectedItem != null)
            {
                if (ResourcesBox.SelectedItem is ResourceInfo)
                {
                    ResourceInfo selected = (ResourceInfo)ResourcesBox.SelectedItem;
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
                        displayElement.Document.Write(Player.current.getResourceFeatures(selected.ResourceID).toHTML());
                        displayElement.Refresh();
                    }
                }
                else if (ResourcesBox.SelectedItem is ModifiedSpell)
                {
                    ModifiedSpell selected = (ModifiedSpell)ResourcesBox.SelectedItem;
                    if (selected != null)
                    {
                        layouting = true;
                        selected.Info = Player.current.getAttack(selected, selected.differentAbility);
                        if ((selected.Level > 0 && selected.RechargeModifier < RechargeModifier.AtWill) || (selected.Level == 0 && selected.RechargeModifier != RechargeModifier.Unmodified && selected.RechargeModifier < RechargeModifier.AtWill))
                        {
                            resourceused.Enabled = true;
                            resourceused.Maximum = 1;
                            resourceused.Value = 0;
                            if (Player.current.getUsedResources(selected.Name)>0) resourceused.Value = 1;
                        }
                        else resourceused.Enabled = false;
                        layouting = false;
                        displayElement.Navigate("about:blank");
                        displayElement.Document.OpenNew(true);
                        displayElement.Document.Write(selected.toHTML());
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
                    Player.MakeHistory("Resource" + ((ResourceInfo)ResourcesBox.SelectedItem).ResourceID);
                    Player.current.setUsedResources(((ResourceInfo)ResourcesBox.SelectedItem).ResourceID, (int)resourceused.Value);
                    UpdateInPlayInner();
                }
                else if (ResourcesBox.SelectedItem is ModifiedSpell)
                {
                    Player.MakeHistory("Resource" + ((ModifiedSpell)ResourcesBox.SelectedItem).Name);
                    Player.current.setUsedResources(((ModifiedSpell)ResourcesBox.SelectedItem).Name, (int)resourceused.Value);
                    UpdateInPlayInner();
                }
            }
        }

        private void shortrest_Click(object sender, EventArgs e)
        {
            Player.MakeHistory("ShortRest");
            foreach (ResourceInfo r in Player.current.getResourceInfo(true).Values)
            {
                if (r.Recharge >= RechargeModifier.ShortRest) Player.current.setUsedResources(r.ResourceID, 0);
            }
            foreach (ModifiedSpell ms in Player.current.getBonusSpells())
            {
                if (ms.RechargeModifier >= RechargeModifier.ShortRest) Player.current.setUsedResources(ms.Name, 0);
            }
            UpdateInPlayInner();
        }

        private void longrest_Click(object sender, EventArgs e)
        {
            Player.MakeHistory("LongRest");
            foreach (ResourceInfo r in Player.current.getResourceInfo(true).Values)
            {
                if (r.Recharge >= RechargeModifier.LongRest) Player.current.setUsedResources(r.ResourceID, 0);
            }
            foreach (ModifiedSpell ms in Player.current.getBonusSpells())
            {
                if (ms.RechargeModifier >= RechargeModifier.LongRest) Player.current.setUsedResources(ms.Name, 0);
            }
            UpdateInPlayInner();
        }

        private void ResourcesBox_DoubleClick(object sender, EventArgs e)
        {
            if (ResourcesBox.SelectedItem != null)
            {
                if (ResourcesBox.SelectedItem is ResourceInfo)
                {
                    ResourceInfo selected = (ResourceInfo)ResourcesBox.SelectedItem;
                    if (selected.Used < selected.Max)
                    {
                        Player.MakeHistory("Resource" + selected.ResourceID);
                        Player.current.setUsedResources(selected.ResourceID, selected.Used + 1);
                        UpdateInPlayInner();
                    }
                }
                else if (ResourcesBox.SelectedItem is ModifiedSpell)
                {
                    ModifiedSpell selected = (ModifiedSpell)ResourcesBox.SelectedItem;
                    Player.MakeHistory("Resource" + selected.Name);
                    Player.current.setUsedResources(selected.Name, 1);
                    UpdateInPlayInner();
                }
            }
        }

        private void Features_DoubleClick(object sender, EventArgs e)
        {
            if (Features.SelectedItem != null && Features.SelectedItem is Feature) {
                Player.MakeHistory("");
                Feature f = (Feature)Features.SelectedItem;
                if (Player.current.HiddenFeatures.RemoveAll(s => StringComparer.OrdinalIgnoreCase.Equals(s, f.Name)) == 0) Player.current.HiddenFeatures.Add(f.Name);
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
                hidefeature.Checked = Player.current.HiddenFeatures.Exists(s => StringComparer.OrdinalIgnoreCase.Equals(s, f.Name));
                layouting = false;
                Choice_DisplayFeature(sender, e);
            }
            else hidefeature.Enabled = false;
        }

        private void hidefeature_CheckedChanged(object sender, EventArgs e)
        {
            if (!layouting && Features.SelectedItem != null && Features.SelectedItem is Feature)
            {
                Player.MakeHistory("");
                Feature f = (Feature)Features.SelectedItem;
                if (hidefeature.Checked) Player.current.HiddenFeatures.Add(f.Name);
                else Player.current.HiddenFeatures.RemoveAll(s => StringComparer.OrdinalIgnoreCase.Equals(s, f.Name));
                //UpdateInPlayInner();
            }
        }

        private void availableConditions_DoubleClick(object sender, EventArgs e)
        {
            if (availableConditions.SelectedItem != null)
            {
                Player.MakeHistory("");
                if (availableConditions.SelectedItem is Condition)
                {
                    Player.current.Conditions.Add(((Condition)availableConditions.SelectedItem).Name);
                }
                else
                {
                    string c = Interaction.InputBox("Custom Condition:", "CB 5");
                    if (c != null && c != "") Player.current.Conditions.Add(c);
                }
                UpdateInPlayInner();
            }
        }

        private void activeConditions_DoubleClick(object sender, EventArgs e)
        {
            if (activeConditions.SelectedItem != null && activeConditions.SelectedItem is Condition)
            {
                Player.MakeHistory("");
                Player.current.Conditions.RemoveAll(t => StringComparer.InvariantCultureIgnoreCase.Equals(t, ((Condition)activeConditions.SelectedItem).Name));
                UpdateInPlayInner();
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem != null && lb.SelectedItem is ModifiedSpell)
            {
                Player.MakeHistory("");
                Player.current.getSpellcasting(lb.Parent.Name).Highlight = ((ModifiedSpell)lb.SelectedItem).Name;
                UpdateInPlayInner();
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem != null && lb.SelectedItem is SpellSlotInfo)
            {
                SpellSlotInfo ssi = (SpellSlotInfo)lb.SelectedItem;
                Player.MakeHistory("");
                if (ssi.Used < ssi.Slots) Player.current.setSpellSlot(ssi.SpellcastingID, ssi.Level, ssi.Used + 1);
                UpdateInPlayInner();
            }
        }

        private void label_DoubleClick(object sender, EventArgs e)
        {
            Player.MakeHistory("");
            Player.current.getSpellcasting(((Control)sender).Parent.Name).Highlight = null;
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
            Player.MakeHistory("");
            Player.current.resetSpellSlots(((Control)sender).Parent.Name);
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
                        Player.MakeHistory("Spellslots" + ssi.SpellcastingID);
                        Player.current.setSpellSlot(ssi.SpellcastingID, ssi.Level, (int)nup.Value);
                        UpdateInPlayInner();
                    }
                }
            }
        }

        private void unpack_Click(object sender, EventArgs e)
        {
            if (inventory.SelectedItem != null)
            {
                Possession p = (Possession)inventory.SelectedItem;
                if (p.BaseItem != null && p.BaseItem != "" && p.Item is Pack)
                {
                    Player.MakeHistory("");
                    for (int i = 0; i < p.Count; i++)
                        Player.current.Items.AddRange(((Pack)p.Item).Contents);
                    Player.current.removePossessionAndItems(p);
                    UpdateLayout();
                }
            }
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            if (!layouting)
            {
                Player.MakeHistory("BonusMaxHP");
                Player.current.BonusMaxHP = (int)bonusMaxHP.Value;
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
                if (Player.UnsavedChanges == 0 || MessageBox.Show("The application needs to restart for that.\n" + Player.UnsavedChanges + " unsaved changes will be lost. Continue?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Launch itself as administrator
                    ProcessStartInfo proc = new ProcessStartInfo();
                    proc.UseShellExecute = true;
                    proc.WorkingDirectory = Environment.CurrentDirectory;
                    proc.FileName = Application.ExecutablePath;
                    proc.Verb = "runas";
                    proc.Arguments = BuildCommandLineFromArgs(new string[] { lastfile, "register" });
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
                Program.register();
            }
        }

        private void portraitBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                e.Effect = DragDropEffects.Copy;
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
                Player.MakeHistory("");
                Player.current.Portrait = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                UpdatePersonal();
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    Player.MakeHistory("");
                    string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (file.Count() == 1) Player.current.Portrait = new Bitmap(file[0]);
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
            DoDragDrop(Player.current.Portrait, DragDropEffects.Copy);
        }

        private void FactionInsignia_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                Player.MakeHistory("");
                Player.current.FactionImage = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                UpdatePersonal();
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    Player.MakeHistory("");
                    string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (file.Count() == 1) Player.current.FactionImage = new Bitmap(file[0]);
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
                            Player p = Player.Load(fs);
                            if (p != null)
                            {
                                if (Player.UnsavedChanges == 0 || MessageBox.Show(Player.UnsavedChanges + " unsaved changes will be lost. Continue?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    Player.UndoBuffer = new LinkedList<Player>();
                                    Player.RedoBuffer = new LinkedList<Player>();
                                    Player.UnsavedChanges = 0;
                                    Player.current = p;
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
            if (journalentrybox.SelectedIndex >= 0 && journalentrybox.SelectedIndex < Player.current.Journal.Count)
                journalbox.Text = Player.current.Journal[journalentrybox.SelectedIndex];
            if (!waslayouting) layouting = false;
        }

        private void newentrybotton_Click(object sender, EventArgs e)
        {
            Player.MakeHistory("");
            Player.current.Journal.Add(journalbox.Text != "" ? journalbox.Text : "-New Entry-");
            UpdatePersonal();
        }

        private void journalbox_TextChanged(object sender, EventArgs e)
        {
            if (!layouting && journalentrybox.SelectedIndex >= 0 && journalentrybox.SelectedIndex < Player.current.Journal.Count)
            {
                Player.MakeHistory("JournalEntry" + journalentrybox.SelectedIndex);
                Player.current.Journal[journalentrybox.SelectedIndex] = journalbox.Text;
                int index = journalbox.Text.IndexOfAny(new char[] { '\r', '\n' });
                journalentrybox.Items[journalentrybox.SelectedIndex] = index == -1 ? journalbox.Text : journalbox.Text.Substring(0, index);
            }
        }

        private void removejournalentry_Click(object sender, EventArgs e)
        {
            if (journalentrybox.SelectedIndex >= 0 && journalentrybox.SelectedIndex < Player.current.Journal.Count)
            {
                Player.MakeHistory("");
                Player.current.Journal.RemoveAt(journalentrybox.SelectedIndex);
                UpdatePersonal();
            }
        }

        private void Display_Generic(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox choicer = (System.Windows.Forms.ListBox)sender;
            if (choicer != null && choicer.SelectedItem != null)
            {
                if (choicer.SelectedItem is IHTML)
                {
                    IHTML selected = (IHTML)choicer.SelectedItem;
                    if (selected != null)
                    {
                        displayElement.Navigate("about:blank");
                        displayElement.Document.OpenNew(true);
                        displayElement.Document.Write(selected.toHTML());
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
                journalCP.Value = je.CP;
                journalSP.Value = je.SP;
                journalGP.Value = je.GP;
                journalEP.Value = je.EP;
                journalPP.Value = je.PP;
                journalXP.Value = je.XP;
                journalTitle.Enabled = true;
                journalTime.Enabled = true;
                journalText.Enabled = true;
                journalCP.Enabled = true;
                journalSP.Enabled = true;
                journalGP.Enabled = true;
                journalEP.Enabled = true;
                journalPP.Enabled = true;
                journalXP.Enabled = true;
                removeJournalButton.Enabled = true;
            } else
            {
                journalTitle.Text = "";
                journalTime.Text = "";
                journalText.Text = "";
                journalCP.Value = 0;
                journalSP.Value = 0;
                journalGP.Value = 0;
                journalEP.Value = 0;
                journalPP.Value = 0;
                journalXP.Value = 0;
                journalTitle.Enabled = false;
                journalTime.Enabled = false;
                journalText.Enabled = false;
                journalCP.Enabled = false;
                journalSP.Enabled = false;
                journalGP.Enabled = false;
                journalEP.Enabled = false;
                journalPP.Enabled = false;
                journalXP.Enabled = false;
                removeJournalButton.Enabled = false;
            }
            if (!waslayouting) layouting = false;

        }

        private void journalTitle_TextChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Player.MakeHistory("JournalTitle");
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
            Player.MakeHistory("JournalText");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.Text = journalText.Text;
            }
        }

        private void journalTime_TextChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Player.MakeHistory("JournalTime");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.Time = journalTime.Text;
            }
        }

        private void journalXP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Player.MakeHistory("JournalXP");
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
            Player.MakeHistory("JournalPP");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.PP = (int)journalPP.Value;
                UpdateSideLayout();
            }
        }

        private void journalGP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Player.MakeHistory("JournalGP");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.GP = (int)journalGP.Value;
                UpdateSideLayout();
            }
        }

        private void journalEP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Player.MakeHistory("JournalEP");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.EP = (int)journalEP.Value;
                UpdateSideLayout();
            }
        }

        private void journalSP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Player.MakeHistory("JournalSP");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.SP = (int)journalSP.Value;
                UpdateSideLayout();
            }
        }

        private void journalCP_ValueChanged(object sender, EventArgs e)
        {
            if (layouting) return;
            Player.MakeHistory("JournalCP");
            if (journalEntries.SelectedItem is JournalEntry)
            {
                JournalEntry je = journalEntries.SelectedItem as JournalEntry;
                je.CP = (int)journalCP.Value;
                UpdateSideLayout();
            }
        }

        private void newJournalEntry_Click(object sender, EventArgs e)
        {
            Player.MakeHistory();
            Player.current.ComplexJournal.Add(new JournalEntry());
            UpdateJournal();
        }

        private void removeJournalButton_Click(object sender, EventArgs e)
        {
            Player.MakeHistory();
            if (journalEntries.SelectedItem is JournalEntry)
            {
                Player.current.ComplexJournal.RemoveAt(journalEntries.SelectedIndex);
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
                displayElement.Document.Write(selected.toHTML());
                displayElement.Refresh();
            }
        }

        private void showDescriptionToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            ConfigManager.Description = showDescriptionToolStripMenuItem.Checked;
        }
    }
}