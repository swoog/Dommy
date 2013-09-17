using Dommy.Business.Scenarios;
using Dommy.Business.Syntax;
using Dommy.Business.Triggers;
using Dommy.Business.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dommy.Business.Scenarios
{
    public class FreeboxScenarioDescription : IScenarioDescription
    {
        private ISpeechLogger speechLogger;

        public FreeboxScenarioDescription(ISpeechLogger speechLogger)
        {
            this.speechLogger = speechLogger;
        }

        public bool FreeboxVersion()
        {
            var value = GetFreeboxInfo(@"{""jsonrpc"":""2.0"",""method"":""system.fw_release_get"",""params"":[true]}");

            if (this.speechLogger != null)
            {
                this.speechLogger.Say(Actor.Dommy, "version " + value.result);
            }

            return true;
        }

        public bool FreeboxReboot()
        {
            var value = GetFreeboxInfo(@"{""jsonrpc"":""2.0"",""method"":""system.reboot"",""params"":[true]}");
            return true;
        }

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
                requestStream.Write(encoding.GetBytes(c), 0,
                    encoding.GetByteCount(c));
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

            //Write the payload to the request body.
            using (Stream requestStream = r.GetRequestStream())
            {
                requestStream.Write(encoding.GetBytes(c), 0,
                    encoding.GetByteCount(c));
            }


            var response = r.GetResponse();
            var stream = response.GetResponseStream();

            var serializer = new DataContractJsonSerializer(typeof(VersionResult));

            var value = (VersionResult)serializer.ReadObject(stream);
            return value;
        }

        [DataContract]
        private class VersionResult
        {
            [DataMember]
            public string jsonrpc { get; set; }

            [DataMember]
            public string result { get; set; }
        }

        public void Create()
        {
            Scenario.Create()
                .SpeechTrigger("version de la freebox")
                .Action(FreeboxVersion)
                .Start();

            Scenario.Create()
                .SpeechTrigger("redémarre la freebox")
                .Say("La freebox redémarre", "Redémarage encours", "C'est encours")
                .Action(FreeboxReboot)
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
                    } while (i < 10);

                    this.speechLogger.Say(Actor.Dommy, "Internet introuvable");

                    return false;
                })
                .Say("Terminé", "Internet est revenus")
                .Start();
        }
    }
}
