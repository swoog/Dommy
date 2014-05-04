
namespace Dommy.Extensions.Kinect.Tests
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MovementCheckerTests
    {
        [TestMethod]
        public void ToRightTest()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromSeconds(1))
                .ToRight(10);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 10, 0, 0));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ToRight2Test()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromSeconds(1))
                .ToRight(10);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 5, 0, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 15, 0, 0));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ToRightTest3()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromDays(1))
                .ToRight(10);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -10, 0, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ToLeftTest()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromDays(1))
                .ToLeft(10);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -10, 0, 0));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ToRightLeftTest()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromSeconds(1))
                .ToLeft(10)
                .ToRight(20);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -10, 0, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 20, 0, 0));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ToRightLeftTest2()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromDays(1))
                .ToLeft(10)
                .ToRight(20);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -10, -10, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -20, -10, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 10, -10, 0));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TimeToCheck()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromSeconds(1))
                .ToLeft(10);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.IsFalse(result);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -10, 0, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.IsFalse(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -10, 0, 0));

            Assert.IsTrue(result);
        }
    }
}
