
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows;
using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// Defines the view model used with the WixFinish panel
    /// </summary>
    public class FinishPanelViewModel : PanelViewModel
    {
        private string _text;
        private bool _launchApp;
        private Visibility _launchAppVisibility = Visibility.Collapsed;
        private bool _showHelp;
        private Visibility _showHelpVisibility = Visibility.Collapsed;

        /// <summary>
        /// Creates an instance of the class, with Text set to "Installation is complete. Click
        /// Finish to close the wizard."
        /// </summary>
        public FinishPanelViewModel()
        {
            Text = "Installation is complete. Click Finish to close the wizard.";
        }

        /// <summary>
        /// The text to display in the panel
        /// </summary>
        public string Text
        {
            get => _text;
            set => Set<string>( ref _text, value );
        }

        /// <summary>
        /// A flag indicating whether or not the application should be launched after the user
        /// clicks Finish
        /// </summary>
        public bool LaunchApp
        {
            get => _launchApp;
            set => Set<bool>( ref _launchApp, value );
        }

        /// <summary>
        /// The visibility of the "launch application" checkbox
        /// </summary>
        public Visibility LaunchAppVisibility
        {
            get => _launchAppVisibility;
            set => Set<Visibility>( ref _launchAppVisibility, value );
        }

        /// <summary>
        /// A flag indicating whether or not online help should be shown after the user clicks Finish.
        /// </summary>
        public bool ShowHelp
        {
            get => _showHelp;
            set => Set<bool>(ref _showHelp, value);
        }

        /// <summary>
        /// The visibility of the "show online help" checkbox
        /// </summary>
        public Visibility ShowHelpVisibility
        {
            get => _showHelpVisibility;
            set => Set<Visibility>(ref _showHelpVisibility, value);
        }

        /// <summary>
        /// Overrides the base implementation to return an instance of StandardButtonsViewModel where
        /// the Previous and Cancel buttons are collapsed/invisible and the Next button's text is set
        /// the Previous and Cancel buttons are collapsed/invisible and the Next button's text is set
        /// to "Finish".
        /// </summary>
        /// <returns>an instance of StandardButtonsViewModel where
        /// the Previous and Cancel buttons are collapsed/invisible and the Next button's text is set
        /// to "Finish".</returns>
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