using iths_labb_3_quiz_configurator.Command;
using iths_labb_3_quiz_configurator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace iths_labb_3_quiz_configurator.ViewModels;

class PlayerViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    private readonly QuestionPackViewModel _activePack;
    private static Random _rng;
    private DispatcherTimer _timer;
    private int _currentTime;
    int _numberOfQuestions;
    int _numberOfCorrectAnswers;
    int _index;
    private string _query;
    private Alternative _correctAlt;
    private Alternative _alt1;
    private Alternative _alt2;
    private Alternative _alt3;
    private Alternative _alt4;
    private string _questionOfTotal;

    public PlayerViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _activePack = _mainWindowViewModel.ActivePack;
        _numberOfQuestions = _activePack.Questions.Count;
        _rng = new Random();
        _currentTime = _activePack.TimeLimitInSeconds;
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += OnTimerTick;
        _index = 0;
        _numberOfCorrectAnswers = 0;

        MakeAGuessCommand = new DelegateCommand(MakeAGuess, CanMakeAGuess);
    }

    public int CurrentTime
    {
        get => _currentTime;
        set
        {
            _currentTime = value;
            RaisePropertyChanged();
        }
    }
    public string Query
    {
        get => _query;
        set
        {
            _query = value;
            RaisePropertyChanged();
        }
    }
    public Alternative Alt1
    {
        get => _alt1;
        set
        {
            _alt1 = value;
            RaisePropertyChanged();
        }

    }
    public Alternative Alt2
    {
        get => _alt2;
        set
        {
            _alt2 = value;
            RaisePropertyChanged();
        }
    }
    public Alternative Alt3
    {
        get => _alt3;
        set
        {
            _alt3 = value;
            RaisePropertyChanged();
        }
    }
    public Alternative Alt4
    {
        get => _alt4;
        set
        {
            _alt4 = value;
            RaisePropertyChanged();
        }
    }

    public string QuestionOfTotal
    {
        get => _questionOfTotal;
        set
        {
            _questionOfTotal = value;
            RaisePropertyChanged();
        }
    }

    public DelegateCommand MakeAGuessCommand { get; set; }

    public void StartGameLoop()
    {
        Shuffle(_activePack.Questions);
        GameLoop();
    }

    private void GameLoop()
    {
        int TimeLimit = _activePack.TimeLimitInSeconds;
        CurrentQuestion();
        _timer.Start();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        if (CurrentTime > 0)
        {
            CurrentTime--;
        }

        if (CurrentTime == 0)
        {
            NextQuestion();
        }
    }

    private bool CanMakeAGuess(object? args)
    {
        return true;
    }

    private async void MakeAGuess(object? obj)
    {
        if (obj is Alternative alt)
        {
            Brush correctColor = Brushes.Green;
            Brush wrongColor = Brushes.Red;
            if (alt == _correctAlt)
            {
                _numberOfCorrectAnswers++;

                if (Alt1 == _correctAlt) Alt1.Bg = correctColor;
                if (Alt2 == _correctAlt) Alt2.Bg = correctColor;
                if (Alt3 == _correctAlt) Alt3.Bg = correctColor;
                if (Alt4 == _correctAlt) Alt4.Bg = correctColor;
            }
            else
            {
                if (Alt1 == alt) Alt1.Bg = wrongColor;
                if (Alt2 == alt) Alt2.Bg = wrongColor;
                if (Alt3 == alt) Alt3.Bg = wrongColor;
                if (Alt4 == alt) Alt4.Bg = wrongColor;

                var correctAnswer = _activePack.Questions[_index].CorrectAnswer;
                if (Alt1 == _correctAlt) Alt1.Bg = correctColor;
                if (Alt2 == _correctAlt) Alt2.Bg = correctColor;
                if (Alt3 == _correctAlt) Alt3.Bg = correctColor;
                if (Alt4 == _correctAlt) Alt4.Bg = correctColor;
            }

            RaisePropertyChanged(nameof(Alt1));
            RaisePropertyChanged(nameof(Alt2));
            RaisePropertyChanged(nameof(Alt3));
            RaisePropertyChanged(nameof(Alt4));
        }

        await Task.Delay(1500);

        NextQuestion();
    }

    private void NextQuestion()
    {
        _timer.Stop();
        _index++;
        if (_index < _numberOfQuestions)
        {
            _currentTime = _activePack.TimeLimitInSeconds;
            CurrentQuestion();
            _timer.Start();
        }
        else
        {
            GameOver();
        }
    }

    private void CurrentQuestion()
    {
        QuestionViewModel q = _activePack.Questions[_index];

        QuestionOfTotal = $"Question {_index + 1} of {_numberOfQuestions}";

        string query = q.Query;
        ObservableCollection<Alternative> alternatives = new();
        _correctAlt = new Alternative(q.CorrectAnswer);
        alternatives.Add(_correctAlt);
        alternatives.Add(new Alternative(q.IncorrectAnswer1));
        alternatives.Add(new Alternative(q.IncorrectAnswer2));
        alternatives.Add(new Alternative(q.IncorrectAnswer3));
        Shuffle(alternatives);

        Query = query;
        Alt1 = alternatives[0];
        Alt2 = alternatives[1];
        Alt3 = alternatives[2];
        Alt4 = alternatives[3];
        RaisePropertyChanged();
    }

    public void Shuffle<T>(ObservableCollection<T> collection)
    {
        var shuffled = collection.OrderBy(_ => _rng.Next()).ToList();
        collection.Clear();
        foreach (var item in shuffled)
            collection.Add(item);
    }
    private void GameOver()
    {
        ViewModelBase gameOverView = new GameOverViewModel(_numberOfQuestions,
        _numberOfCorrectAnswers, _mainWindowViewModel);

        _mainWindowViewModel.ActiveView = gameOverView;
    }

    public void StopGame()
    {
        _timer.Stop();
        _timer.Tick -= OnTimerTick;
    }
}
