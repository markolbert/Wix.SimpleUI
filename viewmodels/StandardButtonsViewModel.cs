using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Olbert.Wix.messages;

namespace Olbert.Wix.ViewModels
{
    public class StandardButtonsViewModel: ViewModelBase
    {
        public const string PreviousButtonID = "previous";
        public const string NextButtonID = "next";
        public const string CancelButtonID = "cancel";

        private WixButtonViewModel _prevVM;
        private WixButtonViewModel _nextVM;
        private WixButtonViewModel _cancelVM;

        public StandardButtonsViewModel()
        {
            PreviousViewModel = new WixButtonViewModel
            {
                ButtonID = PreviousButtonID,
                Text = "< Previous",
                Visibility = Visibility.Visible
            };

            NextViewModel = new WixButtonViewModel
            {
                ButtonID = NextButtonID,
                Text = "Next >",
                Visibility = Visibility.Visible
            };

            CancelViewModel = new WixButtonViewModel
            {
                ButtonID = CancelButtonID,
                Text = "Cancel",
                Visibility = Visibility.Visible
            };
        }

        public WixButtonViewModel PreviousViewModel
        {
            get => _prevVM;
            set => Set<WixButtonViewModel>( ref _prevVM, value );
        }

        public WixButtonViewModel NextViewModel
        {
            get => _nextVM;
            set => Set<WixButtonViewModel>(ref _nextVM, value);
        }

        public WixButtonViewModel CancelViewModel
        {
            get => _cancelVM;
            set => Set<WixButtonViewModel>(ref _cancelVM, value);
        }

    }
}
