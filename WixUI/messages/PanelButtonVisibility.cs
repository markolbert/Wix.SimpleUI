
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows;

namespace Olbert.Wix
{
    /// <summary>
    /// A class used with the MvvmLight Messenger API to communicate which button was made visible
    /// to view models which have registered to monitor the message
    /// </summary>
    public class PanelButtonVisibility
    {
        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="buttonID">the ID of the button, which should be unique</param>
        /// <param name="visibility">the button's current visibility</param>
        public PanelButtonVisibility( string buttonID, Visibility visibility )
        {
            ButtonID = buttonID;
            Visibility = visibility;
        }

        /// <summary>
        /// The button's ID, which should be unique
        /// </summary>
        public string ButtonID { get; }

        /// <summary>
        /// The button's current visibility
        /// </summary>
        public Visibility Visibility { get; }
    }
}