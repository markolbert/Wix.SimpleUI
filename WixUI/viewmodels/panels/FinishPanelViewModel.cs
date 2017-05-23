using System;
using System.Diagnostics;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Olbert.Wix.ViewModels
{
    public class FinishPanelViewModel : PanelViewModel
    {
        private string _text;
        private bool _launchApp;
        private Visibility _launchAppVisibility = Visibility.Collapsed;
        private bool _showHelp;
        private Visibility _showHelpVisibility = Visibility.Collapsed;

        public FinishPanelViewModel()
        {
            Text = "Installation is complete. Click Finish to close the wizard.";
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

        public Visibility LaunchAppVisibility
        {
            get => _launchAppVisibility;
            set => Set<Visibility>( ref _launchAppVisibility, value );
        }

        public bool ShowHelp
        {
            get => _showHelp;
            set => Set<bool>(ref _showHelp, value);
        }

        public Visibility ShowHelpVisibility
        {
            get => _showHelpVisibility;
            set => Set<Visibility>(ref _showHelpVisibility, value);
        }

        public override ViewModelBase GetButtonsViewModel()
        {
            var retVal = new StandardButtonsViewModel();

            retVal.CancelViewModel.Visibility = Visibility.Collapsed;
            retVal.PreviousViewModel.Visibility = Visibility.Collapsed;
            retVal.NextViewModel.Text = "Finish";

            return retVal;
        }

    }
}