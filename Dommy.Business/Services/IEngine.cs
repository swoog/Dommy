//-----------------------------------------------------------------------
// <copyright file="IEngine.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Services
{
    using System.ServiceModel;

    /// <summary>
    /// Service contract for engine.
    /// </summary>
    [ServiceContract]
    public interface IEngine
    {
        /// <summary>
        /// Get engine name.
        /// </summary>
        /// <returns>Engine name.</returns>
        [OperationContract]
        string GetEngineName();
    }
}
