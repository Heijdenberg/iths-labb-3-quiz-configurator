using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iths_labb_3_quiz_configurator.ViewModels;

class GameOverViewModel: ViewModelBase
{
    private int _numberOfQuestions;
    private int _numberOfCorrectAnswers;
    public GameOverViewModel(int numberOfQuestions, int numberOfCorrectAnswers)
    {
        _numberOfQuestions = numberOfQuestions;
        _numberOfCorrectAnswers = numberOfCorrectAnswers;
    }

    public string Result { get => $"You got {_numberOfCorrectAnswers} out of {_numberOfQuestions} answers correct!"; }
}
