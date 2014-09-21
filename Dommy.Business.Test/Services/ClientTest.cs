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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business.Services;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test client WCF proxy.
    /// </summary>
    [TestClass]
    public class ClientTest
    {
        [ServiceContract]
        private interface IStubService
        {
            [OperationContract]
            void FakeService();
        }
    }
}
