using CassiniDev;
using Dommy.Business.Config;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.WebHost
{
    public class WebServerHost
    {
        public class Config : IConfig
        {
            public int Port { get; set; }

            public void Create(Ninject.IKernel kernel)
            {
                kernel.Bind<WebServerHost>().ToSelf()
                    .WithConstructorArgument("port", this.Port);
            }
        }

        private int port;

        private ILogger logger;

        public WebServerHost(ILogger logger, int port)
        {
            this.port = port;
            this.logger = logger;
        }

        CassiniDevServer server;

        public void Start()
        {
            server = new CassiniDevServer();
            server.StartServer("./", this.port, "/", "localhost");
            this.logger.Info("Webserver started on {0} port", this.port);
        }

        public void Stop()
        {
            server.StopServer();
            this.logger.Info("Webserver stoped");
        }
    }
}
