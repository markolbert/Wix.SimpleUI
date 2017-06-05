
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    public class TextPanelViewModel : PanelViewModel
    {
        private string _text;

        public string Text
        {
            get => _text;
            set => Set<string>( ref _text, value );
        }

        public override ViewModelBase GetButtonsViewModel()
        {
            return new StandardButtonsViewModel();
        }
    }
}