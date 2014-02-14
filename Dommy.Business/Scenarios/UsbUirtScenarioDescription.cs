using Dommy.Business.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Scenarios
{
    public class UsbUirtScenarioDescription : IScenarioDescription
    {
        class IrCode
        {
            public string Code { get; set; }
        }

        public void Create()
        {
            var irCode = new IrCode { Code = "" };

            Scenario.Create()
                .SpeechTrigger("Lecture code")
                .Say("Lecture")
                .Action(() =>
                {
                    using (var driver = new UsbUirt.Driver())
                    {
                        using (var l = new UsbUirt.Learner(driver))
                        {
                            irCode.Code = l.Learn();
                        }
                    }

                    return true;
                })
                .Log("IrCode :{Code}", irCode)
                .Say("Terminé")
                .Start();
        }
    }
}
