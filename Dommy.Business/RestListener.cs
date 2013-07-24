using Dommy.Business.Config;
using Dommy.Business.Scenarios;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business
{
    public class RestListener
    {
        public class Config : IConfig
        {
            public int Port { get; set; }

            public void Create(IKernel kernel)
            {
                kernel.Bind<RestListener>().ToSelf()
                    .WithConstructorArgument("port", this.Port)
                    ;
            }
        }

        private Dictionary<string, IScenario> scenarios = new Dictionary<string, IScenario>();
        private HttpListener listener;

        private int port;

        public RestListener(int port)
        {
            this.port = port;
        }

        public void Init()
        {
            this.listener = new HttpListener();
            this.listener.Start();
            this.listener.Prefixes.Add(String.Format("http://+:{0}/", this.port));

            Task.Run(() =>
            {
                while (true)
                {
                    var context = this.listener.GetContext();

                    context.Response.OutputStream.Close();
                    if (this.scenarios.ContainsKey(context.Request.RawUrl))
                    {
                        this.scenarios[context.Request.RawUrl].Run();
                    }
                }
            });
        }

        public void Subscribe(string url, IScenario scenario)
        {
            scenarios.Add(url, scenario);
        }
    }
}
