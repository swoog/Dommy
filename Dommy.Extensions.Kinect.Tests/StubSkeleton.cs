
namespace Dommy.Extensions.Kinect.Tests
{
    using System;
    using System.Collections.Generic;

    public class StubSkeleton : ISkeleton
    {
        public StubSkeleton()
        {
        }

        private Dictionary<BodyJointType, Vector> dico = new Dictionary<BodyJointType, Vector>();

        public Vector this[BodyJointType joint]
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

        public StubSkeleton Set(BodyJointType rightHand, int x, int y, int z)
        {
            if (dico.ContainsKey(BodyJointType.RightHand))
            {
                dico[BodyJointType.RightHand] = new Vector(x, y, z);
            }
            else
            {
                dico.Add(BodyJointType.RightHand, new Vector(x, y, z));
            }

            return this;
        }

        public IList<BodyJointType> GetJointTypes()
        {
            throw new NotImplementedException();
        }
    }
}