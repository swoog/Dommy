// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISkeletonCheck.cs" company="TrollCorp">
//   Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
// <summary>
//   Defines the ISkeletonCheck type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    /// <summary>
    /// Interface of the skeleton check..
    /// </summary>
    public interface ISkeletonCheck
    {
        /// <summary>
        /// Create a checker for movement.
        /// </summary>
        /// <returns>Movement checker.</returns>
        MovementChecker CreateChecker();
    }
}
