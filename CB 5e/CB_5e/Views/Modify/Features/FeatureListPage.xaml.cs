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
        private bool Modal = true;

        public Command Undo { get => Model.Undo; }
        public Command Redo { get => Model.Redo; }

        public FeatureListPage (IEditModel parent, string property, bool modal = true)
		{
            Model = parent;
            Modal = modal;
            parent.PropertyChanged += Parent_PropertyChanged;
            Property = property;
            UpdateFeatures();
			InitializeComponent ();
            InitToolbar(Modal);
            BindingContext = this;
        }

        private void InitToolbar(bool modal)
        {
            if (Modal)
            {
                ToolbarItem undo = new ToolbarItem() { Text = "Undo" };
                undo.SetBinding(MenuItem.CommandProperty, new Binding("Undo"));
                ToolbarItems.Add(undo);
                ToolbarItem redo = new ToolbarItem() { Text = "Redo" };
                redo.SetBinding(MenuItem.CommandProperty, new Binding("Redo"));
                ToolbarItems.Add(redo);
            }
            ToolbarItem add = new ToolbarItem() { Text = "Add" };
            add.Clicked += Add_Clicked;
            ToolbarItems.Add(add);
            ToolbarItem paste = new ToolbarItem() { Text = "Paste" };
            paste.Clicked += Paste_Clicked;
            ToolbarItems.Add(paste);
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
            await Navigation.PushAsync(new SelectPage(new List<SelectOption>()
                {
                    new SelectOption("Basic Feature", "Text and description", new Feature()),
                    new SelectOption("Resource Feature", "Defines or modifies a trackable resource", new ResourceFeature()),
                    new SelectOption("Ability Score Feature", "Modifies ability scores or their maximum values", new AbilityScoreFeature()),
                    new SelectOption("Ability Score Increase / Feat Feature", "Option to increase 2 ability scores or gain a feat", new AbilityScoreFeatFeature(null, 4)),
                    new SelectOption("AC Calculation Feature", "Adds a new way to calculate AC", new ACFeature() { Expression = "if(Armor, if(Light, BaseAC + DexMod, if(Medium, BaseAC + Min(DexMod, 2), BaseAC)), 10 + DexMod) + ShieldBonus"}),
                    new SelectOption("Extra Attack Feature", "Sets the amount of extra attacks when attacking", new ExtraAttackFeature()),
                    new SelectOption("Hitpoint Feature", "Defines a bonus to hitpoints", new HitPointsFeature()),
                    new SelectOption("Stat Bonus Feature", "Defines various conditional stat boni, i.e. attack, AC, skills, damage", new BonusFeature()),
                    new SelectOption("Speed Feature", "Defines the base speed or a stacking speed bonus", new SpeedFeature()),
                    new SelectOption("Vision Feature", "Defines the range of darkvision a character has", new VisionFeature() {Range = 60 }),
                    new SelectOption("Skill Proficiency Feature", "Adds proficiency to skills", new SkillProficiencyFeature()),
                    new SelectOption("Skill Proficiency Choice Feature", "Allows for a choice of skills to add proficiency to", new SkillProficiencyChoiceFeature())
                }, new Command(async (par) => {
                    if (par is SelectOption o && o.Value is Feature d)
                    {
                        FeatureViewModel fvm = new FeatureViewModel(d);
                        Model.MakeHistory();
                        features.Add(d);
                        Features.Add(fvm);
                        await Edit(fvm);
                    }
                })));
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
                    await Edit(fvm);
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
            if (Modal)
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
            return Modal;
        }

        private async Task Edit(FeatureViewModel fvm)
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
                p.Children.Add(new NavigationPage(new EditExpression(model,"AC Calculation", "AC Calculation Expression: (NCalc)", "Expression", "Note: The expression must result in a number. If there are multiple AC Calculation features, the one returning the highest AC is taken.\nThe following values are available: BaseAC(of Armor), ShieldBonus(if equiped), ACBonus(Bonus that will be added due to other features), Str, Dex, Con, Int, Wis, Cha(Total value), StrMod, DexMod, ConMod, IntMod, WisMod, ChaMod(Modifier).\nThe following boolean flags are available: Unarmored, Armor, OffHand(weapon in off - Hand), Shield, Two - Handed(weapon), FreeHand as well as any Keywords of the Armor.\nThe following string values are available: Category(Category of the equipped Armor), Name(Name of the Armor)."))
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
                p.Children.Add(new NavigationPage(new FeatureKeywords(model))
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
                p.Children.Add(new NavigationPage(new FeatureKeywords(model))
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
                p.Children.Add(new NavigationPage(new FeatureKeywords(model))
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
            else {
                IFeatureEditModel model = new FeatureEditModel<Feature>(fvm.Feature, Model, fvm);
                p = Tab(model);
            }
            await Navigation.PushModalAsync(p);
        }

        private TabbedPage Tab(IFeatureEditModel model)
        {
            TabbedPage p = new TabbedPage();

            p.Children.Add(new NavigationPage(new EditFeature(model))
            {
                Title = "Feature"
            });
            p.Children.Add(new NavigationPage(new FeatureKeywords(model))
            {
                Title = "Standalone"
            });
            return p;
        }
    }
}