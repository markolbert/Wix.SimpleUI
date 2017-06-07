
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.Panels
{
    /// <summary>
    /// a UserControl to display a licensing messages. The user is prevented from
    /// proceeding to the next step until he or she agrees to the license terms.
    /// </summary>
    [WixPanel(PanelID, typeof(LicensePanelViewModel))]
    public partial class WixLicense : UserControl
    {
        /// <summary>
        /// The panel's unique ID
        /// </summary>
        public const string PanelID = "license";

        /// <summary>
        /// Creates an instance of the panel
        /// </summary>
        public WixLicense()
        {
            InitializeComponent();
        }
    }
}
