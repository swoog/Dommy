//-----------------------------------------------------------------------
// <copyright file="RecodSkeletonScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using Dommy.Business.Scenarios;
    using Dommy.Business.Syntax;
    using Microsoft.Kinect;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Describe the scenario to recorde kinect sceleton recorder.
    /// </summary>
    public class RecodSkeletonScenarioDescription : IScenarioDescription, ISkeletonListener
    {
        private static List<Skeleton> skeletons = null;

        /// <summary>
        /// Create the scenario to recorde kinect sceleton recorder.
        /// </summary>
        public void Create()
        {
            Scenario.Create("Kinect recorder")
                .SpeechTrigger("enregistre le mouvement")
                .Action(this.Begin)
                .Say("Enregistrement encours")
                .Start();

            Scenario.Create("Kinect end recorder")
                .SpeechTrigger("terminer")
                .Action(this.End)
                .Say("Enregistrement terminé")
                .Start();
        }

        private bool Begin()
        {
            if (skeletons == null)
            {
                skeletons = new List<Skeleton>();
                return true;
            }

            return false;
        }
        private bool End()
        {
            if (skeletons != null)
            {
                using (var file = File.OpenWrite("kinect.txt"))
                using (var writer = new StreamWriter(file))
                {
                    foreach (var item in skeletons)
                    {
                        this.WriteSkeletonToFile(item, writer);
                    }
                }

                skeletons = null;
                return true;
            }

            return false;
        }

        private void WriteSkeletonToFile(Skeleton item, StreamWriter writer)
        {
            writer.Write("Skeleton : ");
            writer.Write(item.TrackingId);

            foreach (JointType joinType in Enum.GetValues(typeof(JointType)))
            {
                writer.Write(" {0} :", joinType);
                writer.Write(item.Joints[joinType]);
            }

        }

        public void NewSkeleton(Microsoft.Kinect.Skeleton skeleton)
        {
            if (skeletons != null)
            {
                skeletons.Add(skeleton);
            }
        }
    }
}
