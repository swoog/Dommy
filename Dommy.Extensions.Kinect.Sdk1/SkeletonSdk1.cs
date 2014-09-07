
namespace Dommy.Extensions.Kinect.Sdk1
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Kinect;

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

        public IEnumerable<BodyJointType> GetJointTypes()
        {
            throw new NotImplementedException();
        }

        public Vector this[BodyJointType joint]
        {
            get { return ToVector(this.skeleton.Joints[ToKinectJointType(joint)].Position); }
        }

        private Microsoft.Kinect.JointType ToKinectJointType(BodyJointType joint)
        {
            switch (joint)
            {
                case BodyJointType.RightHand:
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
