using log4net.Appender;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Tools
{
    public class WebAppender : AppenderSkeleton
    {
        private static string uriAppender = "http://localhost:17909";

        private HubConnection connection;
        private IHubProxy logger;

        private static bool isStarted = false;

        public override void ActivateOptions()
        {
            base.ActivateOptions();
        }

        public static void WebStarted()
        {
            isStarted = true;
        }

        private bool InitConnection()
        {
            try
            {
                // TODO : Use Ninject to get HubConnection
                this.connection = new HubConnection(WebAppender.uriAppender);
                this.logger = connection.CreateHubProxy("logger");
                this.connection.Start().Wait();
                return true;
            }
            catch
            {
                this.connection = null;
                this.logger = null;
            }

            return false;
        }

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            if (isStarted)
            {
                bool init = true;
                if (this.connection == null)
                {
                    init = this.InitConnection();
                }

                if (init)
                {
                    this.logger.Invoke("send", new MessageLogger { Level = loggingEvent.Level.ToString(), Message = loggingEvent.RenderedMessage });
                }
            }
        }
    }
}