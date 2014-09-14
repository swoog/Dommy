// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelloSkeletonCheck.cs" company="TrollCorp">
//   Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
// <summary>
//   The hello skeleton check.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using System;

    /// <summary>
    /// The hello skeleton check.
    /// </summary>
    public class HelloSkeletonCheck : ISkeletonCheck
    {
        /// <summary>
        /// The create checker.
        /// </summary>
        /// <returns>Movement checker</returns>
        public MovementChecker CreateChecker()
        {
            return new MovementChecker(BodyJointType.RightHand, TimeSpan.FromSeconds(4))
                .ToLeft(200)
                .ToRight(200);
        }
    }
}
