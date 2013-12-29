using Dommy.Business.Syntax;
using Dommy.Business.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dommy.Business.Actions
{
    public class SynologyDownloadActions : ISynologyDownloadActions
    {
        private static string urlAuth = "http://{0}/webapi/auth.cgi";
        private static string urlTask = "http://{0}/webapi/DownloadStation/task.cgi";

        /// <summary>
        /// Scenario syntax to use.
        /// </summary>
        private IScenarioSyntax scenario;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynologyDownloadActions" /> class.
        /// </summary>
        /// <param name="scenario">Scenario syntax to use.</param>
        public SynologyDownloadActions(IScenarioSyntax scenario)
        {
            this.scenario = scenario;
        }

        public Syntax.IScenarioSyntax SynologyDownloadCreate(string server, string username, string password, string file)
        {
            return SynologyDownloadCreate(server, username, password, null, file);
        }

        public Syntax.IScenarioSyntax SynologyDownloadCreate(string server, string username, string password, object data, string file)
        {
            this.scenario.Action(() =>
            {
                var urlAuthServer = String.Format(urlAuth, server);
                var parameterAuth = String.Format("api=SYNO.API.Auth&version=2&method=login&account={0}&passwd={1}&session=DownloadStation&format=cookie", username, password);
                CallRequest(urlAuthServer, "GET", parameterAuth);

                var urlTaskServer = String.Format(urlTask, server);
                var fileFormat = HttpUtility.UrlEncode(StringHelper.Format(file, data));
                var parameterTask = String.Format("api=SYNO.DownloadStation.Task&version=1&method=create&uri={0}", fileFormat);

                CallRequest(urlTaskServer, "POST", parameterTask);

                return true;
            });

            return this.scenario;
        }

        private CookieCollection cookies;

        private void CallRequest(string url, string method, string parameter)
        {
            var urlTemp = method == "GET" ? url + "?" + parameter : url;

            HttpWebRequest request = HttpWebRequest.Create(urlTemp) as HttpWebRequest;
            request.CookieContainer = new CookieContainer();
            if (cookies != null)
            {
                foreach (Cookie c in cookies)
                {
                    request.CookieContainer.Add(c);
                }
            }

            request.Method = method;

            if (!String.IsNullOrEmpty(parameter) && method == "POST")
            {
                var requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.WriteLine(parameter);
                requestStream.Flush();
            }

            var response = request.GetResponse() as HttpWebResponse;
            this.cookies = response.Cookies;
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            response.Close();
        }
    }
}
