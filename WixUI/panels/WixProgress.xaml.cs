using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.panels
{
    [WixPanel(PanelID, typeof(ProgressPanelViewModel))]
    public partial class WixProgress : UserControl
    {
        public const string PanelID = "progress";

        public WixProgress()
        {
            InitializeComponent();
        }
    }
}
