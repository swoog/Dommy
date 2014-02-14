//-----------------------------------------------------------------------
// <copyright file="RecodSkeletonScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using Microsoft.Kinect;

    /// <summary>
    /// Interface to listen the movement of skeleton.
    /// </summary>
    public interface ISkeletonListener
    {
        /// <summary>
        /// New skeleton detected.
        /// </summary>
        /// <param name="skeleton">Kinect skeleton.</param>
        void NewSkeleton(Skeleton skeleton);
    }
}
