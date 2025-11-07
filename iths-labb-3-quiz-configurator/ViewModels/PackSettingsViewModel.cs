using iths_labb_3_quiz_configurator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iths_labb_3_quiz_configurator.ViewModels;


class PackSettingsViewModel : ViewModelBase
{
    private QuestionPackViewModel _pack;

    public PackSettingsViewModel(QuestionPackViewModel pack)
    {
        _pack = pack;
    }

    public string Name
    {
        get => _pack.Name;
        set
        {
            _pack.Name = value;
        }
    }

    public Difficulty Difficulty
    {
        get => _pack.Difficulty; set
        {
            _pack.Difficulty = value;
        }
    }

    public Array Difficulties { get; } = Enum.GetValues(typeof(Difficulty));
    public int TimeLimit
    {
        get => _pack.TimeLimitInSeconds;
        set
        {
            _pack.TimeLimitInSeconds = value;
            RaisePropertyChanged();
        }
    }
}
