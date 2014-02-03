using Dommy.Business.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Extensions.Kinect
{
    public class KinectSkeletonTrigger : ITrigger
    {
        public ISkeletonCheck Checker { get; set; }

        public void Init(Business.Engine engine, Business.Scenarios.IScenario scenario)
        {
            engine.Listener<KinectSkeletonListener>().Subscribe(this.Checker, scenario);
        }
    }
}
