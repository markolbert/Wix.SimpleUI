
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

namespace Olbert.Wix
{
    /// <summary>
    /// A class used with the MvvmLight Messenger API to communicate which button was clicked
    /// to view models which have registered to monitor the message
    /// </summary>
    public class PanelButtonClick
    {
        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="buttonID">the ID of the button, which should be unique</param>
        public PanelButtonClick( string buttonID )
        {
            ButtonID = buttonID;
        }

        /// <summary>
        /// The button's ID, which should be unique
        /// </summary>
        public string ButtonID { get; }
    }
}