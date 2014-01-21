using CassiniDev;
using Dommy.Business.Config;
using Dommy.Business.Services;
using Dommy.Business.Tools;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.WebHost
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WebServerHost : IWebServerHost
    {
#if DEBUG
        private const string WebServerPath = @"..\..\..\Dommy.Web\";
#else
        private const string WebServerPath = @".\";
#endif
        public class Config : IConfig
        {
            public int Port { get; set; }

            public void Create(Ninject.IKernel kernel)
            {
                kernel.Bind<WebServerHost>().ToSelf().InSingletonScope()
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
            if (!File.Exists(@"bin\Dommy.Business.dll"))
            {
                if (!Directory.Exists(@"bin"))
                {
                    Directory.CreateDirectory(@"bin");
                }

                foreach (var file in Directory.GetFiles(".", "*.dll"))
                {
                    File.Copy(file, Path.Combine("bin", Path.GetFileName(file)));
                }
            }

            server = new CassiniDevServer();
            server.StartServer(WebServerHost.WebServerPath, this.port, "/", "*");
            this.logger.Info("Webserver started on {0} port", this.port);
        }

        public void Stop()
        {
            server.StopServer();
            this.logger.Info("Webserver stoped");
        }

        public void Started()
        {
            WebAppender.WebStarted();
        }
    }
}
