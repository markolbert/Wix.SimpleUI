using System;
using System.Collections.Generic;

namespace Olbert.Wix
{
    public class WixPackageProperties
    {
        public string Package { get; set; }
        public bool Vital { get; set; }
        public string DisplayName { get; set; }
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
    }
}