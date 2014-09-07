// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISkeleton.cs" company="TrollCorp">
//   Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
// <summary>
//   Defines the ISkeleton type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using System.Collections.Generic;

    /// <summary>
    /// Skeleton interface.
    /// </summary>
    public interface ISkeleton
    {
        /// <summary>
        /// Gets the tracking id.
        /// </summary>
        string TrackingId { get; }

        /// <summary>
        /// Return vector of a body joint.
        /// </summary>
        /// <param name="joint">Body joint.</param>
        /// <returns>Vector of the body joint.</returns>
        Vector this[BodyJointType joint] { get; }

        /// <summary>
        /// The get joint types.
        /// </summary>
        /// <returns>The list of body joints</returns>
        IEnumerable<BodyJointType> GetJointTypes();
    }
}
