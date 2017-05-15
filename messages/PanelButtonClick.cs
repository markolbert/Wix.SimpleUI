namespace Olbert.Wix.messages
{
    public class PanelButtonClick
    {
        public PanelButtonClick( string buttonID )
        {
            ButtonID = buttonID;
        }

        public string ButtonID { get; }
    }
}