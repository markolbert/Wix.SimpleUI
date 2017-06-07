
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// Defines the view model for the WixTextScroller panel
    /// </summary>
    public class TextPanelViewModel : PanelViewModel
    {
        private string _text;

        /// <summary>
        /// The text to display
        /// </summary>
        public string Text
        {
            get => _text;
            set => Set<string>( ref _text, value );
        }

        /// <summary>
        /// Overrides the base implementation to return a default/unmodified instance of StandardButtonsViewModel.
        /// </summary>
        /// <returns>a default/unmodified instance of StandardButtonsViewModel</returns>
        public override ViewModelBase GetButtonsViewModel()
        {
            return new StandardButtonsViewModel();
        }
    }
}