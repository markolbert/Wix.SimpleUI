using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Olbert.Wix.messages;

namespace Olbert.Wix.ViewModels
{
    public class ButtonsViewModel: ViewModelBase
    {
        private string _nextText;
        private bool _nextVisible;
        private string _prevText;
        private bool _prevVisible;
        private string _cancelText;
        private bool _cancelVisible;

        public ButtonsViewModel()
        {
            NextText = "Next >";
            NextVisible = true;

            PreviousText = "< Previous";
            PreviousVisible = true;

            CancelText = "Cancel";
            CancelVisible = true;

            ButtonClick = new RelayCommand<PanelButton>(ButtonClickHandler);

            Messenger.Default.Register<PanelButtonVisibility>( this, PanelButtonVisibilityHandler );
        }

        public RelayCommand<PanelButton> ButtonClick { get; }

        public string NextText
        {
            get => _nextText;
            set => Set<string>( ref _nextText, value );
        }

        public bool NextVisible
        {
            get => _nextVisible;
            set => Set<bool>(ref _nextVisible, value);
        }

        public string PreviousText
        {
            get => _prevText;
            set => Set<string>(ref _prevText, value);
        }

        public bool PreviousVisible
        {
            get => _prevVisible;
            set => Set<bool>( ref _prevVisible, value);
        }

        public string CancelText
        {
            get => _cancelText;
            set => Set<string>(ref _cancelText, value);
        }

        public bool CancelVisible
        {
            get => _cancelVisible;
            set => Set<bool>(ref _cancelVisible, value);
        }

        private void ButtonClickHandler(PanelButton button)
        {
            Messenger.Default.Send<PanelButtonClick>( new PanelButtonClick( button ) );
        }

        private void PanelButtonVisibilityHandler( PanelButtonVisibility btnVisibility )
        {
            if( btnVisibility == null ) return;

            switch( btnVisibility.Button )
            {
                case PanelButton.Cancel:
                    CancelVisible = btnVisibility.IsVisible;
                    break;

                case PanelButton.Next:
                    NextVisible = btnVisibility.IsVisible;
                    break;

                case PanelButton.Previous:
                    PreviousVisible = btnVisibility.IsVisible;
                    break;
            }
        }

    }
}
