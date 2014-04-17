using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Extensions.Kinect
{
    public interface ISkeleton
    {
        string TrackingId { get; }

        IList<JointType> GetJointTypes();

        Vector this[JointType joint] { get; }
    }
}
