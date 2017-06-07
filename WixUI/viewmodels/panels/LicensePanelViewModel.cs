
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// Extends TextPanelViewModel to define a view model for the WixLicense panel
    /// </summary>
    public class LicensePanelViewModel : TextPanelViewModel
    {
        private bool _accepted;

        /// <summary>
        /// Flag indicating whether or not the license terms were accepted
        /// </summary>
        public bool Accepted
        {
            get => _accepted;

            set
            {
                Set<bool>( ref _accepted, value );

                Messenger.Default.Send<PanelButtonVisibility>( new PanelButtonVisibility(
                    StandardButtonsViewModel.NextButtonID, value ? Visibility.Visible : Visibility.Collapsed ) );
            }
        }

        /// <summary>
        /// Overrides the base implementation to return an instance of StandardButtonsViewModel where
        /// the Next button is collapsed/invisible. It gets made visible when the license terms are accepted.
        /// </summary>
        /// <returns>Overrides the base implementation to return an instance of StandardButtonsViewModel where
        /// the Next button is collapsed/invisible.</returns>
        public override ViewModelBase GetButtonsViewModel()
        {
            var retVal = (StandardButtonsViewModel) base.GetButtonsViewModel();

            retVal.NextViewModel.Visibility = Visibility.Collapsed;

            return retVal;
        }
    }
}