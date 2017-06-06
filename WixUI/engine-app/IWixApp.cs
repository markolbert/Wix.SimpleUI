
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System;
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
        /// Used to execute actions on the UI thread, so as to avoid cross-thread exceptions
        /// </summary>
        /// <typeparam name="T">The Type of the parameter passed to the Action which will
        /// be excecuted on the UI thread</typeparam>
        /// <param name="action">the Action to be executed on the UI thread</param>
        /// <param name="item">the parameter to be passed to the Action when it is invoked
        /// on the UI thread</param>
        void CrossThreadAction<T>( Action<T> action, T item );

        /// <summary>
        /// The action the Wix BootstrapperApplication is taking (e.g., install, uninstall)
        /// </summary>
        LaunchAction LaunchAction { get; }

        /// <summary>
        /// Cancels the current installation, and shuts down the Wix Bootstrapper application
        /// </summary>
        void CancelInstallation();

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

        /// <summary>
        /// Completes the Wix BootstrapperApplication's action and terminates it.
        /// </summary>
        void Finish();
    }
}