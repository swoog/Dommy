//-----------------------------------------------------------------------
// <copyright file="KinectSkeletonListener.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using Dommy.Business;
    using Dommy.Business.Config;
    using Microsoft.Kinect;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class KinectSkeletonListener : IListener
    {
        /// <summary>
        /// Instance of the kinect sensor.
        /// </summary>
        private KinectSensorSelector kinect = null;

        public KinectSkeletonListener(KinectSensorSelector kinect)
        {
            this.kinect = kinect;
        }

        public void Init(Engine engine)
        {
        }

        public void Start()
        {
            this.kinect.Start();
            //    using (var skeletonFrame = this.kinect.SkeletonStream.OpenNextFrame(0))
            //    {
            //        // Sometimes we get a null frame back if no data is ready
            //        if (null == skeletonFrame)
            //        {
            //            return;
            //        }

            //        // Reallocate if necessary
            //        if (null == skeletonData || skeletonData.Length != skeletonFrame.SkeletonArrayLength)
            //        {
            //            skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
            //        }

            //        skeletonFrame.CopySkeletonDataTo(skeletonData);
            //        skeletonDrawn = false;
            //    }
        }

        public void Stop()
        {
            this.kinect.Stop();
        }

        public class Config : IConfig
        {
            public void Create(Ninject.IKernel kernel)
            {
                kernel.Bind<IListener>().To<KinectSkeletonListener>();
            }
        }
    }
}
