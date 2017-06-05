
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using GalaSoft.MvvmLight;

namespace Olbert.Wix
{
    public class WixMbaPrereqInformation : ViewModelBase
    {
        //private PackageState _pkgState;
        //private InstallationState _insState;
        private WixPackageProperties _pkgProps;
        //private string _dispName;

        public string PackageID { get; set; }
        public string LicenseUrl { get; set; }

        //public string DisplayName
        //{
        //    get => _dispName;
        //    set => Set<string>( ref _dispName, value );
        //}

        public WixPackageProperties Properties
        {
            get => _pkgProps;

            set
            {
                Set<WixPackageProperties>( ref _pkgProps, value );

                //DisplayName = value == null ? PackageID : value.DisplayName;
            }
        }

        //public InstallationState InstallationState
        //{
        //    get => _insState;
        //    set => Set<InstallationState>( ref _insState, value );
        //}

        //public PackageState PackageState
        //{
        //    get => _pkgState;

        //    set
        //    {
        //        Set<PackageState>( ref _pkgState, value );

        //        switch (value)
        //        {
        //            case PackageState.Absent:
        //                InstallationState = InstallationState.NotInstalled;
        //                break;

        //            case PackageState.Obsolete:
        //            case PackageState.Superseded:
        //                InstallationState = InstallationState.UpgradeNeeded;
        //                break;

        //            case PackageState.Cached:
        //            case PackageState.Present:
        //                InstallationState = InstallationState.Installed;
        //                break;

        //            default:
        //                InstallationState = InstallationState.Detecting;
        //                break;
        //        }
        //    }
        //}
    }
}
