using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Tools
{
    public interface ITvHelper
    {
        void Command(TvCommand command);

        void Canal(int canal);
    }
}
