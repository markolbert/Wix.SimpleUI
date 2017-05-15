using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Olbert.Wix.messages;

namespace Olbert.Wix.ViewModels
{
    public class LicensePanelViewModel : TextPanelViewModel
    {
        private Visibility _accepted;

        public Visibility Accepted
        {
            get => _accepted;

            set
            {
                Set<Visibility>( ref _accepted, value );

                Messenger.Default.Send<PanelButtonVisibility>( new PanelButtonVisibility( StandardButtonsViewModel.NextButtonID, value ) );
            }
        }

        public override ViewModelBase GetButtonsViewModel()
        {
            var retVal = (StandardButtonsViewModel) base.GetButtonsViewModel();

            retVal.NextViewModel.Visibility = Visibility.Collapsed;
            retVal.NextViewModel.Text = "Accepted >";

            return retVal;
        }
    }
}