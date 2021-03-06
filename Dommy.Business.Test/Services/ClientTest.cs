﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientTest.cs" company="TrollCorp">
//   Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
// <summary>
//   Defines the ClientTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dommy.Business.Test.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business.Services;


    using Xunit;

    public class ClientTest
    {
        [ServiceContract]
        private interface IStubService
        {
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
