﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Services
{
    public interface IServiceHost
    {
        void Open();
        void Close();
    }
}
