//-----------------------------------------------------------------------
// <copyright file="KinectSkeletonListener.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business;
    using Dommy.Business.Config;
    using Dommy.Business.Scenarios;
    using Microsoft.Kinect;
    using Ninject.Extensions.Conventions;

    /// <summary>
    /// Listen to movement from kinect sensor.
    /// </summary>
    [Order(50)]
    public sealed class KinectSkeletonListener : IListener
    {
        /// <summary>
        /// All scenarios subscribing to kinect skeleton listener.
        /// </summary>
        private Dictionary<ISkeletonCheck, IScenario> scenarios;

        /// <summary>
        /// Instance of the kinect sensor.
        /// </summary>
        private KinectSensorSelector kinect = null;

        /// <summary>
        /// Current skeletons data.
        /// </summary>
        private Skeleton[] skeletonData;

        /// <summary>
        /// Skeletong listerners.
        /// </summary>
        private IList<ISkeletonListener> skeletonListeners;

        /// <summary>
        /// Initializes a new instance of the <see cref="KinectSkeletonListener"/> class.
        /// </summary>
        /// <param name="kinect">Kinect sensor selector.</param>
        public KinectSkeletonListener(KinectSensorSelector kinect, IList<ISkeletonListener> skeletonListeners)
        {
            this.kinect = kinect;
            this.scenarios = new Dictionary<ISkeletonCheck, IScenario>();
            this.skeletonListeners = skeletonListeners;
        }

        /// <summary>
        /// Initialize kinect sensor.
        /// </summary>
        /// <param name="engine">Dommy engine.</param>
        public void Init(Engine engine)
        {
            this.kinect.Start();
        }

        /// <summary>
        /// Start kinect skeleton stream.
        /// </summary>
        public void Start()
        {
            if (this.kinect.IsKinectFound)
            {
                this.kinect.Sensor.SkeletonStream.Enable();
                this.kinect.Sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;
            }
        }

        /// <summary>
        /// Stop kinect sensor.
        /// </summary>
        public void Stop()
        {
            if (this.kinect.IsKinectFound)
            {
                this.kinect.Stop();
            }
        }

        /// <summary>
        /// Subscribe to the skeleton listener.
        /// </summary>
        /// <param name="skeletonCheck"></param>
        /// <param name="scenario"></param>
        public void Subscribe(ISkeletonCheck skeletonCheck, Business.Scenarios.IScenario scenario)
        {
            this.scenarios.Add(skeletonCheck, scenario);
        }

        /// <summary>
        /// Make recognition when skeleton frame i ready.
        /// </summary>
        /// <param name="sender">Kinect sensor sender.</param>
        /// <param name="e">Skeleton frame.</param>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var skeletonFrame = e.OpenSkeletonFrame())
            {
                // Sometimes we get a null frame back if no data is ready
                if (null == skeletonFrame)
                {
                    return;
                }

                // Reallocate if necessary
                if (null == this.skeletonData || this.skeletonData.Length != skeletonFrame.SkeletonArrayLength)
                {
                    this.skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                }

                skeletonFrame.CopySkeletonDataTo(this.skeletonData);
            }

            foreach (var s in this.skeletonData)
            {
                this.Analyse(s);
            }
        }

        /// <summary>
        /// Analyze a skeleton.
        /// </summary>
        /// <param name="skeleton">Kinect skeleton</param>
        private void Analyse(Skeleton skeleton)
        {
            if (skeleton.TrackingState != SkeletonTrackingState.NotTracked)
            {
                this.UpdateSkeletonHistorique(skeleton);

                foreach (var scenario in this.scenarios)
                {
                    if (scenario.Key.Check(skeleton))
                    {
                        scenario.Value.RunAsync();
                    }
                }
            }
        }

        private void UpdateSkeletonHistorique(Skeleton skeleton)
        {
            foreach (var item in this.skeletonListeners)
            {
                item.NewSkeleton(skeleton);
            }
        }

        /// <summary>
        /// Configuration of kinect skeleton.
        /// </summary>
        public class Config : IConfig
        {
            /// <summary>
            /// Create kinect skeleton Ninject configuration.
            /// </summary>
            /// <param name="kernel">Ninject kernel.</param>
            public void Create(Ninject.IKernel kernel)
            {
                kernel.Bind(c => c.FromAssembliesMatching("*.dll")
                    .SelectAllClasses()
                    .InheritedFrom<ISkeletonListener>()
                    .BindUsingRegex("ISkeletonListener")
                    .Configure(conf => conf.InSingletonScope()));
                kernel.Bind<IListener>().To<KinectSkeletonListener>().InSingletonScope();
            }
        }
    }
}
