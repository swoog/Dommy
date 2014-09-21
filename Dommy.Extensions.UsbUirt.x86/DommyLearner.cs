
namespace Dommy.Extensions.UsbUirt.x86
{
    using global::UsbUirt;
    using System.ServiceModel;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DommyLearner : ILearner
    {
        public string Learn()
        {
            using (var driver = new Driver())
            {
                using (var l = new Learner(driver))
                {
                    return l.Learn();
                }
            }
        }
    }
}
