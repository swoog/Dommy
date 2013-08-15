using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business
{
    public interface IListener
    {
        void Init(Engine engine);
        void Start();
        void Stop();
    }
}
