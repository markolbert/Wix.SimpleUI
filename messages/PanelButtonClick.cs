namespace Olbert.Wix.messages
{
    public class PanelButtonClick
    {
        public PanelButtonClick( PanelButton button )
        {
            Button = button;
        }

        public PanelButton Button { get; }
    }
}