
namespace Dommy.Extensions.Kinect
{
    using System;

    public class HelloSkeletonCheck : ISkeletonCheck
    {
        public MovementChecker CreateChecker()
        {
            return new MovementChecker(BodyJointType.RightHand, TimeSpan.FromSeconds(4))
                .ToLeft(200)
                .ToRight(200);
        }
    }
}
