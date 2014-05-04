
namespace Dommy.Extensions.Kinect
{
    using Dommy.Business.Triggers;

    public class KinectSkeletonTrigger : ITrigger
    {
        public ISkeletonCheck Checker { get; set; }

        public void Init(Business.Engine engine, Business.Scenarios.IScenario scenario)
        {
            foreach (var listener in engine.GetListeners<IKinectSkeletonListener>())
            {
                listener.Subscribe(this.Checker, scenario);
            }
        }
    }
}
