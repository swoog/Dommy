using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Tools
{
    public interface ITVHelper
    {
        void Command(TVCommand tvCommand);

        void Canal(int canalNumber);
    }
}
