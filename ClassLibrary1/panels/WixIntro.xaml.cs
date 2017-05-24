using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.panels
{
    [ WixPanel( PanelID, typeof(IntroPanelViewModel) ) ]
    public partial class WixIntro : UserControl
    {
        public const string PanelID = "intro";

        public WixIntro()
        {
            InitializeComponent();
        }
    }
}
