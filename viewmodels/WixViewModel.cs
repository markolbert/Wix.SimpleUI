using System;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Olbert.Wix.messages;
using Olbert.Wix.Models;

namespace Olbert.Wix.ViewModels
{
    public class WixViewModel : ViewModelBase
    {
        private InstallState _state;
        private string _mesg;
        private WixModel _model;
        private string _windowTitle;
        private UserControl _curPanel;
        private UserControl _curButtons;

        protected WixViewModel()
        {
            _model = new ViewModelLocator().Model ?? throw new NullReferenceException( nameof(WixModel) );

            _model.Application.DetectPackageComplete += DetectPackageCompleteHandler;
            _model.Application.PlanComplete += PlanCompleteHandler;
            _model.Application.ApplyBegin += ApplyBeginHandler;
            _model.Application.ApplyComplete += ApplyCompleteHandler;
            _model.Application.ExecutePackageBegin += ExecutePackageBeginHandler;
            _model.Application.ExecutePackageComplete += ExecutePackageCompleteHandler;

            InstallCommand = new RelayCommand<Window>( StartInstallation, (x) => _state == InstallState.NotPresent );

            UninstallCommand = new RelayCommand(() => _model.PlanAction(LaunchAction.Uninstall),
                () => _state == InstallState.Present);

            CancelCommand = new RelayCommand( CancelCommandHandler, () => _state != InstallState.Canceled );

            WindowTitle = _model.Application.BundleProperties.DisplayName;

            Messenger.Default.Register<PanelButtonClick>(this, PanelButtonClickHandler);
        }

        public string WindowTitle
        {
            get => _windowTitle;
            set => Set<string>( ref _windowTitle, value );
        }

        public UserControl CurrentPanel
        {
            get => _curPanel;
            set => Set<UserControl>( ref _curPanel, value );
        }

        public UserControl CurrentButtons
        {
            get => _curButtons;
            set => Set<UserControl>( ref _curButtons, value );
        }

        public RelayCommand<Window> InstallCommand { get; }
        public RelayCommand UninstallCommand { get; }
        public RelayCommand CancelCommand { get; }

        public string Message
        {
            get => _mesg;
            set => Set<string>( ref _mesg, value );
        }

        public InstallState State
        {
            get => _state;

            set
            {
                Set<InstallState>( ref _state, value );
                Message = value.ToString();

                Refresh();
            }
        }

        protected virtual void PanelButtonClickHandler( PanelButtonClick obj )
        {
            if( obj == null ) return;

            switch( obj.Button )
            {
                case PanelButton.Cancel:
                    OnCancel();
                    break;

                case PanelButton.Next:
                    OnMoveNext();
                    break;

                case PanelButton.Previous:
                    OnMovePrevious();
                    break;
            }
        }

        protected virtual void OnMoveNext()
        {
        }

        protected virtual void OnMovePrevious()
        {
        }

        protected virtual void OnCancel()
        {
        }

        private void Refresh()
        {
            WixApp.Dispatcher.Invoke( (Action) ( () =>
            {
                InstallCommand.RaiseCanExecuteChanged();
                UninstallCommand.RaiseCanExecuteChanged();
                CancelCommand.RaiseCanExecuteChanged();
            } ) );
        }

        private void StartInstallation( Window obj )
        {
            if( obj == null ) return;

            _model.SetWindowHandle( obj );
            _model.PlanAction( LaunchAction.Install );
        }

        private void ExecutePackageCompleteHandler(object sender, ExecutePackageCompleteEventArgs e)
        {
            if( _state == InstallState.Canceled ) e.Result = Result.Cancel;
        }

        private void ExecutePackageBeginHandler(object sender, ExecutePackageBeginEventArgs e)
        {
            if( _state == InstallState.Canceled ) e.Result = Result.Cancel;
        }

        private void ApplyBeginHandler(object sender, ApplyBeginEventArgs e)
        {
            _state=InstallState.Applying;
        }

        private void ApplyCompleteHandler(object sender, ApplyCompleteEventArgs e)
        {
            _model.FinalResult = e.Status;
            WixApp.Dispatcher.InvokeShutdown();
        }

        private void PlanCompleteHandler( object sender, PlanCompleteEventArgs e )
        {
            if( _state == InstallState.Canceled ) WixApp.Dispatcher.InvokeShutdown();
            else _model.ApplyAction();
        }

        private void DetectPackageCompleteHandler( object sender, DetectPackageCompleteEventArgs e )
        {
            if( e.PackageId.Equals( "MyInstaller.msi", StringComparison.OrdinalIgnoreCase ) )
                _state = e.State == PackageState.Present ? InstallState.Present : InstallState.NotPresent;
        }

        private void CancelCommandHandler()
        {
            _model.Log( "Cancelling..." );

            if( _state == InstallState.Applying ) _state = InstallState.Canceled;
            else WixApp.Dispatcher.InvokeShutdown();
        }
    }
}