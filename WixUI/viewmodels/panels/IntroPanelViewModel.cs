
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// Extends TextPanelViewModel to define a view model for the WixIntro panel
    /// </summary>
    public class IntroPanelViewModel : TextPanelViewModel
    {
        private Visibility _detecting;

        /// <summary>
        /// Controls the visibility of the UI element indicating the Wix Bootstrapper
        /// is detecting/analyzing the system
        /// </summary>
        public Visibility Detecting
        {
            get => _detecting;
            set => Set<Visibility>( ref _detecting, value );
        }
    }
}