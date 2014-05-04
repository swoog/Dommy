
namespace Dommy.Extensions.Kinect
{
    using System.Collections.Generic;

    public interface ISkeleton
    {
        string TrackingId { get; }

        IList<BodyJointType> GetJointTypes();

        Vector this[BodyJointType joint] { get; }
    }
}
