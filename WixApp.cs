using System;
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
        private int _cachingProgress;

        protected WixApp()
        {
            System.Diagnostics.Debugger.Launch();
        }

        protected override void Run()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;

            WixViewModel.CancelAction += _vm_CancelAction;
            WixViewModel.Action += _vm_Action;

            Engine.Detect();

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
            }

            base.OnDetectBegin( args );
        }

        protected override void OnDetectComplete( DetectCompleteEventArgs args )
        {
            WixViewModel.EngineState = EngineState.DetectionComplete;

            base.OnDetectComplete( args );
        }

        #endregion

        #region planning phase

        protected override void OnPlanBegin( PlanBeginEventArgs args )
        {
            if( !CancellationRequested( args ) )
                WixViewModel.EngineState = EngineState.Planning;

            base.OnPlanBegin( args );
        }

        protected override void OnPlanComplete( PlanCompleteEventArgs args )
        {
            WixViewModel.EngineState = EngineState.PlanningComplete;

            if( WixViewModel.InstallState == InstallState.Canceled ) Dispatcher.InvokeShutdown();
            else Engine.Apply( _hwnd );

            base.OnPlanComplete( args );
        }

        #endregion

        #region applying phase

        protected override void OnApplyBegin( ApplyBeginEventArgs args )
        {
            WixViewModel.EngineState = EngineState.Applying;
            WixViewModel.InstallState = InstallState.Applying;

            base.OnApplyBegin( args );
        }

        #region applying: caching phase

        protected override void OnCacheBegin( CacheBeginEventArgs args )
        {
            if( !CancellationRequested( args ) )
                WixViewModel.EngineState = EngineState.ApplyingCaching;

            base.OnCacheBegin( args );
        }

        protected override void OnCacheAcquireProgress( CacheAcquireProgressEventArgs args )
        {
            _cachingProgress = args.OverallPercentage;

            WixViewModel.CachingInfo = new CachingInfo
            {
                BytesCached = args.Progress,
                BytesToCache = args.Total,
                CachingPercent = args.OverallPercentage,
                OverallPercent = args.OverallPercentage / 2,
                PackageOrContainerID = args.PackageOrContainerId,
                PayloadID = args.PayloadId
            };

            base.OnCacheAcquireProgress( args );
        }

        protected override void OnCacheComplete( CacheCompleteEventArgs args )
        {
            WixViewModel.EngineState = EngineState.ApplyingCached;

            if( WixViewModel.InstallState == InstallState.Canceled ) Dispatcher.InvokeShutdown();

            base.OnCacheComplete( args );
        }

        #endregion

        #region applying: execution phase

        protected override void OnExecuteBegin( ExecuteBeginEventArgs args )
        {
            if( !CancellationRequested( args ) )
                WixViewModel.EngineState = EngineState.ApplyingExecuting;

            base.OnExecuteBegin( args );
        }

        protected override void OnExecutePackageBegin( ExecutePackageBeginEventArgs args )
        {
            CancellationRequested( args );

            base.OnExecutePackageBegin( args );
        }

        protected override void OnExecuteProgress( ExecuteProgressEventArgs args )
        {
            if( !CancellationRequested( args ) )
            {
                WixViewModel.ExecutionInfo = new ExecutionInfo
                {
                    ExecutionPercent = args.OverallPercentage,
                    PackageID = args.PackageId,
                    OverallPercent = ( args.OverallPercentage + _cachingProgress ) / 2
                };
            }

            base.OnExecuteProgress( args );
        }

        protected override void OnExecutePackageComplete( ExecutePackageCompleteEventArgs args )
        {
            CancellationRequested( args );

            base.OnExecutePackageComplete( args );
        }

        #endregion

        protected override void OnApplyComplete( ApplyCompleteEventArgs args )
        {
            WixViewModel.EngineState = EngineState.ApplyComplete;

            base.OnApplyComplete( args );

            _finalResult = args.Status;
            Dispatcher.InvokeShutdown();
        }

        #endregion

        #region viewmodel event handlers

        private void _vm_Action( object sender, EngineActionEventArgs args )
        {
            OnAction( args );
        }

        private void _vm_CancelAction( object sender, EventArgs e )
        {
            if( WixViewModel.InstallState == InstallState.Applying ) WixViewModel.InstallState = InstallState.Canceled;
            else Dispatcher.InvokeShutdown();
        }

        #endregion
    }
}
