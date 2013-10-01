using Dommy.Business.Actions;
using Dommy.Business.Config;
using Ninject;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            public string Ip { get; set; }
            public string User { get; set; }
            public string Secret { get; set; }

            public void Create(IKernel kernel)
            {
                kernel.Bind<EedomusHelper>().ToSelf()
                    .WithConstructorArgument("apiAddr", this.Ip)
                    .WithConstructorArgument("apiUser", this.User)
                    .WithConstructorArgument("apiSecret", this.Secret)
                    ;
            }
        }

        private AsyncHelper asyncHelper;

        public EedomusHelper(AsyncHelper asyncHelper, string apiAddr, string apiUser, string apiSecret)
        {
            this.asyncHelper = asyncHelper;
            this.apiAddr = apiAddr;
            this.apiUser = apiUser;
            this.apiSecret = apiSecret;
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

        [Inject]
        public ILogger Logger { get; set; }

        public string CallService(EedomusApi api, EedoumusAction action, string eedomusId, string value = null)
        {
            this.Logger.Info("Call eedomus {0} to {1}", eedomusId, value);

            var requestType = getRequestType(action);

            var url = getUrl(api, requestType, action, eedomusId, String.Format("value={0}", value));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(String.Format("Erreur eedomus. Code {0}", response.StatusCode));
            }

            var serializer = new DataContractJsonSerializer(typeof(EedomusResult));

            var jsonString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            jsonString = jsonString.Replace("é", "&#233;");
            jsonString = jsonString.Replace("è", "&#232;");
            jsonString = jsonString.Replace("à", "&#224;");

            jsonString = Regex.Replace(jsonString, @"^[^\{]+", String.Empty);

            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            writer.Write(jsonString);
            writer.Flush();
            memoryStream.Position = 0;

            var result = (EedomusResult)serializer.ReadObject(memoryStream);

            if (!result.Success)
            {
                throw new Exception(String.Format("Erreur eedomus : {0}", result.Body.ErrorMsg));
            }

            value = result.Body.LastValue;

            this.Logger.Info("Eedomus indicate : {0} ({1})", result.Body.LastValue, result.Body.LastValueChange);

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

        private EedoumusRequestType getRequestType(EedoumusAction action)
        {
            switch (action)
            {
                case EedoumusAction.AuthTest:
                case EedoumusAction.PeriphCaract:
                case EedoumusAction.PeriphHistory:
                    return EedoumusRequestType.Get;
                case EedoumusAction.PeriphValue:
                    return EedoumusRequestType.Set;
                default:
                    throw new NotImplementedException();
            }
        }

        private string getUrl(EedomusApi api, EedoumusRequestType requestType, EedoumusAction action, string eedomusId, string parameter)
        {
            var url = localUrl;

            if (api == EedomusApi.Distant)
            {
                url = distantUrl;
            }

            return String.Format(url, this.apiAddr, requestType.ToString().ToLower(), ActionToString(action), eedomusId, parameter, apiUser, apiSecret);
        }

        private string ActionToString(EedoumusAction action)
        {
            switch (action)
            {
                case EedoumusAction.AuthTest:
                    return "action.test";
                case EedoumusAction.PeriphValue:
                    return "periph.value";
                case EedoumusAction.PeriphCaract:
                    return "periph.caract";
                case EedoumusAction.PeriphHistory:
                    return "periph.history";
                default:
                    return String.Empty;
            }
        }
    }
}
