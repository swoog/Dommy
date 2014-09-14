//-----------------------------------------------------------------------
// <copyright file="Configurator.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Test.Configs
{
    using System;

    using Dommy.Business.Configs;

    using Xunit;


    public class ConfiguratorTest
    {
        [Fact]
        public void WithTest()
        {
            var configInstance = new ConfigTest();
            
            new Configurator<ConfigTest>(configInstance)
                .With(c => c.MyProperty, "MyValue");

            Assert.Equal(configInstance.MyProperty, "MyValue");
        }

        [Fact]
        public void WithTest_ArgumentException()
        {
            var configInstance = new ConfigTest();

            Assert.Throws<ArgumentException>(
                () =>
                    {
                        new Configurator<ConfigTest>(configInstance)
                        .With(c => c.FakeMethod(), "MyValue");
                    });
        }


        private class ConfigTest : IConfig
        {
            public string MyProperty { get; set; }

            public void Create(Ninject.IKernel kernel)
            {
                throw new NotImplementedException();
            }

            public string FakeMethod()
            {
                throw new NotImplementedException();
            }
        }
    }
}
