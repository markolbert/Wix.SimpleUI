using System;
using System.Diagnostics;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Olbert.Wix.messages;

namespace Olbert.Wix.ViewModels
{
    public class FinishPanelViewModel : PanelViewModel
    {
        private string _text;
        private bool _launchApp;
        private bool _launchAppVisible;
        private string _helpUrl;
        private bool _helpUrlVisible;

        public FinishPanelViewModel()
        {
            Text = "Installation is complete. Click Finish to close the wizard.";

            HelpUrlClicked = new RelayCommand(HelpUrlClickedHandler);
        }

        public string Text
        {
            get => _text;
            set => Set<string>( ref _text, value );
        }

        public bool LaunchApp
        {
            get => _launchApp;
            set => Set<bool>( ref _launchApp, value );
        }

        public bool LaunchAppVisible
        {
            get => _launchAppVisible;
            set => Set<bool>( ref _launchAppVisible, value );
        }

        public string HelpUrl
        {
            get => _helpUrl;

            set
            {
                Set<string>( ref _helpUrl, value );

                HelpUrlVisible = Uri.TryCreate( _helpUrl, UriKind.Absolute, out Uri uriResult )
                                 && ( uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps );
            }
        }

        public bool HelpUrlVisible
        {
            get => _helpUrlVisible;
            protected set => Set<bool>(ref _helpUrlVisible, value);
        }

        public RelayCommand HelpUrlClicked { get; }

        public override ButtonsViewModel GetButtonsViewModel()
        {
            return new ButtonsViewModel()
            {
                CancelVisible = false,
                PreviousVisible = false,
                NextText = "Finish"
            };
        }

        private void HelpUrlClickedHandler()
        {
            Process.Start( new ProcessStartInfo( HelpUrl ) );
        }

    }
}