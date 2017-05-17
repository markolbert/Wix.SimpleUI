﻿using System;
using System.Linq;
using System.Windows.Interop;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Olbert.Wix.views;
using Olbert.Wix.ViewModels;
using Serilog;

namespace Olbert.Wix
{
    public abstract class WixApp : BootstrapperApplication
    {
        private static Dispatcher Dispatcher { get; set; }

        private int _finalResult;
        private IntPtr _hwnd = IntPtr.Zero;

        protected WixApp()
        {
            System.Diagnostics.Debugger.Launch();
        }

        protected override void Run()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;

            WixViewModel.CancelAction += _vm_CancelAction;
            WixViewModel.StartDetect += _vm_StartDetect;
            WixViewModel.Action += _vm_Action;
            WixViewModel.Finished += _vm_Finished;

            var mainWindow = new WixWindow { DataContext = WixViewModel };
            _hwnd = new WindowInteropHelper( mainWindow ).Handle;

            mainWindow.Show();

            Dispatcher.Run();

            Engine.Quit( _finalResult );
        }

        #region main view model and extensibility points

        protected abstract IWixViewModel WixViewModel { get; }

        protected virtual void OnAction( EngineActionEventArgs args )
        {
        }

        protected virtual bool CancellationRequested( ResultEventArgs args )
        {
            if( WixViewModel.InstallState == InstallState.Canceled )
            {
                args.Result = Result.Cancel;
                return true;
            }

            return false;
        }

        #endregion

        #region detect phase

        protected override void OnDetectBegin( DetectBeginEventArgs args )
        {
            if( !CancellationRequested( args ) )
            {
                WixViewModel.BundleInstalled = args.Installed;
                WixViewModel.EngineState = EngineState.Detecting;
                WixViewModel.EnginePhase = EnginePhase.Detect;
            }

            base.OnDetectBegin( args );
        }

        protected override void OnDetectPackageComplete( DetectPackageCompleteEventArgs args )
        {
            base.OnDetectPackageComplete( args );

            var pkg = WixViewModel.BundleProperties.Packages.SingleOrDefault(
                p => p.Package.Equals( args.PackageId, StringComparison.OrdinalIgnoreCase ) );

            if( pkg != null ) pkg.PackageState = args.State;
        }

        protected override void OnDetectComplete( DetectCompleteEventArgs args )
        {
            base.OnDetectComplete( args );

            WixViewModel.EngineState = EngineState.DetectionComplete;

            var toInstall = WixViewModel.BundleProperties.Packages
                .Where( pkg => !WixViewModel.BundleProperties.Prerequisites
                    .Any( pr => pr.PackageID.Equals( pkg.Package, StringComparison.OrdinalIgnoreCase ) ) )
                .Count( pkg =>
                    pkg.PackageState == PackageState.Absent
                    || pkg.PackageState == PackageState.Obsolete
                    || pkg.PackageState == PackageState.Superseded );

            WixViewModel.InstallState = toInstall == 0 ? InstallState.Present : InstallState.NotPresent;
        }

        #endregion

        #region planning phase

        protected override void OnPlanBegin( PlanBeginEventArgs args )
        {
            if( !CancellationRequested( args ) )
            {
                WixViewModel.EngineState = EngineState.Planning;
                WixViewModel.EnginePhase = EnginePhase.Detect;
                WixViewModel.ReportProgress( "Starting planning phase" );
            }

            base.OnPlanBegin( args );
        }

        protected override void OnPlanComplete( PlanCompleteEventArgs args )
        {
            WixViewModel.EngineState = EngineState.PlanningComplete;

            if( WixViewModel.InstallState == InstallState.Canceled ) Dispatcher.InvokeShutdown();
            else
            {
                WixViewModel.ReportProgress("Beginning installation");
                Engine.Apply( _hwnd );
            }

            base.OnPlanComplete( args );
        }

        #endregion

        #region applying phase

        protected override void OnApplyBegin( ApplyBeginEventArgs args )
        {
            WixViewModel.EngineState = EngineState.Applying;
            WixViewModel.EnginePhase = EnginePhase.Caching;
            WixViewModel.InstallState = InstallState.Applying;

            base.OnApplyBegin( args );
        }

        #region applying: caching phase

        protected override void OnCacheBegin( CacheBeginEventArgs args )
        {
            if( !CancellationRequested( args ) )
            {
                WixViewModel.EngineState = EngineState.ApplyingCaching;
                WixViewModel.ReportProgress("Downloading packages");
            }

            base.OnCacheBegin( args );
        }

        protected override void OnCacheAcquireProgress( CacheAcquireProgressEventArgs args )
        {
            if( !CancellationRequested( args ) )
                WixViewModel.ReportProgress( args.OverallPercentage );

            base.OnCacheAcquireProgress( args );
        }

        protected override void OnCacheComplete( CacheCompleteEventArgs args )
        {
            WixViewModel.EngineState = EngineState.ApplyingCached;

            if( WixViewModel.InstallState == InstallState.Canceled ) Dispatcher.InvokeShutdown();
            else WixViewModel.ReportProgress("Package download complete");

            base.OnCacheComplete( args );
        }

        #endregion

        #region applying: execution phase

        protected override void OnExecuteBegin( ExecuteBeginEventArgs args )
        {
            if( !CancellationRequested( args ) )
            {
                WixViewModel.EngineState = EngineState.ApplyingExecuting;
                WixViewModel.EnginePhase = EnginePhase.Executing;
                WixViewModel.ReportProgress("Installing packages");
            }

            base.OnExecuteBegin( args );
        }

        protected override void OnExecutePackageBegin( ExecutePackageBeginEventArgs args )
        {
            if( !CancellationRequested( args ) )
            {
                var package = WixViewModel.BundleProperties.Packages.SingleOrDefault(
                    pkg => pkg.Package.Equals( args.PackageId, StringComparison.OrdinalIgnoreCase ) );

                if( package != null ) WixViewModel.ReportProgress( $"Installing {package.DisplayName}" );
            }

            base.OnExecutePackageBegin( args );
        }

        protected override void OnExecuteProgress( ExecuteProgressEventArgs args )
        {
            if( !CancellationRequested( args ) )
                WixViewModel.ReportProgress( args.OverallPercentage );

            base.OnExecuteProgress( args );
        }

        protected override void OnExecutePackageComplete( ExecutePackageCompleteEventArgs args )
        {
            if( !CancellationRequested( args ) )
            {
                WixViewModel.ReportProgress("Installation complete");
                WixViewModel.OnInstallationComplete();
            }

            base.OnExecutePackageComplete( args );
        }

        #endregion

        protected override void OnApplyComplete( ApplyCompleteEventArgs args )
        {
            WixViewModel.EngineState = EngineState.ApplyComplete;
            WixViewModel.EnginePhase = EnginePhase.Finished;

            base.OnApplyComplete( args );

            _finalResult = args.Status;
        }

        #endregion

        #region viewmodel event handlers

        private void _vm_StartDetect(object sender, EventArgs e)
        {
            Engine.Detect();
        }

        private void _vm_Action( object sender, EngineActionEventArgs args )
        {
            OnAction( args );
        }

        private void _vm_CancelAction( object sender, EventArgs e )
        {
            if( WixViewModel.InstallState == InstallState.Applying ) WixViewModel.InstallState = InstallState.Canceled;
            else Dispatcher.InvokeShutdown();
        }

        private void _vm_Finished( object sender, EventArgs e )
        {
            Dispatcher.InvokeShutdown();
        }

        #endregion
    }
}
