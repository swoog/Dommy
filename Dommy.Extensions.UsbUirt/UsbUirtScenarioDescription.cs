//-----------------------------------------------------------------------
// <copyright file="UsbUirtScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.UsbUirt
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business.Syntax;
    using Dommy.Business.Scenarios;

    /// <summary>
    /// Describe scenario with USB-UIRT module.
    /// </summary>
    public class UsbUirtScenarioDescription : IScenarioDescription
    {
        private ILearner learner;

        public UsbUirtScenarioDescription(ILearner learner)
        {
            this.learner = learner;
        }

        /// <summary>
        /// Create scenarios.
        /// </summary>
        public void Create()
        {
            var infraRedCode = new IrCode { Code = string.Empty };

            Scenario.Create()
                .SpeechTrigger("Lecture code")
                .Say("Lecture")
                .Action(() =>
                {
                    infraRedCode.Code = this.learner.Learn();
                    return true;
                })
                .Log("IrCode :{Code}", infraRedCode)
                .Say("Terminé")
                .Start();
        }

        /// <summary>
        /// Class used for get infrared code.
        /// </summary>
        private class IrCode
        {
            /// <summary>
            /// Gets or sets infra red code.
            /// </summary>
            public string Code { get; set; }
        }
    }
}
