using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dommy.Extensions.Kinect.Sdk2.Tests
{
    [TestClass]
    public class BodySdk2Tests
    {
        [TestMethod]
        public void TrackingIdTest()
        {
            var kinectSkeleton = Activator.CreateInstance(Type.GetType("Microsoft.Kinect.Body, Microsoft.Kinect")) as Microsoft.Kinect.Body;
            //kinectSkeleton.TrackingId = 1;

            var skeleton = new BodySdk2(kinectSkeleton);

            Assert.AreEqual("1", skeleton.TrackingId);
        }

        [TestMethod]
        public void ThisTest()
        {
            var kinectSkeleton = Activator.CreateInstance(Type.GetType("Microsoft.Kinect.Body, Microsoft.Kinect")) as Microsoft.Kinect.Body;
            var joint = new Microsoft.Kinect.Joint
            {
                TrackingState = Microsoft.Kinect.TrackingState.Tracked,
                Position = new Microsoft.Kinect.CameraSpacePoint() { X = 1, Y = 2, Z = 3 }
            };

            var propertyInfo = joint.GetType().GetField("<JointType>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            propertyInfo.SetValueDirect(__makeref(joint), Microsoft.Kinect.JointType.HandRight);

            //kinectSkeleton.Joints[Microsoft.Kinect.JointType.HandRight] = joint;

            var skeleton = new BodySdk2(kinectSkeleton);

            Assert.AreEqual(new Vector(1, 2, 3), skeleton[JointType.RightHand]);
        }
    }
}
