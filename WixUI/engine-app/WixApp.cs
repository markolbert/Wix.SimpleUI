
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Interop;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Olbert.JumpForJoy.WPF;
using Olbert.Wix.views;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix
{
    /// <summary>
    /// Extends the Wix BootstrapperApplication class so that it can interact with
    /// the SimpleUI API
    /// </summary>
    public abstract class WixApp : BootstrapperApplication, IWixApp
    {
        private static Dispatcher _dispatcher { get; set; }

        private int _finalResult;
        private IntPtr _hwnd = IntPtr.Zero;

        /// <summary>
        /// Creates an instance of the class. When executed in Debug mode, pauses execution
        /// until a debugger is attached.
        /// </summary>
        protected WixApp()
        {
            WaitForDebugger();
        }

        [ Conditional( "WAITFORDEBUGGER" ) ]
        private void WaitForDebugger()
        {
            System.Diagnostics.Debugger.Launch();

            while( !System.Diagnostics.Debugger.IsAttached )
            {
                Thread.Sleep( 100 );
            }
        }

        /// <summary>
        /// The action the Wix BootstrapperApplication is taking (e.g., install, uninstall)
        /// </summary>
        public LaunchAction LaunchAction => Command.Action;

        /// <summary>
        /// Executes an action on the UI thread via Dispatcher.CurrentDispatcher, so as to avoid cross-thread exceptions
        /// </summary>
        /// <typeparam name="T">The Type of the parameter passed to the Action which will
        /// be excecuted on the UI thread</typeparam>
        /// <param name="action">the Action to be executed on the UI thread</param>
        /// <param name="item">the parameter to be passed to the Action when it is invoked
        /// on the UI thread</param>
        public void CrossThreadAction<T>( Action<T> action, T item )
        {
            _dispatcher.BeginInvoke( action, item );
        }

        /// <summary>
        /// Called by the Wix framework to start BootstrapperApplication execution. Creates and
        /// displays a WPF Window (WixWindow) to provide a user interface.
        /// </summary>
        protected override void Run()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            if ( WixViewModel.LaunchAction != LaunchAction.Unknown
                && WixViewModel.SupportedActions.All( x => x != WixViewModel.LaunchAction ) )
            {
                new J4JMessageBox().Title( "Action Not Supported" )
                    .Message( $"The requested action ({WixViewModel.LaunchAction}) is not supported" )
                    .ButtonText( "Okay" )
                    .ButtonVisibility( true, false, false )
                    .ShowMessageBox();

                Engine.Quit( _finalResult );

                return;
            }

            var mainWindow = new WixWindow { DataContext = WixViewModel };
            _hwnd = new WindowInteropHelper( mainWindow ).Handle;

            mainWindow.Show();

            Dispatcher.Run();

            Engine.Quit( _finalResult );
        }

        #region main view model and extensibility points

        /// <summary>
        /// The view model for the installation, which exposes Wix functionality and properties
        /// to the WPF user interface created by the SimpleUI framework.
        /// 
        /// Must be implemented in a derived class in order to tie a particular implementation
        /// of IWixViewModel to a particular instance of IWixApp
        /// </summary>
        protected abstract IWixViewModel WixViewModel { get; }

        /// <summary>
        /// Starts the Wix detection process by calling Engine.Detect()
        /// </summary>
        public virtual void StartDetect()
        {
            Engine.Detect();
        }

        /// <summary>
        /// Executes a particular Wix LaunchAction.
        /// 
        /// This must be implemented in a derived class in order to provide a particular UI experience.
        /// </summary>
        /// <param name="action">the LaunchAction to execute</param>
        /// <returns>a tuple indicating that the action succeeded or failed, and an optional
        /// error message</returns>
        public abstract (bool, string) ExecuteAction(LaunchAction action);

        /// <summary>
        /// Closes out the Wix installer process by call Dispatcher.Current.InvokeShutdown()
        /// </summary>
        public virtual void Finish()
        {
            _dispatcher.InvokeShutdown();
        }

        /// <summary>
        /// Cancels, or starts the cancellation of, the current Wix action. 
        /// 
        /// If WixViewModel.InstallState is 
        /// InstallState.Applying, WixViewModel.InstallState is set to InstallState.Canceled.
        /// 
        /// If WixViewModel.InstallState is anything other than InstallState.Applying, invokes
        /// Dispatcher.Current.InvokeShutdown().
        /// </summary>
        public virtual void CancelInstallation()
        {
            if (WixViewModel.InstallState == InstallState.Applying)
                WixViewModel.InstallState = InstallState.Canceled;
            else _dispatcher.InvokeShutdown();
        }

        /// <summary>
        /// Checks to see if the Wix installer is in the process of being canceled. If it is,
        /// args.Result is set to Result.Cancel to notify the event caller of the fact that the
        /// installer is being canceled.
        /// </summary>
        /// <param name="args">the event handler parameter to update</param>
        /// <returns>true if WixViewModel.InstallState is InstallState.Canceled, false otherwise.</returns>
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

        /// <summary>
        /// Overrides the base implementation to capture information about the bundle being
        /// detected and the Wix Bootstrapper engine's state.
        /// </summary>
        /// <param name="args">the DetectBeginEventArgs object provided by the Wix Bootstrapper</param>
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

        /// <summary>
        /// Overrides the base implementation to capture information on whether or not the package
        /// is installed on the system (i.e., sets PackageState)
        /// </summary>
        /// <param name="args">the DetectPackageCompleteEventArgs object provided by the Wix Bootstrapper</param>
        protected override void OnDetectPackageComplete( DetectPackageCompleteEventArgs args )
        {
            base.OnDetectPackageComplete( args );

            var pkg = WixViewModel.BundleProperties.Packages.SingleOrDefault(
                p => p.Package.Equals( args.PackageId, StringComparison.OrdinalIgnoreCase ) );

            if( pkg != null ) pkg.PackageState = args.State;
        }

        /// <summary>
        /// Overrides the base implementation to inform WixViewModel that the detection phase
        /// is complete by raising WixViewModel.OnDetectionComplete().
        /// </summary>
        /// <param name="args">the DetectCompleteEventArgs object provided by the Wix Bootstrapper</param>
        protected override void OnDetectComplete( DetectCompleteEventArgs args )
        {
            base.OnDetectComplete( args );

            WixViewModel.OnDetectionComplete();
        }

        #endregion

        #region planning phase

        /// <summary>
        /// Overrides base implementation to notify WixViewModel that the planning phase
        /// has begun
        /// </summary>
        /// <param name="args">the PlanBeginEventArgs object provided by the Wix Bootstrapper</param>
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

        /// <summary>
        /// Overrides base implementation to notify WixViewModel that the planning phase
        /// has completed
        /// </summary>
        /// <param name="args">the PlanCompleteEventArgs object provided by the Wix Bootstrapper</param>
        protected override void OnPlanComplete( PlanCompleteEventArgs args )
        {
            WixViewModel.EngineState = EngineState.PlanningComplete;

            if( WixViewModel.InstallState == InstallState.Canceled ) _dispatcher.InvokeShutdown();
            else
            {
                WixViewModel.ReportProgress("Beginning installation");
                Engine.Apply( _hwnd );
            }

            base.OnPlanComplete( args );
        }

        #endregion

        #region applying phase

        /// <summary>
        /// Overrides the base implementation to inform WixViewModel that the application
        /// phase has begun
        /// </summary>
        /// <param name="args">the ApplyBeginEventArgs object provided by the Wix Bootstrapper</param>
        protected override void OnApplyBegin( ApplyBeginEventArgs args )
        {
            WixViewModel.EngineState = EngineState.Applying;
            WixViewModel.EnginePhase = EnginePhase.Caching;
            WixViewModel.InstallState = InstallState.Applying;

            base.OnApplyBegin( args );
        }

        #region applying: caching phase

        /// <summary>
        /// Overrides the base implementation to inform WixViewModel that caching
        /// downloads has begun
        /// </summary>
        /// <param name="args">the CacheBeginEventArgs object provided by the Wix Bootstrapper</param>
        protected override void OnCacheBegin( CacheBeginEventArgs args )
        {
            if( !CancellationRequested( args ) )
            {
                WixViewModel.EngineState = EngineState.ApplyingCaching;
                WixViewModel.ReportProgress("Downloading packages");
            }

            base.OnCacheBegin( args );
        }

        /// <summary>
        /// Overrides the base implementation to inform WixViewModel about the caching
        /// progress that has occurred
        /// </summary>
        /// <param name="args">the CacheAcquireProgressEventArgs object provided by the Wix Bootstrapper</param>
        protected override void OnCacheAcquireProgress( CacheAcquireProgressEventArgs args )
        {
            if( !CancellationRequested( args ) )
                WixViewModel.ReportProgress( args.OverallPercentage );

            base.OnCacheAcquireProgress( args );
        }

        /// <summary>
        /// Overrides the base implementation to inform WixViewModel that caching
        /// has completed
        /// </summary>
        /// <param name="args">the CacheCompleteEventArgs object provided by the Wix Bootstrapper</param>
        protected override void OnCacheComplete( CacheCompleteEventArgs args )
        {
            WixViewModel.EngineState = EngineState.ApplyingCached;

            if( WixViewModel.InstallState == InstallState.Canceled ) _dispatcher.InvokeShutdown();
            else WixViewModel.ReportProgress("Package download complete");

            base.OnCacheComplete( args );
        }

        #endregion

        #region applying: execution phase

        /// <summary>
        /// Overrides the base implementation to inform WixViewModel that the execution phase
        /// has begun
        /// </summary>
        /// <param name="args">the ExecuteBeginEventArgs object provided by the Wix Bootstrapper</param>
        protected override void OnExecuteBegin( ExecuteBeginEventArgs args )
        {
            if( !CancellationRequested( args ) )
            {
                WixViewModel.EngineState = EngineState.ApplyingExecuting;
                WixViewModel.EnginePhase = EnginePhase.Executing;
            }

            base.OnExecuteBegin( args );
        }

        /// <summary>
        /// Overrides the base implementation to inform WixViewModel that installation of
        /// a specific package has begun
        /// </summary>
        /// <param name="args">the ExecutePackageBeginEventArgs object provided by the Wix Bootstrapper</param>
        protected override void OnExecutePackageBegin( ExecutePackageBeginEventArgs args )
        {
            if( !CancellationRequested( args ) )
            {
                var package = WixViewModel.BundleProperties.Packages.SingleOrDefault(
                    pkg => pkg.Package.Equals( args.PackageId, StringComparison.OrdinalIgnoreCase ) );

                if( package != null ) WixViewModel.ReportProgress( $"Working on {package.DisplayName}" );
            }

            base.OnExecutePackageBegin( args );
        }

        /// <summary>
        /// Overrides the base implementation to inform WixViewModel about the progress made
        /// in the execution phase
        /// </summary>
        /// <param name="args">the ExecuteProgressEventArgs object provided by the Wix Bootstrapper</param>
        protected override void OnExecuteProgress( ExecuteProgressEventArgs args )
        {
            if( !CancellationRequested( args ) )
                WixViewModel.ReportProgress( args.OverallPercentage );

            base.OnExecuteProgress( args );
        }

        /// <summary>
        /// Overrides the base implementation to inform WixViewModel that installation of
        /// a specific package has completed
        /// </summary>
        /// <param name="args">the ExecutePackageCompleteEventArgs object provided by the Wix Bootstrapper</param>
        protected override void OnExecutePackageComplete( ExecutePackageCompleteEventArgs args )
        {
            if( !CancellationRequested( args ) )
            {
                var package = WixViewModel.BundleProperties.Packages.SingleOrDefault(
                    pkg => pkg.Package.Equals(args.PackageId, StringComparison.OrdinalIgnoreCase));

                if( package != null ) WixViewModel.ReportProgress( $"Finished with {package.DisplayName}" );
                WixViewModel.OnInstallationComplete();
            }

            base.OnExecutePackageComplete( args );
        }

        #endregion

        /// <summary>
        /// Overrides the base implementation to inform WixViewModel that the apply phase
        /// has completed
        /// </summary>
        /// <param name="args">the ApplyCompleteEventArgs object provided by the Wix Bootstrapper</param>
        protected override void OnApplyComplete( ApplyCompleteEventArgs args )
        {
            WixViewModel.EngineState = EngineState.ApplyComplete;
            WixViewModel.EnginePhase = EnginePhase.Finished;

            base.OnApplyComplete( args );

            _finalResult = args.Status;
        }

        #endregion
    }
}
