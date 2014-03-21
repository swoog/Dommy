//-----------------------------------------------------------------------
// <copyright file="UsbUirtScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business.Syntax;

    /// <summary>
    /// Describe scenario with USB-UIRT module.
    /// </summary>
    public class UsbUirtScenarioDescription : IScenarioDescription
    {
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
                    using (var driver = new UsbUirt.Driver())
                    {
                        using (var l = new UsbUirt.Learner(driver))
                        {
                            infraRedCode.Code = l.Learn();
                        }
                    }

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
