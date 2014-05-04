
namespace Dommy.Extensions.Kinect.Sdk2
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Kinect;

    public class BodySdk2 : ISkeleton
    {
        private Body skeleton;

        public BodySdk2(Body skeleton)
        {
            this.skeleton = skeleton;
        }


        public string TrackingId
        {
            get
            {
                return this.skeleton.TrackingId.ToString();
            }
        }

        public IList<BodyJointType> GetJointTypes()
        {
            throw new NotImplementedException();
        }
        public Vector this[BodyJointType joint]
        {
            get { return ToVector(this.skeleton.Joints[ToKinectJointType(joint)].Position); }
        }

        private JointType ToKinectJointType(BodyJointType joint)
        {
            switch (joint)
            {
                case BodyJointType.RightHand:
                    return Microsoft.Kinect.JointType.HandRight;
                default:
                    throw new NotImplementedException();
            }
        }

        private Vector ToVector(CameraSpacePoint position)
        {
            return new Vector(position.X * 1000, position.Y * 1000, position.Z * 1000);
        }
    }
}
