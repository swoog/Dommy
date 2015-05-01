//-----------------------------------------------------------------------
// <copyright file="IWebRequest.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Tools
{
    using System.IO;

    /// <summary>
    /// Interface to get stream from http web request.
    /// </summary>
    public interface IWebRequest
    {
        /// <summary>
        /// Create stream from uri.
        /// </summary>
        /// <param name="uri">Uri of the stream.</param>
        /// <returns>An opened stream.</returns>
        Stream Create(System.Uri uri);
    }
}
