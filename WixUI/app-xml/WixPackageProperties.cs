using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Olbert.Wix.viewmodels;

namespace Olbert.Wix
{
    public class WixPackageProperties : ViewModelBase
    {
        private PackageState _pkgState;
        private InstallationState _insState;
        private string _dispName;

        public string Package { get; set; }
        public bool Vital { get; set; }

        public string DisplayName
        {
            get => _dispName;
            set => Set<string>( ref _dispName, value );
        }
        public string Description { get; set; }
        public long DownloadSize { get; set; }
        public long PackageSize { get; set; }
        public long InstalledSize { get; set; }
        public PackageType PackageType { get; set; }
        public bool Permanent { get; set; }
        public string LogPathVariable { get; set; }
        public string RollbackLogPathVariable { get; set; }
        public bool Compressed { get; set; }
        public bool DisplayInternalUI { get; set; }
        public Guid ProductCode { get; set; }
        public Guid UpgradeCode { get; set; }
        public Version Version { get; set; }
        public string InstallCondition { get; set; }
        public YesNoAlways Cache { get; set; }
        public List<WixPackageFeatureInfo> Features { get; } = new List<WixPackageFeatureInfo>();
        public InstallationState InstallationState
        {
            get => _insState;
            set => Set<InstallationState>(ref _insState, value);
        }

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