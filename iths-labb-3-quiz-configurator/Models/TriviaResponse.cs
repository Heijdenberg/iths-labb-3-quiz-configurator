using iths_labb_3_quiz_configurator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iths_labb_3_quiz_configurator.Models;

public class TriviaResponse
{
    public ObservableCollection<Category> Trivia_Categories { get; set; } = new();
}
