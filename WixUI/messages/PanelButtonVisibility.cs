
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows;

namespace Olbert.Wix.messages
{
    public class PanelButtonVisibility
    {
        public PanelButtonVisibility( string buttonID, Visibility visibility )
        {
            ButtonID = buttonID;
            Visibility = visibility;
        }

        public string ButtonID { get; }
        public Visibility Visibility { get; }
    }
}