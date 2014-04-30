using System.ServiceModel;

namespace Dommy.Extensions.UsbUirt
{
    [ServiceContract]
    public interface ITransmitter
    {
        [OperationContract]
        void Transmit(string infraRedCode);
    }
}