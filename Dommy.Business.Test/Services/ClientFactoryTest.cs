//-----------------------------------------------------------------------
// <copyright file="ClientFactoryTest.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Test.Services
{
    using System.ServiceModel;

    using Dommy.Business.Services;

    using Xunit;

    public class ClientFactoryTest
    {

        [ServiceContract]
        private interface IStubService
        {
            [OperationContract]
            void FakeService();
        }

        [Fact]
        public void CreateTest()
        {
            using (var c = new ClientFactory<IStubService>().Create())
            {
                Assert.NotNull(c.Channel);
            }
        }
    }
}
