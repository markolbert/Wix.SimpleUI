
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;
using Olbert.Wix.messages;

namespace Olbert.Wix
{
    /// <summary>
    /// A customization of the WPF Button class, which adds a ButtonID and a NormalBackground Brush
    /// property, as well as a system for translating button clicks to MvvmLight Messenger messages
    /// for use within the MvvmLight API.
    /// </summary>
    public class WixButton : Button
    {
        /// <summary>
        /// The button's ID
        /// </summary>
        public static readonly DependencyProperty ButtonIDProperty =
            DependencyProperty.Register( nameof(ButtonID), typeof(string), typeof(WixButton),
                new PropertyMetadata( String.Empty ) );

        /// <summary>
        /// The Brush used to render the button's normal (i.e., not highlighted) background
        /// </summary>
        public static readonly DependencyProperty NormalBackgroundProperty =
            DependencyProperty.Register("NormalBackground", typeof(Brush), typeof(WixButton),
                new PropertyMetadata(Brushes.LightGray));

        /// <summary>
        /// Creates an instance and sets up the button click event handler
        /// </summary>
        public WixButton()
        {
            this.Click += WixButton_Click;
        }

        /// <summary>
        /// The button's ID, which should be unique
        /// </summary>
        public string ButtonID
        {
            get => (string) GetValue( ButtonIDProperty );
            set => SetValue( ButtonIDProperty, value );
        }

        /// <summary>
        /// The Brush used to fill the button's background when it is in the normal (i.e.,
        /// not highlighted) state
        /// </summary>
        public Brush NormalBackground
        {
            get => (Brush) GetValue( NormalBackgroundProperty );
            set => SetValue( NormalBackgroundProperty, value );
        }

        private void WixButton_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<PanelButtonClick>( new PanelButtonClick( ButtonID ) );
        }

    }
}
