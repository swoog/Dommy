using Dommy.Business.Actions;
using Dommy.Business.Configs;
using Ninject;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dommy.Business.Tools
{
    public class EedomusHelper
    {
        public class Config : IConfig
        {
            public string IP { get; set; }
            public string User { get; set; }
            public string Secret { get; set; }

            public void Create(IKernel kernel)
            {
                kernel.Bind<EedomusHelper>().ToSelf()
                    .WithConstructorArgument("apiAddr", this.IP)
                    .WithConstructorArgument("apiUser", this.User)
                    .WithConstructorArgument("apiSecret", this.Secret)
                    ;
            }
        }

        public EedomusHelper(string apiAddr, string apiUser, string apiSecret, IWebRequest requestWeb)
        {
            this.apiAddr = apiAddr;
            this.apiUser = apiUser;
            this.apiSecret = apiSecret;
            this.requestWeb = requestWeb;
        }

        public enum EedoumusRequestType
        {
            Get,
            Set,
        }

        const string localUrl = "http://{0}/api/{1}?action={2}&periph_id={3}&{4}&api_user={5}&api_secret={6}";
        const string distantUrl = "http://api.eedomus.com/{1}?action={2}&periph_id={3}&{4}&api_user={5}&api_secret={6}";

        private string apiAddr;
        private string apiUser;
        private string apiSecret;

        private IWebRequest requestWeb;

        [Inject]
        public ILogger Logger { get; set; }

        public string CallService(EedomusApi api, EedomusAction action, string eedomusId, string value = null)
        {
            this.Logger.Info("Call eedomus {0} to {1}", eedomusId, value);

            var requestType = getRequestType(action);

            var url = getUrl(api, requestType, action, eedomusId, String.Format(CultureInfo.InvariantCulture, "value={0}", value));

            try
            {

                using (var responseStream = this.requestWeb.Create(new Uri(url)))
                {

                    var serializer = new DataContractJsonSerializer(typeof(EedomusResult));

                    var jsonString = new StreamReader(responseStream).ReadToEnd();

                    jsonString = jsonString.Replace("é", "&#233;");
                    jsonString = jsonString.Replace("è", "&#232;");
                    jsonString = jsonString.Replace("à", "&#224;");

                    jsonString = Regex.Replace(jsonString, @"^[^\{]+", String.Empty);

                    EedomusResult result = null;
                    using (var memoryStream = new MemoryStream())
                    {
                        var writer = new StreamWriter(memoryStream);
                        writer.Write(jsonString);
                        writer.Flush();
                        memoryStream.Position = 0;

                        result = (EedomusResult)serializer.ReadObject(memoryStream);
                    }

                    if (!result.Success)
                    {
                        throw new EedomusException(String.Format(CultureInfo.InvariantCulture, "Erreur eedomus : {0}", result.Body.ErrorMsg));
                    }

                    value = result.Body.LastValue;

                    this.Logger.Info("Eedomus indicate : {0} ({1})", result.Body.LastValue, result.Body.LastValueChange);
                }
            }
            catch (UriFormatException)
            {
                this.Logger.Error(url);
                throw;
            }

            return value;
        }

        [DataContract]
        private class EedomusResult
        {
            [DataMember(Name = "success")]
            public bool Success { get; set; }

            [DataMember(Name = "body")]
            public EedomusBody Body { get; set; }
        }

        [DataContract]
        private class EedomusBody
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "last_value")]
            public string LastValue { get; set; }

            [DataMember(Name = "last_value_change")]
            public string LastValueChange { get; set; }

            [DataMember(Name = "error_code")]
            public string ErrorCode { get; set; }

            [DataMember(Name = "error_msg")]
            public string ErrorMsg { get; set; }
        }

        private static EedoumusRequestType getRequestType(EedomusAction action)
        {
            switch (action)
            {
                case EedomusAction.AuthTest:
                case EedomusAction.PeriphCaract:
                case EedomusAction.PeriphHistory:
                    return EedoumusRequestType.Get;
                case EedomusAction.PeriphValue:
                    return EedoumusRequestType.Set;
                default:
                    throw new NotImplementedException();
            }
        }

        private string getUrl(EedomusApi api, EedoumusRequestType requestType, EedomusAction action, string eedomusId, string parameter)
        {
            var url = localUrl;

            if (api == EedomusApi.Distant)
            {
                url = distantUrl;
            }

            return String.Format(CultureInfo.InvariantCulture, url, this.apiAddr, requestType.ToString().ToLower(CultureInfo.InvariantCulture), ActionToString(action), eedomusId, parameter, apiUser, apiSecret);
        }

        private static string ActionToString(EedomusAction action)
        {
            switch (action)
            {
                case EedomusAction.AuthTest:
                    return "action.test";
                case EedomusAction.PeriphValue:
                    return "periph.value";
                case EedomusAction.PeriphCaract:
                    return "periph.caract";
                case EedomusAction.PeriphHistory:
                    return "periph.history";
                default:
                    return String.Empty;
            }
        }
    }
}
