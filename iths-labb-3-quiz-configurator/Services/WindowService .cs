using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using iths_labb_3_quiz_configurator.Views;

namespace iths_labb_3_quiz_configurator.Services;

class WindowService : IWindowServices
{
    public bool? ShowDialog(object viewModel)
    {
        var window = new Dialog
        {
            DataContext = viewModel,
            Owner = Application.Current.MainWindow
        };

        if (viewModel is IRequestClose requestClose)
        {
            EventHandler<RequestCloseEventArgs>? handler = null;
            handler = (sender, e) =>
            {
                requestClose.RequestClose -= handler;
                window.DialogResult = e.DialogResult;
                window.Close();
            };
            requestClose.RequestClose += handler;
        }

        return window.ShowDialog();
    }

    public void ShowWindow(object viewModel)
    {
        throw new NotImplementedException();
    }
}
