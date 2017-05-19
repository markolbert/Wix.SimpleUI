using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Olbert.JumpForJoy.WPF;
using Olbert.Wix.messages;
using Olbert.Wix.panels;

namespace Olbert.Wix.ViewModels
{
    public abstract class WixViewModel : ViewModelBase, IWixViewModel
    {
        //public event EventHandler StartDetect;
        //public event EventHandler<EngineActionEventArgs> Action;
        //public event EventHandler Finished;
        //public event EventHandler CancelAction;

        private InstallState _state;
        private string _windowTitle;
        private bool _bundleInstalled;
        private int _cachePct;
        private int _exePct;

        protected WixViewModel( IWixApp wixApp )
        {
            WixApp = wixApp ?? throw new NullReferenceException( nameof(wixApp) );

            WindowTitle = "Application Installer";

            BundleProperties = WixBundleProperties.Load() ??
                               throw new NullReferenceException( nameof(BundleProperties) );

            InstallState = InstallState.NotPresent;

            Messenger.Default.Register<PanelButtonClick>(this, PanelButtonClickHandler);
        }

        protected IWixApp WixApp { get; }

        public WixBundleProperties BundleProperties { get; private set; }

        public abstract IEnumerable<LaunchAction> SupportedActions { get; }

        public LaunchAction LaunchAction { get; set; } = LaunchAction.Unknown;

        public InstallState InstallState
        {
            get => _state;
            set => Set(ref _state, value);
        }

        public EngineState EngineState { get; set; } = EngineState.NotStarted;

        public EnginePhase EnginePhase { get; set; } = EnginePhase.NotStarted;

        public string WindowTitle
        {
            get => _windowTitle;
            set => Set( ref _windowTitle, value );
        }

        public CurrentPanelInfo Current { get; } = new CurrentPanelInfo();

        protected void CreatePanel( string id, string stage = null )
        {
            if( stage == null ) stage = id;

            if( !WixPanels.Instance.Contains( id.ToLower() ) )
            {
                var mesg = $"No panel defined with ID '{id}'";
                throw new ArgumentOutOfRangeException( nameof(CreatePanel), mesg );
            }

            var panelInfo = WixPanels.Instance[ id ];

            Current.ID = panelInfo.ID;
            Current.Stage = stage;

            Current.PanelViewModel = (PanelViewModel) Activator.CreateInstance( panelInfo.ViewModelType );

            Current.Buttons = (UserControl) Activator.CreateInstance( panelInfo.ButtonsType );
            Current.ButtonsViewModel = Current.PanelViewModel.GetButtonsViewModel();
            Current.Buttons.DataContext = Current.ButtonsViewModel;

            Current.Panel = (UserControl) Activator.CreateInstance( panelInfo.UserControlType );
            Current.Panel.DataContext = Current.PanelViewModel;
        }

        public bool BundleInstalled
        {
            get => _bundleInstalled;
            set => Set( ref _bundleInstalled, value );
        }

        public virtual void OnDetectionComplete()
        {
            EngineState = EngineState.DetectionComplete;
            InstallState = BundleProperties.GetNumPackagesToInstall() == 0 ? InstallState.Present : InstallState.NotPresent;
        }

        public virtual void ReportProgress( string mesg )
        {
        }

        public virtual void ReportProgress( int phasePct )
        {
            switch( EnginePhase )
            {
                case EnginePhase.Caching:
                    _cachePct = phasePct;
                    break;

                case EnginePhase.Executing:
                    _exePct = phasePct;
                    break;
            }

            ProgressPanelViewModel vm = Current.PanelViewModel as ProgressPanelViewModel;

            if ( vm != null )
            {
                vm.PhasePercent = phasePct;
                vm.OverallPercent = ( _cachePct + _exePct ) / 2;
            }
        }

        protected PanelViewModel CurrentPanelViewModel { get; set; }

        protected abstract void MoveNext();

        protected abstract void MovePrevious();

        //protected virtual void OnStartDetect()
        //{
        //    EventHandler handler = StartDetect;
        //    handler?.Invoke(this, EventArgs.Empty);
        //}

        //protected virtual void OnAction( LaunchAction action )
        //{
        //    EventHandler<EngineActionEventArgs> handler = Action;

        //    if( handler != null )
        //    {
        //        var args = new EngineActionEventArgs { Action = action };

        //        handler.Invoke( this, args );

        //        if( !args.Processed )
        //            new J4JMessageBox().Title( "Problem Encountered" )
        //                .Message( args.Message )
        //                .ButtonText( "Okay" )
        //                .ShowMessageBox();
        //    }
        //}

        //protected virtual void OnCancelInstallation()
        //{
        //    EventHandler handler = CancelAction;

        //    if( handler != null )
        //    {
        //        handler.Invoke( this, EventArgs.Empty );

        //        new J4JMessageBox().Title( "Installation Message" )
        //            .Message( "Cancellation requested" )
        //            .ButtonText( "Okay" )
        //            .ShowMessageBox();
        //    }
        //}

        //protected virtual void OnFinished()
        //{
        //    EventHandler handler = Finished;
        //    handler?.Invoke( this, EventArgs.Empty );
        //}

        public virtual void OnInstallationComplete()
        {
        }

        protected string GetEmbeddedTextFile(string fileName, string ns = null)
        {
            ns = ns ?? this.GetType().Namespace;

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream($"{ns}.{fileName}"))
            {
                if (stream == null) return null;

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private void PanelButtonClickHandler(PanelButtonClick obj)
        {
            if (obj == null) return;

            switch (obj.ButtonID)
            {
                case StandardButtonsViewModel.CancelButtonID:
                    var msgBox = new J4JMessageBox().Title( "Please Confirm" )
                        .Message( "Are you sure you want to cancel the installation?" )
                        .DefaultButton( 1 )
                        .ButtonText( "Yes", "No" );

                    if (msgBox.ShowMessageBox() == 1) WixApp.CancelInstallation();

                    break;

                case StandardButtonsViewModel.NextButtonID:
                    MoveNext();
                    break;

                case StandardButtonsViewModel.PreviousButtonID:
                    MovePrevious();
                    break;
            }
        }

    }
}