//-----------------------------------------------------------------------
// <copyright file="ISynologyDownloadActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dommy.Business.Syntax;

    /// <summary>
    /// Use Synology API.
    /// </summary>
    public interface ISynologyDownloadActions : IExtendSyntax
    {
        /// <summary>
        /// Add file to synology download manager.
        /// </summary>
        /// <param name="server">Server name with port.</param>
        /// <param name="username">Username used for connect to synology.</param>
        /// <param name="password">Password used for connect to synology.</param>
        /// <param name="file">Url file to download (EDK2, HTTP, ...)</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax SynologyDownloadCreate(string server, string username, string password, string file);

        /// <summary>
        /// Add file to synology download manager.
        /// </summary>
        /// <param name="server">Server name with port.</param>
        /// <param name="username">Username used for connect to synology.</param>
        /// <param name="password">Password used for connect to synology.</param>
        /// <param name="data">Data used to create file url.</param>
        /// <param name="file">Format url file to download (EDK2, HTTP, ...)</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax SynologyDownloadCreate(string server, string username, string password, object data, string file);
    }
}
