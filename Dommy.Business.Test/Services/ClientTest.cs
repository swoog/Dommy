// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientTest.cs" company="TrollCorp">
//   Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
// <summary>
//   Defines the ClientTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dommy.Business.Test.Services
{
    using System.ServiceModel;

    using Dommy.Business.Services;


    using Xunit;

    public class ClientTest
    {
        /// <summary>
        /// Stub service interface.
        /// </summary>
        [ServiceContract]
        private interface IStubService
        {
            /// <summary>
            /// Fake operation contract.
            /// </summary>
            [OperationContract]
            void FakeService();
        }

        /// <summary>
        /// Create test.
        /// </summary>
        [Fact]
        public void CreateTest()
        {
            using (var c = Client<IStubService>.Create())
            {
                Assert.NotNull(c.Channel);
            }
        }
    }
}
