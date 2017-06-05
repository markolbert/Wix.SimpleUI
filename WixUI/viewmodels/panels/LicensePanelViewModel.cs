
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows;
using GalaSoft.MvvmLight;
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

                Messenger.Default.Send<PanelButtonVisibility>( new PanelButtonVisibility(
                    StandardButtonsViewModel.NextButtonID, value ? Visibility.Visible : Visibility.Collapsed ) );
            }
        }

        public override ViewModelBase GetButtonsViewModel()
        {
            var retVal = (StandardButtonsViewModel) base.GetButtonsViewModel();

            retVal.NextViewModel.Visibility = Visibility.Collapsed;

            return retVal;
        }
    }
}