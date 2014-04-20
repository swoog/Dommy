namespace Dommy.Business.Test.WebHost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business.WebHost;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ninject;
    using Ninject.Parameters;

    [TestClass]
    public class WebServerHostTest
        : BaseTest
    {
        [TestMethod]
        public void StartTest()
        {
            var kernel = this.CreateKernel();

            var server = kernel.Get<WebServerHost>(new ConstructorArgument("port", 5555));

            server.Start();

            server.Stop();
        }
    }
}
