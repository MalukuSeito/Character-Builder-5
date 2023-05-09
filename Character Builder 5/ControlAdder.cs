﻿using Character_Builder;
using OGL;
using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Character_Builder_5
{
    class ControlAdder
    {
        private class RefComp : IEqualityComparer<object>
        {
            public new bool Equals(object x, object y)
            {
                return object.ReferenceEquals(x, y);
            }

            public int GetHashCode(object obj)
            {
                return RuntimeHelpers.GetHashCode(obj);
            }
        }
        private static Dictionary<Feature, List<Spell>> bskcfSpells = new Dictionary<Feature, List<Spell>>(new RefComp());
        private static Dictionary<Feature, List<Item>> iccfitems = new Dictionary<Feature, List<Item>>();
        private static Dictionary<Feature, List<Item>> icfitems = new Dictionary<Feature, List<Item>>();
        private static Dictionary<Feature, List<Item>> tpccfitems = new Dictionary<Feature, List<Item>>();
        private static Dictionary<Feature, List<ChoiceList>> bskcfboxes = new Dictionary<Feature, List<ChoiceList>>(new RefComp());
        private static Dictionary<Feature, List<System.Windows.Forms.Label>> bskcflabels = new Dictionary<Feature, List<System.Windows.Forms.Label>>(new RefComp());
        private static Dictionary<Feature, List<ChoiceList>> cfboxes = new Dictionary<Feature, List<ChoiceList>>(new RefComp());
        private static Dictionary<Feature, List<System.Windows.Forms.Label>> cflabels = new Dictionary<Feature, List<System.Windows.Forms.Label>>(new RefComp());
        private static Dictionary<Feature, List<ChoiceList>> ccfboxes = new Dictionary<Feature, List<ChoiceList>>(new RefComp());
        private static Dictionary<Feature, List<System.Windows.Forms.Label>> ccflabels = new Dictionary<Feature, List<System.Windows.Forms.Label>>(new RefComp());
        private static Dictionary<Feature, List<ChoiceList>> iccfboxes = new Dictionary<Feature, List<ChoiceList>>(new RefComp());
        private static Dictionary<Feature, List<System.Windows.Forms.Label>> iccflabels = new Dictionary<Feature, List<System.Windows.Forms.Label>>(new RefComp());
        private static Dictionary<Feature, List<ChoiceList>> icfboxes = new Dictionary<Feature, List<ChoiceList>>(new RefComp());
        private static Dictionary<Feature, List<System.Windows.Forms.Label>> icflabels = new Dictionary<Feature, List<System.Windows.Forms.Label>>(new RefComp());
        private static Dictionary<Feature, List<ChoiceList>> lcfboxes = new Dictionary<Feature, List<ChoiceList>>(new RefComp());
        private static Dictionary<Feature, List<System.Windows.Forms.Label>> lcflabels = new Dictionary<Feature, List<System.Windows.Forms.Label>>(new RefComp());
        private static Dictionary<Feature, List<ChoiceList>> spcfboxes = new Dictionary<Feature, List<ChoiceList>>(new RefComp());
        private static Dictionary<Feature, List<System.Windows.Forms.Label>> spcflabels = new Dictionary<Feature, List<System.Windows.Forms.Label>>(new RefComp());
        private static Dictionary<Feature, List<ChoiceList>> tpccfboxes = new Dictionary<Feature, List<ChoiceList>>(new RefComp());
        private static Dictionary<Feature, List<System.Windows.Forms.Label>> tpccflabels = new Dictionary<Feature, List<System.Windows.Forms.Label>>(new RefComp());
        private static Dictionary<Description, List<ChoiceList>> descboxes = new Dictionary<Description, List<ChoiceList>>(new RefComp());
        private static Dictionary<Description, List<System.Windows.Forms.Label>> desclabels = new Dictionary<Description, List<System.Windows.Forms.Label>>(new RefComp());
        private static List<Language> langs = null;

        public static void AddClassControls(Player player, List<System.Windows.Forms.Control> control, int level)
        {
            player.ResetChoices();
            foreach (PlayerClass p in player.Classes)
            {
                int l = p.getClassLevelUpToLevel(level);
                if (l > 0) AddControls(p, control, player, level, player);
            }
        }

        public static void AddControls(PlayerClass p, List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level, Player player)
        {
            AddControls(p.GetClass(Program.Context), control, choiceProvider, level);
            int classlevel = p.getClassLevelUpToLevel(level);
            foreach (Feature f in p.GetFeatures(level, player, Program.Context).OrderBy(a => a.Level)) AddControl(control,choiceProvider, classlevel, f);
        }

        public static void AddControl(List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level, Feature f)
        {
            if (f is BonusSpellKeywordChoiceFeature)
                AddControl(control, choiceProvider, level, f as BonusSpellKeywordChoiceFeature);
            if (f is ChoiceFeature)
                AddControl(control, choiceProvider, level, f as ChoiceFeature);
            if (f is ItemChoiceFeature)
                AddControl(control, choiceProvider, level, f as ItemChoiceFeature);
            if (f is CollectionChoiceFeature)
                AddControl(control, choiceProvider, level, f as CollectionChoiceFeature);
            if (f is ItemChoiceConditionFeature)
                AddControl(control, choiceProvider, level, f as ItemChoiceConditionFeature);
            if (f is LanguageChoiceFeature)
                AddControl(control, choiceProvider, level, f as LanguageChoiceFeature);
            if (f is SkillProficiencyChoiceFeature)
                AddControl(control, choiceProvider, level, f as SkillProficiencyChoiceFeature);
            if (f is ToolProficiencyChoiceConditionFeature)
                AddControl(control, choiceProvider, level, f as ToolProficiencyChoiceConditionFeature);
        }

        public static void AddControl(List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level, BonusSpellKeywordChoiceFeature f)
        {

            if (!bskcfSpells.ContainsKey(f)) bskcfSpells.Add(f, Utils.FilterSpell(Program.Context, f.Condition, null));
            if (!bskcfboxes.ContainsKey(f)) bskcfboxes.Add(f, new List<ChoiceList>(f.Amount));
            if (!bskcflabels.ContainsKey(f)) bskcflabels.Add(f, new List<System.Windows.Forms.Label>(f.Amount));
            List<ChoiceList> choiceboxes = bskcfboxes[f];
            List<System.Windows.Forms.Label> choicelabels = bskcflabels[f];
            List<Spell> spells = bskcfSpells[f];
            List<String> taken = new List<string>();
            int offset = choiceProvider.GetChoiceOffset(f, f.UniqueID, f.Amount);
            for (int c = 0; c < choiceProvider.GetChoiceTotal(f.UniqueID); c++)
            {
                String counter = "";
                if (c > 0) counter = "_" + c.ToString();
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                if (cho != null && cho.Value != "") taken.Add(cho.Value);
            }
            for (int c = 0; c < f.Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                if (choiceboxes.Count <= c)
                {
                    choiceboxes.Add(new ChoiceList());
                    ChoiceList choicebox = choiceboxes[c];
                    choicebox.Dock = System.Windows.Forms.DockStyle.Top;
                    choicebox.FormattingEnabled = true;
                    choicebox.Name = f.UniqueID + counter;
                    choicebox.Size = new System.Drawing.Size(472, 95);
                    choicebox.SelectedIndexChanged += new System.EventHandler(Program.MainWindow.Choice_DisplaySpell);
                    choicebox.DoubleClick += new System.EventHandler(Program.MainWindow.Choice_DoubleClick);
                    choicebox.Leave += new System.EventHandler(Program.MainWindow.listbox_Deselect_on_Leave);
                    choicebox.MouseWheel += Program.MainWindow.listbox_MouseWheel;
                }
                if (choicelabels.Count <= c)
                {
                    choicelabels.Add(new System.Windows.Forms.Label());
                    System.Windows.Forms.Label choicelabel = choicelabels[c];
                    choicelabel.AutoSize = true;
                    choicelabel.Dock = System.Windows.Forms.DockStyle.Top;
                    choicelabel.Name = f.UniqueID + counter + "_label";
                    choicelabel.Padding = new System.Windows.Forms.Padding(0, (c > 0 ? 3 : 8), 0, 3);
                    choicelabel.Text = f.Name + (f.Amount > 1 ? " (" + (c + 1) + "/" + f.Amount + ")" : "");
                }
                control.Add(choicelabels[c]);
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                ChoiceList cbox = choiceboxes[c];
                cbox.ChoiceProvider = choiceProvider;
                cbox.Items.Clear();
                cbox.ForeColor = System.Drawing.SystemColors.WindowText;
                if (cho == null || cho.Value == "") cbox.Items.AddRange(spells.FindAll(e => !taken.Exists(t => ConfigManager.SourceInvariantComparer.Equals(t, e.Name + " " + ConfigManager.SourceSeperator + " " + e.Source))).ToArray());
                else
                {
                    cbox.ForeColor = Config.SelectColor;
                    cbox.Items.Add(Program.Context.GetSpell(cho.Value, f.Source));
                }
                cbox.Height = cbox.Items.Count * cbox.ItemHeight + 10;
                control.Add(cbox);
            }
        }
        public static void AddControl(List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level, ChoiceFeature f)
        {
            if (!cfboxes.ContainsKey(f)) cfboxes.Add(f, new List<ChoiceList>(f.Amount));
            if (!cflabels.ContainsKey(f)) cflabels.Add(f, new List<System.Windows.Forms.Label>(f.Amount));
            List<ChoiceList> choiceboxes = cfboxes[f];
            List<System.Windows.Forms.Label> choicelabels = cflabels[f];
            List<String> taken = new List<string>();
            int offset = choiceProvider.GetChoiceOffset(f, f.UniqueID, f.Amount);
            for (int c = 0; c < choiceProvider.GetChoiceTotal(f.UniqueID); c++)
            {
                String counter = "";
                if (c > 0) counter = "_" + c.ToString();
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                if (cho != null && cho.Value != "") taken.Add(cho.Value);
            }
            for (int c = 0; c < f.Amount; c++)
            {
                String counter = "";
				if (c + offset > 0) counter = "_" + (c + offset).ToString();
				if (choiceboxes.Count <= c)
                {
                    choiceboxes.Add(new ChoiceList());
                    ChoiceList choicebox = choiceboxes[c];
                    choicebox.Dock = System.Windows.Forms.DockStyle.Top;
                    choicebox.FormattingEnabled = true;
                    choicebox.Name = f.UniqueID + counter;
                    choicebox.Size = new System.Drawing.Size(472, 95);
                    choicebox.SelectedIndexChanged += new System.EventHandler(Program.MainWindow.Choice_DisplayFeature);
                    choicebox.DoubleClick += new System.EventHandler(Program.MainWindow.Choice_DoubleClick);
                    choicebox.Leave += new System.EventHandler(Program.MainWindow.listbox_Deselect_on_Leave);
                    choicebox.MouseWheel += Program.MainWindow.listbox_MouseWheel;
                }
                if (choicelabels.Count <= c)
                {
                    choicelabels.Add(new System.Windows.Forms.Label());
                    System.Windows.Forms.Label choicelabel = choicelabels[c];
                    choicelabel.AutoSize = true;
                    choicelabel.Dock = System.Windows.Forms.DockStyle.Top;
                    choicelabel.Name = f.UniqueID + counter + "_label";
                    choicelabel.Padding = new System.Windows.Forms.Padding(0, (c > 0 ? 3 : 8), 0, 3);
                    choicelabel.Text = f.Name + (f.Amount > 1 ? " (" + (c + 1) + "/" + f.Amount + ")" : "");
                }
                control.Add(choicelabels[c]);
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                ChoiceList cbox = choiceboxes[c];
                cbox.ChoiceProvider = choiceProvider;
                cbox.Items.Clear();
                cbox.ForeColor = System.Drawing.SystemColors.WindowText;
                if (cho == null || cho.Value == "") cbox.Items.AddRange(f.Choices.FindAll(e => f.AllowSameChoice || !taken.Exists(t => ConfigManager.SourceInvariantComparer.Equals(t, e.Name + " " + ConfigManager.SourceSeperator + " " + e.Source))).ToArray());
                else
                {
                    cbox.ForeColor = Config.SelectColor;
                    Feature res = f.Choices.Find(feat => feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source == cho.Value);
                    if (res == null)  res = f.Choices.Find(feat => ConfigManager.SourceInvariantComparer.Equals(feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source, cho.Value));
                    if (res != null) cbox.Items.Add(res);
                }
                cbox.Height = cbox.Items.Count * cbox.ItemHeight + 10;
                control.Add(cbox);
            }
        }
        public static void AddControl(List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level, CollectionChoiceFeature f)
        {
            if (!ccfboxes.ContainsKey(f)) ccfboxes.Add(f, new List<ChoiceList>(f.Amount));
            if (!ccflabels.ContainsKey(f)) ccflabels.Add(f, new List<System.Windows.Forms.Label>(f.Amount));
            List<ChoiceList> choiceboxes = ccfboxes[f];
            List<System.Windows.Forms.Label> choicelabels = ccflabels[f];
            List<String> taken = new List<string>();
            if (f.Collection == null || f.Collection == "") taken.AddRange(Program.Context.Player.GetFeatNames());
            int offset = choiceProvider.GetChoiceOffset(f, f.UniqueID, f.Amount);
            for (int c = 0; c < choiceProvider.GetChoiceTotal(f.UniqueID); c++)
            {
                String counter = "";
                if (c > 0) counter = "_" + (c).ToString();
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                if (cho != null && cho.Value != "") taken.Add(cho.Value);
            }
            for (int c = 0; c < f.Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                if (choiceboxes.Count <= c)
                {
                    choiceboxes.Add(new ChoiceList());
                    ChoiceList choicebox = choiceboxes[c];
                    choicebox.Dock = System.Windows.Forms.DockStyle.Top;
                    choicebox.FormattingEnabled = true;
                    choicebox.Name = f.UniqueID + counter;
                    choicebox.Size = new System.Drawing.Size(472, 95);
                    choicebox.SelectedIndexChanged += new System.EventHandler(Program.MainWindow.Choice_DisplayFeature);
                    choicebox.DoubleClick += new System.EventHandler(Program.MainWindow.Choice_DoubleClick);
                    choicebox.Leave += new System.EventHandler(Program.MainWindow.listbox_Deselect_on_Leave);
                    choicebox.MouseWheel += Program.MainWindow.listbox_MouseWheel;
                }
                if (choicelabels.Count <= c)
                {
                    choicelabels.Add(new System.Windows.Forms.Label());
                    System.Windows.Forms.Label choicelabel = choicelabels[c];
                    choicelabel.AutoSize = true;
                    choicelabel.Dock = System.Windows.Forms.DockStyle.Top;
                    choicelabel.Name = f.UniqueID + counter + "_label";
                    choicelabel.Padding = new System.Windows.Forms.Padding(0, (c > 0 ? 3 : 8), 0, 3);
                    choicelabel.Text = f.Name + (f.Amount > 1 ? " (" + (c + 1) + "/" + f.Amount + ")" : "");
                }
                control.Add(choicelabels[c]);
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                ChoiceList cbox = choiceboxes[c];
                cbox.ChoiceProvider = choiceProvider;
                cbox.Items.Clear();
                cbox.ForeColor = System.Drawing.SystemColors.WindowText;
                List<Feature> fl = Program.Context.GetFeatureCollection(f.Collection);
                if (cho == null || cho.Value == "" || !fl.Exists(feat => ConfigManager.SourceInvariantComparer.Equals(feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source, cho.Value))) cbox.Items.AddRange(fl.FindAll(e => f.AllowSameChoice || !taken.Exists(t => ConfigManager.SourceInvariantComparer.Equals(t, e.Name + " " + ConfigManager.SourceSeperator + " " + e.Source)) && e.Level <= level).ToArray());
                else
                {
                    cbox.ForeColor = Config.SelectColor;
                    Feature res = Program.Context.GetFeatureCollection(f.Collection).Find(feat => feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source == cho.Value);
                    if (res == null) res = Program.Context.GetFeatureCollection(f.Collection).Find(feat => ConfigManager.SourceInvariantComparer.Equals(feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source, cho.Value));
                    cbox.Items.Add(res);
                }
                cbox.Height = cbox.Items.Count * cbox.ItemHeight + 10;
                control.Add(cbox);
            }
        }
        public static void AddControl(List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level, ItemChoiceConditionFeature f)
        {
            if (!iccfboxes.ContainsKey(f)) iccfboxes.Add(f, new List<ChoiceList>(f.Amount));
            if (!iccflabels.ContainsKey(f)) iccflabels.Add(f, new List<System.Windows.Forms.Label>(f.Amount));
            List<ChoiceList> choiceboxes = iccfboxes[f];
            List<System.Windows.Forms.Label> choicelabels = iccflabels[f];
            if (!iccfitems.ContainsKey(f)) iccfitems.Add(f, Utils.Filter(Program.Context, f.Condition));
            List<Item> items = iccfitems[f];
            List<String> taken = new List<string>();
            int offset = choiceProvider.GetChoiceOffset(f, f.UniqueID, f.Amount);
            for (int c = 0; c < choiceProvider.GetChoiceTotal(f.UniqueID); c++)
            {
                String counter = "";
                if (c > 0) counter = "_" + c.ToString();
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                if (cho != null && cho.Value != "") taken.Add(cho.Value);
            }
            for (int c = 0; c < f.Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                if (choiceboxes.Count <= c)
                {
                    choiceboxes.Add(new ChoiceList());
                    ChoiceList choicebox = choiceboxes[c];
                    choicebox.Dock = System.Windows.Forms.DockStyle.Top;
                    choicebox.FormattingEnabled = true;
                    choicebox.Name = f.UniqueID + counter;
                    choicebox.Size = new System.Drawing.Size(472, 95);
                    choicebox.SelectedIndexChanged += new System.EventHandler(Program.MainWindow.Choice_DisplayItem);
                    choicebox.DoubleClick += new System.EventHandler(Program.MainWindow.Choice_DoubleClick);
                    choicebox.Leave += new System.EventHandler(Program.MainWindow.listbox_Deselect_on_Leave);
                    choicebox.MouseWheel += Program.MainWindow.listbox_MouseWheel;
                }
                if (choicelabels.Count <= c)
                {
                    choicelabels.Add(new System.Windows.Forms.Label());
                    System.Windows.Forms.Label choicelabel = choicelabels[c];
                    choicelabel.AutoSize = true;
                    choicelabel.Dock = System.Windows.Forms.DockStyle.Top;
                    choicelabel.Name = f.UniqueID + counter + "_label";
                    choicelabel.Padding = new System.Windows.Forms.Padding(0, (c > 0 ? 3 : 8), 0, 3);
                    choicelabel.Text = f.Name + (f.Amount > 1 ? " (" + (c + 1) + "/" + f.Amount + ")" : "");
                }
                control.Add(choicelabels[c]);
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                ChoiceList cbox = choiceboxes[c];
                cbox.ChoiceProvider = choiceProvider;
                cbox.Items.Clear();
                cbox.ForeColor = System.Drawing.SystemColors.WindowText;
                if (cho == null || cho.Value == "") cbox.Items.AddRange(items.FindAll(e => !taken.Exists(t => ConfigManager.SourceInvariantComparer.Equals(t, e.Name + " " + ConfigManager.SourceSeperator + " " + e.Source))).ToArray());
                else
                {
                    cbox.ForeColor = Config.SelectColor;
                    cbox.Items.Add(Program.Context.GetItem(cho.Value, f.Source));
                }
                cbox.Height = cbox.Items.Count * cbox.ItemHeight + 10;
                control.Add(cbox);
            }
        }
        public static void AddControl(List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level, ItemChoiceFeature f)
        {
            if (!icfboxes.ContainsKey(f)) icfboxes.Add(f, new List<ChoiceList>(f.Amount));
            if (!icflabels.ContainsKey(f)) icflabels.Add(f, new List<System.Windows.Forms.Label>(f.Amount));
            List<ChoiceList> choiceboxes = icfboxes[f];
            List<System.Windows.Forms.Label> choicelabels = icflabels[f];
            int offset = choiceProvider.GetChoiceOffset(f, f.UniqueID, f.Amount);
            if (!icfitems.ContainsKey(f))
            {
                List<Item> it = new List<Item>();
                foreach (String s in f.Items) it.Add(Program.Context.GetItem(s, f.Source));
                icfitems.Add(f, it);
            }
            List<Item> items = icfitems[f];
            List<String> taken = new List<string>();
            for (int c = 0; c < choiceProvider.GetChoiceTotal(f.UniqueID); c++)
            {
                String counter = "";
                if (c > 0) counter = "_" + c.ToString();
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                if (cho != null && cho.Value != "") taken.Add(cho.Value);
            }
            for (int c = 0; c < f.Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                if (choiceboxes.Count <= c)
                {
                    choiceboxes.Add(new ChoiceList());
                    ChoiceList choicebox = choiceboxes[c];
                    choicebox.Dock = System.Windows.Forms.DockStyle.Top;
                    choicebox.FormattingEnabled = true;
                    choicebox.Name = f.UniqueID + counter;
                    choicebox.Size = new System.Drawing.Size(472, 95);
                    choicebox.SelectedIndexChanged += new System.EventHandler(Program.MainWindow.Choice_DisplayItem);
                    choicebox.DoubleClick += new System.EventHandler(Program.MainWindow.Choice_DoubleClick);
                    choicebox.Leave += new System.EventHandler(Program.MainWindow.listbox_Deselect_on_Leave);
                    choicebox.MouseWheel += Program.MainWindow.listbox_MouseWheel;
                }
                if (choicelabels.Count <= c)
                {
                    choicelabels.Add(new System.Windows.Forms.Label());
                    System.Windows.Forms.Label choicelabel = choicelabels[c];
                    choicelabel.AutoSize = true;
                    choicelabel.Dock = System.Windows.Forms.DockStyle.Top;
                    choicelabel.Name = f.UniqueID + counter + "_label";
                    choicelabel.Padding = new System.Windows.Forms.Padding(0, (c > 0 ? 3 : 8), 0, 3);
                    choicelabel.Text = f.Name + (f.Amount > 1 ? " (" + (c + 1) + "/" + f.Amount + ")" : "");
                }
                control.Add(choicelabels[c]);
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                ChoiceList cbox = choiceboxes[c];
                cbox.ChoiceProvider = choiceProvider;
                cbox.Items.Clear();
                cbox.ForeColor = System.Drawing.SystemColors.WindowText;
                if (cho == null || cho.Value == "") cbox.Items.AddRange(items.FindAll(e => !taken.Exists(t => ConfigManager.SourceInvariantComparer.Equals(t, e.Name + " " + ConfigManager.SourceSeperator + " " + e.Source))).ToArray());
                else
                {
                    cbox.ForeColor = Config.SelectColor;
                    cbox.Items.Add(Program.Context.GetItem(cho.Value, f.Source));
                }
                cbox.Height = cbox.Items.Count * cbox.ItemHeight + 10;
                control.Add(cbox);
            }
        }
        public static void AddControl(List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level, LanguageChoiceFeature f)
        {
            if (langs == null)
            {
                langs = new List<Language>(Program.Context.Languages.Values);
                langs.Sort();
            }
            if (!lcfboxes.ContainsKey(f)) lcfboxes.Add(f, new List<ChoiceList>(f.Amount));
            if (!lcflabels.ContainsKey(f)) lcflabels.Add(f, new List<System.Windows.Forms.Label>(f.Amount));
            List<ChoiceList> choiceboxes = lcfboxes[f];
            List<System.Windows.Forms.Label> choicelabels = lcflabels[f];
            List<String> taken = new List<string>();
            int offset = choiceProvider.GetChoiceOffset(f, f.UniqueID, f.Amount);
            for (int c = 0; c < choiceProvider.GetChoiceTotal(f.UniqueID); c++)
            {
                String counter = "";
                if (c > 0) counter = "_" + c.ToString();
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                if (cho != null && cho.Value != "") taken.Add(cho.Value);
            }
            for (int c = 0; c < f.Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                if (choiceboxes.Count <= c)
                {
                    choiceboxes.Add(new ChoiceList());
                    ChoiceList choicebox = choiceboxes[c];
                    choicebox.Dock = System.Windows.Forms.DockStyle.Top;
                    choicebox.FormattingEnabled = true;
                    choicebox.Name = f.UniqueID + counter;
                    choicebox.Size = new System.Drawing.Size(472, 95);
                    choicebox.SelectedIndexChanged += new System.EventHandler(Program.MainWindow.Choice_DisplayLanguage);
                    choicebox.DoubleClick += new System.EventHandler(Program.MainWindow.Choice_DoubleClick);
                    choicebox.Leave += new System.EventHandler(Program.MainWindow.listbox_Deselect_on_Leave);
                    choicebox.MouseWheel += Program.MainWindow.listbox_MouseWheel;
                }
                if (choicelabels.Count <= c)
                {
                    choicelabels.Add(new System.Windows.Forms.Label());
                    System.Windows.Forms.Label choicelabel = choicelabels[c];
                    choicelabel.AutoSize = true;
                    choicelabel.Dock = System.Windows.Forms.DockStyle.Top;
                    choicelabel.Name = f.UniqueID + counter + "_label";
                    String name = f.Name;
                    if (f.Name == "") name = "Language";
                    choicelabel.Padding = new System.Windows.Forms.Padding(0, (c > 0 ? 3 : 8), 0, 3);
                    choicelabel.Text = name + (f.Amount > 1 ? " (" + (c + 1) + "/" + f.Amount + ")" : "");
                }
                control.Add(choicelabels[c]);
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                ChoiceList cbox = choiceboxes[c];
                cbox.ChoiceProvider = choiceProvider;
                cbox.Items.Clear();
                cbox.ForeColor = System.Drawing.SystemColors.WindowText;
                if (cho == null || cho.Value == "") cbox.Items.AddRange(langs.FindAll(e => !taken.Exists(t => ConfigManager.SourceInvariantComparer.Equals(t, e.Name + " " + ConfigManager.SourceSeperator + " " + e.Source))).ToArray());
                else
                {
                    cbox.ForeColor = Config.SelectColor;
                    cbox.Items.Add(Program.Context.GetLanguage(cho.Value, f.Source));
                }
                cbox.Height = cbox.Items.Count * cbox.ItemHeight + 10;
                control.Add(cbox);
            }
        }
        public static void AddControl(List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level, SkillProficiencyChoiceFeature f)
        {
            List<Skill> shown;
            if (f.Skills.Count == 0) shown = (from s in Program.Context.Skills.Values orderby s select s).ToList();
            else shown = new List<Skill>(from s in f.Skills select Program.Context.GetSkill(s, f.Source));
            if (f.OnlyAlreadyKnownSkills)
            {
                IEnumerable<Skill> known = Program.Context.Player.GetSkillProficiencies();
                shown.RemoveAll(e => !known.Any(s => s == e));
            }
            if (!spcfboxes.ContainsKey(f)) spcfboxes.Add(f, new List<ChoiceList>(f.Amount));
            if (!spcflabels.ContainsKey(f)) spcflabels.Add(f, new List<System.Windows.Forms.Label>(f.Amount));
            List<ChoiceList> choiceboxes = spcfboxes[f];
            List<System.Windows.Forms.Label> choicelabels = spcflabels[f];
            List<String> taken = new List<string>();
            int offset = choiceProvider.GetChoiceOffset(f, f.UniqueID, f.Amount);
            for (int c = 0; c < choiceProvider.GetChoiceTotal(f.UniqueID); c++)
            {
                String counter = "";
                if (c > 0) counter = "_" + c.ToString();
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                if (cho != null && cho.Value != "") taken.Add(cho.Value);
            }
            for (int c = 0; c < f.Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                if (choiceboxes.Count <= c)
                {
                    choiceboxes.Add(new ChoiceList());
                    ChoiceList choicebox = choiceboxes[c];
                    choicebox.Dock = System.Windows.Forms.DockStyle.Top;
                    choicebox.FormattingEnabled = true;
                    choicebox.Name = f.UniqueID + counter;
                    choicebox.Size = new System.Drawing.Size(472, 95);
                    choicebox.SelectedIndexChanged += new System.EventHandler(Program.MainWindow.Choice_DisplaySkill);
                    choicebox.DoubleClick += new System.EventHandler(Program.MainWindow.Choice_DoubleClick);
                    choicebox.Leave += new System.EventHandler(Program.MainWindow.listbox_Deselect_on_Leave);
                    choicebox.MouseWheel += Program.MainWindow.listbox_MouseWheel;
                }
                if (choicelabels.Count <= c)
                {
                    choicelabels.Add(new System.Windows.Forms.Label());
                    System.Windows.Forms.Label choicelabel = choicelabels[c];
                    choicelabel.AutoSize = true;
                    choicelabel.Dock = System.Windows.Forms.DockStyle.Top;
                    choicelabel.Name = f.UniqueID + counter + "_label";
                    choicelabel.Padding = new System.Windows.Forms.Padding(0, (c > 0 ? 3 : 8), 0, 3);
                    choicelabel.Text = f.Name + (f.Amount > 1 ? " (" + (c + 1) + "/" + f.Amount + ")" : "");
                }
                control.Add(choicelabels[c]);
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                ChoiceList cbox = choiceboxes[c];
                cbox.ChoiceProvider = choiceProvider;
                cbox.Items.Clear();
                cbox.ForeColor = System.Drawing.SystemColors.WindowText;
                if (cho == null || cho.Value == "") cbox.Items.AddRange((from e in shown where !taken.Exists(t => ConfigManager.SourceInvariantComparer.Equals(t, e.Name + " " + ConfigManager.SourceSeperator + " " + e.Source)) select e).ToArray());
                else
                {
                    cbox.ForeColor = Config.SelectColor;
                    cbox.Items.Add(Program.Context.GetSkill(cho.Value, f.Source));
                }
                cbox.Height = cbox.Items.Count * cbox.ItemHeight + 10;
                control.Add(cbox);
            }
        }
        public static void AddControl(List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level, ToolProficiencyChoiceConditionFeature f)
        {
            if (!tpccfitems.ContainsKey(f)) tpccfitems.Add(f, Utils.Filter(Program.Context, f.Condition));
            List<Item> items = tpccfitems[f];
            if (!tpccfboxes.ContainsKey(f)) tpccfboxes.Add(f, new List<ChoiceList>(f.Amount));
            if (!tpccflabels.ContainsKey(f)) tpccflabels.Add(f, new List<System.Windows.Forms.Label>(f.Amount));
            List<ChoiceList> choiceboxes = tpccfboxes[f];
            List<System.Windows.Forms.Label> choicelabels = tpccflabels[f];
            List<String> taken = new List<string>();
            int offset = choiceProvider.GetChoiceOffset(f, f.UniqueID, f.Amount);
            for (int c = 0; c < choiceProvider.GetChoiceTotal(f.UniqueID); c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                if (cho != null && cho.Value != "") taken.Add(cho.Value);
            }
            for (int c = 0; c < f.Amount; c++)
            {
                String counter = "";
                if (c > 0) counter = "_" + c.ToString();
                if (choiceboxes.Count <= c)
                {
                    choiceboxes.Add(new ChoiceList());
                    ChoiceList choicebox = choiceboxes[c];
                    choicebox.Dock = System.Windows.Forms.DockStyle.Top;
                    choicebox.FormattingEnabled = true;
                    choicebox.Name = f.UniqueID + counter;
                    choicebox.Size = new System.Drawing.Size(472, 95);
                    choicebox.SelectedIndexChanged += new System.EventHandler(Program.MainWindow.Choice_DisplayItem);
                    choicebox.DoubleClick += new System.EventHandler(Program.MainWindow.Choice_DoubleClick);
                    choicebox.Leave += new System.EventHandler(Program.MainWindow.listbox_Deselect_on_Leave);
                    choicebox.MouseWheel += Program.MainWindow.listbox_MouseWheel;
                }
                if (choicelabels.Count <= c)
                {
                    choicelabels.Add(new System.Windows.Forms.Label());
                    System.Windows.Forms.Label choicelabel = choicelabels[c];
                    choicelabel.AutoSize = true;
                    choicelabel.Dock = System.Windows.Forms.DockStyle.Top;
                    choicelabel.Name = f.UniqueID + counter + "_label";
                    choicelabel.Padding = new System.Windows.Forms.Padding(0, (c > 0 ? 3 : 8), 0, 3);
                    choicelabel.Text = f.Name + (f.Amount > 1 ? " (" + (c + 1) + "/" + f.Amount + ")" : "");
                }
                control.Add(choicelabels[c]);
                Choice cho = choiceProvider.GetChoice(f.UniqueID + counter);
                ChoiceList cbox = choiceboxes[c];
                cbox.ChoiceProvider = choiceProvider;
                cbox.Items.Clear();
                cbox.ForeColor = System.Drawing.SystemColors.WindowText;
                if (cho == null || cho.Value == "") cbox.Items.AddRange(items.FindAll(e => !taken.Exists(t => ConfigManager.SourceInvariantComparer.Equals(t, e.Name + " " + ConfigManager.SourceSeperator + " " + e.Source))).ToArray());
                else
                {
                    cbox.ForeColor = Config.SelectColor;
                    cbox.Items.Add(Program.Context.GetItem(cho.Value, f.Source));
                }
                cbox.Height = cbox.Items.Count * cbox.ItemHeight + 10;
                control.Add(cbox);
            }
        }

        public static void AddControl(List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, Description desc)
        {
            if (desc is TableDescription)
                AddControl(control, choiceProvider, desc as TableDescription);
        }

        public static void AddControl(List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, TableDescription desc)
        {
            if (!descboxes.ContainsKey(desc)) descboxes.Add(desc, new List<ChoiceList>(desc.Amount));
            if (!desclabels.ContainsKey(desc)) desclabels.Add(desc, new List<System.Windows.Forms.Label>(desc.Amount));
            List<ChoiceList> choiceboxes = descboxes[desc];
            List<System.Windows.Forms.Label> choicelabels = desclabels[desc];
            List<String> taken = new List<string>();
            for (int c = 0; c < desc.Amount; c++)
            {
                String counter = "";
                if (c > 0) counter = "_" + c.ToString();
                Choice cho = choiceProvider.GetChoice(desc.UniqueID + counter);
                if (cho != null && cho.Value != "") taken.Add(cho.Value);
            }
            for (int c = 0; c < desc.Amount; c++)
            {
                String counter = "";
                if (c > 0) counter = "_" + c.ToString();
                if (choiceboxes.Count <= c)
                {
                    choiceboxes.Add(new ChoiceList());
                    ChoiceList choicebox = choiceboxes[c];
                    choicebox.Dock = System.Windows.Forms.DockStyle.Top;
                    choicebox.FormattingEnabled = true;
                    choicebox.Name = desc.UniqueID + counter;
                    choicebox.Size = new System.Drawing.Size(472, 95);
                    choicebox.DoubleClick += new System.EventHandler(Program.MainWindow.ChoiceCustom_DoubleClick);
                    choicebox.Leave += new System.EventHandler(Program.MainWindow.listbox_Deselect_on_Leave);
                    choicebox.MouseWheel += Program.MainWindow.listbox_MouseWheel;
                }
                if (choicelabels.Count <= c)
                {
                    choicelabels.Add(new System.Windows.Forms.Label());
                    System.Windows.Forms.Label choicelabel = choicelabels[c];
                    choicelabel.AutoSize = true;
                    choicelabel.Dock = System.Windows.Forms.DockStyle.Top;
                    choicelabel.Name = desc.UniqueID + counter + "_label";
                    choicelabel.Padding = new System.Windows.Forms.Padding(0, (c > 0 ? 3 : 8), 0, 3);
                    choicelabel.Text = desc.Name + (desc.Amount > 1 ? " (" + (c + 1) + "/" + desc.Amount + ")" : "");
                }
                control.Add(choicelabels[c]);
                Choice cho = choiceProvider.GetChoice(desc.UniqueID + counter);
                ChoiceList cbox = choiceboxes[c];
                cbox.ChoiceProvider = choiceProvider;
                cbox.Items.Clear();
                cbox.ForeColor = System.Drawing.SystemColors.WindowText;
                if (cho == null || cho.Value == "")
                {
                    cbox.Items.AddRange(desc.Entries.FindAll(e => !taken.Exists(t => ConfigManager.SourceInvariantComparer.Equals(t, e.ToString()))).ToArray<TableEntry>());
                    cbox.Items.Add("- Custom -");
                }
                else
                {
                    cbox.ForeColor = Config.SelectColor;
                    cbox.Items.Add(cho.Value);
                }
                cbox.Height = cbox.Items.Count * cbox.ItemHeight + 10;
                control.Add(cbox);
            }
        }
        public static void AddControls(Race r, List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level)
        {
            foreach (Description d in r.Descriptions) AddControl(control, choiceProvider, d);
        }
        public static void AddControls(Background r, List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level)
        {
            foreach (Description d in r.Descriptions) AddControl(control, choiceProvider, d);
        }
        public static void AddControls(ClassDefinition r, List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level)
        {
            foreach (Description d in r.Descriptions) AddControl(control, choiceProvider, d);
        }
        public static void AddControls(SubClass r, List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level)
        {
            foreach (Description d in r.Descriptions) AddControl(control, choiceProvider, d);
        }
        public static void AddControls(SubRace r, List<System.Windows.Forms.Control> control, IChoiceProvider choiceProvider, int level)
        {
            foreach (Description d in r.Descriptions) AddControl(control, choiceProvider, d);
        }
    }
}
