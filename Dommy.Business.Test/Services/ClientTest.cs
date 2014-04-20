
namespace Dommy.Business.Test.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.ServiceModel;

    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public void CreateTest()
        {
            using (var c = Client<IStubService>.Create())
            {
                Assert.IsNotNull(c.Channel);
            }
        }

        [ServiceContract]
        private interface IStubService
        {
            [OperationContract]
            void FakeService();
        }
    }
}
