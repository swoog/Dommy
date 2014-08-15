﻿//-----------------------------------------------------------------------
// <copyright file="ClientTest.cs" company="TrollCorp">
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
