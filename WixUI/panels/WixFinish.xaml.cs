using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.panels
{
    [WixPanel(PanelID, typeof(FinishPanelViewModel))]
    public partial class WixFinish : UserControl
    {
        public const string PanelID = "finish";

        public WixFinish()
        {
            InitializeComponent();
        }
    }
}
