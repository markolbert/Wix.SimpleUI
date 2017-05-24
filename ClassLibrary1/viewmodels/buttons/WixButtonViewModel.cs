using System;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Olbert.Wix.messages;

namespace Olbert.Wix.ViewModels
{
    public class WixButtonViewModel : ViewModelBase
    {
        private string _text;
        private Visibility _visibility;
        private string _buttonID;
        private Brush _normalBkgnd;
        private Brush _hiliteBkgnd;

        public WixButtonViewModel()
        {
            HighlightedBackground = new SolidColorBrush( Colors.Orange );

            ButtonClick = new RelayCommand<string>( ButtonClickHandler );

            Messenger.Default.Register<PanelButtonVisibility>( this, PanelButtonVisibilityHandler );
        }

        public RelayCommand<string> ButtonClick { get; }

        public string Text
        {
            get => _text;
            set => Set<string>( ref _text, value );
        }

        public Visibility Visibility
        {
            get => _visibility;
            set => Set<Visibility>( ref _visibility, value );
        }

        public string ButtonID
        {
            get => _buttonID;
            set => Set<string>( ref _buttonID, value );
        }

        public Brush NormalBackground
        {
            get => _normalBkgnd;
            set => Set<Brush>(ref _normalBkgnd, value);
        }

        public Brush HighlightedBackground
        {
            get => _hiliteBkgnd;
            set => Set<Brush>(ref _hiliteBkgnd, value);
        }

        private void ButtonClickHandler( string buttonID )
        {
            Messenger.Default.Send<PanelButtonClick>( new PanelButtonClick( buttonID ) );
        }

        private void PanelButtonVisibilityHandler( PanelButtonVisibility btnVisibility )
        {
            if( btnVisibility == null ||
                !btnVisibility.ButtonID.Equals( ButtonID, StringComparison.OrdinalIgnoreCase ) ) return;

            Visibility = btnVisibility.Visibility;
        }

    }
}
