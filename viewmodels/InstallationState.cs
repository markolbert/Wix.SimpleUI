using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbert.Wix.viewmodels
{
    public enum InstallationState
    {
        Detecting,
        NotInstalled,
        Installed,
        UpgradeNeeded
    }
}
