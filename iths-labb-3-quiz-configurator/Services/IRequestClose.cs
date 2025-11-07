using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iths_labb_3_quiz_configurator.Services;

interface IRequestClose
{
    event EventHandler<RequestCloseEventArgs> RequestClose;
}
