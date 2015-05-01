//-----------------------------------------------------------------------
// <copyright file="ClientFactoryTest.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Test.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ClientFactoryTest
    {

        [ServiceContract]
        private interface IStubService
        {
            [OperationContract]
            void FakeService();
        }

        [TestMethod]
        public void CreateTest()
        {
            using (var c = new ClientFactory<IStubService>().Create())
            {
                Assert.IsNotNull(c.Channel);
            }
        }
    }
}
