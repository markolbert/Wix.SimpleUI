
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    public abstract class PanelViewModel : ViewModelBase
    {
        public abstract ViewModelBase GetButtonsViewModel();
    }
}
