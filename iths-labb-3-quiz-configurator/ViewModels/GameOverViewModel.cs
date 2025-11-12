using iths_labb_3_quiz_configurator.Command;

namespace iths_labb_3_quiz_configurator.ViewModels;

class GameOverViewModel: ViewModelBase
{
    private readonly MainWindowViewModel _mainWindowViewModel;

    private int _numberOfQuestions;
    private int _numberOfCorrectAnswers;
    public GameOverViewModel(int numberOfQuestions, int numberOfCorrectAnswers, MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
        _numberOfQuestions = numberOfQuestions;
        _numberOfCorrectAnswers = numberOfCorrectAnswers;
        RestartCommand = new DelegateCommand(Restart,CanRestart);
    }

    public string Result { get => $"You got {_numberOfCorrectAnswers} out of {_numberOfQuestions} answers correct!"; }

    public DelegateCommand RestartCommand { get; }
    private bool CanRestart(object? arg)
    {
        return _mainWindowViewModel.ActivePack != null;
    }
    private void Restart(object? obj)
    {
        _mainWindowViewModel.ShowPlayer(obj);
    }
}
