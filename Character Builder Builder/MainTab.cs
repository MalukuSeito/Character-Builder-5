using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Character_Builder_5;
using Microsoft.VisualBasic;
using System.IO;

namespace Character_Builder_Builder
{
    public partial class MainTab : Form
    {
        public MainTab()
        {
            InitializeComponent();
            Race.ImportAll();
            SubRace.ImportAll();
            fill(racesList, Race.races.Keys, null);
            fill(subRaceList, SubRace.subraces.Keys, null);
            DefaultSource.DataBindings.Add("Text", ConfigManager.loaded, "Source", true, DataSourceUpdateMode.OnPropertyChanged);
            CoinWeight.DataBindings.Add("Value", ConfigManager.loaded, "WeightOfACoin", true, DataSourceUpdateMode.OnPropertyChanged);
            PDFExport.Items = ConfigManager.loaded.PDF;
            Slots.Items = ConfigManager.loaded.Slots;
            AllFeatures.features = ConfigManager.loaded.FeaturesForAll;
            Multiclassing.features = ConfigManager.loaded.FeaturesForMulticlassing;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage tab = TabControls.SelectedTab;
            if (tab == racesTab)
            {
                Race.ImportAll();
                SubRace.ImportAll();
                fill(racesList, Race.races.Keys, null);
                fill(subRaceList, SubRace.subraces.Keys, null);
            }
            else if (tab == featuresTab)
            {
                FeatureCollection.ImportAll();
                FeatCats.Items.Clear();
                FeatCats.Items.Add("Feats");
                foreach (string s in SourceManager.EnumerateCategories(ConfigManager.Directory_Features)) FeatCats.Items.Add(s);
            }
            else if (tab == classesTab)
            {
                ClassDefinition.ImportAll();
                fill(classList, ClassDefinition.classes.Keys, null);
                SubClass.ImportAll();
                fill(subclassList, SubClass.subclasses.Keys, null);
            }
            else if (tab == langTab)
            {
                Language.ImportAll();
                fill(langBox, Language.languages.Keys, null);
            }
            else if (tab == backTab)
            {
                Background.ImportAll();
                fill(backBox, Background.backgrounds.Keys, null);
            }
            else if (tab == itemTab)
            {
                Item.ImportAll();
                ItemCat.Items.Clear();
                ItemCat.Items.Add("Items");
                foreach (string s in SourceManager.EnumerateCategories(ConfigManager.Directory_Items)) ItemCat.Items.Add(s);
            }
            else if (tab == skillsTab)
            {
                Skill.ImportAll();
                fill(skillList, Skill.skills.Keys, null);
            }
            else if (tab == conditionsTab)
            {
                Condition.ImportAll();
                fill(condList, Condition.conditions.Keys, null);
            }
            else if (tab == levelTab)
            {
                Level c = Level.Load(ConfigManager.loaded.Levels);
                LevelXP.Items = c.Experience;
                LevelProficiency.Items = c.Proficiency;

            }
            else if (tab == arraysTab)
            {
                AbilityScores ab = AbilityScores.Load(ConfigManager.loaded.AbilityScores);
                PointBuyPoints.DataBindings.Clear();
                PointBuyPoints.DataBindings.Add("Value", ab, "PointBuyPoints", true, DataSourceUpdateMode.OnPropertyChanged);
                PointBuyMin.DataBindings.Clear();
                PointBuyMin.DataBindings.Add("Value", ab, "PointBuyMinScore", true, DataSourceUpdateMode.OnPropertyChanged);
                PointBuyMax.DataBindings.Clear();
                PointBuyMax.DataBindings.Add("Value", ab, "PointBuyMaxScore", true, DataSourceUpdateMode.OnPropertyChanged);
                MaxScore.DataBindings.Clear();
                MaxScore.DataBindings.Add("Value", ab, "DefaultMax", true, DataSourceUpdateMode.OnPropertyChanged);
                PointBuyList.DataBindings.Clear();
                PointBuyList.DataBindings.Add("Start", ab, "PointBuyMinScore", true, DataSourceUpdateMode.OnPropertyChanged);
                PointBuyList.Items = ab.PointBuyCost;
                Arrays.Items = ab.Arrays;
            }
            else if (tab == spellsTab)
            {
                Spell.ImportAll();
                fill(spellBox, Spell.spells.Keys, null);
            }
            else if (tab == magicTab)
            {
                MagicProperty.ImportAll();
                magicCatBox.Items.Clear();
                magicCatBox.Items.Add("Magic");
                foreach (string s in SourceManager.EnumerateCategories(ConfigManager.Directory_Magic)) magicCatBox.Items.Add(s);
            }
            else if (tab == settingsTab)
            {
                DefaultSource.AutoCompleteCustomSource.Clear();
                DefaultSource.AutoCompleteCustomSource.AddRange(SourceManager.Sources.ToArray());
            }
        }

        private void fill(ListBox box, IEnumerable<string> list, string select)
        {
            int index = box.SelectedIndex;
            int top = box.TopIndex;
            box.Items.Clear();
            List<string> keys = new List<string>(list);
            keys.Sort();
            for (int i = 0; i < keys.Count; i++)
            {
                string s = keys[i];
                if (select == s) index = i;
                box.Items.Add(s);
            }
            if (index >= 0 && index < box.Items.Count)
            {
                int visibleItems = box.ClientSize.Height / box.ItemHeight;
                if (top >= 0 && top < box.Items.Count - visibleItems)
                {
                    box.TopIndex = top;
                }
                box.SelectedIndex = index;
            }
        }

        private void racesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (racesList.SelectedItem == null) return;
            Race selected = Race.Get((string)racesList.SelectedItem, null);
            if (selected != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(selected.toHTML());
                preview.Refresh();
            }
        }

        private void editRace(object sender, EventArgs e)
        {
            if (racesList.SelectedItem == null) return;
            Race selected = Race.Get((string)racesList.SelectedItem, null);
            if (selected != null)
            {
                string sel = selected.Name;
                RaceForm r = new RaceForm(selected.clone());
                r.Saved += RaceSaved;
                r.Show();
                
            }
        }

        private void RaceSaved(object sender, string id)
        {
            Race.ImportAll();
            fill(racesList, Race.races.Keys, id);
        }

        private void SubRaceSaved(object sender, string id)
        {
            SubRace.ImportAll();
            fill(subRaceList, SubRace.subraces.Keys, id);
        }

        private void subRaceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (subRaceList.SelectedItem == null) return;
            SubRace selected = SubRace.Get((string)subRaceList.SelectedItem, null);
            if (selected != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(selected.toHTML());
                preview.Refresh();
            }
        }

        private void edit_Subrace(object sender, EventArgs e)
        {
            if (subRaceList.SelectedItem == null) return;
            SubRace selected = SubRace.Get((string)subRaceList.SelectedItem, null);
            if (selected != null)
            {
                string sel = selected.Name;
                SubRaceForm r = new SubRaceForm(selected.clone());
                r.Saved += SubRaceSaved;
                r.Show();

            }
        }

        private void newRaceBtn_Click(object sender, EventArgs e)
        {
            RaceForm r = new RaceForm(new Race());
            r.Saved += RaceSaved;
            r.Show();
        }

        private void newSubRaceBtn_Click(object sender, EventArgs e)
        {
            SubRaceForm r = new SubRaceForm(new SubRace());
            r.Saved += SubRaceSaved;
            r.Show();
        }

        private void FeatCats_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cat = (string)FeatCats.SelectedItem;
            FeatCollection.Items.Clear();
            button1.Enabled = false;
            button2.Enabled = false;
            if (cat != null) 
            {
                button1.Enabled = true;
                button2.Enabled = true;
                if (FeatureCollection.Container.ContainsKey(cat)) foreach (FeatureContainer cont in FeatureCollection.Container[cat]) FeatCollection.Items.Add(cont);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (FeatCats.SelectedItem == null) return;
            string cat = Interaction.InputBox("New category name", "New Sub Category");
            cat = string.Join("_", cat.Split(Path.GetInvalidFileNameChars()));
            SourceManager.getDirectory(ConfigManager.DefaultSource, Path.Combine((string)FeatCats.SelectedItem, cat));
            if (TabControls.SelectedTab == featuresTab) tabControl1_SelectedIndexChanged(null, null);
            else TabControls.SelectedTab = featuresTab;
        }

        private void FeatCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FeatCollection.SelectedItem == null) return;
            FeatureContainer selected = (FeatureContainer)FeatCollection.SelectedItem;
            if (selected != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(selected.toHTML());
                preview.Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FeatCats.SelectedItem == null) return;
            FeatureContainer f = new FeatureContainer();
            f.Source = ConfigManager.DefaultSource;
            f.category = (string)FeatCats.SelectedItem;
            FeatureContainerForm r = new FeatureContainerForm(f);
            r.Saved += ContainerSaved;
            r.Show();
        }

        private void editContainer(object sender, EventArgs e)
        {
            if (FeatCollection.SelectedItem == null) return;
            FeatureContainer selected = (FeatureContainer)FeatCollection.SelectedItem;
            if (selected != null)
            {
                string sel = selected.Name;
                FeatureContainerForm r = new FeatureContainerForm(selected.clone());
                r.Saved += ContainerSaved;
                r.Show();

            }
        }

        private ScrollInfo savescroll(ListBox box)
        {
            return new ScrollInfo(box.SelectedIndex, box.TopIndex);
        }

        private void restore(ListBox box, ScrollInfo info, String id)
        {
            if (id != null)
            {
                for (int i = 0; i < box.Items.Count; i++)
                {
                    object o = box.Items[i];
                    if (o != null && id.Equals(o.ToString()))
                    {
                        info.index = i;
                        break;
                    }
                }
            }
            if (info.index >= 0 && info.index < box.Items.Count)
            {
                int visibleItems = box.ClientSize.Height / box.ItemHeight;
                if (info.top >= 0 && info.top < box.Items.Count - visibleItems)
                {
                    box.TopIndex = info.top;
                }
                box.SelectedIndex = info.index;
            }
            
        }

        private void ContainerSaved(object sender, string id)
        {
            string s = (string)FeatCats.SelectedItem;
            FeatureCollection.ImportAll();
            var scroll = savescroll(FeatCollection);
            if (TabControls.SelectedTab == featuresTab) tabControl1_SelectedIndexChanged(null, null);
            else TabControls.SelectedTab = featuresTab;
            FeatCats.SelectedItem = s;
            if (s == null) FeatCollection.Items.Clear();
            else restore(FeatCollection, scroll, id);
        }

        private void classList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (classList.SelectedItem == null) return;
            ClassDefinition selected = ClassDefinition.Get((string)classList.SelectedItem, null);
            if (selected != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(selected.toHTML());
                preview.Refresh();
            }
        }

        private void editClass(object sender, EventArgs e)
        {
            if (classList.SelectedItem == null) return;
            ClassDefinition selected = ClassDefinition.Get((string)classList.SelectedItem, null);
            if (selected != null)
            {
                string sel = selected.Name;
                ClassForm r = new ClassForm(selected.clone());
                r.Saved += ClassSaved ;
                r.Show();

            }
        }

        private void ClassSaved(object sender, string id)
        {
            ClassDefinition.ImportAll();
            fill(classList, ClassDefinition.classes.Keys, id);
        }

        private void NewClassBtn_Click(object sender, EventArgs e)
        {
            ClassForm r = new ClassForm(new ClassDefinition());
            r.Saved += ClassSaved;
            r.Show();
        }

        private void NewSubclass_Click(object sender, EventArgs e)
        {
            SubClassForm r = new SubClassForm(new SubClass());
            r.Saved += SubClassSaved;
            r.Show();
        }

        private void subclassList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (subclassList.SelectedItem == null) return;
            SubClass selected = SubClass.Get((string)subclassList.SelectedItem, null);
            if (selected != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(selected.toHTML());
                preview.Refresh();
            }
        }

        private void editSubClass(object sender, EventArgs e)
        {
            if (subclassList.SelectedItem == null) return;
            SubClass selected = SubClass.Get((string)subclassList.SelectedItem, null);
            if (selected != null)
            {
                string sel = selected.Name;
                SubClassForm r = new SubClassForm(selected.clone());
                r.Saved += SubClassSaved;
                r.Show();

            }
        }

        private void SubClassSaved(object sender, string id)
        {
            SubClass.ImportAll();
            fill(subclassList, SubClass.subclasses.Keys, id);
        }

        private void NewLang_Click(object sender, EventArgs e)
        {
            LanguageForm r = new LanguageForm(new Language());
            r.Saved += LangSaved;
            r.Show();
        }

        private void langList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (langBox.SelectedItem == null) return;
            Language selected = Language.Get((string)langBox.SelectedItem, null);
            if (selected != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(selected.toHTML());
                preview.Refresh();
            }
        }

        private void editLang(object sender, EventArgs e)
        {
            if (langBox.SelectedItem == null) return;
            Language selected = Language.Get((string)langBox.SelectedItem, null);
            if (selected != null)
            {
                string sel = selected.Name;
                LanguageForm r = new LanguageForm(selected.clone());
                r.Saved += LangSaved;
                r.Show();

            }
        }

        private void LangSaved(object sender, string id)
        {
            Language.ImportAll();
            fill(langBox, Language.languages.Keys, id);
        }

        private void NewBack_Click(object sender, EventArgs e)
        {
            BackgroundForm r = new BackgroundForm(new Background());
            r.Saved += BackSaved;
            r.Show();
        }

        private void backList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (backBox.SelectedItem == null) return;
            Background selected = Background.Get((string)backBox.SelectedItem, null);
            if (selected != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(selected.toHTML());
                preview.Refresh();
            }
        }

        private void editBackground(object sender, EventArgs e)
        {
            if (backBox.SelectedItem == null) return;
            Background selected = Background.Get((string)backBox.SelectedItem, null);
            if (selected != null)
            {
                string sel = selected.Name;
                BackgroundForm r = new BackgroundForm(selected.clone());
                r.Saved += BackSaved;
                r.Show();

            }
        }

        private void BackSaved(object sender, string id)
        {
            Background.ImportAll();
            fill(backBox, Background.backgrounds.Keys, id);
        }

        private void NewItem_Click(object sender, EventArgs e)
        {
            NewItemCTM.Show(NewItem, new Point(0, NewItem.Size.Height));
        }


        private void ItemCats_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cat = (string)ItemCat.SelectedItem;
            ItemBox.Items.Clear();
            NewItemCat.Enabled = false;
            NewItem.Enabled = false;
            if (cat != null)
            {
                NewItemCat.Enabled = true;
                NewItem.Enabled = true;
                Category category = Category.Make(cat);
                foreach (Item i in from i in Item.items.Values where category.Equals(i.Category) orderby i.Name select i) ItemBox.Items.Add(i);
            }
        }

        private void NewItemCat_Click(object sender, EventArgs e)
        {
            if (ItemCat.SelectedItem == null) return;
            string cat = Interaction.InputBox("New category name", "New Sub Category");
            cat = string.Join("_", cat.Split(Path.GetInvalidFileNameChars()));
            SourceManager.getDirectory(ConfigManager.DefaultSource, Path.Combine((string)ItemCat.SelectedItem, cat));
            if (TabControls.SelectedTab == itemTab) tabControl1_SelectedIndexChanged(null, null);
            else TabControls.SelectedTab = itemTab;
        }

        private void Items_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ItemBox.SelectedItem == null) return;
            Item selected = (Item)ItemBox.SelectedItem;
            if (selected != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(selected.toHTML());
                preview.Refresh();
            }
        }

        private void editItem(object sender, EventArgs e)
        {
            if (ItemBox.SelectedItem == null) return;
            Item selected = (Item)ItemBox.SelectedItem;
            if (selected != null)
            {
                string sel = selected.Name;
                if (selected is Armor)
                {
                    ArmorForm r = new ArmorForm((Armor)selected.clone());
                    r.Saved += ItemSaved;
                    r.Show();
                }
                else if (selected is Shield)
                {
                    ShieldForm r = new ShieldForm((Shield)selected.clone());
                    r.Saved += ItemSaved;
                    r.Show();
                }
                else if (selected is Weapon)
                {
                    WeaponForm r = new WeaponForm((Weapon)selected.clone());
                    r.Saved += ItemSaved;
                    r.Show();
                }
                else if (selected is Pack)
                {
                    PackForm r = new PackForm((Pack)selected.clone());
                    r.Saved += ItemSaved;
                    r.Show();
                }
                else if (selected is Tool)
                {
                    ToolForm r = new ToolForm((Tool)selected.clone());
                    r.Saved += ItemSaved;
                    r.Show();
                }
                else
                {
                    ItemForm r = new ItemForm(selected.clone());
                    r.Saved += ItemSaved;
                    r.Show();
                }
            }
        }

        private void ItemSaved(object sender, string id)
        {
            string s = (string)ItemCat.SelectedItem;
            Item.ImportAll();
            var scroll = savescroll(ItemBox);
            if (TabControls.SelectedTab == itemTab) tabControl1_SelectedIndexChanged(null, null);
            else TabControls.SelectedTab = itemTab;
            ItemCat.SelectedItem = s;
            if (s == null) ItemBox.Items.Clear();
            else restore(ItemBox, scroll, id);
        }

        private void itemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ItemCat.SelectedItem == null) return;
            Item i = new Item();
            i.Category = Category.Make((string)ItemCat.SelectedItem);
            ItemForm r = new ItemForm(i);
            r.Saved += ItemSaved;
            r.Show();
        }

        private void toolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ItemCat.SelectedItem == null) return;
            Tool i = new Tool();
            i.Category = Category.Make((string)ItemCat.SelectedItem);
            ToolForm r = new ToolForm(i);
            r.Saved += ItemSaved;
            r.Show();
        }

        private void weaponToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ItemCat.SelectedItem == null) return;
            Weapon i = new Weapon();
            i.Category = Category.Make((string)ItemCat.SelectedItem);
            WeaponForm r = new WeaponForm(i);
            r.Saved += ItemSaved;
            r.Show();
        }

        private void armorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ItemCat.SelectedItem == null) return;
            Armor i = new Armor();
            i.Category = Category.Make((string)ItemCat.SelectedItem);
            ArmorForm r = new ArmorForm(i);
            r.Saved += ItemSaved;
            r.Show();
        }

        private void shieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ItemCat.SelectedItem == null) return;
            Shield i = new Shield();
            i.Category = Category.Make((string)ItemCat.SelectedItem);
            ShieldForm r = new ShieldForm(i);
            r.Saved += ItemSaved;
            r.Show();
        }

        private void packToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ItemCat.SelectedItem == null) return;
            Pack i = new Pack();
            i.Category = Category.Make((string)ItemCat.SelectedItem);
            PackForm r = new PackForm(i);
            r.Saved += ItemSaved;
            r.Show();
        }

        private void NewSkill_Click(object sender, EventArgs e)
        {
            SkillForm r = new SkillForm(new Skill());
            r.Saved += SkillSaved;
            r.Show();
        }

        private void skillList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (skillList.SelectedItem == null) return;
            Skill selected = Skill.Get((string)skillList.SelectedItem, null);
            if (selected != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(selected.toHTML());
                preview.Refresh();
            }
        }

        private void editSkill(object sender, EventArgs e)
        {
            if (skillList.SelectedItem == null) return;
            Skill selected = Skill.Get((string)skillList.SelectedItem, null);
            if (selected != null)
            {
                string sel = selected.Name;
                SkillForm r = new SkillForm(selected.clone());
                r.Saved += SkillSaved;
                r.Show();

            }
        }

        private void SkillSaved(object sender, string id)
        {
            Skill.ImportAll();
            fill(skillList, Skill.skills.Keys, id);
        }
        private void NewCond_Click(object sender, EventArgs e)
        {
            ConditionForm r = new ConditionForm(new Condition());
            r.Saved += ConditionSaved;
            r.Show();
        }

        private void condList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (condList.SelectedItem == null) return;
            Condition selected = Condition.Get((string)condList.SelectedItem, null);
            if (selected != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(selected.toHTML());
                preview.Refresh();
            }
        }

        private void editCondition(object sender, EventArgs e)
        {
            if (condList.SelectedItem == null) return;
            Condition selected = Condition.Get((string)condList.SelectedItem, null);
            if (selected != null)
            {
                string sel = selected.Name;
                ConditionForm r = new ConditionForm(selected.clone());
                r.Saved += ConditionSaved;
                r.Show();

            }
        }

        private void ConditionSaved(object sender, string id)
        {
            Condition.ImportAll();
            fill(condList, Condition.conditions.Keys, id);
        }

        private void save_Click(object sender, EventArgs e)
        {
            Level.Save(ConfigManager.loaded.Levels);
        }

        private void AbilitySave_Click(object sender, EventArgs e)
        {
            AbilityScores.Save();
        }

        private void SaveSettings_Click(object sender, EventArgs e)
        {
            ConfigManager.loaded.Save(Path.Combine(Application.StartupPath, "Config.xml"));
        }

        private void NewSpell_Click(object sender, EventArgs e)
        {
            Spell s = new Spell();
            s.Level = 1;
            s.Keywords.Add(new Character_Builder_5.Keyword("Verbal"));
            s.Keywords.Add(new Character_Builder_5.Keyword("Somatic"));
            SpellForm r = new SpellForm(s);
            r.Saved += SpellSaved;
            r.Show();
        }

        private void SpellList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (spellBox.SelectedItem == null) return;
            Spell selected = Spell.Get((string)spellBox.SelectedItem, null);
            if (selected != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(selected.toHTML());
                preview.Refresh();
            }
        }

        private void editSpell(object sender, EventArgs e)
        {
            if (spellBox.SelectedItem == null) return;
            Spell selected = Spell.Get((string)spellBox.SelectedItem, null);
            if (selected != null)
            {
                string sel = selected.Name;
                SpellForm r = new SpellForm(selected.clone());
                r.Saved += SpellSaved;
                r.Show();

            }
        }

        private void SpellSaved(object sender, string id)
        {
            Spell.ImportAll();
            fill(spellBox, Spell.spells.Keys, id);
        }

        private void ApplySpellsBtn_Click(object sender, EventArgs e)
        {
            Spell.ImportAll();
            ClassDefinition.ImportAll();
            foreach (Spell s in Spell.spells.Values)
            {
                if (s.KWChanged) s.save(false);
            }
        }

        private void ApplyFeatsBtn_Click(object sender, EventArgs e)
        {
            FeatureCollection.ImportAll();
            ClassDefinition.ImportAll();
            foreach (List<FeatureContainer> c in FeatureCollection.Container.Values)
            {
                foreach (FeatureContainer s in c)
                {
                    if (s.Features.Any(f => f.KWChanged)) s.save(false);
                }
            }
        }

        private void NewMagicProp_Click(object sender, EventArgs e)
        {
            if (magicCatBox.SelectedItem == null) return;
            MagicProperty i = new MagicProperty();
            string cat = (string)magicCatBox.SelectedItem;
            i.Category = cat;
            MagicForm r = new MagicForm(i);
            r.Saved += MagicSaved;
            r.Show();
        }


        private void MagicCats_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cat = (string)magicCatBox.SelectedItem;
            magicBox.Items.Clear();
            NewMagicCatBtn.Enabled = false;
            newMagicBtn.Enabled = false;
            if (cat != null)
            {
                NewMagicCatBtn.Enabled = true;
                newMagicBtn.Enabled = true;
                if (MagicProperty.Categories.ContainsKey(cat)) foreach(MagicProperty i in from i in MagicProperty.Categories[cat].Contents orderby i.Name select i) magicBox.Items.Add(i);
            }
        }

        private void NewMagicCat_Click(object sender, EventArgs e)
        {
            if (magicCatBox.SelectedItem == null) return;
            string cat = Interaction.InputBox("New category name", "New Sub Category");
            cat = string.Join("_", cat.Split(Path.GetInvalidFileNameChars()));
            SourceManager.getDirectory(ConfigManager.DefaultSource, Path.Combine((string)magicCatBox.SelectedItem, cat));
            if (TabControls.SelectedTab == magicTab) tabControl1_SelectedIndexChanged(null, null);
            else TabControls.SelectedTab = magicTab;
        }

        private void Magic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (magicBox.SelectedItem == null) return;
            MagicProperty selected = (MagicProperty)magicBox.SelectedItem;
            if (selected != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(selected.toHTML());
                preview.Refresh();
            }
        }

        private void editMagic(object sender, EventArgs e)
        {
            if (magicBox.SelectedItem == null) return;
            MagicProperty selected = (MagicProperty)magicBox.SelectedItem;
            MagicForm r = new MagicForm(selected.clone());
            r.Saved += MagicSaved;
            r.Show();
        }

        private void MagicSaved(object sender, string id)
        {
            string s = (string)magicCatBox.SelectedItem;
            var scroll = savescroll(magicBox);
            MagicProperty.ImportAll();
            if (TabControls.SelectedTab == magicTab) tabControl1_SelectedIndexChanged(null, null);
            else TabControls.SelectedTab = magicTab;
            magicCatBox.SelectedItem = s;
            if (s == null) magicBox.Items.Clear();
            else restore(magicBox, scroll, id);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox1.Text == "")
            {
                fill(spellBox, Spell.spells.Keys, null);
            } else
            {
                try
                {
                    fill(spellBox, from s in Spell.filter(textBox1.Text) select s.Name, null);
                } catch(Exception ex)
                {
                    spellBox.Items.Clear();
                }
            }
        }
    }
}
