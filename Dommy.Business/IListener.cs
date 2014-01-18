//-----------------------------------------------------------------------
// <copyright file="IListener.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    /// <summary>
    /// Interface of a listener.
    /// </summary>
    public interface IListener
    {
        /// <summary>
        /// Initialize listener with the dommy engine.
        /// </summary>
        /// <param name="engine">Dommy engine.</param>
        void Init(Engine engine);

        /// <summary>
        /// Start the listener.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the listener.
        /// </summary>
        void Stop();
    }
}
