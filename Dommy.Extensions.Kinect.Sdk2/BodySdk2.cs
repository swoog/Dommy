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
            get { throw new NotImplementedException(); }
        }
    }
}
