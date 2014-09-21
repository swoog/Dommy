// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EedomusHelper.cs" company="TrollCorp">
//   Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Dommy.Business.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text.RegularExpressions;
    using Dommy.Business.Actions;
    using Dommy.Business.Configs;

    using JetBrains.Annotations;

    using Ninject;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Helper to call Eedomus API.
    /// </summary>
    public class EedomusHelper
    {
        /// <summary>
        /// The local url.
        /// </summary>
        private const string LocalUrl = "http://{0}/api/{1}?action={2}&periph_id={3}&{4}&api_user={5}&api_secret={6}";

        /// <summary>
        /// The distant url.
        /// </summary>
        private const string DistantUrl = "http://api.eedomus.com/{1}?action={2}&periph_id={3}&{4}&api_user={5}&api_secret={6}";

        /// <summary>
        /// The Eedomus address.
        /// </summary>
        private readonly string apiAddr;

        /// <summary>
        /// The Eedomus user.
        /// </summary>
        private readonly string apiUser;

        /// <summary>
        /// The Eedomus secret.
        /// </summary>
        private readonly string apiSecret;

        /// <summary>
        /// Request web.
        /// </summary>
        private readonly IWebRequest requestWeb;

        /// <summary>
        /// Errors logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EedomusHelper"/> class.
        /// </summary>
        /// <param name="apiAddr">The Eedomus address.</param>
        /// <param name="apiUser">The Eedomus user.</param>
        /// <param name="apiSecret">The Eedomus secret.</param>
        /// <param name="requestWeb">The request web.</param>
        /// <param name="logger">Errors logger.</param>
        public EedomusHelper(string apiAddr, string apiUser, string apiSecret, IWebRequest requestWeb, ILogger logger)
        {
            this.apiAddr = apiAddr;
            this.apiUser = apiUser;
            this.apiSecret = apiSecret;
            this.requestWeb = requestWeb;
            this.logger = logger;
        }

        /// <summary>
        /// The Eedomus request type.
        /// </summary>
        private enum EedomusRequestType
        {
            /// <summary>
            /// Get API.
            /// </summary>
            Get,

            /// <summary>
            /// Set API.
            /// </summary>
            Set,
        }

        /// <summary>
        /// Call Eedomus API.
        /// </summary>
        /// <param name="api">Type of API (Local or Distant)</param>
        /// <param name="action">Eedomus action.</param>
        /// <param name="eedomusId">Id of the Eedomus element.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Result value.</returns>
        public string CallService(EedomusApi api, EedomusAction action, string eedomusId, string value = null)
        {
            this.logger.Info("Call eedomus {0} to {1}", eedomusId, value);

            var requestType = GetRequestType(action);

            var url = this.GetUrl(api, requestType, action, eedomusId, string.Format(CultureInfo.InvariantCulture, "value={0}", value));

            try
            {
                using (var responseStream = this.requestWeb.Create(new Uri(url)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(EedomusResult));

                    var jsonString = new StreamReader(responseStream).ReadToEnd();

                    jsonString = jsonString.Replace("é", "&#233;");
                    jsonString = jsonString.Replace("è", "&#232;");
                    jsonString = jsonString.Replace("à", "&#224;");

                    jsonString = Regex.Replace(jsonString, @"^[^\{]+", string.Empty);

                    EedomusResult result;
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
                        throw new EedomusException(string.Format(CultureInfo.InvariantCulture, "Erreur eedomus : {0}", result.Body.ErrorMsg));
                    }

                    value = result.Body.LastValue;

                    if (string.IsNullOrEmpty(value) && result.Body.History != null)
                    {
                        value = result.Body.History.OrderByDescending(k => k.Value).First().Key;
                    }

                    this.logger.Info("Eedomus indicate : {0} ({1})", result.Body.LastValue, result.Body.LastValueChange);
                }
            }
            catch (UriFormatException ex)
            {
                this.logger.Error(ex, "Url error : {0}", url);
                throw;
            }

            return value;
        }

        /// <summary>
        /// Get the request type from the Eedomus action.
        /// </summary>
        /// <param name="action">Eedomus action to execute.</param>
        /// <returns>Eedomus request type.</returns>
        private static EedomusRequestType GetRequestType(EedomusAction action)
        {
            switch (action)
            {
                case EedomusAction.AuthTest:
                case EedomusAction.PeriphCaract:
                case EedomusAction.PeriphHistory:
                    return EedomusRequestType.Get;
                case EedomusAction.PeriphValue:
                    return EedomusRequestType.Set;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Return the string text of the Eedomus action.
        /// </summary>
        /// <param name="action">Eedomus action.</param>
        /// <returns>Text of the action.</returns>
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
                    return string.Empty;
            }
        }

        /// <summary>
        /// Get the url to call API.
        /// </summary>
        /// <param name="api">API (Local or Distant)</param>
        /// <param name="requestType">Eedomus request type.</param>
        /// <param name="action">Eedomus action.</param>
        /// <param name="eedomusId">Eedomus IP.</param>
        /// <param name="parameter">Parameters of the request.</param>
        /// <returns>Url of the API.</returns>
        private string GetUrl(EedomusApi api, EedomusRequestType requestType, EedomusAction action, string eedomusId, string parameter)
        {
            var url = LocalUrl;

            if (api == EedomusApi.Distant)
            {
                url = DistantUrl;
            }

            return string.Format(CultureInfo.InvariantCulture, url, this.apiAddr, requestType.ToString().ToLower(CultureInfo.InvariantCulture), ActionToString(action), eedomusId, parameter, this.apiUser, this.apiSecret);
        }

        /// <summary>
        /// Configuration of the Eedomus helper.
        /// </summary>
        public class Config : IConfig
        {
            /// <summary>
            /// Gets or sets IP of the Eedomus box.
            /// </summary>
            /// <returns></returns>
            [UsedImplicitly]
            public string Ip { get; set; }

            /// <summary>
            /// Gets or sets user of the Eedomus API.
            /// </summary>
            /// <returns></returns>
            [UsedImplicitly]
            public string User { get; set; }

            /// <summary>
            /// Gets or sets secret of the Eedomus API.
            /// </summary>
            /// <returns></returns>
            [UsedImplicitly]
            public string Secret { get; set; }

            /// <summary>
            /// Create config of the Eedomus helper.
            /// </summary>
            /// <param name="kernel">Ninject kernel.</param>
            public void Create(IKernel kernel)
            {
                kernel.Bind<EedomusHelper>().ToSelf()
                    .WithConstructorArgument("apiAddr", this.Ip)
                    .WithConstructorArgument("apiUser", this.User)
                    .WithConstructorArgument("apiSecret", this.Secret);
            }
        }

        /// <summary>
        /// Eedomus result.
        /// </summary>
        [DataContract]
        private class EedomusResult
        {
            /// <summary>
            /// Gets or sets a value indicating whether success.
            /// </summary>
            [DataMember(Name = "success")]
            public bool Success { get; set; }

            /// <summary>
            /// Gets or sets the body.
            /// </summary>
            [DataMember(Name = "body")]
            public EedomusBody Body { get; set; }
        }

        /// <summary>
        /// Eedomus result body.
        /// </summary>
        [DataContract]
        private class EedomusBody
        {
            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the last value.
            /// </summary>
            [DataMember(Name = "last_value")]
            public string LastValue { get; set; }

            [DataMember(Name = "last_value_text")]
            public string LastValueText { get; set; }

            /// <summary>
            /// Gets or sets the last value change.
            /// </summary>
            [DataMember(Name = "last_value_change")]
            public string LastValueChange { get; set; }

            /// <summary>
            /// Gets or sets the error code.
            /// </summary>
            [DataMember(Name = "error_code")]
            public string ErrorCode { get; set; }

            /// <summary>
            /// Gets or sets the error message.
            /// </summary>
            [DataMember(Name = "error_msg")]
            public string ErrorMsg { get; set; }

            [DataMember(Name = "history")]
            public Dictionary<string, DateTime> History { get; set; }
        }
    }
}
