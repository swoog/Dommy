
namespace Dommy.Web.Hubs
{
    using Microsoft.AspNet.SignalR;

    public class Logger : Hub
    {
        public void Send(string message)
        {
            Clients.All.Receive(message);
        }
    }
}