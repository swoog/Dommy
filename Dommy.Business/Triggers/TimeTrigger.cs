//-----------------------------------------------------------------------
// <copyright file="TimeTrigger.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Triggers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business.Scenarios;
    using Ninject.Extensions.Logging;
    public sealed class TimeTrigger : ITrigger, IDisposable
    {

        private System.Threading.Timer timer;

        private IScenario scenario;
        private ILogger logger;

        public TimeTrigger(ILogger logger)
        {
            this.logger = logger;
        }

        private void CallBack(object state)
        {
            try
            {
                this.scenario.Run();
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error in timer");
            }
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

        public DateTime StartDate { get; set; }

        public TimeSpan Tick { get; set; }

        public void Init(Engine currentEngine, Scenarios.IScenario currentScenario)
        {
            this.scenario = currentScenario;
            timer = new System.Threading.Timer(CallBack, null, TimeSpan.FromSeconds(0), this.Tick);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
