﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Extensions.UsbUirt
{
    [ServiceContract]
    public interface IReceiver
    {
        [OperationContract]
        void Start();

        [OperationContract]
        void Stop();
    }
}
