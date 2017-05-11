namespace Olbert.Wix.ViewModels
{
    public class TextPanelViewModel : PanelViewModel
    {
        private string _text;

        public string Text
        {
            get => _text;
            set => Set<string>( ref _text, value );
        }

        public override ButtonsViewModel GetButtonsViewModel()
        {
            return new ButtonsViewModel { PreviousVisible = false };
        }
    }
}