using iths_labb_3_quiz_configurator.Command;
using iths_labb_3_quiz_configurator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iths_labb_3_quiz_configurator.ViewModels;

class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private QuestionViewModel? _activeQuestion;

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        RemoveQuestionCommand = new DelegateCommand(_mainWindowViewModel.RemoveQuestion, _mainWindowViewModel.CanRemoveQuestion);
        AddQuestionCommand = new DelegateCommand(_mainWindowViewModel.AddQuestion, _mainWindowViewModel.CanAddQuestion);
        PackSettingsCommand = new DelegateCommand(_mainWindowViewModel.OpenPackSettings, _mainWindowViewModel.CanOpenPackSettings);

        _mainWindowViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(MainWindowViewModel.ActivePack))
            {
                RaisePropertyChanged(nameof(MainWindowViewModel.ActivePack));
                ActiveQuestion = null;
                RemoveQuestionCommand.RaiseCanExecuteChanged();
            }
        };
    }

    public QuestionPackViewModel? ActivePack => _mainWindowViewModel.ActivePack;
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
    public DelegateCommand RemoveQuestionCommand { get; }
    public DelegateCommand AddQuestionCommand { get; }
    public DelegateCommand PackSettingsCommand { get; }
}
