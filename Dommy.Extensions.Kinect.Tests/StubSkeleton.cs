using System;
using System.Collections.Generic;

namespace Dommy.Extensions.Kinect.Tests
{
    public class StubSkeleton : ISkeleton
    {
        public StubSkeleton()
        {
        }

        private Dictionary<JointType, Vector> dico = new Dictionary<JointType, Vector>();

        public Vector this[JointType joint]
        {
            get
            {
                return this.dico[joint];
            }
        }

        public string TrackingId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public StubSkeleton Set(JointType rightHand, int x, int y, int z)
        {
            if (dico.ContainsKey(JointType.RightHand))
            {
                dico[JointType.RightHand] = new Vector(x, y, z);
            }else
            {
                dico.Add(JointType.RightHand, new Vector(x, y, z));
            }

            return this;
        }

        public IList<JointType> GetJointTypes()
        {
            throw new NotImplementedException();
        }
    }
}