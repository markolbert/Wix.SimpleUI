
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// Defines a standard set of buttons (previous, next, cancel) which can be used in
    /// the SimpleUI
    /// </summary>
    public class StandardButtonsViewModel: ViewModelBase
    {
        /// <summary>
        /// The unique ID of the previous button
        /// </summary>
        public const string PreviousButtonID = "previous";

        /// <summary>
        /// The unique ID of the next button
        /// </summary>
        public const string NextButtonID = "next";

        /// <summary>
        /// The unique ID of the cancel button
        /// </summary>
        public const string CancelButtonID = "cancel";

        private WixButtonViewModel _prevVM;
        private WixButtonViewModel _nextVM;
        private WixButtonViewModel _cancelVM;

        /// <summary>
        /// Creates an instance of the class, initializing the button view models as follows:
        /// 
        /// Previous:
        /// - ID set to PreviousButtonID
        /// - Text set to "&lt; Previous"
        /// - Visible
        /// - NormalBackground set to #bb911e
        /// 
        /// Next:
        /// - ID set to NextButtonID
        /// - Text set to "Next &gt;"
        /// - Visible
        /// - NormalBackground set to #252315
        /// 
        /// Cancel:
        /// - ID set to CancelButtonID
        /// - Text set to "Cancel"
        /// - Visible
        /// - NormalBackground set to #bc513e
        /// </summary>
        public StandardButtonsViewModel()
        {
            PreviousViewModel = new WixButtonViewModel
            {
                ButtonID = PreviousButtonID,
                Text = "< Previous",
                Visibility = Visibility.Visible,
                NormalBackground = (SolidColorBrush) new BrushConverter().ConvertFrom( "#bb911e" )
            };

            NextViewModel = new WixButtonViewModel
            {
                ButtonID = NextButtonID,
                Text = "Next >",
                Visibility = Visibility.Visible,
                NormalBackground = (SolidColorBrush) new BrushConverter().ConvertFrom( "#252315" )
            };

            CancelViewModel = new WixButtonViewModel
            {
                ButtonID = CancelButtonID,
                Text = "Cancel",
                Visibility = Visibility.Visible,
                NormalBackground = (SolidColorBrush) new BrushConverter().ConvertFrom( "#bc513e" )
            };
        }

        /// <summary>
        /// The view model for the previous button
        /// </summary>
        public WixButtonViewModel PreviousViewModel
        {
            get => _prevVM;
            set => Set<WixButtonViewModel>( ref _prevVM, value );
        }

        /// <summary>
        /// The view model for the next button
        /// </summary>
        public WixButtonViewModel NextViewModel
        {
            get => _nextVM;
            set => Set<WixButtonViewModel>(ref _nextVM, value);
        }

        /// <summary>
        /// The view model for the cancel button
        /// </summary>
        public WixButtonViewModel CancelViewModel
        {
            get => _cancelVM;
            set => Set<WixButtonViewModel>(ref _cancelVM, value);
        }

    }
}
