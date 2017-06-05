
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Collections.Generic;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Olbert.Wix
{
    public class WixPackageFeatureInfo
    {
        public WixPackageProperties Package { get; set; }
        public List<WixPackageFeatureInfo> Features { get; }= new List<WixPackageFeatureInfo>();
        public string PackageID { get; set; }
        public WixPackageFeatureInfo ParentFeature { get; set; }
        public string Parent { get; set; }
        public string Feature { get; set; }
        public long Size { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Display Display { get; set; }
        public int Level { get; set; }
        public string Directory { get; set; }
        public int Attributes { get; set; }
    }
}