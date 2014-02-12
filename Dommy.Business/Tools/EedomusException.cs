using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Tools
{
    public class EedomusException : Exception
    {
        public EedomusException(string message)
            : base(message)
        {

        }
    }
}
