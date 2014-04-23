using Dommy.Extensions.Kinect;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Extensions.Kinect.Sdk2
{
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

        private Vector ToVector(CameraSpacePoint position)
        {
            return new Vector(position.X * 1000, position.Y * 1000, position.Z * 1000);
        }
    }
}
