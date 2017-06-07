
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Collections.Generic;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// Defines the required interface for any class acting as a view model for the SimpleUI
    /// </summary>
    public interface IWixViewModel
    {
        /// <summary>
        /// The installation parameters and properties exposed by the Wix BootstrapperApplication
        /// </summary>
        WixBundleProperties BundleProperties { get; }

        /// <summary>
        /// The action (e.g., install, uninstall) being processed
        /// </summary>
        LaunchAction LaunchAction { get; set; }

        /// <summary>
        /// The LaunchActions supported by this UI; may be only a subset of what Wix provides
        /// </summary>
        IEnumerable<LaunchAction> SupportedActions { get; }

        /// <summary>
        /// The current state of the action being processed
        /// </summary>
        InstallState InstallState { get; set; }

        /// <summary>
        /// The current state of the Wix Bootstrapper engine
        /// </summary>
        EngineState EngineState { get; set; }

        /// <summary>
        /// The current phase of the action being processed
        /// </summary>
        EnginePhase EnginePhase { get; set; }

        /// <summary>
        /// The window title displayed by the SimpleUI
        /// </summary>
        string WindowTitle { get; set; }

        /// <summary>
        /// Information about the current panel being displayed to the user
        /// </summary>
        CurrentPanelInfo Current { get; }

        /// <summary>
        /// Flag indicating whether or not the targeted bundle is already installed
        /// </summary>
        bool BundleInstalled { get; set; }

        /// <summary>
        /// Report Wix Bootstrapper engine status information through the SimpleUI. 
        /// </summary>
        /// <param name="mesg">a status message</param>
        void ReportProgress( string mesg );

        /// <summary>
        /// Report Wix Bootstrapper engine progress information through the SimpleUI. 
        /// </summary>
        /// <param name="phasePct">the percent progress (which means different things at different
        /// stages of the Wix Bootstrapper's efforts)</param>
        void ReportProgress( int phasePct );

        /// <summary>
        /// Report that the Wix Bootstrapper's engine detection process is complete
        /// </summary>
        void OnDetectionComplete();

        /// <summary>
        /// Report that the Wix Bootstrapper's engine has completed the LaunchAction
        /// </summary>
        void OnInstallationComplete();
    }
}