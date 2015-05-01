

namespace Dommy.Extensions.UsbUirt
{
    using System.ServiceModel;

    [ServiceContract]
    public interface ILearner
    {
        [OperationContract]
        string Learn();
    }
}