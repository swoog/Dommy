//-----------------------------------------------------------------------
// <copyright file="KinectSkeletonListener.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.Kinect.Sdk2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business;
    using Dommy.Business.Configs;
    using Dommy.Business.Scenarios;
    using Microsoft.Kinect;

    /// <summary>
    /// Listen to movement from kinect sensor.
    /// </summary>
    [Order(50)]
    public sealed class Kinect2SkeletonListener : IKinectSkeletonListener
    {
        private BodyFrameReader frameReader;

        /// <summary>
        /// All scenarios subscribing to kinect skeleton listener.
        /// </summary>
        private Dictionary<MovementChecker, IScenario> scenarios;

        /// <summary>
        /// Instance of the kinect sensor.
        /// </summary>
        private Kinect2SensorSelector kinect = null;

        /// <summary>
        /// Current skeletons data.
        /// </summary>
        private Body[] skeletonData;

        /// <summary>
        /// Skeletong listerners.
        /// </summary>
        private IList<ISkeletonListener> skeletonListeners;

        /// <summary>
        /// Initializes a new instance of the <see cref="Kinect2SkeletonListener"/> class.
        /// </summary>
        /// <param name="kinect">Kinect sensor selector.</param>
        public Kinect2SkeletonListener(Kinect2SensorSelector kinect, IList<ISkeletonListener> skeletonListeners)
        {
            this.kinect = kinect;
            this.scenarios = new Dictionary<MovementChecker, IScenario>();
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
                this.frameReader = this.kinect.Sensor.BodyFrameSource.OpenReader();
                this.frameReader.FrameArrived += FrameArrived;
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
            var movementChecker = skeletonCheck.CreateChecker();

            this.scenarios.Add(movementChecker, scenario);
        }

        /// <summary>
        /// Make recognition when skeleton frame i ready.
        /// </summary>
        /// <param name="sender">Kinect sensor sender.</param>
        /// <param name="e">Skeleton frame.</param>
        private void FrameArrived(object sender, BodyFrameArrivedEventArgs  e)
        {
            using (var skeletonFrame = e.FrameReference.AcquireFrame())
            {
                // Sometimes we get a null frame back if no data is ready
                if (null == skeletonFrame)
                {
                    return;
                }

                // Reallocate if necessary
                // TODO : AG : Add BodyCount
                //if (null == this.skeletonData || this.skeletonData.Length != skeletonFrame.BodyFrameSource.BodyCount)
                //{
                //    this.skeletonData = new Body[skeletonFrame.BodyFrameSource.BodyCount];
                //}

                skeletonFrame.GetAndRefreshBodyData(this.skeletonData);
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
        private void Analyse(Body skeleton)
        {
            if (skeleton.IsTracked)
            {
                var body = new BodySdk2(skeleton);

                this.UpdateSkeletonHistorique(body);

                foreach (var scenario in this.scenarios)
                {
                    if (scenario.Key.Check(body))
                    {
                        scenario.Value.RunAsync();
                    }
                }
            }
        }

        private void UpdateSkeletonHistorique(BodySdk2 skeleton)
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
                kernel.Bind<IListener>().To<Kinect2SkeletonListener>().InSingletonScope();
            }
        }
    }
}
