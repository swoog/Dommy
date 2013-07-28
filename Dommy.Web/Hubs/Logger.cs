using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Dommy.Web.Hubs
{
    public class Logger : Hub
    {
        public void Send(string message)
        {
            Clients.All.Receive(message);
        }
    }
}