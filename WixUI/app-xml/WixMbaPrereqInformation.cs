
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using GalaSoft.MvvmLight;

namespace Olbert.Wix
{
    /// <summary>
    /// Information about the prerequisites required for the installation. This is
    /// derived from MvvmLight ViewModelBase because some of its properties need
    /// to be accessible as dependency properties in the WPF UI.
    /// </summary>
    public class WixMbaPrereqInformation : ViewModelBase
    {
        private WixPackageProperties _pkgProps;

        /// <summary>
        /// The package's Wix ID
        /// </summary>
        public string PackageID { get; set; }

        /// <summary>
        /// The link to the package's licensing information
        /// </summary>
        public string LicenseUrl { get; set; }

        /// <summary>
        /// The WixPackageProperties for this prerequisite
        /// </summary>
        public WixPackageProperties Properties
        {
            get => _pkgProps;
            set => Set<WixPackageProperties>( ref _pkgProps, value );
        }
    }
}
