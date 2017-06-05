
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

namespace Olbert.Wix.messages
{
    public class PanelButtonClick
    {
        public PanelButtonClick( string buttonID )
        {
            ButtonID = buttonID;
        }

        public string ButtonID { get; }
    }
}