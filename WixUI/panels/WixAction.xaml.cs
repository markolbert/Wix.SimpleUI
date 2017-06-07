
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.Panels
{
    /// <summary>
    /// a UserControl for selecting what kind of Wix LaunchAction (e.g., install)
    /// to perform
    /// </summary>
    [WixPanel(PanelID, typeof(ActionPanelViewModel))]
    public partial class WixAction : UserControl
    {
        /// <summary>
        /// The panel's unique ID
        /// </summary>
        public const string PanelID = "actions";

        /// <summary>
        /// Creates an instance of the panel
        /// </summary>
        public WixAction()
        {
            InitializeComponent();
        }
    }
}
