
namespace Dommy.Extensions.Kinect
{
    using Dommy.Business;

    public interface IKinectSkeletonListener : IListener
    {
        void Subscribe(ISkeletonCheck skeletonCheck, Business.Scenarios.IScenario scenario);
    }
}
