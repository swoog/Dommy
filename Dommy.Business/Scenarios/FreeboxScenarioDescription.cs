//-----------------------------------------------------------------------
// <copyright file="FreeboxScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System.IO;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading;
    using Dommy.Business.Syntax;

    /// <summary>
    /// Freebox scenarios.
    /// </summary>
    public class FreeboxScenarioDescription : IScenarioDescription
    {
        /// <summary>
        /// Speech logger.
        /// </summary>
        private ISpeechLogger speechLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FreeboxScenarioDescription"/> class.
        /// </summary>
        /// <param name="speechLogger">Speech logger.</param>
        public FreeboxScenarioDescription(ISpeechLogger speechLogger)
        {
            this.speechLogger = speechLogger;
        }

        /// <summary>
        /// Call the freebox to get version.
        /// </summary>
        /// <returns>Indicate if the scenario must continue.</returns>
        public bool FreeboxVersion()
        {
            var value = GetFreeboxInfo(@"{""jsonrpc"":""2.0"",""method"":""system.fw_release_get"",""params"":[true]}");

            if (this.speechLogger != null)
            {
                this.speechLogger.Say(Actor.Dommy, "version " + value.Result);
            }

            return true;
        }

        /// <summary>
        /// Call the freebox to reboot.
        /// </summary>
        /// <returns>Indicate if the scenario must continue.</returns>
        public bool FreeboxReboot()
        {
            var value = GetFreeboxInfo(@"{""jsonrpc"":""2.0"",""method"":""system.reboot"",""params"":[true]}");
            return true;
        }

        /// <summary>
        /// Create scenarios.
        /// </summary>
        public void Create()
        {
            Scenario.Create()
                .SpeechTrigger("version de la freebox")
                .Action(this.FreeboxVersion)
                .Start();

            Scenario.Create()
                .SpeechTrigger("redémarre la freebox")
                .Say("La freebox redémarre", "Redémarage encours", "C'est encours")
                .Action(this.FreeboxReboot)
                .Action(() =>
                {
                    int i = 0;
                    do
                    {
                        Thread.Sleep(10000);
                        i++;
                        try
                        {
                            Ping p = new Ping();
                            var result = p.Send("www.google.fr");

                            if (result.Status == IPStatus.Success)
                            {
                                return true;
                            }
                        }
                        catch
                        {
                        }
                    }
                    while (i < 10);

                    this.speechLogger.Say(Actor.Dommy, "Internet introuvable");

                    return false;
                })
                .Say("Terminé", "Internet est revenus")
                .Start();
        }

        /// <summary>
        /// Call the freebox.
        /// </summary>
        /// <param name="req">JSON query for calling API.</param>
        /// <returns>Returned information by Freebox API.</returns>
        private static VersionResult GetFreeboxInfo(string req)
        {
            var c = "login=freebox&passwd=pendore2010";
            var r = System.Net.HttpWebRequest.CreateHttp("http://mafreebox.fr/login.php");
            r.Method = "POST";
            UTF8Encoding encoding = new UTF8Encoding();
            r.ContentType = "application/x-www-form-urlencoded";
            r.ContentLength = encoding.GetByteCount(c);
            r.Credentials = CredentialCache.DefaultCredentials;
            r.CookieContainer = new CookieContainer();
            r.AllowAutoRedirect = false;
            var cookie = r.CookieContainer;
            using (var requestStream = r.GetRequestStream())
            {
                requestStream.Write(encoding.GetBytes(c), 0, encoding.GetByteCount(c));
            }

            var responseCookie = r.GetResponse() as HttpWebResponse;

            var responseString = new StreamReader(responseCookie.GetResponseStream()).ReadToEnd();

            string code = responseCookie.Cookies["FBXSID"].Value;

            c = req;
            r = System.Net.HttpWebRequest.CreateHttp("http://mafreebox.free.fr/system.cgi");
            r.Method = "POST";
            r.ContentLength = encoding.GetByteCount(c);
            r.Credentials = CredentialCache.DefaultCredentials;
            r.Accept = "application/json";
            r.ContentType = "application/json";
            r.CookieContainer = new CookieContainer();
            r.CookieContainer.Add(new Cookie("FBXSID", code, "/", "mafreebox.free.fr"));

            // Write the payload to the request body.
            using (Stream requestStream = r.GetRequestStream())
            {
                requestStream.Write(encoding.GetBytes(c), 0, encoding.GetByteCount(c));
            }

            var response = r.GetResponse();
            var stream = response.GetResponseStream();

            var serializer = new DataContractJsonSerializer(typeof(VersionResult));

            var value = (VersionResult)serializer.ReadObject(stream);
            return value;
        }

        /// <summary>
        /// Freebox version class.
        /// </summary>
        [DataContract]
        private class VersionResult
        {
            /// <summary>
            /// Gets or sets JSON result.
            /// </summary>
            [DataMember(Name = "jsonrpc")]
            public string Jsonrpc { get; set; }

            /// <summary>
            /// Gets or sets result.
            /// </summary>
            [DataMember(Name = "result")]
            public string Result { get; set; }
        }
    }
}
