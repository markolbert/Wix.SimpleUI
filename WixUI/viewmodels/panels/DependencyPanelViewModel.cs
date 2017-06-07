
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// Defines the view model used with the WixDependencies panel
    /// </summary>
    public class DependencyPanelViewModel : PanelViewModel
    {
        /// <summary>
        /// The list of prerequisites/dependencies needed for the installation to succeed
        /// </summary>
        public List<WixMbaPrereqInformation> Dependencies { get; set; }

        /// <summary>
        /// Overrides the base implementation to return an instance of StandardButtonsViewModel where
        /// the Next button's text is "Install".
        /// </summary>
        /// <returns>an instance of StandardButtonsViewModel where
        /// the Next button's text is "Install"</returns>
        public override ViewModelBase GetButtonsViewModel()
        {
            return new StandardButtonsViewModel
            {
                NextViewModel = { Text = "Install" }
            };
        }

    }
}