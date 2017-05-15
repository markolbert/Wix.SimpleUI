using System;
using System.Windows.Controls;

namespace Olbert.Wix.ViewModels
{
    public interface IWixViewModel
    {
        event EventHandler<EngineActionEventArgs> Action;
        event EventHandler CancelAction;

        WixBundleProperties BundleProperties { get; }
        InstallState InstallState { get; set; }
        EngineState EngineState { get; set; }

        string WindowTitle { get; set; }
        UserControl CurrentPanel { get; set; }
        UserControl CurrentButtons { get; set; }

        bool BundleInstalled { get; set; }
        CachingInfo CachingInfo { get; set; }
        ExecutionInfo ExecutionInfo { get; set; }

        bool IsInDesignMode { get; }
        void Cleanup();
    }
}