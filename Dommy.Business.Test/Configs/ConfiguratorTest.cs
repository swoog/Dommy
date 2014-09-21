//-----------------------------------------------------------------------
// <copyright file="Configurator.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Test.Configs
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Dommy.Business.Configs;

    [TestClass]
    public class ConfiguratorTest
    {
        [TestMethod]
        public void WithTest()
        {
            var configInstance = new ConfigTest();
            
            new Configurator<ConfigTest>(configInstance)
                .With(c => c.MyProperty, "MyValue");

            Assert.AreEqual(configInstance.MyProperty, "MyValue");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithTest_ArgumentException()
        {
            var configInstance = new ConfigTest();

            new Configurator<ConfigTest>(configInstance)
                .With(c => c.FakeMethod(), "MyValue");
        }


        private class ConfigTest : IConfig
        {
            public string MyProperty { get; set; }

            public void Create(Ninject.IKernel kernel)
            {
            }

            public string FakeMethod()
            {
                return this.MyProperty;
            }
        }
    }
}
