
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.Panels
{
    /// <summary>
    /// a UserControl to display final messages, after the installation is completed, but
    /// prior to the Wix Bootstrapper shutting down.
    /// 
    /// Options are provided to show online help and to launch the installed application
    /// </summary>
    [WixPanel(PanelID, typeof(FinishPanelViewModel))]
    public partial class WixFinish : UserControl
    {
        /// <summary>
        /// The panel's unique ID
        /// </summary>
        public const string PanelID = "finish";

        /// <summary>
        /// Creates an instance of the panel
        /// </summary>
        public WixFinish()
        {
            InitializeComponent();
        }
    }
}
