
namespace Dommy.Business.Test
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Ninject;
    using Ninject.MockingKernel.Moq;

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

        protected virtual MoqMockingKernel CreateKernel()
        {
            var kernel = new MoqMockingKernel();
            kernel.Load("Dommy.*.dll");

            return kernel;
        }
    }
}
