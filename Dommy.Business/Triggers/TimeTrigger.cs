
namespace Dommy.Business.Triggers
{
    using System;
    using Dommy.Business.Scenarios;

    public sealed class TimeTrigger : ITrigger, IDisposable
    {
        public DateTime StartDate { get; set; }

        public TimeSpan Tick { get; set; }

        System.Threading.Timer timer;

        private IScenario scenario;

        public void Init(Engine currentEngine, Scenarios.IScenario currentScenario)
        {
            this.scenario = currentScenario;
            timer = new System.Threading.Timer(CallBack, null, TimeSpan.FromSeconds(0), this.Tick);
        }

        private void CallBack(object state)
        {
            this.scenario.Run();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (timer != null)
                {
                    this.timer.Dispose();
                }
            }
        }
    }
}
