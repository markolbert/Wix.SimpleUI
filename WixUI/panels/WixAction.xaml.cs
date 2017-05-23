using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.panels
{
    [WixPanel(PanelID, typeof(ActionPanelViewModel))]
    public partial class WixAction : UserControl
    {
        public const string PanelID = "actions";

        public WixAction()
        {
            InitializeComponent();
        }
    }
}
