using System.Collections.ObjectModel;


namespace iths_labb_3_quiz_configurator.Models;

public class TriviaResponse
{
    public ObservableCollection<Category> Trivia_Categories { get; set; } = new();
}
