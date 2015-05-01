// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DommyWebRequest.cs" company="TrollCorp">
//   Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Dommy.Business.Tools
{
    using System.IO;
    using System.Net;

    /// <summary>
    /// Interface to get stream from http web request.
    /// </summary>
    public class DommyWebRequest : IWebRequest
    {
        /// <summary>
        /// Create stream from uri.
        /// </summary>
        /// <param name="uri">Uri of the stream.</param>
        /// <returns>An opened stream.</returns>
        public Stream Create(System.Uri uri)
        {
            var request = WebRequest.Create(uri);

            var response = request.GetResponse();

            return response.GetResponseStream();
        }
    }
}
