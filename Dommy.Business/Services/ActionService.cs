using Dommy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Services
{
    [ServiceContract()]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ActionService
    {
        private Engine engine;

        public ActionService(Engine engine)
        {
            this.engine = engine;
        }

        [OperationContract]
        public ActionData GetData(int id)
        {
            return this.engine.GetData(id);
        }

        [OperationContract]
        public ActionData[] GetDatas(int[] ids)
        {
            return (from id in ids
                    select this.engine.GetData(id)).ToArray();
        }

        [OperationContract]
        public void Run(ActionData data)
        {
            this.engine.Run(data);
        }

        [OperationContract]
        public ActionData[] GetActions()
        {
            return this.engine.GetDatas();
        }
    }
}
