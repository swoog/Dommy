//-----------------------------------------------------------------------
// <copyright file="TimeTrigger.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Triggers
{
    using System;
    using System.Threading;

    using Dommy.Business.Scenarios;

    using JetBrains.Annotations;

    using Ninject.Extensions.Logging;

    /// <summary>
    /// Timer trigger.
    /// </summary>
    [UsedImplicitly]
    public sealed class TimeTrigger : ITrigger, IDisposable
    {
        /// <summary>
        /// Timer used for callback scenario.
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Scenario to callback.
        /// </summary>
        private IScenario scenario;
        private ILogger logger;

        public TimeTrigger(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets the time span.
        /// </summary>
        public TimeSpan Tick { private get; set; }

        /// <summary>
        /// Initialize the timer trigger.
        /// </summary>
        /// <param name="currentEngine">Current dommy engine.</param>
        /// <param name="currentScenario">Trigger scenario.</param>
        public void Init(Engine currentEngine, IScenario currentScenario)
        {
            this.scenario = currentScenario;
            this.timer = new Timer(this.CallBack, null, TimeSpan.FromSeconds(0), this.Tick);
        }


        /// <summary>
        /// Run scenario at callback timer.
        /// </summary>
        /// <param name="state">State of the timer.</param>
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

        /// <summary>
        /// Dispose timer trigger.
        /// </summary>
        /// <param name="disposing">Indicate if dispose .Net object.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.timer != null)
                {
                    this.timer.Dispose();
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
