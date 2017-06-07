
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.Panels
{
    /// <summary>
    /// a UserControl to display text in a scroller.
    /// </summary>
    [WixPanel( PanelID, typeof(TextPanelViewModel) ) ]
    public partial class WixTextScroller : UserControl
    {
        /// <summary>
        /// The panel's unique ID
        /// </summary>
        public const string PanelID = "textscroller";

        /// <summary>
        /// Creates an instance of the panel
        /// </summary>
        public WixTextScroller()
        {
            InitializeComponent();
        }
    }
}
