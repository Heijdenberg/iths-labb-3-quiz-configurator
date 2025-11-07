using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iths_labb_3_quiz_configurator.Services;

class RequestCloseEventArgs : EventArgs
{
    public bool? DialogResult { get; }
    public RequestCloseEventArgs(bool? dialogResult)
    {
        DialogResult = dialogResult;
    }
}
