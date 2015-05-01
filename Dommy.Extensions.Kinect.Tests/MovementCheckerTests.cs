using System;
using System.Threading;

namespace Dommy.Extensions.Kinect.Tests
{
    using Xunit;

    public class MovementCheckerTests
    {
        [Fact]
        public void ToRightTest()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromSeconds(1))
                .ToRight(10);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 10, 0, 0));

            Assert.True(result);
        }

        [Fact]
        public void ToRight2Test()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromSeconds(1))
                .ToRight(10);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 5, 0, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 15, 0, 0));

            Assert.True(result);
        }

        [Fact]
        public void ToRightTest3()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromDays(1))
                .ToRight(10);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -10, 0, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.True(result);
        }

        [Fact]
        public void ToLeftTest()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromDays(1))
                .ToLeft(10);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -10, 0, 0));

            Assert.True(result);
        }

        [Fact]
        public void ToRightLeftTest()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromSeconds(1))
                .ToLeft(10)
                .ToRight(20);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -10, 0, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 20, 0, 0));

            Assert.True(result);
        }

        [Fact]
        public void ToRightLeftTest2()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromDays(1))
                .ToLeft(10)
                .ToRight(20);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -10, -10, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -20, -10, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 10, -10, 0));

            Assert.True(result);
        }

        [Fact]
        public void TimeToCheck()
        {
            var checker = new MovementChecker(BodyJointType.RightHand, TimeSpan.FromSeconds(1))
                .ToLeft(10);

            var result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.False(result);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -10, 0, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, 0, 0, 0));

            Assert.False(result);

            result = checker.Check(new StubSkeleton().Set(BodyJointType.RightHand, -10, 0, 0));

            Assert.True(result);
        }
    }
}
