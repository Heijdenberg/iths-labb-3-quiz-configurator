
namespace iths_labb_3_quiz_configurator.Services;

interface IRequestClose
{
    event EventHandler<RequestCloseEventArgs>? RequestClose;
}
