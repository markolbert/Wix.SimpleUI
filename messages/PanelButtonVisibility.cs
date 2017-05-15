using System.Windows;

namespace Olbert.Wix.messages
{
    public class PanelButtonVisibility
    {
        public PanelButtonVisibility( string buttonID, Visibility visibility )
        {
            ButtonID = buttonID;
            Visibility = visibility;
        }

        public string ButtonID { get; }
        public Visibility Visibility { get; }
    }
}