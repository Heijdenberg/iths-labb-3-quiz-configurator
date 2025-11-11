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
using System.Windows.Interop;

namespace iths_labb_3_quiz_configurator.ViewModels;

class ImportPackViewModel : ViewModelBase, IRequestClose
{
    public event EventHandler<RequestCloseEventArgs> RequestClose;
    private readonly MainWindowViewModel _mainWindowViewModel;
    private readonly IApiService _apiService;
    private readonly IWindowServices _windowService;
    public static readonly HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://opentdb.com/")
    };

    private int _noOfQuestions;
    private Difficulty _selectedDifficulty;
    public string categoryUrl;
    private ObservableCollection<Category> _categories;
    private Category _selectedCategorie;

    public ImportPackViewModel(MainWindowViewModel mainWindowViewModel, IApiService apiService, IWindowServices windowService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _apiService = apiService;
        _windowService = windowService;

        NoOfQuestions = 1;
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
        try
        {
            Categories = await _apiService.GetAsync(sharedClient, categoryUrl, _apiService.CategoriesFromJson);

            if (Categories == null || Categories.Count == 0)
            {
                string msg = "Could not load any categories from the trivia service.";

                _windowService.ShowMessage(
                    msg,
                    "Load categories failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                _mainWindowViewModel.UserMessage = "Failed to load categories: API returned no categories.";
                RequestClose?.Invoke(this, new RequestCloseEventArgs(false));
            }
        }
        catch (HttpRequestException)
        {
            string msg = "Could not reach the trivia service.\nPlease check your internet connection and try again.";

            _windowService.ShowMessage(
                msg,
                "Load categories failed",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            _mainWindowViewModel.UserMessage = "Failed to load categories: network error.";
            RequestClose?.Invoke(this, new RequestCloseEventArgs(false));
        }
        catch (Exception)
        {
            string msg = "An unexpected error occurred while loading categories.";

            _windowService.ShowMessage(
                msg,
                "Load categories failed",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            _mainWindowViewModel.UserMessage = "Failed to load categories: unexpected error.";
            RequestClose?.Invoke(this, new RequestCloseEventArgs(false));
        }
    }
    public bool CanGetPack()
    {
        bool canGetPack = false;

        if(SelectedCategorie != null) canGetPack = true;

        return canGetPack;
    }
    public async Task GetPack()
    {
        try
        {
            string diff = SelectedDifficulty.ToString().ToLowerInvariant();
            string questionsUrl = $"api.php?amount={_noOfQuestions}&category={SelectedCategorie.Id}&difficulty={diff}&type=multiple";
            ObservableCollection<Question> questions = await _apiService.GetAsync(sharedClient, questionsUrl, _apiService.QuestionsFromJson);

            

            if (questions == null || questions.Count == 0)
            {
                string msg = "Import failed: the trivia service returned no questions for the selected settings.";
                
                _windowService.ShowMessage(
                    msg,
                    "Import failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                _mainWindowViewModel.UserMessage = "Import failed: no questions returned from API.";
                RequestClose?.Invoke(this, new RequestCloseEventArgs(false));
                return;
            }

            QuestionPack newPack = new(SelectedCategorie.Name, SelectedDifficulty, 30);

            foreach (Question q in questions)
            {
                newPack.Questions.Add(q);
            }

            _mainWindowViewModel.CreateNewPack(newPack);

            _mainWindowViewModel.UserMessage = $"Import complete: New pack {newPack.Name}, created with {questions.Count} questions.";

            RequestClose?.Invoke(this, new RequestCloseEventArgs(true));
        }
        catch (HttpRequestException)
        {
            string msg = "Import failed: could not reach the trivia service.\nPlease check your internet connection and try again.";

            _windowService.ShowMessage(
                msg,
                "Import failed",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            _mainWindowViewModel.UserMessage = "Import failed: network error.";
            RequestClose?.Invoke(this, new RequestCloseEventArgs(false));
        }
        catch (Exception)
        {
            string msg = "Import failed due to an unexpected error.";

            _windowService.ShowMessage(
                msg,
                "Import failed",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            _mainWindowViewModel.UserMessage = "Import failed: unexpected error.";
            RequestClose?.Invoke(this, new RequestCloseEventArgs(false));
        }
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
