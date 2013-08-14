using Dommy.Business.Config;
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
        }

        private Dictionary<string, ScenarioData> scenarios = new Dictionary<string, ScenarioData>();
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
                        this.scenarios[context.Request.RawUrl].Scenario.Run();
                    }
                    else
                    {
                        // Test pattern matching
                        foreach (var item in this.scenarios.Values)
                        {
                            if (item.IsMatch(context.Request.RawUrl))
                            {
                                item.Scenario.Run();
                                break;
                            }
                        }
                    }
                }
            });
        }

        public void Subscribe(string url, object data, IScenario scenario)
        {
            scenarios.Add(url, new ScenarioData { Scenario = scenario, Data = data, Url = url });
        }
    }
}
