
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Olbert.Wix.viewmodels;

namespace Olbert.Wix
{
    /// <summary>
    /// Defines the properties of a Wix installation package. This is derived from
    /// MvvmLight's ViewModelBase because some of the properties need to be
    /// dependency properties accessible from a WPF UI.
    /// </summary>
    public class WixPackageProperties : ViewModelBase
    {
        private PackageState _pkgState;
        private InstallationState _insState;
        private string _dispName;

        /// <summary>
        /// The Wix package ID
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// TBD
        /// </summary>
        public bool Vital { get; set; }

        /// <summary>
        /// The display name of the package
        /// </summary>
        public string DisplayName
        {
            get => _dispName;
            set => Set<string>( ref _dispName, value );
        }

        /// <summary>
        /// A description of the package
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The size, in bytes, of the package when it is downloaded (which may not be the
        /// ultimate package size, for a number of reasons).
        /// </summary>
        public long DownloadSize { get; set; }

        /// <summary>
        /// The size, in bytes, of the package when it is ready to be installed.
        /// </summary>
        public long PackageSize { get; set; }

        /// <summary>
        /// The size, in bytes, of the package after it's installed
        /// </summary>
        public long InstalledSize { get; set; }

        /// <summary>
        /// The type of package (e.g., MSI)
        /// </summary>
        public PackageType PackageType { get; set; }

        /// <summary>
        /// TBD
        /// </summary>
        public bool Permanent { get; set; }

        /// <summary>
        /// TBD
        /// </summary>
        public string LogPathVariable { get; set; }

        /// <summary>
        /// TBD
        /// </summary>
        public string RollbackLogPathVariable { get; set; }

        /// <summary>
        /// TBD
        /// </summary>
        public bool Compressed { get; set; }

        /// <summary>
        /// TBD
        /// </summary>
        public bool DisplayInternalUI { get; set; }

        /// <summary>
        /// The package's Wix product code
        /// </summary>
        public Guid ProductCode { get; set; }

        /// <summary>
        /// The package's Wix upgrade code
        /// </summary>
        public Guid UpgradeCode { get; set; }

        /// <summary>
        /// The version of the package
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// TBD
        /// </summary>
        public string InstallCondition { get; set; }

        /// <summary>
        /// TBD
        /// </summary>
        public YesNoAlways Cache { get; set; }

        /// <summary>
        /// The features supported by the package
        /// </summary>
        public List<WixPackageFeatureInfo> Features { get; } = new List<WixPackageFeatureInfo>();

        /// <summary>
        /// The package's installation state on the system (e.g., is it already installed?)
        /// </summary>
        public InstallationState InstallationState
        {
            get => _insState;
            set => Set<InstallationState>(ref _insState, value);
        }

        /// <summary>
        /// The package's state on the system.
        /// 
        /// Setting this property also sets the InstallationState
        /// </summary>
        public PackageState PackageState
        {
            get => _pkgState;

            set
            {
                Set<PackageState>(ref _pkgState, value);

                switch (value)
                {
                    case PackageState.Absent:
                        InstallationState = InstallationState.NotInstalled;
                        break;

                    case PackageState.Obsolete:
                    case PackageState.Superseded:
                        InstallationState = InstallationState.UpgradeNeeded;
                        break;

                    case PackageState.Cached:
                    case PackageState.Present:
                        InstallationState = InstallationState.Installed;
                        break;

                    default:
                        InstallationState = InstallationState.Detecting;
                        break;
                }
            }
        }
    }
}