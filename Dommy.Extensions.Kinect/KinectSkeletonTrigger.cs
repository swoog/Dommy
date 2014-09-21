// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KinectSkeletonTrigger.cs" company="TrollCorp">
//   Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
// <summary>
//   Defines the KinectSkeletonTrigger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using Dommy.Business.Triggers;

    /// <summary>
    /// Kinect skeleton trigger.
    /// </summary>
    public class KinectSkeletonTrigger : ITrigger
    {
        /// <summary>
        /// Gets or sets the skeleton checker.
        /// </summary>
        public ISkeletonCheck Checker { get; set; }

        /// <summary>
        /// Initialize the engine and scenario of the kinect skeleton trigger.
        /// </summary>
        /// <param name="engine">Dommy engine.</param>
        /// <param name="scenario">Scenario to execute for this trigger.</param>
        public void Init(Business.Engine engine, Business.Scenarios.IScenario scenario)
        {
            foreach (var listener in engine.GetListeners<IKinectSkeletonListener>())
            {
                listener.Subscribe(this.Checker, scenario);
            }
        }
    }
}
