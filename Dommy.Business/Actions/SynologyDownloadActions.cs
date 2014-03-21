//-----------------------------------------------------------------------
// <copyright file="SynologyDownloadActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Web;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;

    /// <summary>
    /// Use Synology API.
    /// </summary>
    public class SynologyDownloadActions : ISynologyDownloadActions
    {
        /// <summary>
        /// Url used for authentication.
        /// </summary>
        private static string urlAuth = "http://{0}/webapi/auth.cgi";

        /// <summary>
        /// Url used for download station.
        /// </summary>
        private static string urlTask = "http://{0}/webapi/DownloadStation/task.cgi";

        /// <summary>
        /// Scenario syntax to use.
        /// </summary>
        private IScenarioSyntax scenario;

        /// <summary>
        /// Last cookie returned by last HTTP call.
        /// </summary>
        private CookieCollection cookies;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynologyDownloadActions" /> class.
        /// </summary>
        /// <param name="scenario">Scenario syntax to use.</param>
        public SynologyDownloadActions(IScenarioSyntax scenario)
        {
            this.scenario = scenario;
        }

        /// <summary>
        /// Add file to synology download manager.
        /// </summary>
        /// <param name="server">Server name with port.</param>
        /// <param name="userName">Username used for connect to synology.</param>
        /// <param name="password">Password used for connect to synology.</param>
        /// <param name="file">Url file to download (EDK2, HTTP, ...)</param>
        /// <returns>Scenario syntax.</returns>
        public Syntax.IScenarioSyntax SynologyDownloadCreate(string server, string userName, string password, string file)
        {
            return this.SynologyDownloadCreate(server, userName, password, null, file);
        }

        /// <summary>
        /// Add file to synology download manager.
        /// </summary>
        /// <param name="server">Server name with port.</param>
        /// <param name="userName">Username used for connect to synology.</param>
        /// <param name="password">Password used for connect to synology.</param>
        /// <param name="data">Data used to create file url.</param>
        /// <param name="file">Format url file to download (EDK2, HTTP, ...)</param>
        /// <returns>Scenario syntax.</returns>
        public Syntax.IScenarioSyntax SynologyDownloadCreate(string server, string userName, string password, object data, string file)
        {
            this.scenario.Action(() =>
            {
                var urlAuthServer = string.Format(CultureInfo.InvariantCulture, urlAuth, server);
                var parameterAuth = string.Format(CultureInfo.InvariantCulture, "api=SYNO.API.Auth&version=2&method=login&account={0}&passwd={1}&session=DownloadStation&format=cookie", userName, password);
                CallRequest(urlAuthServer, "GET", parameterAuth);

                var urlTaskServer = string.Format(CultureInfo.InvariantCulture, urlTask, server);
                var fileFormat = HttpUtility.UrlEncode(StringHelper.Format(file, data));
                var parameterTask = string.Format(CultureInfo.InvariantCulture, "api=SYNO.DownloadStation.Task&version=1&method=create&uri={0}", fileFormat);

                CallRequest(urlTaskServer, "POST", parameterTask);

                return true;
            });

            return this.scenario;
        }

        /// <summary>
        /// Make an HTTP call.
        /// </summary>
        /// <param name="url">Url to call.</param>
        /// <param name="method">Method GET or POST.</param>
        /// <param name="parameter">HTTP parameters.</param>
        private void CallRequest(string url, string method, string parameter)
        {
            Contract.Requires(!string.IsNullOrEmpty(url));
            Contract.Requires(method == "GET" || method == "POST");
            var urlTemp = method == "GET" ? url + "?" + parameter : url;

            HttpWebRequest request = HttpWebRequest.Create(urlTemp) as HttpWebRequest;
            request.CookieContainer = new CookieContainer();
            if (this.cookies != null)
            {
                foreach (Cookie c in this.cookies)
                {
                    request.CookieContainer.Add(c);
                }
            }

            request.Method = method;

            if (!string.IsNullOrEmpty(parameter) && method == "POST")
            {
                var requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.WriteLine(parameter);
                requestStream.Flush();
            }

            var response = request.GetResponse() as HttpWebResponse;
            this.cookies = response.Cookies;
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            if (string.IsNullOrEmpty(responseString))
            {
                throw new FormatException();
            }

            response.Close();
        }
    }
}
