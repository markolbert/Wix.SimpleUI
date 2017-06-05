
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.panels
{
    [ WixPanel( PanelID, typeof(TextPanelViewModel) ) ]
    public partial class WixTextScroller : UserControl
    {
        public const string PanelID = "textscroller";

        public WixTextScroller()
        {
            InitializeComponent();
        }
    }
}
