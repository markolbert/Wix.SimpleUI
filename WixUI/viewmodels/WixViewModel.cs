
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private InstallState _state;
        private string _windowTitle;
        private bool _bundleInstalled;
        private int _cachePct;
        private int _exePct;

        protected WixViewModel( IWixApp wixApp )
        {
            WixApp = wixApp ?? throw new NullReferenceException( nameof(wixApp) );

            LaunchAction = WixApp.LaunchAction;
            WindowTitle = "Application Installer";

            BundleProperties = WixBundleProperties.Load() ??
                               throw new NullReferenceException( nameof(BundleProperties) );

            InstallState = InstallState.NotPresent;

            Messenger.Default.Register<PanelButtonClick>(this, PanelButtonClickHandler);
        }

        protected IWixApp WixApp { get; }

        public CurrentPanelInfo Current { get; } = new CurrentPanelInfo();

        protected void CreatePanel(string id, string stage = null)
        {
            if (stage == null) stage = id;

            if (!WixPanels.Instance.Contains(id.ToLower()))
            {
                var mesg = $"No panel defined with ID '{id}'";
                throw new ArgumentOutOfRangeException(nameof(CreatePanel), mesg);
            }

            var panelInfo = WixPanels.Instance[id];

            Current.ID = panelInfo.ID;
            Current.Stage = stage;

            Current.PanelViewModel = (PanelViewModel)Activator.CreateInstance(panelInfo.ViewModelType);

            Current.Buttons = (UserControl)Activator.CreateInstance(panelInfo.ButtonsType);
            Current.ButtonsViewModel = Current.PanelViewModel.GetButtonsViewModel();
            Current.Buttons.DataContext = Current.ButtonsViewModel;

            Current.Panel = (UserControl)Activator.CreateInstance(panelInfo.UserControlType);
            Current.Panel.DataContext = Current.PanelViewModel;
        }

        protected abstract void MoveNext();

        protected abstract void MovePrevious();

        public abstract IEnumerable<LaunchAction> SupportedActions { get; }

        public WixBundleProperties BundleProperties { get; private set; }

        public bool BundleInstalled
        {
            get => _bundleInstalled;
            set => Set(ref _bundleInstalled, value);
        }

        public LaunchAction LaunchAction { get; set; }

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

        public virtual void OnDetectionComplete()
        {
            EngineState = EngineState.DetectionComplete;
            InstallState = BundleProperties.GetNumPackagesToInstall() == 0 ? InstallState.Present : InstallState.NotPresent;
        }

        public virtual void ReportProgress( string mesg )
        {
            ProgressPanelViewModel vm = Current.PanelViewModel as ProgressPanelViewModel;

            if( vm != null ) WixApp.CrossThreadAction( vm.Messages.Add, mesg );
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

                var totalPct = _cachePct + _exePct;
                if( LaunchAction == LaunchAction.Install ) totalPct /= 2;
                vm.OverallPercent = totalPct;
            }
        }

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

        protected bool IsProcessRunning( string processName )
        {
            if( String.IsNullOrEmpty( processName ) ) return false;

            return Process.GetProcesses()
                .Any( p => p.ProcessName.Equals( processName, StringComparison.OrdinalIgnoreCase ) );
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