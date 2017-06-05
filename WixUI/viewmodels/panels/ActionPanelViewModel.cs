
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
using Olbert.Wix.messages;

namespace Olbert.Wix.ViewModels
{
    public class ActionPanelViewModel : PanelViewModel
    {
        public class SelectedAction : ViewModelBase
        {
            private bool _selected;
            private ActionPanelViewModel _vm;

            public SelectedAction( ActionPanelViewModel vm )
            {
                _vm = vm ?? throw new NullReferenceException( nameof(vm) );
            }

            public LaunchAction Action { get; set; }
            public string Description { get; set; }

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

        public ObservableCollection<SelectedAction> Actions { get; set; }

        public override ViewModelBase GetButtonsViewModel()
        {
            return new StandardButtonsViewModel
            {
                NextViewModel = { Visibility = Visibility.Collapsed }
            };
        }

        protected void SelectionUpdated()
        {
            Messenger.Default.Send<PanelButtonVisibility>( new PanelButtonVisibility(
                StandardButtonsViewModel.NextButtonID,
                Actions.Any( a => a.Selected ) ? Visibility.Visible : Visibility.Collapsed ) );
        }

    }
}
