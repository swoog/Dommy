
namespace Dommy.Extensions.Kinect.Sdk1.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SkeletonSdk1Tests
    {
        [TestMethod]
        public void TrackingIdTest()
        {
            var kinectSkeleton = new Microsoft.Kinect.Skeleton();
            kinectSkeleton.TrackingId = 1;

            var skeleton = new SkeletonSdk1(kinectSkeleton);

            Assert.AreEqual("1", skeleton.TrackingId);
        }

        [TestMethod]
        public void ThisTest()
        {
            var kinectSkeleton = new Microsoft.Kinect.Skeleton();
            var joint = new Microsoft.Kinect.Joint
            {
                TrackingState = Microsoft.Kinect.JointTrackingState.Tracked,
                Position = new Microsoft.Kinect.SkeletonPoint() { X = 1, Y = 2, Z = 3 }
            };

            var propertyInfo = joint.GetType().GetField("<JointType>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            propertyInfo.SetValueDirect(__makeref(joint), Microsoft.Kinect.JointType.HandRight);

            kinectSkeleton.Joints[Microsoft.Kinect.JointType.HandRight] = joint;

            var skeleton = new SkeletonSdk1(kinectSkeleton);

            Assert.AreEqual(new Vector(1, 2, 3), skeleton[BodyJointType.RightHand]);
        }
    }
}
