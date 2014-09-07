// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKinectSkeletonListener.cs" company="TrollCorp">
//   Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
// <summary>
//   Defines the IKinectSkeletonListener type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using Dommy.Business;
    using Dommy.Business.Scenarios;

    /// <summary>
    /// Interface for a kinect skeleton listener.
    /// </summary>
    public interface IKinectSkeletonListener : IListener
    {
        /// <summary>
        /// Subscribe to the listener.
        /// </summary>
        /// <param name="skeletonCheck">Skeleton checker</param>
        /// <param name="scenario">Scenario to execute.</param>
        void Subscribe(ISkeletonCheck skeletonCheck, IScenario scenario);
    }
}
