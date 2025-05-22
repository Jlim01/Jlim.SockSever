using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jlim.SockSever.Interfaces
{
    public interface IMainViewModel
    {
        ICommand SaveCommand { get; }
    }
}
