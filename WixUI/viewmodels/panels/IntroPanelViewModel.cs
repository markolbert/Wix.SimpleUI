
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows;

namespace Olbert.Wix.ViewModels
{
    public class IntroPanelViewModel : TextPanelViewModel
    {
        private Visibility _detecting;

        public Visibility Detecting
        {
            get => _detecting;
            set => Set<Visibility>( ref _detecting, value );
        }
    }
}