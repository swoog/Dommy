using Dommy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Win8.ViewModel
{
    public class OnOffLightActionViewModel : ActionViewModel<OnOffLightActionData>
    {
        public bool IsOn
        {
            get { return this.Data.IsOn; }
            set
            {
                var isOn = this.Data.IsOn;
                this.Set(ref isOn, value);
                this.Data.IsOn = isOn;

                ServiceSingleton.Current.RunAsync(this.Data);
            }
        }
    }
}
