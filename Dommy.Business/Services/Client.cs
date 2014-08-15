//-----------------------------------------------------------------------
// <copyright file="Client.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.Text;
    using System.Threading.Tasks;
    public sealed class Client<T> : IDisposable
    {
        public Client(T channel)
        {
            this.Channel = channel;
        }

        public T Channel { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Channel != null)
                {
                    ((IDisposable)this.Channel).Dispose();
                }
            }
        }
    }
}
