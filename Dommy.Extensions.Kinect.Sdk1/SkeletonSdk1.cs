using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Dommy.Extensions.Kinect.Sdk1
{
    public class SkeletonSdk1 : ISkeleton
    {
        private Microsoft.Kinect.Skeleton skeleton;

        public SkeletonSdk1(Microsoft.Kinect.Skeleton skeleton)
        {
            this.skeleton = skeleton;
        }
        public string TrackingId
        {
            get
            {
                return string.Format("{0}", this.skeleton.TrackingId);
            }
        }

        public IList<JointType> GetJointTypes()
        {
            throw new NotImplementedException();
        }

        public Vector this[JointType joint]
        {
            get { return ToVector(this.skeleton.Joints[ToKinectJointType(joint)].Position); }
        }

        private Microsoft.Kinect.JointType ToKinectJointType(JointType joint)
        {
            switch (joint)
            {
                case JointType.RightHand:
                    return Microsoft.Kinect.JointType.HandRight;
                default:
                    throw new NotImplementedException();
            }
        }

        private Vector ToVector(SkeletonPoint position)
        {
            return new Vector(position.X, position.Y, position.Z);
        }
    }
}
