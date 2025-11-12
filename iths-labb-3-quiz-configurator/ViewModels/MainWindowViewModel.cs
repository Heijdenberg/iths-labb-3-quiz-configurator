using iths_labb_3_quiz_configurator.Models;
using iths_labb_3_quiz_configurator.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace iths_labb_3_quiz_configurator.ViewModels;

class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

    private QuestionPackViewModel? _activePack;
    private ViewModelBase? _activeView;
    private readonly IWindowServices _windowService;
    private readonly IDataService _dataService;
    private readonly DispatcherTimer _autoSaveTimer;
    private QuestionViewModel? _activeQuestion;
    private WindowState _windowState;
    private string _userMessage = "";


    public MainWindowViewModel(IWindowServices windowService, IDataService dataService)
    {
        _windowService = windowService;
        _dataService = dataService;
        ConfigurationViewModel = new ConfigurationViewModel(this);
        MenuViewModel = new MenuViewModel(this);
        Packs = new ObservableCollection<QuestionPackViewModel>();
        UserMessage = "Ready";

        _autoSaveTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(10)
        };
        _autoSaveTimer.Tick += async (_, _) => await SaveAsync("Auto Save Successful.");

        ActiveView = ConfigurationViewModel;
    }

    public QuestionPackViewModel? ActivePack
    {
        get => _activePack;
        set
        {
            if (ActiveView != ConfigurationViewModel)
            {
                ShowConfiguration();
            }
            _activePack = value;
            RaisePropertyChanged();
            if (ActivePack != null)
            {
                UserMessage = $"Pack {ActivePack.Name} Selected";
            }
            else
            {
                UserMessage = $"No Pack Selected";
            }
        }
    }
    public QuestionViewModel? ActiveQuestion
    {
        get => _activeQuestion;
        set
        {
            _activeQuestion = value;
            RaisePropertyChanged();
        }
    }
    public ViewModelBase? ActiveView
    {
        get => _activeView;
        set
        {
            if (_activeView is PlayerViewModel player) { player.StopGame(); }
            _activeView = value;
            RaisePropertyChanged();
        }
    }
    public string UserMessage
    {
        get => _userMessage;
        set
        {
            _userMessage = value;
            RaisePropertyChanged();
        }
    }
    public PlayerViewModel? PlayerViewModel { get; set; }
    public ConfigurationViewModel? ConfigurationViewModel { get; }
    public MenuViewModel? MenuViewModel { get; }
    public WindowState WindowState
    {
        get => _windowState;
        set
        {
            if (_windowState != value)
            {
                _windowState = value;
                RaisePropertyChanged();
            }
        }
    }

    public bool CanOpenNewPack(object? arg)
    {
        return true;
    }
    public void OpenNewPack(object? obj)
    {
        NewPackViewModel newPackViewModel = new NewPackViewModel(this);
        _windowService.ShowDialog(newPackViewModel);
    }
    public bool CanOpenImportPack(object? arg)
    {
        return true;
    }
    public void OpenImportPack(object? obj)
    {
        IApiService apiService = new ApiService();
        var ImportPackViewModel = new ImportPackViewModel(this, apiService, _windowService);
        _windowService.ShowDialog(ImportPackViewModel);
    }

    public bool CanOpenPackSettings(object? arg)
    {
        return true;
    }
    public void OpenPackSettings(object? obj)
    {
        if (ActivePack is null) return;

        PackSettingsViewModel PackSettingsViewModel = new PackSettingsViewModel(ActivePack);
        string name = PackSettingsViewModel.Name;
        bool? result = _windowService.ShowDialog(PackSettingsViewModel);

        UserMessage = $"Settings for {name} where uppdated";
    }

    public bool CanCreateNewPack(object? arg)
    {
        return true;
    }
    public void CreateNewPack(object? obj)
    {
        if (obj is QuestionPack questionPack)
        {
            var newPack = new QuestionPackViewModel(questionPack);

            Packs.Add(newPack);
            ActivePack = newPack;

            UserMessage = "New Pack Crated";
        }
    }

    public bool CanShowPlayer(object? arg)
    {
        return ActivePack != null;
    }
    public void ShowPlayer(object? obj)
    {
        PlayerViewModel = new PlayerViewModel(this);

        if (PlayerViewModel != null)
        {
            ActiveView = PlayerViewModel;
            PlayerViewModel.StartGameLoop();
        }
    }
    public bool CanShowConfiguration(object? arg)
    {
        return true;
    }
    public void ShowConfiguration(object? obj)
    {
        if (ConfigurationViewModel is null) return;

        ActiveView = ConfigurationViewModel;
    }
    public void ShowConfiguration()
    {
        ShowConfiguration(null);
    }
    public async Task InitializeAsync()
    {
        var packs = await _dataService.LoadPacksAsync();

        Packs.Clear();
        foreach (var p in packs)
        {
            Packs.Add(new QuestionPackViewModel(p));
        }

        ActivePack = Packs.FirstOrDefault();
        _autoSaveTimer.Start();
    }

    public async Task SaveAsync(string message = "Save Successful.")
    {
        if (Packs.Count == 0)
            return;

        try
        {
            var packsToSave = Packs.Select(p => p.Model).ToList();
            await _dataService.SavePacksAsync(packsToSave);
            UserMessage = message;
        }
        catch (Exception ex)
        {
            string msg = $"Save Failed: {ex.Message}";

            _windowService.ShowMessage(
                msg,
                "Save failed",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            UserMessage = $"Save Failed!";
        }
    }

    public bool CanRemoveQuestion(object? arg)
    {
        return ActiveQuestion != null;
    }
    public void RemoveQuestion(object? obj)
    {
        if (ActiveQuestion is null || ActivePack is null) return;

        ActivePack.Questions.Remove(ActiveQuestion);
        UserMessage = "Question Removed!";
    }

    public bool CanRemoveQuestionPack(object? arg)
    {
        return ActivePack != null;
    }
    public void RemoveQuestionPack(object? obj)
    {
        if (ActivePack is null) return;

        Packs.Remove(ActivePack);
        ActivePack = Packs.FirstOrDefault();

        UserMessage = "Remove Question Pack!";
    }
    public bool CanAddQuestion(object? arg)
    {
        return true;
    }

    public void AddQuestion(object? obj)
    {
        if (ActivePack is null) return;

        ActivePack.Questions.Add(new QuestionViewModel());
        UserMessage = "Question Added";
    }
    public bool CanExit(object? arg)
    {
        return true;
    }
    public void Exit(object? obj)
    {
        _windowService.CloseApplication();
    }
    public void ToggleMaximize()
    {
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }
}
