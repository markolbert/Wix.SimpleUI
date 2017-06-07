
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// An enum defining the current state of the installation process
    /// </summary>
    public enum InstallState
    {
        Initializing,
        Present,
        NotPresent,
        Applying,
        Canceled
    }
}