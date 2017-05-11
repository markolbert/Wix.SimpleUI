using GalaSoft.MvvmLight.Messaging;
using Olbert.Wix.messages;

namespace Olbert.Wix.ViewModels
{
    public class LicensePanelViewModel : TextPanelViewModel
    {
        private bool _accepted;

        public bool Accepted
        {
            get => _accepted;

            set
            {
                Set<bool>( ref _accepted, value );

                Messenger.Default.Send<PanelButtonVisibility>( new PanelButtonVisibility( PanelButton.Next, value ) );
            }
        }

        public override ButtonsViewModel GetButtonsViewModel()
        {
            return new ButtonsViewModel() { NextVisible = false, NextText = "Accepted >" };
        }
    }
}