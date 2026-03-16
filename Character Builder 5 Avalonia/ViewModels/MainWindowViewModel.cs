using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Character_Builder;
using OGL;
using CommunityToolkit.Mvvm.ComponentModel;
using OGL.Common;
using CharacterBuilder5.Common;
using Character_Builder_IO;

namespace CharacterBuilder5.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private BuilderContext _context;

    [ObservableProperty]
    private string _title = "Character Builder 5 Avalonia";
    
    public System.Collections.Generic.IEnumerable<Race> AvailableRaces => Context.Races.Values.OrderBy(r => r.Name);
    
    private System.Collections.Generic.List<SourceItemViewModel>? _sourceViewModels;
    public System.Collections.Generic.IEnumerable<SourceItemViewModel> SourceItems => 
        _sourceViewModels ??= SourceManager.Sources.Select(s => new SourceItemViewModel(s, this)).ToList();

    public class SourceItemViewModel : ViewModelBase
    {
        private readonly string _name;
        private readonly MainWindowViewModel _mainViewModel;

        public SourceItemViewModel(string name, MainWindowViewModel mainViewModel)
        {
            _name = name;
            _mainViewModel = mainViewModel;
        }

        public string Name => _name;

        public bool IsEnabled
        {
            get => !_mainViewModel.Context.ExcludedSources.Contains(_name, StringComparer.OrdinalIgnoreCase);
            set
            {
                if (value != IsEnabled)
                {
                    _mainViewModel.ToggleSource(_name, value);
                    OnPropertyChanged();
                }
            }
        }
    }

    public void ToggleSource(string name, bool enable)
    {
        Context.MakeHistory("Sources");
        if (enable)
        {
            Context.Player.ExcludedSources.RemoveAll(s => StringComparer.OrdinalIgnoreCase.Equals(s, name));
        }
        else
        {
            if (!Context.Player.ExcludedSources.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                Context.Player.ExcludedSources.Add(name);
            }
        }
        Context.ExcludedSources.Clear();
        Context.ExcludedSources.UnionWith(Context.Player.ExcludedSources);
        
        // Reload data like Program.ReloadData()
        InitializeContext();
        RefreshAll();
        // OnPropertyChanged(nameof(SourceItems)); // No longer needed as we are updating the existing view models
        OnPropertyChanged(nameof(AvailableRaces));
        OnPropertyChanged(nameof(AvailableClasses));
        OnPropertyChanged(nameof(AvailableBackgrounds));
    }

    public System.Collections.Generic.IEnumerable<SubRace> AvailableSubRaces
    {
        get
        {
            if (Context.Player.Race == null) return Enumerable.Empty<SubRace>();
            return Context.SubRaceFor(new System.Collections.Generic.List<string> { Context.Player.Race.Name }).OrderBy(s => s.Name);
        }
    }

    [ObservableProperty]
    private string? _currentSelectionHTML;

    public void UpdateSelectionHTML(IXML? obj)
    {
        CurrentSelectionHTML = obj?.ToHTML();
    }
    
    public void UpdateRace()
    {
        OnPropertyChanged();
        RefreshStats();
    }


    private SubRace? _previewSubRace;
    public SubRace? PreviewSubRace
    {
        get => _previewSubRace;
        set
        {
            if (SetProperty(ref _previewSubRace, value))
            {
                UpdateSelectionHTML(value);
            }
        }
    }

    public SubRace SelectedSubRace
    {
        get => Context.Player.SubRace;
        set
        {
            if (Context.Player.SubRace != value)
            {
                Context.MakeHistory("");
                Context.Player.SubRace = value;
                OnPropertyChanged();
                RefreshStats();
            }
        }
    }

    public void SelectSubRace()
    {
        if (PreviewSubRace != null)
        {
            SelectedSubRace = PreviewSubRace;
        }
    }
    
    public System.Collections.Generic.IEnumerable<ClassDefinition> AvailableClasses => Context.Classes.Values.OrderBy(c => c.Name);

    public System.Collections.Generic.List<PlayerClass> PlayerClasses => Context.Player.Classes;

    private ClassDefinition _previewClassToAdd;
    public ClassDefinition PreviewClassToAdd
    {
        get => _previewClassToAdd;
        set
        {
            if (SetProperty(ref _previewClassToAdd, value))
            {
                OnPropertyChanged(nameof(ClassDescription));
                UpdateSelectionHTML(value);
            }
        }
    }

    private ClassDefinition _selectedClassToAdd;
    public ClassDefinition SelectedClassToAdd
    {
        get => _selectedClassToAdd;
        set
        {
            if (SetProperty(ref _selectedClassToAdd, value))
            {
                OnPropertyChanged(nameof(ClassDescription));
                // UpdateSelectionHTML(value); // No longer needed here as it's in PreviewClassToAdd
            }
        }
    }

    public string ClassDescription
    {
        get
        {
            if (PreviewClassToAdd != null) return PreviewClassToAdd.Description;
            if (SelectedClassToAdd != null) return SelectedClassToAdd.Description;
            return "Select a class to see its description.";
        }
    }

    public void AddLevel()
    {
        var classToAdd = PreviewClassToAdd;
        if (classToAdd != null)
        {
            Context.MakeHistory("");
            Context.Player.AddClass(classToAdd, Context.Player.GetLevel() + 1);
            OnPropertyChanged(nameof(PlayerClasses));
            RefreshStats();
        }
    }

    public void RemoveLevel(PlayerClass pc)
    {
        if (pc != null)
        {
            Context.MakeHistory("");
            Context.Player.DeleteClass(pc.ClassLevelAtLevel.Last());
            OnPropertyChanged(nameof(PlayerClasses));
            RefreshStats();
        }
    }

    public int BaseStrength
    {
        get => Context.Player.BaseStrength;
        set { Context.Player.BaseStrength = value; OnPropertyChanged(); RefreshStats(); }
    }
    public int BaseDexterity
    {
        get => Context.Player.BaseDexterity;
        set { Context.Player.BaseDexterity = value; OnPropertyChanged(); RefreshStats(); }
    }
    public int BaseConstitution
    {
        get => Context.Player.BaseConstitution;
        set { Context.Player.BaseConstitution = value; OnPropertyChanged(); RefreshStats(); }
    }
    public int BaseIntelligence
    {
        get => Context.Player.BaseIntelligence;
        set { Context.Player.BaseIntelligence = value; OnPropertyChanged(); RefreshStats(); }
    }
    public int BaseWisdom
    {
        get => Context.Player.BaseWisdom;
        set { Context.Player.BaseWisdom = value; OnPropertyChanged(); RefreshStats(); }
    }
    public int BaseCharisma
    {
        get => Context.Player.BaseCharisma;
        set { Context.Player.BaseCharisma = value; OnPropertyChanged(); RefreshStats(); }
    }

    public System.Collections.Generic.IEnumerable<Background> AvailableBackgrounds => Context.Backgrounds.Values.OrderBy(b => b.Name);

    private Background? _previewBackground;
    public Background? PreviewBackground
    {
        get => _previewBackground;
        set
        {
            if (SetProperty(ref _previewBackground, value))
            {
                OnPropertyChanged(nameof(BackgroundDescription));
                UpdateSelectionHTML(value);
            }
        }
    }

    public Background SelectedBackground
    {
        get => Context.Player.Background;
        set
        {
            if (Context.Player.Background != value)
            {
                Context.MakeHistory("");
                Context.Player.Background = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BackgroundDescription));
                UpdateSelectionHTML(value);
            }
        }
    }

    public void SelectBackground()
    {
        if (PreviewBackground != null)
        {
            SelectedBackground = PreviewBackground;
        }
    }

    public string BackgroundDescription
    {
        get
        {
            if (PreviewBackground != null) return PreviewBackground.Description;
            if (SelectedBackground != null) return SelectedBackground.Description;
            return "Select a background to see its description.";
        }
    }

    public System.Collections.Generic.IEnumerable<SkillInfo> Skills => Context.Player.GetSkills();

    public int CurrentLevel => Context.Player.GetLevel();
    public string ClassSummary => string.Join(" | ", Context.Player.GetClassesStrings());
    public string RaceName => Context.Player.GetRaceSubName() ?? "None";
    public int AC => Context.Player.GetAC();
    public int Initiative => Context.Player.GetInitiative();
    public string Money => Context.Player.GetMoney().ToString();

    public string CharacterName
    {
        get => Context.Player.Name;
        set { Context.Player.Name = value; OnPropertyChanged(); }
    }

    public string Alignment
    {
        get => Context.Player.Alignment;
        set { Context.Player.Alignment = value; OnPropertyChanged(); }
    }

    public string PlayerName
    {
        get => Context.Player.PlayerName;
        set { Context.Player.PlayerName = value; OnPropertyChanged(); }
    }

    public async Task NewCharacter()
    {
        Context.MakeHistory("");
        Context.Player = new Player();
        Context.Player.Context = Context;
        RefreshAll();
    }

    public async Task OpenCharacter()
    {
        // This would normally use a file picker, but for now just a placeholder
    }

    public async Task SaveCharacter()
    {
        // This would normally use a file picker
    }

    private void RefreshAll()
    {
        OnPropertyChanged(nameof(SelectedSubRace));
        OnPropertyChanged(nameof(AvailableSubRaces));
        OnPropertyChanged(nameof(SelectedBackground));
        OnPropertyChanged(nameof(PlayerClasses));
        OnPropertyChanged(nameof(BaseStrength));
        OnPropertyChanged(nameof(BaseDexterity));
        OnPropertyChanged(nameof(BaseConstitution));
        OnPropertyChanged(nameof(BaseIntelligence));
        OnPropertyChanged(nameof(BaseWisdom));
        OnPropertyChanged(nameof(BaseCharisma));
        OnPropertyChanged(nameof(CharacterName));
        OnPropertyChanged(nameof(Alignment));
        OnPropertyChanged(nameof(PlayerName));
        OnPropertyChanged(nameof(Skills));
        OnPropertyChanged(nameof(CurrentSelectionHTML));
        RefreshStats();
    }

    private void RefreshStats()
    {
        OnPropertyChanged(nameof(CurrentLevel));
        OnPropertyChanged(nameof(ClassSummary));
        OnPropertyChanged(nameof(RaceName));
        OnPropertyChanged(nameof(AC));
        OnPropertyChanged(nameof(Initiative));
        OnPropertyChanged(nameof(Money));
    }

    public MainWindowViewModel()
    {
        _context = new BuilderContext();
        InitializeContext();
    }

    private void InitializeContext()
    {
        // Equivalent to Program.LoadData()
        string startupPath = AppContext.BaseDirectory;
        Config.LoadConfig(Context, startupPath);
        SourceManager.Init(Context, startupPath, true);
        
        Context.LoadLevel(ImportExtensions.Fullpath(startupPath, "Levels.xml"));
        Context.ImportZips(false);
        Context.ImportSkills(false);
        Context.ImportLanguages(false);
        Context.ImportSpells(false);
        Context.ImportItems(false);
        Context.ImportBackgrounds(false);
        Context.ImportRaces(false);
        Context.ImportSubRaces(false);
        Context.ImportStandaloneFeatures(false);
        Context.ImportConditions(false);
        Context.ImportMagic(false);
        Context.ImportClasses(false, true);
        Context.ImportSubClasses(false, true);
        
        foreach (ClassDefinition c in Context.Classes.Values) c.ApplyKeywords(Context);
        foreach (SubClass c in Context.SubClasses.Values) c.ApplyKeywords(Context);
        
        Context.ImportMonsters(false);

        Context.Player.ChoiceCounter.Clear();
        Context.Player.ChoiceTotal.Clear();
        Context.Player.Context = Context;
    }
}
