using GalaSoft.MvvmLight;

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

        public override ViewModelBase GetButtonsViewModel()
        {
            return new StandardButtonsViewModel();
        }
    }
}