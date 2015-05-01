namespace Dommy.Business.Test
{
    using Xunit;

    public class BootstrapTests
    {
        [Fact]
        public void Should_load_configuration_when_run()
        {
            Bootstrap.Run();
        }
    }
}