
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.viewmodels
{
    public static class FluentButtons
    {
        public static WixButtonViewModel Show( this WixButtonViewModel vm )
        {
            vm.Visibility = Visibility.Visible;
            return vm;
        }

        public static WixButtonViewModel Hide( this WixButtonViewModel vm)
        {
            vm.Visibility=Visibility.Collapsed;
            return vm;
        }

        public static WixButtonViewModel SetText( this WixButtonViewModel vm, string text )
        {
            vm.Text = text;
            return vm;
        }

        public static WixButtonViewModel SetID(this WixButtonViewModel vm, string id)
        {
            vm.ButtonID = id;
            return vm;
        }
    }
}
