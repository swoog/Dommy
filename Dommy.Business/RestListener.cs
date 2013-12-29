﻿using Dommy.Business.Config;
using Dommy.Business.Scenarios;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Dommy.Business
{
    public sealed class RestListener : IListener, IDisposable
    {
        public class Config : IConfig
        {
            public int Port { get; set; }

            public void Create(IKernel kernel)
            {
                kernel.Bind<IListener>().To<RestListener>()
                    .WithConstructorArgument("port", this.Port)
                    ;
            }
        }

        private class ScenarioData
        {
            public IScenario Scenario { get; set; }

            public object Data { get; set; }

            public string Url
            {
                set
                {
                    urlRegex = String.Format("^{0}$", Regex.Replace(value, @"\{([a-zA-Z0-9]+)\}", M));
                }
            }

            private List<string> elements = new List<string>();

            private string urlRegex;

            private string M(Match match)
            {
                elements.Add(match.Groups[1].Value);

                return "(.+)";
            }

            public bool IsMatch(string url)
            {
                var m = Regex.Match(url, this.urlRegex);

                if (m.Success)
                {
                    try
                    {
                        for (int i = 1; i < m.Groups.Count; i++)
                        {
                            var p = elements[i - 1];

                            var pi = this.Data.GetType().GetProperty(p);

                            var val = HttpUtility.UrlDecode(m.Groups[i].Value);

                            pi.SetValue(this.Data, Convert.ChangeType(val, pi.PropertyType));
                        }
                    }
                    catch
                    {
                        return false;
                    }

                    return true;
                }

                return false;
            }

            internal void SetListernerContext(HttpListenerContext context)
            {
                if (this.Data != null)
                {
                    var property = this.Data.GetType().GetProperty("Context");

                    if (property != null && property.PropertyType == typeof(HttpListenerContext))
                    {
                        property.SetValue(this.Data, context);
                    }
                }
            }
        }

        private Dictionary<string, ScenarioData> scenarios = new Dictionary<string, ScenarioData>();
        private HttpListener listener;

        private int port;

        public RestListener(int port)
        {
            this.port = port;
        }

        public void Init(Engine engine)
        {

        }

        public void Start()
        {
            this.listener = new HttpListener();
            this.listener.Start();
            this.listener.Prefixes.Add(String.Format("http://+:{0}/", this.port));

            Task.Run(() =>
            {
                while (true)
                {
                    var context = this.listener.GetContext();

                    StartRequest(context);
                }
            });
        }

        private async void StartRequest(HttpListenerContext context)
        {
            try
            {
                if (this.scenarios.ContainsKey(context.Request.RawUrl))
                {
                    await this.scenarios[context.Request.RawUrl].Scenario.RunAsync();
                }
                else
                {
                    // Test pattern matching
                    foreach (var item in this.scenarios.Values)
                    {
                        if (item.IsMatch(context.Request.RawUrl))
                        {
                            item.SetListernerContext(context);

                            await item.Scenario.RunAsync();
                            break;
                        }
                    }
                }
            }
            finally
            {
                context.Response.OutputStream.Close();
            }
        }

        public void Subscribe(string url, object data, IScenario scenario)
        {
            scenarios.Add(url, new ScenarioData { Scenario = scenario, Data = data, Url = url });
        }

        public void Stop()
        {
            if (this.listener != null)
            {
                this.listener.Stop();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool p)
        {
            if (this.listener != null)
            {
                ((IDisposable)this.listener).Dispose();
            }
        }
    }
}
