using System;
using System.Windows.Controls;

namespace Olbert.Wix.ViewModels
{
    public interface IWixViewModel
    {
        event EventHandler StartDetect;
        event EventHandler<EngineActionEventArgs> Action;
        event EventHandler CancelAction;
        event EventHandler Finished;

        WixBundleProperties BundleProperties { get; }
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
        void OnInstallationComplete();

        bool IsInDesignMode { get; }
        void Cleanup();
    }
}