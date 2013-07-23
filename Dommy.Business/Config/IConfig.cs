using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Config
{
    public interface IConfig
    {
        void Create(IKernel kernel);
    }
}
