﻿
namespace Dommy.Business.WebHost
{
    using System.IO;
    using System.ServiceModel;
    using CassiniDev;
    using Dommy.Business.Configs;
    using Dommy.Business.Services;
    using Dommy.Business.Tools;
    using Ninject.Extensions.Logging;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WebServerHost : IWebServerHost
    {
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
            server.StartServer("./", this.port, "/", "*");
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
