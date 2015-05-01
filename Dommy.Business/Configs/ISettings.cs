using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Configs
{
    public interface ISettings
    {
        IList<SettingsInfo> GetSettings();
    }
}
