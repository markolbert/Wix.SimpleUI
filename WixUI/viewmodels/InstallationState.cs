
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// An enum defining the installation state of a package or prerequisite
    /// </summary>
    public enum InstallationState
    {
        Detecting,
        NotInstalled,
        Installed,
        UpgradeNeeded
    }
}
