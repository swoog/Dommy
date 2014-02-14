﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Services
{
    [ServiceContract]
    public interface IEngine
    {
        [OperationContract]
        string GetEngineName();
    }
}
