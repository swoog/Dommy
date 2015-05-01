
namespace Dommy.Business.WebHost
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.ServiceModel;
    using CassiniDev;
    using Dommy.Business.Configs;
    using Dommy.Business.Services;
    using Dommy.Business.Tools;

    using Microsoft.AspNet.SignalR.Client;
    using Microsoft.Owin.Hosting.Services;
    using Microsoft.Owin.Hosting.Starter;

    using Ninject.Extensions.Logging;

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

                kernel.Bind<HubConnection>().ToMethod(c =>
                {
                    return new HubConnection(String.Format("http://localhost:{0}", this.Port + 1));
                }).InSingletonScope();
            }
        }

        private int port;

        private ILogger logger;

        public WebServerHost(ILogger logger, HubConnection connection, int port)
        {
            this.port = port;
            this.logger = logger;
            this.connection = connection;
        }

        private CassiniDevServer server;
        private IDisposable disposable;
        private HubConnection connection;

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
                    File.Copy(file, Path.Combine("bin", Path.GetFileName(file)), true);
                }
            }

            IServiceProvider services = ServicesFactory.Create();
            IHostingStarter service = services.GetService<IHostingStarter>();
            var options = new Microsoft.Owin.Hosting.StartOptions();
            var url = String.Format("http://*:{0}", this.port + 1);
            options.Urls.Add(url);
            options.ServerFactory = "Microsoft.Owin.Host.HttpListener";

            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
            {
                var assemblyName = e.Name.Split(',');

                if (assemblyName.Length > 0)
                {
                    var file = new FileInfo(Path.Combine(WebServerPath, "bin", assemblyName[0] + ".dll")).FullName;

                    if (File.Exists(file))
                    {
                        return Assembly.LoadFile(file);
                }
            }

                return null;
            };

            options.Settings["directory"] = WebServerHost.WebServerPath;
            options.AppStartup = "Dommy.Web.Startup.Configuration,Dommy.Web";

            disposable = service.Start(options);
            this.logger.Info("SignalR started on {0} port", this.port + 1);

            server = new CassiniDevServer();
            server.StartServer(WebServerHost.WebServerPath, System.Net.IPAddress.Any, this.port, "/", "*");
            this.logger.Info("Webserver started on {0} port", this.port);
        }

        public void Stop()
        {
            server.StopServer();
            disposable.Dispose();
            this.logger.Info("Webserver stoped");
        }

        public void Started()
        {
            WebAppender.WebStarted();
        }

        public void StartSignalR()
        {
            connection.Start().Wait();
            this.logger.Info("SignalR client connection started");
        }
    }
}
