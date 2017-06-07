
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// Defines a simple fluent API for setting various properties of the buttons used in SimpleUI
    /// </summary>
    public static class FluentButtons
    {
        /// <summary>
        /// Makes the button visible
        /// </summary>
        /// <param name="vm">the view model of the button</param>
        /// <returns>the view model of the button</returns>
        public static WixButtonViewModel Show( this WixButtonViewModel vm )
        {
            vm.Visibility = Visibility.Visible;
            return vm;
        }

        /// <summary>
        /// Makes the button collapsed/invisible
        /// </summary>
        /// <param name="vm">the view model of the button</param>
        /// <returns>the view model of the button</returns>
        public static WixButtonViewModel Hide( this WixButtonViewModel vm)
        {
            vm.Visibility=Visibility.Collapsed;
            return vm;
        }

        /// <summary>
        /// Sets the button's text
        /// </summary>
        /// <param name="vm">the view model of the button</param>
        /// <param name="text">the text to display in the button</param>
        /// <returns>the view model of the button</returns>
        public static WixButtonViewModel SetText( this WixButtonViewModel vm, string text )
        {
            vm.Text = text;
            return vm;
        }

        /// <summary>
        /// Sets the button's unique ID
        /// </summary>
        /// <param name="vm">the view model of the button</param>
        /// <param name="id">the button's unique ID</param>
        /// <returns>the view model of the button</returns>
        public static WixButtonViewModel SetID(this WixButtonViewModel vm, string id)
        {
            vm.ButtonID = id;
            return vm;
        }
    }
}
