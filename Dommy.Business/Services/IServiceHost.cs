//-----------------------------------------------------------------------
// <copyright file="IServiceHost.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Services
{
    /// <summary>
    /// Create a service host.
    /// </summary>
    public interface IServiceHost
    {
        /// <summary>
        /// Open a service host.
        /// </summary>
        void Open();

        /// <summary>
        /// Close a service host.
        /// </summary>
        void Close();
    }
}
