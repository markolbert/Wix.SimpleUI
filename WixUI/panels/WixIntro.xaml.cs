
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.Panels
{
    /// <summary>
    /// a UserControl to display introductory messages while the Wix Bootstrapper is
    /// analyzing the system.
    /// </summary>
    [WixPanel( PanelID, typeof(IntroPanelViewModel) ) ]
    public partial class WixIntro : UserControl
    {
        /// <summary>
        /// The panel's unique ID
        /// </summary>
        public const string PanelID = "intro";

        /// <summary>
        /// Creates an instance of the panel
        /// </summary>
        public WixIntro()
        {
            InitializeComponent();
        }
    }
}
