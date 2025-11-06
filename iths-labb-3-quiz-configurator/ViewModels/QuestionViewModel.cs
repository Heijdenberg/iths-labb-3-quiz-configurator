using iths_labb_3_quiz_configurator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iths_labb_3_quiz_configurator.ViewModels;

class QuestionViewModel : ViewModelBase
{
    private Question _question;


    public QuestionViewModel()
    {
        _question = new Question("New Question", "", "", "", "");
    }
    public QuestionViewModel(Question question)
    {
        _question = question ?? throw new ArgumentNullException(nameof(question));
    }

    public QuestionViewModel(string query, string correctAnswer,
    string incorrectAnswer1, string incorrectAnswer2, string incorrectAnswer3)
    {
        _question = new(query, correctAnswer,
    incorrectAnswer1, incorrectAnswer2, incorrectAnswer3);
    }

    public Question Question => _question;
    
    public string Query
    {
        get => _question.Query;
        set
        {
            if (_question.Query != value)
            {
                _question.Query = value;
                RaisePropertyChanged();
            }
        }
    }
    public string CorrectAnswer
    { 
        get => _question.CorrectAnswer; 
        set
        {
            _question.CorrectAnswer = value;
            RaisePropertyChanged();
        }
    }
    public string IncorrectAnswer1
    {
        get => _question.IncorrectAnswers[0];
        set
        {
            _question.IncorrectAnswers[0] = value;
            RaisePropertyChanged();
        }
    }
    public string IncorrectAnswer2
    {
        get => _question.IncorrectAnswers[1];
        set
        {
            _question.IncorrectAnswers[1] = value;
            RaisePropertyChanged();
        }
    }
    public string IncorrectAnswer3
    {
        get => _question.IncorrectAnswers[2];
        set
        {
            _question.IncorrectAnswers[2] = value;
            RaisePropertyChanged();
        }
    }
    public override string ToString()
    {
        return $"{_question.Query}";
    }
}
