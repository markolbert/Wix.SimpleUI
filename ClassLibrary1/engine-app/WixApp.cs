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
    public abstract class WixApp : BootstrapperApplication, IWixApp
    {
        private static Dispatcher _dispatcher { get; set; }

        private int _finalResult;
        private IntPtr _hwnd = IntPtr.Zero;

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

        public LaunchAction LaunchAction => Command.Action;

        public void CrossThreadAction<T>( Action<T> action, T item )
        {
            _dispatcher.BeginInvoke( action, item );
        }

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

        protected abstract IWixViewModel WixViewModel { get; }

        public virtual void StartDetect()
        {
            Engine.Detect();
        }

        public abstract (bool, string) ExecuteAction(LaunchAction action);

        public virtual void Finish()
        {
            _dispatcher.InvokeShutdown();
        }

        public virtual void CancelInstallation()
        {
            if (WixViewModel.InstallState == InstallState.Applying)
                WixViewModel.InstallState = InstallState.Canceled;
            else _dispatcher.InvokeShutdown();
        }

        protected virtual bool CancellationRequested(ResultEventArgs args)
        {
            if (WixViewModel.InstallState == InstallState.Canceled)
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

            WixViewModel.OnDetectionComplete();
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

            if( WixViewModel.InstallState == InstallState.Canceled ) _dispatcher.InvokeShutdown();
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
            }

            base.OnExecuteBegin( args );
        }

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
                var package = WixViewModel.BundleProperties.Packages.SingleOrDefault(
                    pkg => pkg.Package.Equals(args.PackageId, StringComparison.OrdinalIgnoreCase));

                if( package != null ) WixViewModel.ReportProgress( $"Finished with {package.DisplayName}" );
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
    }
}
