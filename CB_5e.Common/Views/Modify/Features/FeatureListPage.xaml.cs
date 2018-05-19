using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CB_5e.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CB_5e.ViewModels;
using OGL.Features;
using CB_5e.ViewModels.Modify;
using CB_5e.ViewModels.Modify.Features;
using CB_5e.Services;
using CB_5e.Views.Modify.Collections;
using OGL;
using OGL.Items;

namespace CB_5e.Views.Modify.Features
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FeatureListPage : ContentPage
	{
        private List<Feature> features;
        public ObservableRangeCollection<FeatureViewModel> Features { get; set; } = new ObservableRangeCollection<FeatureViewModel>();
        public IEditModel Model { get; private set; }
        public string Property { get; private set; }
        private int move = -1;
        private bool TopLevel = true;

        public Command Undo { get => Model.Undo; }
        public Command Redo { get => Model.Redo; }

        public FeatureListPage (IEditModel parent, string property, bool topLevel = true)
		{
            Model = parent;
            TopLevel = topLevel;
            parent.PropertyChanged += Parent_PropertyChanged;
            Property = property;
            UpdateFeatures();
			InitializeComponent ();
            InitToolbar();
            BindingContext = this;
        }

        private void InitToolbar()
        {
            if (TopLevel)
            {
                ToolbarItem undo = new ToolbarItem() { Text = "Undo" };
                undo.SetBinding(MenuItem.CommandProperty, new Binding("Undo"));
                ToolbarItems.Add(undo);
                ToolbarItem redo = new ToolbarItem() { Text = "Redo" };
                redo.SetBinding(MenuItem.CommandProperty, new Binding("Redo"));
                ToolbarItems.Add(redo);
            }
            else
            {
                ToolbarItem back = new ToolbarItem() { Text = "back" };
                back.Clicked += Back_Clicked;
                ToolbarItems.Add(back);
            }
            ToolbarItem add = new ToolbarItem() { Text = "Add" };
            add.Clicked += Add_Clicked;
            ToolbarItems.Add(add);
            ToolbarItem paste = new ToolbarItem() { Text = "Paste" };
            paste.Clicked += Paste_Clicked;
            ToolbarItems.Add(paste);
        }
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Model.SaveAsync(true);
            await Navigation.PopModalAsync();
        }
        private void Parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == Property) UpdateFeatures();
        }

        private void UpdateFeatures()
        {
            features = (List<Feature>)Model.GetType().GetRuntimeProperty(Property).GetValue(Model);
            Fill();
        }
        private void Fill() => Features.ReplaceRange(features.Select(f => new FeatureViewModel(f)));

        private void Paste_Clicked(object sender, EventArgs e)
        {
            try
            {
                Model.MakeHistory();
                foreach (Feature f in Feature.LoadString(DependencyService.Get<IClipboardService>().GetTextData())) features.Add(f);
                Fill();
            } catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectPage(GetOptions(), new Command(async (par) => {
                    if (par is SelectOption o && o.Value is Feature d)
                    {
                        FeatureViewModel fvm = new FeatureViewModel(d);
                        Model.MakeHistory();
                        features.Add(d);
                        Features.Add(fvm);
                        await Navigation.PushModalAsync(Edit(fvm, Model));
                }
                })));
        }

        public static IEnumerable<SelectOption> GetOptions()
        {
            return new List<SelectOption>()
                {
                    new SelectOption("Basic Feature", "Text and description", new Feature()),
                    new SelectOption("Resource Feature", "Defines or modifies a trackable resource", new ResourceFeature()),
                    new SelectOption("Ability Score Feature", "Modifies ability scores or their maximum values", new AbilityScoreFeature()),
                    new SelectOption("Ability Score Increase / Feat Feature", "Option to increase 2 ability scores or gain a feat", new AbilityScoreFeatFeature(null, 4)),
                    new SelectOption("AC Calculation Feature", "Adds a new way to calculate AC", new ACFeature() { Expression = "if(Armor, if(Light, BaseAC + DexMod, if(Medium, BaseAC + Min(DexMod, 2), BaseAC)), 10 + DexMod) + ShieldBonus"}),
                    new SelectOption("Extra Attack Feature", "Sets the amount of extra attacks when attacking", new ExtraAttackFeature()),
                    new SelectOption("Hitpoint Feature", "Defines a bonus to hitpoints", new HitPointsFeature()),
                    new SelectOption("Stat Bonus Feature", "Defines various conditional stat boni, i.e. attack, AC, skills, damage", new BonusFeature()),
                    new SelectOption("Resistance Feature", "A feature for Resistances", new ResistanceFeature()),
                    new SelectOption("Speed Feature", "Defines the base speed or a stacking speed bonus", new SpeedFeature()),
                    new SelectOption("Vision Feature", "Defines the range of darkvision a character has", new VisionFeature() {Range = 60 }),
                    new SelectOption("Skill Proficiency Feature", "Adds proficiency to skills", new SkillProficiencyFeature()),
                    new SelectOption("Skill Proficiency Choice Feature", "Allows for a choice of skills to add proficiency to", new SkillProficiencyChoiceFeature()),
                    new SelectOption("Language Proficiency Feature", "Adds proficiency with specific languages", new LanguageProficiencyFeature()),
                    new SelectOption("Language Proficiency Choice Feature", "Adds proficiency with chosen languages", new LanguageChoiceFeature()),
                    new SelectOption("Tool Proficiency Feature", "Adds proficiency with specific tools", new ToolProficiencyFeature()),
                    new SelectOption("Tool Proficiency Choice Feature", "Adds proficiency with chosen tools", new ToolProficiencyChoiceConditionFeature()),
                    new SelectOption("Tool Proficiency by Expression Feature", "Adds proficiency with a category of tools", new ToolKWProficiencyFeature()),
                    new SelectOption("Save Proficiency Feature", "Adds proficiency to saving throws", new SaveProficiencyFeature()),
                    new SelectOption("Other Proficiency Feature", "Basic feature whose description is shown as proficiency", new OtherProficiencyFeature()),
                    new SelectOption("Spellcasting Feature", "Sets up Spellcasting, required by non-bonusspell spellcasting features", new SpellcastingFeature()),
                    new SelectOption("Spellslot Feature", "Defines the total amount of spellslots for one level and spellcasting", new SpellSlotsFeature() { Name = "Spellslots" }),
                    new SelectOption("Add Spells Feature", "Adds spells to a spellcasting feature", new BonusSpellPrepareFeature()),
                    new SelectOption("Spell Modify Feature", "Modifies spells and is displayed with them", new SpellModifyFeature() {Spells = "(Fire or Cold) and Melee and Attack and Level >= 3" }),
                    new SelectOption("Spellchoice Feature", "Sets up spellchoices to be added to a spellcasting feature", new SpellChoiceFeature()),
                    new SelectOption("Increase Spellchoice Feature", "Increases the amount of spells selectable for a spellchoice", new IncreaseSpellChoiceAmountFeature()),
                    new SelectOption("Modify Spellchoice Feature", "Adds more spells as options for a spellchoice", new ModifySpellChoiceFeature()),
                    new SelectOption("Bonus Spell Feature", "Adds a spell as a resource outside of a spellcasting system", new BonusSpellFeature()),
                    new SelectOption("Bonus Spell Choice Feature", "Adds selectable spells as a resource", new BonusSpellKeywordChoiceFeature()),
                    new SelectOption("Free Item Gold Feature", "Adds items and/or gold", new FreeItemAndGoldFeature()),
                    new SelectOption("Free Item Choice Feature", "Adds a choice of items and/or gold", new ItemChoiceFeature()),
                    new SelectOption("Free Item Choice by Condition Feature", "Adds a choice of items matching an expression", new ItemChoiceConditionFeature() { Condition="Armor and Heavy" }),
                    new SelectOption("Multi Feature", "Contains other features with an optional activation condition. Also used in Feature Choice Features and Standalone Features", new MultiFeature()),
                    new SelectOption("Feature Choice Feature", "Adds a choice of contained features", new ChoiceFeature()),
                    new SelectOption("Collection Feature Choice Feature", "Adds a choice of standalone features using a condition", new CollectionChoiceFeature() { Collection = "Category = 'Feats'" }),
                    new SelectOption("Subclass Feature", "Adds a parent to the list classes used to generate available subclasses", new SubClassFeature()),
                    new SelectOption("Subrace Feature", "Adds parents to the list of races used to generate available subraces", new SubRaceFeature()),
                    new SelectOption("Forms/Companions Feature", "Adds a selectable form or companion (summon) option", new FormsCompanionsFeature()),
                    new SelectOption("Forms/Companions Bonus Feature", "Adds Boni to a selected form or companion", new FormsCompanionsBonusFeature())
                };
        }

        private async void Features_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is FeatureViewModel fvm) {
                if (move >= 0)
                {
                    Model.MakeHistory();
                    foreach (FeatureViewModel ff in Features) ff.Moving = false;
                    int target = features.FindIndex(ff => fvm.Feature == ff);
                    if (target >= 0 && move != target)
                    {
                        features.Insert(target, features[move]);
                        if (target < move) move++;
                        features.RemoveAt(move);
                        Fill();
                        (sender as ListView).SelectedItem = null;
                    }
                    move = -1;
                } else
                {
                    await Navigation.PushModalAsync(Edit(fvm, Model));
                    (sender as ListView).SelectedItem = null;
                }
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is FeatureViewModel f)
            {
                Model.MakeHistory();
                int i = features.FindIndex(ff => f.Feature == ff);
                features.RemoveAt(i);
                Fill();
            }
        }

        private async void Info_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is FeatureViewModel f) await Navigation.PushAsync(InfoPage.Show(f.Feature));
        }

        private void Move_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is FeatureViewModel f)
            {
                foreach (FeatureViewModel ff in Features) ff.Moving = false;
                f.Moving = true;
                move = features.FindIndex(ff => f.Feature == ff);
            }
            
        }

        private void Cut_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is FeatureViewModel f)
            {
                Model.MakeHistory();
                DependencyService.Get<IClipboardService>().PutTextData(f.Feature.Save(), f.Detail);
                int i = features.FindIndex(ff => f.Feature == ff);
                features.RemoveAt(i);
                Fill();
            }
        }

        private void Copy_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is FeatureViewModel f)
            {
                DependencyService.Get<IClipboardService>().PutTextData(f.Feature.Save(), f.Detail);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (TopLevel)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (Model.UnsavedChanges > 0)
                    {
                        if (await DisplayAlert("Unsaved Changes", "You have " + Model.UnsavedChanges + " unsaved changes. Do you want to save them before leaving?", "Yes", "No"))
                        {
                            bool written = await Model.SaveAsync(false);
                            if (!written)
                            {
                                if (await DisplayAlert("File Exists", "Overwrite File?", "Yes", "No"))
                                {
                                    await Model.SaveAsync(true);
                                    await Navigation.PopModalAsync();
                                }
                            }
                            else await Navigation.PopModalAsync();
                        }
                        else await Navigation.PopModalAsync();
                    }
                    else await Navigation.PopModalAsync();
                });
            }
            else
            {
                Task.Run(async () =>
                {
                    await Model.SaveAsync(true);
                    await Navigation.PopModalAsync();
                });
            }
            return true;
        }

        public static Page Edit(FeatureViewModel fvm, IEditModel Model)
        {
            TabbedPage p;
            if (fvm.Feature is ResourceFeature rf)
            {
                ResourceFeatureEditModel model = new ResourceFeatureEditModel(rf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditResourceFeature(model))
                {
                    Title = "Resource"
                });
            }
            else if (fvm.Feature is AbilityScoreFeature asf)
            {
                AbilityFeatureEditModel model = new AbilityFeatureEditModel(asf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditAbilityFeature(model))
                {
                    Title = "Abilities"
                });
            }
            else if (fvm.Feature is ACFeature acf)
            {
                IFeatureEditModel model = new ACFeatureEditModel(acf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditExpression(model, "AC Calculation", "AC Calculation Expression: (NCalc)", "Expression", "Note: The expression must result in a number. If there are multiple AC Calculation features, the one returning the highest AC is taken.\nThe following values are available: BaseAC(of Armor), ShieldBonus(if equiped), ACBonus(Bonus that will be added due to other features), Str, Dex, Con, Int, Wis, Cha(Total value), StrMod, DexMod, ConMod, IntMod, WisMod, ChaMod(Modifier).\nThe following boolean flags are available: Unarmored, Armor, OffHand(weapon in off - Hand), Shield, Two - Handed(weapon), FreeHand as well as any Keywords of the Armor.\nThe following string values are available: Category(Category of the equipped Armor), Name(Name of the Armor)."))
                {
                    Title = "AC"
                });
            }
            else if (fvm.Feature is HitPointsFeature hpf)
            {
                IFeatureEditModel model = new HitpointsFeatureEditModel(hpf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditExpression(model, "Hitpoints", "Hitpoints Expression: (NCalc)", "Expression", "Note: The expression must result in a number. The following number values are available: Str, Dex, Con, Int, Wis, Cha (Value) and StrMod, DexMod, ConMod, IntMod, WisMod, ChaMod (Modifier), PlayerLevel (character level), ClassLevel (class level if in class, PlayerLevel otherwise), ClassLevel(\"classname\") (function for classlevel)"))
                {
                    Title = "HP"
                });
            }
            else if (fvm.Feature is VisionFeature vf)
            {
                IFeatureEditModel model = new VisionFeatureEditModel(vf, Model, fvm);
                p = new TabbedPage();
                p.Children.Add(new NavigationPage(new EditIntFeature(model, "Vision Feature", "Darkvision Range: (doesn't stack, highest counts)", "Value", 5))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, GetClassesAsync(Model.Context)))
                {
                    Title = "Standalone"
                });
            }
            else if (fvm.Feature is ExtraAttackFeature eaf)
            {
                IFeatureEditModel model = new ExtraAttackFeatureEditModel(eaf, Model, fvm);
                p = new TabbedPage();
                p.Children.Add(new NavigationPage(new EditIntFeature(model, "Extra Attack Feature", "Additional Attacks: (doesn't stack, highest counts)", "Value"))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, GetClassesAsync(Model.Context)))
                {
                    Title = "Standalone"
                });
            }
            else if (fvm.Feature is AbilityScoreFeatFeature aff)
            {
                AbilityFeatFeatureEditModel model = new AbilityFeatFeatureEditModel(aff, Model, fvm);
                p = new TabbedPage();
                p.Children.Add(new NavigationPage(new EditAbilityFeatFeature(model))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, GetClassesAsync(Model.Context)))
                {
                    Title = "Standalone"
                });
            }
            else if (fvm.Feature is SpeedFeature sf)
            {
                SpeedFeatureEditModel model = new SpeedFeatureEditModel(sf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditSpeedFeature(model))
                {
                    Title = "Speed"
                });
            }
            else if (fvm.Feature is BonusFeature bf)
            {
                BonusFeatureEditModel model = new BonusFeatureEditModel(bf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditBonusFeature(model))
                {
                    Title = "Bonus"
                });
                p.Children.Add(new NavigationPage(new BonusFeatureSkillsPage(model))
                {
                    Title = "Skills"
                });
                p.Children.Add(new NavigationPage(new BonusFeatureWeaponPage(model))
                {
                    Title = "Weapon"
                });

            }
            else if (fvm.Feature is SkillProficiencyFeature spf)
            {
                SkillProficiencyFeatureEditModel model = new SkillProficiencyFeatureEditModel(spf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new SkillProficiencyFeaturePage(model))
                {
                    Title = "Skills"
                });
            }
            else if (fvm.Feature is SkillProficiencyChoiceFeature spcf)
            {
                SkillProficiencyChoiceFeatureEditModel model = new SkillProficiencyChoiceFeatureEditModel(spcf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new SkillProficiencyChoiceFeaturePage(model))
                {
                    Title = "Skills"
                });
            }
            else if (fvm.Feature is LanguageProficiencyFeature lpf)
            {
                LanguageProficiencyFeatureEditModel model = new LanguageProficiencyFeatureEditModel(lpf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new StringListPage(model, "Languages", GetLanguagesAsync(Model.Context), false))
                {
                    Title = "Languages"
                });
            }
            else if (fvm.Feature is LanguageChoiceFeature lcf)
            {
                LanguageProficiencyChoiceFeatureEditModel model = new LanguageProficiencyChoiceFeatureEditModel(lcf, Model, fvm);
                p = new TabbedPage();
                p.Children.Add(new NavigationPage(new EditLanguageChoiceFeature(model))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, GetClassesAsync(Model.Context)))
                {
                    Title = "Standalone"
                });
            }
            else if (fvm.Feature is ToolProficiencyFeature tpf)
            {
                ToolProficiencyFeatureEditModel model = new ToolProficiencyFeatureEditModel(tpf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new StringListPage(model, "Tools", GetToolsAsync(Model.Context), false))
                {
                    Title = "Tools"
                });
            }
            else if (fvm.Feature is ToolProficiencyChoiceConditionFeature tpcf)
            {
                ToolProficiencyChoiceFeatureEditModel model = new ToolProficiencyChoiceFeatureEditModel(tpcf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new ToolProficiencyChoicePage(model))
                {
                    Title = "Tools"
                });
            }
            else if (fvm.Feature is ToolKWProficiencyFeature tkpf)
            {
                ToolProficiencyExpressionFeatureEditModel model = new ToolProficiencyExpressionFeatureEditModel(tkpf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new ToolProficiencyExpressionPage(model))
                {
                    Title = "Tools"
                });
            }
            else if (fvm.Feature is SaveProficiencyFeature saf)
            {
                SaveProficiencyFeatureEditModel model = new SaveProficiencyFeatureEditModel(saf, Model, fvm);
                p = new TabbedPage();
                p.Children.Add(new NavigationPage(new EditSaveProficiencyFeature(model))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, GetClassesAsync(Model.Context)))
                {
                    Title = "Standalone"
                });
            }
            else if (fvm.Feature is SpellcastingFeature scf)
            {
                SpellcastingFeatureEditModel model = new SpellcastingFeatureEditModel(scf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditSpellcasting(model))
                {
                    Title = "Spellcasting"
                });
            }
            else if (fvm.Feature is SpellSlotsFeature ssf)
            {
                SpellSlotsFeatureEditModel model = new SpellSlotsFeatureEditModel(ssf, Model, fvm);
                p = new TabbedPage();
                p.Children.Add(new NavigationPage(new EditSpellslotFeature(model))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, GetClassesAsync(Model.Context)))
                {
                    Title = "Standalone"
                });
                p.Children.Add(new NavigationPage(new IntListPage(model, "Slots", "Spellslot Level ", "0 'slots'", Keyboard.Numeric, false))
                {
                    Title = "Slots"
                });
            }
            else if (fvm.Feature is BonusSpellPrepareFeature bspf)
            {
                AddSpellsFeatureEditModel model = new AddSpellsFeatureEditModel(bspf, Model, fvm);
                p = new TabbedPage();
                var load = GetClassesAsync(Model.Context);
                p.Children.Add(new NavigationPage(new EditAddSpellsFeature(model))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, load))
                {
                    Title = "Standalone"
                });
                p.Children.Add(new NavigationPage(new DualSpellListPage(model))
                {
                    Title = "Spell"
                });
                p.Children.Add(new NavigationPage(new KeywordListPage(model, "AdditionalKeywords", "Keywords to add to the selected spells:", KeywordListPage.KeywordGroup.SPELL, load, false))
                {
                    Title = "Keywords"
                });
            }
            else if (fvm.Feature is SpellModifyFeature smf)
            {
                SpellModifyFeatureEditModel model = new SpellModifyFeatureEditModel(smf, Model, fvm);
                p = new TabbedPage();
                p.Children.Add(new NavigationPage(new EditSpellModifyFeature(model))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, GetClassesAsync(Model.Context)))
                {
                    Title = "Standalone"
                });
            }
            else if (fvm.Feature is SpellChoiceFeature sof)
            {
                SpellchoiceFeatureEditModel model = new SpellchoiceFeatureEditModel(sof, Model, fvm);
                p = new TabbedPage();
                var load = GetClassesAsync(Model.Context);
                p.Children.Add(new NavigationPage(new EditFeature(model))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, load))
                {
                    Title = "Standalone"
                });
                p.Children.Add(new NavigationPage(new EditSpellchoiceFeature(model))
                {
                    Title = "Spellchoice"
                });
                p.Children.Add(new NavigationPage(new KeywordListPage(model, "AdditionalKeywords", "Keywords to add to the selected spells:", KeywordListPage.KeywordGroup.SPELL, load, false))
                {
                    Title = "Keywords"
                });
            }
            else if (fvm.Feature is ModifySpellChoiceFeature msf)
            {
                ModifySpellchoiceEditModel model = new ModifySpellchoiceEditModel(msf, Model, fvm);
                p = new TabbedPage();
                var load = GetClassesAsync(Model.Context);
                p.Children.Add(new NavigationPage(new EditModifySpellchoiceFeature(model))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, load))
                {
                    Title = "Standalone"
                });
                p.Children.Add(new NavigationPage(new DualSpellListPage(model))
                {
                    Title = "Spells"
                });
                p.Children.Add(new NavigationPage(new KeywordListPage(model, "AdditionalKeywords", "Keywords to add to the selected spells:", KeywordListPage.KeywordGroup.SPELL, load, false))
                {
                    Title = "Keywords"
                });
            }
            else if (fvm.Feature is IncreaseSpellChoiceAmountFeature imf)
            {
                IncreaseSpellchoiceEditModel model = new IncreaseSpellchoiceEditModel(imf, Model, fvm);
                p = new TabbedPage();
                p.Children.Add(new NavigationPage(new EditIncreaseSpellchoiceFeature(model))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, GetClassesAsync(Model.Context)))
                {
                    Title = "Standalone"
                });
            }
            else if (fvm.Feature is BonusSpellFeature bsf)
            {
                BonusSpellFeatureEditModel model = new BonusSpellFeatureEditModel(bsf, Model, fvm);
                p = new TabbedPage();
                var load = GetClassesAsync(Model.Context);
                p.Children.Add(new NavigationPage(new EditFeature(model))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, load))
                {
                    Title = "Standalone"
                });
                p.Children.Add(new NavigationPage(new EditBonusSpellFeature(model))
                {
                    Title = "Spells"
                });
                p.Children.Add(new NavigationPage(new KeywordListPage(model, "AdditionalKeywords", "Keywords to add to the bonus spell:", KeywordListPage.KeywordGroup.SPELL, load, false))
                {
                    Title = "Keywords"
                });
            }
            else if (fvm.Feature is BonusSpellKeywordChoiceFeature bskcf)
            {
                BonusSpellChoiceFeatureEditModel model = new BonusSpellChoiceFeatureEditModel(bskcf, Model, fvm);
                p = new TabbedPage();
                var load = GetClassesAsync(Model.Context);
                p.Children.Add(new NavigationPage(new EditFeature(model))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, load))
                {
                    Title = "Standalone"
                });
                p.Children.Add(new NavigationPage(new EditBonusSpellChoiceFeature(model))
                {
                    Title = "Spells"
                });
                p.Children.Add(new NavigationPage(new KeywordListPage(model, "AdditionalKeywords", "Keywords to add to the bonus spells:", KeywordListPage.KeywordGroup.SPELL, load, false))
                {
                    Title = "Keywords"
                });
            }
            else if (fvm.Feature is ItemChoiceFeature icf)
            {
                ItemChoiceEditModel model = new ItemChoiceEditModel(icf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new FreeItemChoicePage(model))
                {
                    Title = "Items"
                });
            }
            else if (fvm.Feature is FreeItemAndGoldFeature figf)
            {
                FreeItemGoldFeatureEditModel model = new FreeItemGoldFeatureEditModel(figf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new FreeItemPage(model))
                {
                    Title = "Items"
                });
            }
            else if (fvm.Feature is ItemChoiceConditionFeature iccf)
            {
                ItemChoiceCondtionEditModel model = new ItemChoiceCondtionEditModel(iccf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditItemChoiceConditionFeature(model))
                {
                    Title = "Items"
                });
            }
            else if (fvm.Feature is MultiFeature mf)
            {
                MultiFeatureEditModel model = new MultiFeatureEditModel(mf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditExpression(model, "Condition", "Condition:", "Condition", "Note: If no condition is set or the condtion evaluates to true, the features are added to the character (The stats are before boni are added from features)\nThe following number values are known: Str, Dex, Con, Int, Wis, Cha (Value) and StrMod, DexMod, ConMod, IntMod, WisMod, ChaMod (Modifier)\nPlayerLevel (character level), ClassLevel (class level if in class, PlayerLevel otherwise), ClassLevel('classname') (function for classlevel)\nThe following text values are known: Race, SubRace, SubClass('classname'): names of the subrace, race and subclasses respectively."))
                {
                    Title = "Condition"
                });
                p.Children.Add(new NavigationPage(new FeatureListPage(model, "Features", false))
                {
                    Title = "Features"
                });
            }
            else if (fvm.Feature is ChoiceFeature cf)
            {
                ChoiceFeatureEditModel model = new ChoiceFeatureEditModel(cf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditFeatureChoiceFeature(model))
                {
                    Title = "Choice"
                });
                p.Children.Add(new NavigationPage(new FeatureListPage(model, "Choices", false))
                {
                    Title = "Choices"
                });
            }
            else if (fvm.Feature is CollectionChoiceFeature ccf)
            {
                CollectionChoiceFeatureEditModel model = new CollectionChoiceFeatureEditModel(ccf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditFeatureCollectionChoice(model))
                {
                    Title = "Choice"
                });
            }
            else if (fvm.Feature is SubClassFeature sclf)
            {
                SubClassFeatureEditModel model = new SubClassFeatureEditModel(sclf, Model, fvm);
                p = new TabbedPage();
                var load = GetClassesAsync(Model.Context);
                p.Children.Add(new NavigationPage(new EditSubClassFeature(model, load))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model, load))
                {
                    Title = "Standalone"
                });
            }
            else if (fvm.Feature is SubRaceFeature srf)
            {
                SubRaceFeatureEditModel model = new SubRaceFeatureEditModel(srf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new StringListPage(model, "ParentRaces", GetRacesAsync(Model.Context), false))
                {
                    Title = "Races"
                });
            }
            else if (fvm.Feature is ResistanceFeature resf)
            {
                ResistanceFeatureEditModel model = new ResistanceFeatureEditModel(resf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new StringListPage(model, "Resistances", null, false)) { Title = "Resistances" });
                p.Children.Add(new NavigationPage(new StringListPage(model, "Vulnerabilities", null, false)) { Title = "Vulnerabilities" });
                p.Children.Add(new NavigationPage(new StringListPage(model, "Immunities", null, false)) { Title = "Immunities" });
                p.Children.Add(new NavigationPage(new StringListPage(model, "SavingThrowAdvantages", null, false)) { Title = "SavingThrowAdvantages" });
            }
            else if (fvm.Feature is FormsCompanionsBonusFeature fcbf)
            {
                FormsCompanionsBonusFeatureEditModel model = new FormsCompanionsBonusFeatureEditModel(fcbf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditFormsCompanionsBonusFeature(model)) { Title = "Forms/Companions Bonus" });
                p.Children.Add(new NavigationPage(new StringListPage(model, "Senses", null, false)) { Title = "Add. Senses" });
                p.Children.Add(new NavigationPage(new StringListPage(model, "Speed", null, false)) { Title = "Add. Speed" });
                p.Children.Add(new NavigationPage(new StringListPage(model, "Languages", null, false)) { Title = "Add. Languages" });
            }
            else if (fvm.Feature is FormsCompanionsFeature fcf)
            {
                FormsCompanionsFeatureEditModel model = new FormsCompanionsFeatureEditModel(fcf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditFormsCompanionsFeature(model)) { Title = "Forms/Companions Choice" });
            }
            else {
                IFeatureEditModel model = new FeatureEditModel<Feature>(fvm.Feature, Model, fvm);
                p = Tab(model);
            }
            return p;
        }

        public static async Task<IEnumerable<string>> GetLanguagesAsync(OGLContext context)
        {
            if (context.LanguagesSimple.Count == 0)
            {
                await context.ImportLanguagesAsync();
            }
            return context.LanguagesSimple.Keys.OrderBy(s => s);
        }

        public static async Task<IEnumerable<string>> GetToolsAsync(OGLContext context)
        {
            if (context.ItemsSimple.Count == 0)
            {
                await context.ImportItemsAsync();
            }
            return context.ItemsSimple.Values.Where(s=>s is Tool).Select(s=>s.Name).OrderBy(s => s);
        }

        public static async Task<IEnumerable<string>> GetClassesAsync(OGLContext context)
        {
            if (context.ClassesSimple.Count == 0)
            {
                await context.ImportClassesAsync();
            }
            return context.ClassesSimple.Keys.OrderBy(s => s).ToList();
        }

        public static async Task<IEnumerable<string>> GetRacesAsync(OGLContext context)
        {
            if (context.RacesSimple.Count == 0)
            {
                await context.ImportRacesAsync();
            }
            return context.RacesSimple.Keys.OrderBy(s => s).ToList();
        }

        public static TabbedPage Tab(IFeatureEditModel model)
        {
            TabbedPage p = new TabbedPage();

            p.Children.Add(new NavigationPage(new EditFeature(model))
            {
                Title = "Feature"
            });
            p.Children.Add(new NavigationPage(new FeatureKeywords(model, GetClassesAsync(model.Context)))
            {
                Title = "Standalone"
            });
            return p;
        }
    }
}