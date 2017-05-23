using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.panels
{
    [ WixPanel( PanelID, typeof(TextPanelViewModel) ) ]
    public partial class WixTextScroller : UserControl
    {
        public const string PanelID = "textscroller";

        public WixTextScroller()
        {
            InitializeComponent();
        }
    }
}
