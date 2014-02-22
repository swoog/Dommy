using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Extensions.Kinect.Sdk1
{
    public class SkeletonSdk1 : ISkeleton
    {
        private Microsoft.Kinect.Skeleton skeleton;

        public SkeletonSdk1(Microsoft.Kinect.Skeleton skeleton)
        {
            // TODO: Complete member initialization
            this.skeleton = skeleton;
        }
        public string TrackingId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<IJointType> GetJointTypes()
        {
            throw new NotImplementedException();
        }

        public IJointType this[IJointType joint]
        {
            get { throw new NotImplementedException(); }
        }
    }
}
