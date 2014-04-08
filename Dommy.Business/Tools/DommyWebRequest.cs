//-----------------------------------------------------------------------
// <copyright file="IWebRequest.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Tools
{
    using System.IO;
    using System.Net;

    /// <summary>
    /// Interface to get stream from http web request.
    /// </summary>
    public class DommyWebRequest : IWebRequest
    {
        public Stream Create(System.Uri uri)
        {
            var request = HttpWebRequest.Create(uri);

            var response = request.GetResponse();

            return response.GetResponseStream();
        }
    }
}
