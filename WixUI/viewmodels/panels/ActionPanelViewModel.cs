
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// Defines the view model used with the WixAction panel
    /// </summary>
    public class ActionPanelViewModel : PanelViewModel
    {
        /// <summary>
        /// Defines a view model for selecting a particular LaunchAction from a list of
        /// possible actions
        /// </summary>
        public class SelectedAction : ViewModelBase
        {
            private bool _selected;
            private ActionPanelViewModel _vm;

            /// <summary>
            /// Creates an instance of the class linked to a particular panel control's view model
            /// </summary>
            /// <param name="vm">the view model for the panel control where this SelectedAction will be referenced</param>
            public SelectedAction( ActionPanelViewModel vm )
            {
                _vm = vm ?? throw new NullReferenceException( nameof(vm) );
            }

            /// <summary>
            /// A Wix LaunchAction
            /// </summary>
            public LaunchAction Action { get; set; }

            /// <summary>
            /// A description of the action
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// An MvvmLight dependency property indicating whether or not this action was selected
            /// by the user
            /// </summary>
            public bool Selected
            {
                get => _selected;

                set
                {
                    Set( ref _selected, value ); 
                    
                    _vm.SelectionUpdated();
                }
            }
        }

        /// <summary>
        /// The collection of possible LaunchActions from which the user can select a particular one
        /// </summary>
        public ObservableCollection<SelectedAction> Actions { get; set; }

        /// <summary>
        /// Overrides the base implementation to return an instance of StandardButtonsViewModel where
        /// the Next button is collapsed/invisible (it will become visible after the user makes a selection).
        /// </summary>
        /// <returns>an instance of StandardButtonsViewModel where
        /// the Next button is collapsed/invisible</returns>
        public override ViewModelBase GetButtonsViewModel()
        {
            return new StandardButtonsViewModel
            {
                NextViewModel = { Visibility = Visibility.Collapsed }
            };
        }

        private void SelectionUpdated()
        {
            Messenger.Default.Send<PanelButtonVisibility>( new PanelButtonVisibility(
                StandardButtonsViewModel.NextButtonID,
                Actions.Any( a => a.Selected ) ? Visibility.Visible : Visibility.Collapsed ) );
        }

    }
}
