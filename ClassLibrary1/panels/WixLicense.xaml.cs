using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.panels
{
    [WixPanel(PanelID, typeof(LicensePanelViewModel))]
    public partial class WixLicense : UserControl
    {
        public const string PanelID = "license";

        public WixLicense()
        {
            InitializeComponent();
        }
    }
}
