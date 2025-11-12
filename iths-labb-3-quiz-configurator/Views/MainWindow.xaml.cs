using iths_labb_3_quiz_configurator.Services;
using iths_labb_3_quiz_configurator.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iths_labb_3_quiz_configurator.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool _isClosingAfterSave;

    public MainWindow()
    {
        InitializeComponent();
        _isClosingAfterSave = false;
        var windowService = new WindowService();
        var jsonDataService = new JsonDataService();
        var viewModel = new MainWindowViewModel(windowService, jsonDataService);
        DataContext = viewModel;
        Loaded += async (_, _) => await viewModel.InitializeAsync();

        Closing += async (_, e) =>
        {
            if (_isClosingAfterSave)
                return;

            if (DataContext is MainWindowViewModel vm)
            {
                e.Cancel = true;

                try
                {
                    await vm.SaveAsync();
                    _isClosingAfterSave = true;
                    Close();
                }
                catch (Exception ex)
                {
                    string msg = $"Could not save before exit.\n\nDetails: {ex.Message}\n\nClose without saving?";

                    var result = windowService.Confirm(msg, "Save failed");

                    if (result)
                    {
                        _isClosingAfterSave = true;
                        Close();
                    }
                    else
                    {
                        vm.UserMessage = "Close cancelled. Please try saving again.";
                    }
                }
            }
        };
    }
}