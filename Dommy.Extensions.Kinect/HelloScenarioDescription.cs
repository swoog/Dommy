//-----------------------------------------------------------------------
// <copyright file="HelloScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using Dommy.Business.Scenarios;
    using Dommy.Business.Syntax;

    /// <summary>
    /// Describe hello world kinect scenario.
    /// </summary>
    public class HelloScenarioDescription : IScenarioDescription
    {
        /// <summary>
        /// Create hello world scenario.
        /// </summary>
        public void Create()
        {
            Scenario.Create("Kinect hello")
                .KinectTrigger<HelloSkeletonCheck>()
                .Say("Bonjour");
        }
    }
}
