using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Dommy.Business.Tools;

namespace Dommy.Web.Hubs
{
    using Microsoft.AspNet.SignalR;

    public class Logger : Hub
    {
        public void Send(MessageLogger message)
        {
            Clients.All.Receive(message);
        }
    }
}