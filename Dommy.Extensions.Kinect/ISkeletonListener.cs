//-----------------------------------------------------------------------
// <copyright file="RecodSkeletonScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    /// <summary>
    /// Interface to listen the movement of skeleton.
    /// </summary>
    public interface ISkeletonListener
    {
        /// <summary>
        /// New skeleton detected.
        /// </summary>
        /// <param name="skeleton">Kinect skeleton.</param>
        void NewSkeleton(ISkeleton skeleton);
    }
}
