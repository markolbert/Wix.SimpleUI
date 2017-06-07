
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.Panels
{
    /// <summary>
    /// a UserControl to display installation progress, for both the current phase
    /// and the overall install.
    /// </summary>
    [WixPanel(PanelID, typeof(ProgressPanelViewModel))]
    public partial class WixProgress : UserControl
    {
        /// <summary>
        /// The panel's unique ID
        /// </summary>
        public const string PanelID = "progress";

        /// <summary>
        /// Creates an instance of the panel
        /// </summary>
        public WixProgress()
        {
            InitializeComponent();
        }
    }
}
