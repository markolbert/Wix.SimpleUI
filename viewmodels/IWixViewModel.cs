using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Olbert.Wix.ViewModels
{
    public interface IWixViewModel
    {
        //event EventHandler StartDetect;
        //event EventHandler<EngineActionEventArgs> Action;
        //event EventHandler CancelAction;
        //event EventHandler Finished;

        WixBundleProperties BundleProperties { get; }
        LaunchAction LaunchAction { get; set; }
        IEnumerable<LaunchAction> SupportedActions { get; }
        InstallState InstallState { get; set; }
        EngineState EngineState { get; set; }
        EnginePhase EnginePhase { get; set; }

        string WindowTitle { get; set; }
        //UserControl CurrentPanel { get; set; }
        //UserControl CurrentButtons { get; set; }
        CurrentPanelInfo Current { get; }

        bool BundleInstalled { get; set; }
        void ReportProgress( string mesg );
        void ReportProgress( int phasePct );
        void OnDetectionComplete();
        void OnInstallationComplete();

        bool IsInDesignMode { get; }
        void Cleanup();
    }
}