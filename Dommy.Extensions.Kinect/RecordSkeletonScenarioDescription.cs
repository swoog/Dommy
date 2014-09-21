// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordSkeletonScenarioDescription.cs" company="TrollCorp">
//   Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using System.Collections.Generic;
    using System.IO;

    using Dommy.Business.Scenarios;
    using Dommy.Business.Syntax;

    using JetBrains.Annotations;

    /// <summary>
    /// Describe the scenario to record kinect skeleton recorder.
    /// </summary>
    [UsedImplicitly]
    public class RecordSkeletonScenarioDescription : IScenarioDescription, ISkeletonListener
    {
        /// <summary>
        /// List of skeletons.
        /// </summary>
        private static List<ISkeleton> skeletons;

        /// <summary>
        /// Create the scenario to record kinect skeleton recorder.
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

        /// <summary>
        /// Intercept a new skeleton detection.
        /// </summary>
        /// <param name="skeleton">Skeleton detected.</param>
        public void NewSkeleton(ISkeleton skeleton)
        {
            if (skeletons != null)
            {
                skeletons.Add(skeleton);
            }
        }

        /// <summary>
        /// Begin a skeleton recording.
        /// </summary>
        /// <returns>Indicate that the skeleton recording started.</returns>
        private bool Begin()
        {
            if (skeletons == null)
            {
                skeletons = new List<ISkeleton>();
                return true;
            }

            return false;
        }

        /// <summary>
        /// End a skeleton recording.
        /// </summary>
        /// <returns>Indicate that the skeleton recording ending.</returns>
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

        /// <summary>
        /// Write skeleton to a file.
        /// </summary>
        /// <param name="item">Skeleton item to write.</param>
        /// <param name="writer">Stream writer of the file.</param>
        private void WriteSkeletonToFile(ISkeleton item, StreamWriter writer)
        {
            writer.Write("Skeleton : ");
            writer.Write(item.TrackingId);

            foreach (BodyJointType joinType in item.GetJointTypes())
            {
                writer.Write(" {0} :", joinType);
                writer.Write(item[joinType]);
            }
        }
    }
}
