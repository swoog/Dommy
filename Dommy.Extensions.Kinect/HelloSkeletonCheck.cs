using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Extensions.Kinect
{
    public class HelloSkeletonCheck : ISkeletonCheck
    {
        public MovementChecker CreateChecker()
        {
            return new MovementChecker(JointType.RightHand, TimeSpan.FromSeconds(2))
                .ToLeft(10)
                .ToRight(10);
        }
    }
}
