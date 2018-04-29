using Character_Builder_Forms;
using OGL;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class Features : UserControl
    {
        private List<Feature> list;
        private MouseEventArgs drag = null;
        public IHistoryManager HistoryManager { get; set; }

        public List<Feature> features
        {
            get
            {
                return list;
            }
            set
            {
                list = value;
                fill();
            }
        }
        public WebBrowser preview { get; set; }
        public Features()
        {
            InitializeComponent();
            Feature.DETAILED_TO_STRING = true;
        }

        private void fill()
        {
            listBox1.Items.Clear();
            if (list == null) return;
            foreach (object o in list) listBox1.Items.Add(o);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                HistoryManager?.MakeHistory(null);
                list.Remove((Feature)listBox1.SelectedItem);
            }
            fill();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(new FeatureContainer((Feature)listBox1.SelectedItem).ToHTML());
                preview.Refresh();
            }
        }

        private void editFeature(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                list[listBox1.SelectedIndex] = FeatureForms.FeatureForm.dispatch((Feature)listBox1.SelectedItem, HistoryManager);
            fill();
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) drag = e;
        }

        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = null;
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag == null || this.listBox1.SelectedItem == null) return;
            Point point = listBox1.PointToClient(new Point(e.X, e.Y));
            if ((e.X - drag.X) * (e.X - drag.X) + (e.Y - drag.Y) * (e.Y - drag.Y) < 6) return;
            this.listBox1.DoDragDrop(this.listBox1.SelectedItem, DragDropEffects.Move);
            drag = null;
        }

        private void listBox1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point point = listBox1.PointToClient(new Point(e.X, e.Y));
            if (point.Y < 0 || point.Y > listBox1.Size.Height) return;
            int index = this.listBox1.IndexFromPoint(point);
            if (index < 0) index = this.listBox1.Items.Count - 1;
            object data = e.Data.GetData(e.Data.GetFormats()[0]);
            if (data == null) return;
            this.listBox1.Items.Remove(data);
            this.listBox1.Items.Insert(index, data);
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            Point point = listBox1.PointToClient(new Point(e.X, e.Y));
            int index = this.listBox1.IndexFromPoint(point);
            if (index < 0) index = this.listBox1.Items.Count - 1;
            object data = e.Data.GetData(e.Data.GetFormats()[0]);
            if (data == null)
                return;
            if (data is Feature)
            {
                HistoryManager?.MakeHistory(null);
                list.Remove(data as Feature);
                list.Insert(index, data as Feature);
                fill();
            }
        }

        private void listBox1_DragLeave(object sender, EventArgs e)
        {
            fill();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            contextMenuStrip2.Show(button1, new Point(0, button1.Size.Height));
        }

        private void basicFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Feature f = new Feature();
            f = new FeatureForms.FeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void abilityScoreFeatSelectionFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbilityScoreFeatFeature f = new AbilityScoreFeatFeature("", 4);
            f = new FeatureForms.AbilityScoreFeatFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void abilityScoreFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbilityScoreFeature f = new AbilityScoreFeature();
            f = new FeatureForms.AbilityScoreFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void aCCalculationFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ACFeature f = new ACFeature()
            {
                Expression = "if(Armor, if(Light, BaseAC + DexMod, if(Medium, BaseAC + Min(DexMod, 2), BaseAC)), 10 + DexMod) + ShieldBonus"
            };
            f = new FeatureForms.ACFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void statBonusFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BonusFeature f = new BonusFeature();
            f = new FeatureForms.BonusFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void bonusSpellFeatureToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BonusSpellFeature f = new BonusSpellFeature();
            f = new FeatureForms.BonusSpellFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void bonusSpellCoiceFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BonusSpellKeywordChoiceFeature f = new BonusSpellKeywordChoiceFeature();
            f = new FeatureForms.BonusSpellKeywordChoiceFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string s = ((Feature)listBox1.SelectedItem).Save();
                Clipboard.SetText(s, TextDataFormat.UnicodeText);
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string s = ((Feature)listBox1.SelectedItem).Save();
                Clipboard.SetText(s);
                HistoryManager?.MakeHistory(null);
                list.Remove((Feature)listBox1.SelectedItem);
                fill();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    HistoryManager?.MakeHistory(null);
                    foreach (Feature f in Feature.LoadString(Clipboard.GetText())) list.Add(f);
                    fill();
                }
            } catch (Exception) { }
        }

        private void bonusAlwaysPreparedSpellsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BonusSpellPrepareFeature f = new BonusSpellPrepareFeature();
            f = new FeatureForms.BonusSpellPrepareFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void featureChoiceFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoiceFeature f = new ChoiceFeature();
            f = new FeatureForms.ChoiceFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void featureCollectionChoiceFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollectionChoiceFeature f = new CollectionChoiceFeature()
            {
                Collection = "Category = 'Feats'"
            };
            f = new FeatureForms.CollectionChoiceFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void extraAttackFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtraAttackFeature f = new ExtraAttackFeature("Extra Attack", "Beginning at 5th level, you can attack twice, instead of once, whenever you take the Attack action on your turn.", 1, 5);
            f = new FeatureForms.ExtraAttackFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void freeItemGoldFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FreeItemAndGoldFeature f = new FreeItemAndGoldFeature();
            f = new FeatureForms.FreeItemAndGoldFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void hitPointFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HitPointsFeature f = new HitPointsFeature();
            f = new FeatureForms.HitPointsFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void increaseSpellchoicesFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IncreaseSpellChoiceAmountFeature f = new IncreaseSpellChoiceAmountFeature();
            f = new FeatureForms.IncreaseSpellChoiceAmountFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void itemChoiceByConditionFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemChoiceConditionFeature f = new ItemChoiceConditionFeature()
            {
                Condition = "Armor and Heavy"
            };
            f = new FeatureForms.ItemChoiceConditionFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void itemChoiceFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemChoiceFeature f = new ItemChoiceFeature();
            f = new FeatureForms.ItemChoiceFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void languageChoiceFeatureToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LanguageChoiceFeature f = new LanguageChoiceFeature();
            f = new FeatureForms.LanguageChoiceFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void languageProficiencyFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LanguageProficiencyFeature f = new LanguageProficiencyFeature();
            f = new FeatureForms.LanguageProficiencyFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void otherProficiencyFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Feature f = new OtherProficiencyFeature();
            f = new FeatureForms.FeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void saveProficiencyFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProficiencyFeature f = new SaveProficiencyFeature();
            f = new FeatureForms.SaveProficiencyFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            ResourceFeature f = new ResourceFeature();
            f = new FeatureForms.ResourceFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void skillProficiencyFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkillProficiencyFeature f = new SkillProficiencyFeature();
            f = new FeatureForms.SkillProficiencyFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void skillChoiceProficiencyFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkillProficiencyChoiceFeature f = new SkillProficiencyChoiceFeature();
            f = new FeatureForms.SkillProficiencyChoiceFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void speedFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpeedFeature f = new SpeedFeature();
            f = new FeatureForms.SpeedFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void modifySpellchoiceFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifySpellChoiceFeature f = new ModifySpellChoiceFeature();
            f = new FeatureForms.ModifySpellChoiceFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void multiFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MultiFeature f = new MultiFeature();
            f = new FeatureForms.MultiFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void spellcastingFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpellcastingFeature f = new SpellcastingFeature();
            f = new FeatureForms.SpellcastingFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            SpellChoiceFeature f = new SpellChoiceFeature();
            f = new FeatureForms.SpellChoiceFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void modifySpellFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpellModifyFeature f = new SpellModifyFeature();
            f = new FeatureForms.SpellModifyFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            SpellSlotsFeature f = new SpellSlotsFeature();
            f = new FeatureForms.SpellSlotsFeatureForm(f).edit(HistoryManager);
            list.Add(f);
            fill();
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C) copyToolStripMenuItem_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.V) pasteToolStripMenuItem_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.X) cutToolStripMenuItem_Click(sender, e);
            else if (e.KeyCode == Keys.Delete) deleteToolStripMenuItem_Click(sender, e);
        }

        private void subclassFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Add(new FeatureForms.SubClassFeatureForm(new SubClassFeature()).edit(HistoryManager));
            fill();
        }

        private void subraceFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Add(new FeatureForms.SubRaceFeatureForm(new SubRaceFeature()).edit(HistoryManager));
            fill();
        }

        private void toolProficiencyChoiceByKeywordFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Add(new FeatureForms.ToolKWProficiencyFeatureForm(new ToolKWProficiencyFeature("","","Armor and Light", "all light armor")).edit(HistoryManager));
            fill();
        }

        private void toolProficiencyChoiceFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Add(new FeatureForms.ToolProficiencyChoiceConditionFeatureForm(new ToolProficiencyChoiceConditionFeature()).edit(HistoryManager));
            fill();
        }

        private void toolProficiencyFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Add(new FeatureForms.ToolProficiencyFeatureForm(new ToolProficiencyFeature()).edit(HistoryManager));
            fill();
        }

        private void visionFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Add(new FeatureForms.VisionFeatureForm(new VisionFeature()).edit(HistoryManager));
            fill();
        }

        private void resistanceFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Add(new FeatureForms.ResistanceFeatureForm(new ResistanceFeature()).edit(HistoryManager));
            fill();
        }

        private void formsCompanionFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Add(new FeatureForms.FormsCompanionsFeatureForm(new FormsCompanionsFeature()).edit(HistoryManager));
            fill();
        }

        private void formsCompanionsBonusFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Add(new FeatureForms.FormsCompanionsBonusFeatureForm(new FormsCompanionsBonusFeature()).edit(HistoryManager));
            fill();
        }
    }
}
