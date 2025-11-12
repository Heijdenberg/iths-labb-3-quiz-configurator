using iths_labb_3_quiz_configurator.Command;
using iths_labb_3_quiz_configurator.Models;
using iths_labb_3_quiz_configurator.Services;
using System.Collections.ObjectModel;

namespace iths_labb_3_quiz_configurator.ViewModels;

class NewPackViewModel : ViewModelBase, IRequestClose
{
    public event EventHandler<RequestCloseEventArgs>? RequestClose;
    private readonly MainWindowViewModel _mainWindowViewModel;

    private string _title = "";
    private string _newPackTitle = "";
    private Difficulty _selectedDifficulty;
    private int _timeLimit;

    public NewPackViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
        Title = "New Question Pack";
        NewPackTitle = "< PackName >";
        SelectedDifficulty = Difficulty.Medium;
        TimeLimit = 5;
        OnCreateCommand = new DelegateCommand(OnCreate, CanOnCreate);
        OnCancelCommand = new DelegateCommand(OnCancel, CanOnCancel);
    }
    public string Title
    {
        get => _title;
        set => _title = value;
    }
    public string NewPackTitle
    {
        get => _newPackTitle;
        set => _newPackTitle = value;
    }
    public Difficulty SelectedDifficulty
    {
        get => _selectedDifficulty;
        set => _selectedDifficulty = value;
    }
    public int TimeLimit
    {
        get => _timeLimit;
        set
        {
            _timeLimit = value;
            RaisePropertyChanged();
        }
    }

    public QuestionPack NewPack => new QuestionPack(NewPackTitle, SelectedDifficulty, TimeLimit);
    public DelegateCommand OnCreateCommand { get; }
    public DelegateCommand OnCancelCommand { get; }
    public ObservableCollection<string> Difficulties { get; } =
    new ObservableCollection<string> { "Easy", "Medium", "Hard" };

    private bool CanOnCreate(object? arg)
    {
        return true;
    }
    private void OnCreate(object? obj)
    {
        _mainWindowViewModel.CreateNewPack(NewPack);
        RequestClose?.Invoke(this, new RequestCloseEventArgs(true));
    }
    private bool CanOnCancel(object? arg)
    {
        return true;
    }
    private void OnCancel(object? obj)
    {
        RequestClose?.Invoke(this, new RequestCloseEventArgs(false));
    }

}
