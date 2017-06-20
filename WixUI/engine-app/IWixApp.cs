
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Olbert.Wix
{
    /// <summary>
    /// The minimal interface for a class which derives from the Wix BoostrapperApplication so
    /// that it can support the SimpleUI API
    /// </summary>
    public interface IWixApp
    {
        /// <summary>
        /// The action the Wix BootstrapperApplication is taking (e.g., install, uninstall)
        /// </summary>
        LaunchAction LaunchAction { get; }

        /// <summary>
        /// Starts the Wix BootstrapperApplication's detection phase, where it determines what packages
        /// and prerequisites are already installed and which ones need to be installed or updated.
        /// </summary>
        void StartDetect();

        /// <summary>
        /// Causes the SimpleUI system to execute a particular Wix LaunchAction
        /// </summary>
        /// <param name="action">the LaunchAction to execute</param>
        /// <returns>a tuple indicating that the action succeeded or failed, and an optional
        /// error message</returns>
        (bool, string) ExecuteAction( LaunchAction action );
    }
}