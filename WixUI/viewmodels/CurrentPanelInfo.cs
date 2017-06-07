
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows.Controls;
using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// Defines the components of the panel currently visible to the user
    /// </summary>
    public class CurrentPanelInfo : ViewModelBase
    {
        private UserControl _panel;
        private UserControl _buttons;
        private PanelViewModel _panelVM;
        private ViewModelBase _btnVM;

        /// <summary>
        /// The panel's unique ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// An arbitrary, but unique, string identifying the stage of the installation action
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// The UserControl currently being displayed as a panel
        /// </summary>
        public UserControl Panel
        {
            get => _panel;
            set => Set<UserControl>( ref _panel, value );
        }

        /// <summary>
        /// The buttons UserControl currently being displayed below the panel
        /// </summary>
        public UserControl Buttons
        {
            get => _buttons;
            set => Set<UserControl>( ref _buttons, value );
        }

        /// <summary>
        /// The panel's view model
        /// </summary>
        public PanelViewModel PanelViewModel
        {
            get => _panelVM;
            set => Set<PanelViewModel>( ref _panelVM, value );
        }

        /// <summary>
        /// The buttons' view model
        /// </summary>
        public ViewModelBase ButtonsViewModel
        {
            get => _btnVM;
            set => Set<ViewModelBase>(ref _btnVM, value);
        }
    }
}