
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// The base view model for a button used in the SimpleUI.
    /// </summary>
    public class WixButtonViewModel : ViewModelBase
    {
        private string _text;
        private Visibility _visibility;
        private string _buttonID;
        private Brush _normalBkgnd;
        private Brush _hiliteBkgnd;

        /// <summary>
        /// Creates an instance of the class, with HighlightedBackground set to orange
        /// </summary>
        public WixButtonViewModel()
        {
            HighlightedBackground = new SolidColorBrush( Colors.Orange );

            //ButtonClick = new RelayCommand<string>( ButtonClickHandler );

            Messenger.Default.Register<PanelButtonVisibility>( this, PanelButtonVisibilityHandler );
        }

        ///// <summary>
        ///// The MvvmLight RelayCommand executed when the button is clicked.
        ///// </summary>
        //public RelayCommand<string> ButtonClick { get; }

        /// <summary>
        /// The text displayed in the button
        /// </summary>
        public string Text
        {
            get => _text;
            set => Set<string>( ref _text, value );
        }

        /// <summary>
        /// The button's visibility
        /// </summary>
        public Visibility Visibility
        {
            get => _visibility;
            set => Set<Visibility>( ref _visibility, value );
        }

        /// <summary>
        /// The button's ID, which must be unique
        /// </summary>
        public string ButtonID
        {
            get => _buttonID;
            set => Set<string>( ref _buttonID, value );
        }

        /// <summary>
        /// The button's normal background color
        /// </summary>
        public Brush NormalBackground
        {
            get => _normalBkgnd;
            set => Set<Brush>(ref _normalBkgnd, value);
        }

        /// <summary>
        /// The button's highlighted background color
        /// </summary>
        public Brush HighlightedBackground
        {
            get => _hiliteBkgnd;
            set => Set<Brush>(ref _hiliteBkgnd, value);
        }

        //private void ButtonClickHandler( string buttonID )
        //{
        //    Messenger.Default.Send<PanelButtonClick>( new PanelButtonClick( buttonID ) );
        //}

        private void PanelButtonVisibilityHandler( PanelButtonVisibility btnVisibility )
        {
            if( btnVisibility == null ||
                !btnVisibility.ButtonID.Equals( ButtonID, StringComparison.OrdinalIgnoreCase ) ) return;

            Visibility = btnVisibility.Visibility;
        }

    }
}
