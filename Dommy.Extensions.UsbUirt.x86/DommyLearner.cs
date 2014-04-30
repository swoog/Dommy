using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::UsbUirt;
using System.ServiceModel;

namespace Dommy.Extensions.UsbUirt.x86
{
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
