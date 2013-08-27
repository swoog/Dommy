using Ninject;
using Ninject.MockingKernel.Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Test
{
    public class BaseTest
    {
        protected MoqMockingKernel CreateKernel()
        {
            var kernel = new MoqMockingKernel();
            kernel.Load("Dommy.*.dll");

            return kernel;
        }
    }
}
