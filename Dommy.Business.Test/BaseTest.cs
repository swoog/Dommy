using Ninject;
using Ninject.MockingKernel.Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Test
{
    public class BaseTest
    {
        static BaseTest()
        {
            var listener = new HttpListener();
            listener.Start();
            listener.Prefixes.Add(String.Format("http://+:4444/"));

            Task.Run(() =>
            {
                while (true)
                {
                    var context = listener.GetContext();

                }
            });
        }

        public void SetupTest()
        {

        }

        protected MoqMockingKernel CreateKernel()
        {
            var kernel = new MoqMockingKernel();
            kernel.Load("Dommy.*.dll");

            return kernel;
        }
    }
}
