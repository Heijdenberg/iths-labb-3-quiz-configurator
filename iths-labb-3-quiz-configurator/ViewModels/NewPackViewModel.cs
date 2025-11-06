using iths_labb_3_quiz_configurator.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace iths_labb_3_quiz_configurator.ViewModels;

class NewPackViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    private string _title;
    private string _newPackTitle;
    private string _selectedDifficulty;
    private int _timeLimit;

    public NewPackViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        Title = "New Question Pack";
        NewPackTitle = "< PackName >";
        SelectedDifficulty = "Medium";
        TimeLimit = 5;
        CreateNewPackCommand = new DelegateCommand(_mainWindowViewModel.CreateNewPack, _mainWindowViewModel.CanCreateNewPack);
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
    public string SelectedDifficulty
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
    public DelegateCommand CreateNewPackCommand { get; }
    public ObservableCollection<string> Difficulties { get; } =
    new ObservableCollection<string> { "Easy", "Medium", "Hard" };
}
