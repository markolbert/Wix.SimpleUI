using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.panels
{
    [ WixPanel( PanelID, typeof(DependencyPanelViewModel) ) ]
    public partial class WixDependencies : UserControl
    {
        public const string PanelID = "dependencies";

        public WixDependencies()
        {
            InitializeComponent();
        }
    }
}
