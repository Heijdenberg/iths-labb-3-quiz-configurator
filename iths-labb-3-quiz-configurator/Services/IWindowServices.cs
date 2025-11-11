using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iths_labb_3_quiz_configurator.Services;

public interface IWindowServices
{
    bool? ShowDialog(object viewModel);
    void ShowWindow(object viewModel);
    void CloseApplication();
    void ShowMessage(string message, string title, MessageBoxButton buttons, MessageBoxImage icon);
}
