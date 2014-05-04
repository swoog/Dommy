using System.ServiceModel;

namespace Dommy.Extensions.UsbUirt
{
    [ServiceContract]
    public interface ILearner
    {
        [OperationContract]
        string Learn();
    }
}