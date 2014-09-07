using Dommy.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Extensions.Kinect
{
    public interface IKinectSkeletonListener : IListener
    {
        void Subscribe(ISkeletonCheck skeletonCheck, Business.Scenarios.IScenario scenario);
    }
}
