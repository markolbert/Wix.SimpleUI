
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.panels
{
    [WixPanel(PanelID, typeof(ActionPanelViewModel))]
    public partial class WixAction : UserControl
    {
        public const string PanelID = "actions";

        public WixAction()
        {
            InitializeComponent();
        }
    }
}
