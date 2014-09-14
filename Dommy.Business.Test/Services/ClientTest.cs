
namespace Dommy.Business.Test.Services
{
    using Dommy.Business.Services;

    using System.ServiceModel;

    using Xunit;

    public class ClientTest
    {
        [Fact]
        public void CreateTest()
        {
            using (var c = Client<IStubService>.Create())
            {
                Assert.NotNull(c.Channel);
            }
        }

        [ServiceContract]
        private interface IStubService
        {
            [OperationContract]
            void FakeService();
        }
    }
}
