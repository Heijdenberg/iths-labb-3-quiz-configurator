using System.Windows;

namespace iths_labb_3_quiz_configurator.Services;

public interface IWindowServices
{
    bool? ShowDialog(object viewModel);
    void ShowWindow(object viewModel);
    void CloseApplication();
    void ShowMessage(string message, string title, MessageBoxButton buttons, MessageBoxImage icon);
}
