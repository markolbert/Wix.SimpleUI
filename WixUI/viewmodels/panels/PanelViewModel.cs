
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// Defines a base class, derived from MvvmLight's ViewModelBase, for the panel
    /// controls used in SimpleUI
    /// </summary>
    public abstract class PanelViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets the view model to be used with the buttons control associated with
        /// the panel UserControl linked to this view model
        /// </summary>
        /// <returns>the view model to be used with the buttons control associated with
        /// the panel UserControl linked to this view model</returns>
        public abstract ViewModelBase GetButtonsViewModel();
    }
}
