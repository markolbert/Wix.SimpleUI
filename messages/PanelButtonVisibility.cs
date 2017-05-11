namespace Olbert.Wix.messages
{
    public class PanelButtonVisibility
    {
        public PanelButtonVisibility( PanelButton button, bool isVisible )
        {
            Button = button;
            IsVisible = isVisible;
        }

        public PanelButton Button { get; }
        public bool IsVisible { get; }
    }
}