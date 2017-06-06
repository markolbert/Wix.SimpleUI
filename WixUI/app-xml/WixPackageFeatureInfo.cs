
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Collections.Generic;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Olbert.Wix
{
    /// <summary>
    /// Information about a feature in a Wix package
    /// </summary>
    public class WixPackageFeatureInfo
    {
        /// <summary>
        /// The WixPackageProperties for which this feature is defined
        /// </summary>
        public WixPackageProperties Package { get; set; }

        /// <summary>
        /// List of child features
        /// </summary>
        public List<WixPackageFeatureInfo> Features { get; }= new List<WixPackageFeatureInfo>();

        /// <summary>
        /// The Wix ID for the package of which this is a feature
        /// </summary>
        public string PackageID { get; set; }

        /// <summary>
        /// The parent feature of this instance (can be null for a first level feature)
        /// </summary>
        public WixPackageFeatureInfo ParentFeature { get; set; }

        /// <summary>
        /// The ID of the parent feature (can be null for a first level feature)
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// The feature's Wix ID
        /// </summary>
        public string Feature { get; set; }

        /// <summary>
        /// The number of bytes required for this feature
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// The title of this feature
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of this feature
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// TBD
        /// </summary>
        public Display Display { get; set; }

        /// <summary>
        /// TBD
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// TBD
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// TBD
        /// </summary>
        public int Attributes { get; set; }
    }
}