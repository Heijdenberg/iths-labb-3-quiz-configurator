using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iths_labb_3_quiz_configurator.Models;

public class Result
{
    public string Question { get; set; }
    public string Correct_answer { get; set; }
    public List<string> Incorrect_answers { get; set; }
}
