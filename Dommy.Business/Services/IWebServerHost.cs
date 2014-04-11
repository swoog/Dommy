//-----------------------------------------------------------------------
// <copyright file="IWebServerHost.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Services
{
    using System.ServiceModel;

    /// <summary>
    /// Service contract for web server host.
    /// </summary>
    [ServiceContract]
    public interface IWebServerHost
    {
        /// <summary>
        /// Indicate that the web application is started.
        /// </summary>
        [OperationContract]
        void Started();
    }
}
