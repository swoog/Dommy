namespace Dommy.Business.Test.WebHost
{
    using Dommy.Business.WebHost;

    using Ninject;
    using Ninject.Parameters;

    using Xunit;

    public class WebServerHostTest
        : BaseTest
    {
        [Fact]
        public void StartTest()
        {
            var kernel = this.CreateKernel();

            var server = kernel.Get<WebServerHost>(new ConstructorArgument("port", 5555));

            server.Start();

            server.Stop();
        }
    }
}
