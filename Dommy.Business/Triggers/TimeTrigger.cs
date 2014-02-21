﻿using Dommy.Business.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public sealed class TimeTrigger : ITrigger, IDisposable
    {
        public DateTime StartDate { get; set; }

        public TimeSpan Tick { get; set; }

        private System.Threading.Timer timer;

        private IScenario scenario;

        public void Init(Engine engine, Scenarios.IScenario scenario)
        {
            this.scenario = scenario;
            timer = new System.Threading.Timer(CallBack, null, TimeSpan.FromSeconds(0), this.Tick);
        }

        private void CallBack(object state)
        {
            try
            {
                this.scenario.Run();
            }
            catch (Exception)
            {
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void Dispose(bool b)
        {
            if (timer != null)
            {
                this.timer.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}