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
    using Microsoft.Kinect;

    /// <summary>
    /// Listen to movement from kinect sensor.
    /// </summary>
    public sealed class KinectSkeletonListener : IListener
    {
        /// <summary>
        /// Instance of the kinect sensor.
        /// </summary>
        private KinectSensorSelector kinect = null;

        /// <summary>
        /// Current skeletons data.
        /// </summary>
        private Skeleton[] skeletonData;

        /// <summary>
        /// Initializes a new instance of the <see cref="KinectSkeletonListener"/> class.
        /// </summary>
        /// <param name="kinect">Kinect sensor selector.</param>
        public KinectSkeletonListener(KinectSensorSelector kinect)
        {
            this.kinect = kinect;
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
            this.kinect.Sensor.SkeletonStream.Enable();
            this.kinect.Sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;
        }

        /// <summary>
        /// Stop kinect sensor.
        /// </summary>
        public void Stop()
        {
            this.kinect.Stop();
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

            Parallel.ForEach(this.skeletonData, this.Analyse);
        }

        /// <summary>
        /// Analyze a skeleton.
        /// </summary>
        /// <param name="skeleton">Kinect skeleton</param>
        private void Analyse(Skeleton skeleton)
        {
            if (skeleton.TrackingState != SkeletonTrackingState.NotTracked)
            {
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
                kernel.Bind<IListener>().To<KinectSkeletonListener>();
            }
        }
    }
}
