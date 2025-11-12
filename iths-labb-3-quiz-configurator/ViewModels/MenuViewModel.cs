using iths_labb_3_quiz_configurator.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace iths_labb_3_quiz_configurator.ViewModels;

class MenuViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public MenuViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
        OpenNewPackDialogCommand = new DelegateCommand(_mainWindowViewModel.OpenNewPack, _mainWindowViewModel.CanOpenNewPack);
        OpenImportPackDialogCommand = new DelegateCommand(_mainWindowViewModel.OpenImportPack, _mainWindowViewModel.CanOpenImportPack);
        CahngeActivePackCommand = new DelegateCommand(CahngeActivePack, CanCahngeActivePack);
        ShowPlayerCommand = new DelegateCommand(_mainWindowViewModel.ShowPlayer, _mainWindowViewModel.CanShowPlayer);
        ShowConfigurationCommand = new DelegateCommand(_mainWindowViewModel.ShowConfiguration, _mainWindowViewModel.CanShowConfiguration);
        SaveCommand = new AsyncDelegateCommand(() => _mainWindowViewModel.SaveAsync(), () => true);
        RemoveQuestionCommand = new DelegateCommand(_mainWindowViewModel.RemoveQuestion, _mainWindowViewModel.CanRemoveQuestion);
        AddQuestionCommand = new DelegateCommand(_mainWindowViewModel.AddQuestion, _mainWindowViewModel.CanAddQuestion);
        PackSettingsCommand = new DelegateCommand(_mainWindowViewModel.OpenPackSettings, _mainWindowViewModel.CanOpenPackSettings);
        ExitCommand = new DelegateCommand(_mainWindowViewModel.Exit, _mainWindowViewModel.CanExit);
        ToggleMaximizeCommand = new DelegateCommand(_ => _mainWindowViewModel.ToggleMaximize());
        RemovePackCommand = new DelegateCommand(_mainWindowViewModel.RemoveQuestionPack, _mainWindowViewModel.CanRemoveQuestionPack);

        _mainWindowViewModel.PropertyChanged += MainOnPropertyChanged;
    }
    public DelegateCommand OpenNewPackDialogCommand { get; }
    public DelegateCommand OpenImportPackDialogCommand { get; }
    public DelegateCommand ShowPlayerCommand { get; }
    public DelegateCommand ShowConfigurationCommand { get; }
    public DelegateCommand CahngeActivePackCommand { get; }
    public AsyncDelegateCommand SaveCommand { get; }
    public ObservableCollection<QuestionPackViewModel> Packs => _mainWindowViewModel.Packs;
    public DelegateCommand RemoveQuestionCommand { get; }
    public DelegateCommand AddQuestionCommand { get; }
    public DelegateCommand PackSettingsCommand { get; }
    public DelegateCommand ExitCommand { get; }
    public DelegateCommand ToggleMaximizeCommand { get; }
    public DelegateCommand RemovePackCommand { get; }


    public QuestionPackViewModel ActivePack
    {
        get => _mainWindowViewModel.ActivePack;
        set => _mainWindowViewModel.ActivePack = value;
    }
    public QuestionViewModel? ActiveQuestion
    {
        get => _mainWindowViewModel.ActiveQuestion;
        set
        {
            _mainWindowViewModel.ActiveQuestion = value;
            RaisePropertyChanged();
            RemoveQuestionCommand.RaiseCanExecuteChanged();
        }
    }

    public bool CanCahngeActivePack(object? arg)
    {
        return true;
    }
    public void CahngeActivePack(object? obj)
    {
        if (obj is QuestionPackViewModel pack) ActivePack = pack;
    }
    private void MainOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainWindowViewModel.ActiveQuestion))
        {
            RemoveQuestionCommand.RaiseCanExecuteChanged();
        }

        if (e.PropertyName == nameof(MainWindowViewModel.ActivePack))
        {
            RemoveQuestionCommand.RaiseCanExecuteChanged();
            ShowPlayerCommand.RaiseCanExecuteChanged();
            RemovePackCommand.RaiseCanExecuteChanged();
        }
    }
}
