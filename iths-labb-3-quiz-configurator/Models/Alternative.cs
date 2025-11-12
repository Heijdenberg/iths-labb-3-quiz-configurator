
using System.Windows.Media;

namespace iths_labb_3_quiz_configurator.Models;

class Alternative
{
    private readonly string _alternative;
    private Brush _bgColor;

    public Alternative(string alternative)
    {
        _alternative = alternative;
        _bgColor = Brushes.Transparent;
    }

    public string Alt
    {
        get => _alternative;
    }

    public Brush Bg
    {
        get => _bgColor;
        set => _bgColor = value;
    }
}
