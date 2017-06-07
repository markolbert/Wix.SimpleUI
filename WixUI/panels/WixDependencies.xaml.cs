
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.Panels
{
    /// <summary>
    /// a UserControl for displaying the prerequisites that are needed
    /// for the installation
    /// </summary>
    [WixPanel( PanelID, typeof(DependencyPanelViewModel) ) ]
    public partial class WixDependencies : UserControl
    {
        /// <summary>
        /// The panel's unique ID
        /// </summary>
        public const string PanelID = "dependencies";

        /// <summary>
        /// Creates an instance of the panel
        /// </summary>
        public WixDependencies()
        {
            InitializeComponent();
        }
    }
}
