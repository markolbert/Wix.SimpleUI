
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows.Controls;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.panels
{
    [ WixPanel( PanelID, typeof(IntroPanelViewModel) ) ]
    public partial class WixIntro : UserControl
    {
        public const string PanelID = "intro";

        public WixIntro()
        {
            InitializeComponent();
        }
    }
}
