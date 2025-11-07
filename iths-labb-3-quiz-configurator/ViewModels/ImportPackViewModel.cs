using iths_labb_3_quiz_configurator.Command;
using iths_labb_3_quiz_configurator.Models;
using iths_labb_3_quiz_configurator.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace iths_labb_3_quiz_configurator.ViewModels;

class ImportPackViewModel : ViewModelBase, IRequestClose
{
    public event EventHandler<RequestCloseEventArgs> RequestClose;
    private readonly MainWindowViewModel _mainWindowViewModel;

    public static readonly HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://opentdb.com/")
    };
    private int _noOfQuestions;
    private Difficulty _selectedDifficulty;
    public int cat;
    public string categoryUrl;
    private ObservableCollection<Category> _categories;
    private Category _selectedCategorie;
    private ApiService _apiService;

    public ImportPackViewModel(MainWindowViewModel mainWindowViewModel, ApiService apiService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        NoOfQuestions = 1;
        cat = 9;
        _apiService = apiService;
        categoryUrl = "api_category.php";
        GetCategories();
        GetPackCommand = new AsyncDelegateCommand(GetPack, CanGetPack);
        OnCancelCommand = new DelegateCommand(OnCancel, CanOnCancel);
    }

    public Array Difficultys => Enum.GetValues(typeof(Difficulty));
    public Difficulty SelectedDifficulty
    {
        get => _selectedDifficulty;
        set 
        {
            _selectedDifficulty = value;
            RaisePropertyChanged();
            GetPackCommand.RaiseCanExecuteChanged();
        }
    }

    public int NoOfQuestions
    { 
        get => _noOfQuestions;
        set
        {
            _noOfQuestions = value;
            RaisePropertyChanged();
        }
    }

    public Category SelectedCategorie
    {
        get => _selectedCategorie;
        set
        {
            _selectedCategorie = value;
            RaisePropertyChanged();
            GetPackCommand.RaiseCanExecuteChanged();
        }
    }

    public ObservableCollection<Category> Categories
    {
        get => _categories;
        set
        {
            _categories = value;
            RaisePropertyChanged();
            GetPackCommand.RaiseCanExecuteChanged();
        }
    }

    public AsyncDelegateCommand GetPackCommand { get; }
    public DelegateCommand OnCancelCommand { get; }

    public async Task GetCategories()
    {
        Categories = await _apiService.GetAsync(sharedClient, categoryUrl, _apiService.CategoriesFromJson);
    }

    public bool CanGetPack()
    {
        bool canGetPack = false;

        if(SelectedCategorie != null) canGetPack = true;

        return canGetPack;
    }
    public async Task GetPack()
    {
        string diff = SelectedDifficulty.ToString().ToLowerInvariant();
        string questionsUrl = $"api.php?amount={_noOfQuestions}&category={SelectedCategorie.Id}&difficulty={diff}&type=multiple";
        ObservableCollection<Question> questions = await _apiService.GetAsync(sharedClient, questionsUrl, _apiService.QuestionsFromJson);

        QuestionPack newPack = new(SelectedCategorie.Name, SelectedDifficulty, 30);

        foreach (Question q in questions)
        {
            newPack.Questions.Add(q);
        }

        _mainWindowViewModel.CreateNewPack(newPack);
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
