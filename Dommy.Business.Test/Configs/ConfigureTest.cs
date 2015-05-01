
namespace Dommy.Business.Test.Configs
{
    using System;

    using Dommy.Business.Configs;

    using Xunit;

    public class ConfigureTest
        : BaseTest
    {
        [Fact]
        public void LoadSaveConfigTest()
        {
            Configure.InitKernel(this.CreateKernel());
            Configure.Config<ConfigTest>();

            Configure.LoadConfig("config.xml");

            Configure.SaveConfig();
        }

        public class ConfigTest : IConfig
        {
            public void Create(Ninject.IKernel kernel)
            {
                throw new NotImplementedException();
            }
        }
    }
}
