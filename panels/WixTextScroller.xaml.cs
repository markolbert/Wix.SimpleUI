﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
