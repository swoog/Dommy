//-----------------------------------------------------------------------
// <copyright file="KinectScenarioSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using Dommy.Business.Syntax;

    /// <summary>
    /// Kinect scenario syntax.
    /// </summary>
    public static class KinectScenarioSyntax
    {
        /// <summary>
        /// Initialize the scenario with kinect trigger.
        /// </summary>
        /// <typeparam name="T">Type of the Skeleton checker.</typeparam>
        /// <param name="scenario">Scenario to add trigger.</param>
        /// <returns>Scenario syntax trigger.</returns>
        public static ITriggerScenarioSyntax KinectTrigger<T>(this ITriggerSyntax scenario)
            where T : ISkeletonCheck, new()
        {
            var check = new T();

            var kinect = new KinectSkeletonTrigger();

            kinect.Checker = check;

            scenario.Trigger(kinect);

            return scenario as ITriggerScenarioSyntax;
        }
    }
}
