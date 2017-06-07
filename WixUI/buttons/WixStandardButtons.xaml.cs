
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Windows.Controls;

namespace Olbert.Wix.Buttons
{
    /// <summary>
    /// A WPF UserControl composed of three configurable WixButtons, typically used
    /// to signify Previous, Next and Cancel actions selected by the user
    /// </summary>
    public partial class WixStandardButtons : UserControl
    {
        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        public WixStandardButtons()
        {
            InitializeComponent();
        }
    }
}
