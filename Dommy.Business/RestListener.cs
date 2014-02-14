//-----------------------------------------------------------------------
// <copyright file="RestListener.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using Dommy.Business.Configs;
    using Dommy.Business.Scenarios;
    using Ninject;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Listen of a REST request on an HTTP port.
    /// </summary>
    public sealed class RestListener : IListener, IDisposable
    {
        /// <summary>
        /// All scenarios associate with this url pattern.
        /// </summary>
        private Dictionary<string, ScenarioData> scenarios = new Dictionary<string, ScenarioData>();

        /// <summary>
        /// The HTTP listener.
        /// </summary>
        private HttpListener listener;

        /// <summary>
        /// Port to listen.
        /// </summary>
        private int port;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestListener"/> class.
        /// </summary>
        /// <param name="port">Port to listen.</param>
        public RestListener(int port)
        {
            this.port = port;
        }

        /// <summary>
        /// Initialize the engine to use.
        /// </summary>
        /// <param name="engine">Dommy engine</param>
        public void Init(Engine engine)
        {
        }

        /// <summary>
        /// Start the HTTP Listener.
        /// </summary>
        public void Start()
        {
            this.listener = new HttpListener();
            this.listener.Start();
            this.listener.Prefixes.Add(string.Format("http://+:{0}/", this.port));

            Task.Run(() =>
            {
                while (true)
                {
                    var context = this.listener.GetContext();

                    StartRequest(context);
                }
            });
        }

        /// <summary>
        /// Subscribe to the HTTP listener.
        /// </summary>
        /// <param name="url">Url to listen.</param>
        /// <param name="data">Object data to set property. (Optional)</param>
        /// <param name="scenario">Scenario to execute.</param>
        public void Subscribe(string url, object data, IScenario scenario)
        {
            Contract.Requires(!string.IsNullOrEmpty(url));

            this.scenarios.Add(url, new ScenarioData { Scenario = scenario, Data = data, Url = url });
        }

        /// <summary>
        /// Stop HTTP listener.
        /// </summary>
        public void Stop()
        {
            if (this.listener != null)
            {
                this.listener.Stop();
            }
        }

        /// <summary>
        /// Dispose HTTP listener.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Stat scenario associate to the context.
        /// </summary>
        /// <param name="context">HTTP Context.</param>
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

         /// <summary>
        /// Dispose HTTP listener.
        /// </summary>
        /// <param name="p">Indicate dispose finalize.</param>
        private void Dispose(bool p)
        {
            if (this.listener != null)
            {
                ((IDisposable)this.listener).Dispose();
            }
        }

        /// <summary>
        /// Class used for configuration of the REST listener.
        /// </summary>
        public class Config : IConfig
        {
            /// <summary>
            /// Gets or sets listen port.
            /// </summary>
            public int Port { get; set; }

            /// <summary>
            /// Create configuration.
            /// </summary>
            /// <param name="kernel">Ninject kernel.</param>
            public void Create(IKernel kernel)
            {
                kernel.Bind<IListener>().To<RestListener>()
                    .WithConstructorArgument("port", this.Port);
            }
        }

        /// <summary>
        /// Class used to make url matching.
        /// </summary>
        private class ScenarioData
        {
            /// <summary>
            /// String elements value.
            /// </summary>
            private List<string> elements = new List<string>();

            /// <summary>
            /// Url regex.
            /// </summary>
            private string urlRegex;

            /// <summary>
            /// Gets or sets scenario to execute.
            /// </summary>
            public IScenario Scenario { get; set; }

            /// <summary>
            /// Gets or sets associated data.
            /// </summary>
            public object Data { get; set; }

            /// <summary>
            /// Sets url to match.
            /// </summary>
            public string Url
            {
                set
                {
                    Contract.Requires(value != null);

                    this.urlRegex = string.Format("^{0}$", Regex.Replace(value, @"\{([a-zA-Z0-9]+)\}", this.M));
                }
            }

            /// <summary>
            /// Indicate if the url match the pattern.
            /// </summary>
            /// <param name="url">Url to test.</param>
            /// <returns>Boolean to indicate the success of the match.</returns>
            public bool IsMatch(string url)
            {
                Contract.Requires(!string.IsNullOrEmpty(url));

                var m = Regex.Match(url, this.urlRegex);

                if (m.Success)
                {
                    try
                    {
                        for (int i = 1; i < m.Groups.Count; i++)
                        {
                            var p = this.elements[i - 1];

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

            /// <summary>
            /// Set this context in the property "Context" of the data instance.
            /// </summary>
            /// <param name="context">Context to set.</param>
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

            /// <summary>
            /// Replace match.
            /// </summary>
            /// <param name="match">Regex match.</param>
            /// <returns>Replace string.</returns>
            private string M(Match match)
            {
                Contract.Requires(match != null);

                this.elements.Add(match.Groups[1].Value);

                return "(.+)";
            }
        }
    }
}
