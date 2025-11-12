
namespace iths_labb_3_quiz_configurator.Models;

public class Question
{
    public Question()
    {
        IncorrectAnswers = new string[3];
    }

    public Question(string query, string correctAnswer,
        string incorrectAnswer1, string incorrectAnswer2, string incorrectAnswer3)
    {
        Query = query;
        CorrectAnswer = correctAnswer;
        IncorrectAnswers = new[] { incorrectAnswer1, incorrectAnswer2, incorrectAnswer3 };
    }

    public string Query { get; set; }

    public string CorrectAnswer { get; set; }

    public string[] IncorrectAnswers { get; set; }
}
