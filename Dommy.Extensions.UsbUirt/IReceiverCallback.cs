
namespace Dommy.Extensions.UsbUirt
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IReceiverCallback
    {
        [OperationContract]
        void Receive(string infraredCode);
    }
}
