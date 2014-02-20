using CassiniDev;
using Dommy.Business.Configs;
using Dommy.Business.Services;
using Dommy.Business.Tools;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Hosting.Services;
using Microsoft.Owin.Hosting.Starter;
using Ninject.Extensions.Logging;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

//namespace Dommy.Business
//{
//    public class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            app.UseCors(CorsOptions.AllowAll);
//            app.MapSignalR();
//        }
//    }
//}

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

                kernel.Bind<HubConnection>().ToMethod(c =>
                {
                    var connection = new HubConnection(String.Format("http://localhost:{0}", this.Port));
                    connection.Start().Wait();

                    return connection;
                }).InSingletonScope();
            }
        }

        private int port;

        private ILogger logger;

        public WebServerHost(ILogger logger, int port)
        {
            this.port = port;
            this.logger = logger;
        }

        private CassiniDevServer server;
        private IDisposable disposable;

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

            //Program.ResolveAssembliesFromDirectory(Path.Combine(Directory.GetCurrentDirectory(), "bin"));
            //Program.WriteLine("Starting with " + Program.GetDisplayUrl(options));
            IServiceProvider services = ServicesFactory.Create();
            IHostingStarter service = services.GetService<IHostingStarter>();
            var options = new Microsoft.Owin.Hosting.StartOptions();
            var url = String.Format("http://*:{0}", this.port);
            options.Urls.Add(url);
            options.ServerFactory = "Microsoft.Owin.Host.HttpListener";

            foreach (var file in Directory.GetFiles(Path.Combine(WebServerPath, "bin"), "*.dll"))
            {
                try
                {
                    System.Reflection.Assembly.LoadFile(new FileInfo(file).FullName);
                }
                catch (Exception)
                {
                }
            }

            options.Settings["directory"] = WebServerHost.WebServerPath;
            options.AppStartup = "Dommy.Web.Startup.Configuration,Dommy.Web";

            disposable = service.Start(options);
            //Program.WriteLine("Started successfully");
            //Program.WriteLine("Press Enter to exit");
            //Console.ReadLine();
            //Program.WriteLine("Terminating.");
            //disposable.Dispose();

            //server = new CassiniDevServer();
            //server.StartServer(WebServerHost.WebServerPath, this.port, "/", "*");
            this.logger.Info("Webserver started on {0} port", this.port);
        }

        public void Stop()
        {
            disposable.Dispose();
            //server.StopServer();
            this.logger.Info("Webserver stoped");
        }

        public void Started()
        {
            WebAppender.WebStarted();
        }
    }
}