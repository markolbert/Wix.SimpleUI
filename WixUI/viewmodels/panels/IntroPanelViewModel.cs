﻿using System.Windows;

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