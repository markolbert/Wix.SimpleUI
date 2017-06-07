
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
using Olbert.Wix.Panels;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// Defines the base class, dervied from MvvmLight's ViewModelBase, for the view model
    /// used by SimpleUI.
    /// </summary>
    public abstract class WixViewModel : ViewModelBase, IWixViewModel
    {
        private InstallState _state;
        private string _windowTitle;
        private bool _bundleInstalled;
        private int _cachePct;
        private int _exePct;

        /// <summary>
        /// Creates an instance of the view model, linked to a particular IWixApp object.
        /// 
        /// Sets LaunchAction to wixApp.LaunchAction, WindowTitle to "Application Installer",
        /// InstallState to InstallState.NotPresent and loads the properties and parameters exposed
        /// thru the Wix BoostrapperApplication's generated xml file.
        /// 
        /// A NullReferenceException is thrown if the wixApp is undefined.
        /// </summary>
        /// <param name="wixApp">the IWixApp with which this view model needs to be integrated</param>
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

        /// <summary>
        /// The IWixApp object with which this view model is integrated
        /// </summary>
        protected IWixApp WixApp { get; }

        /// <summary>
        /// Information about the current panel being displayed by the SimpleUI
        /// </summary>
        public CurrentPanelInfo Current { get; } = new CurrentPanelInfo();

        /// <summary>
        /// Creates a panel, makes it the current one being displayed, and updates
        /// the Current property.
        /// 
        /// An ArgumentOutOfRangeException is thrown if no panel with an ID equal to
        /// the id parameter can be found. The search is done in a case-insensitive fashion.
        /// </summary>
        /// <param name="id">the unique ID of the panel to be created</param>
        /// <param name="stage">the stage of installation; if not specified, the value of the
        /// id parameter is used</param>
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

        /// <summary>
        /// Defines how to move to the next panel. This method must be overriden in a derived class.
        /// </summary>
        protected abstract void MoveNext();

        /// <summary>
        /// Defines how to move to the previous panel. This method must be overriden in a derived class.
        /// </summary>
        protected abstract void MovePrevious();

        /// <summary>
        /// The LaunchActions supported by this UI. Must be overriden in a derived class.
        /// </summary>
        public abstract IEnumerable<LaunchAction> SupportedActions { get; }

        /// <summary>
        /// The parameters and properties of this installation exposed thru the Wix BootstrapperApplication's
        /// generated xml file.
        /// </summary>
        public WixBundleProperties BundleProperties { get; private set; }

        /// <summary>
        /// Flag indicating whether or not the bundle is already installed
        /// </summary>
        public bool BundleInstalled
        {
            get => _bundleInstalled;
            set => Set(ref _bundleInstalled, value);
        }

        /// <summary>
        /// The LaunchAction being performed
        /// </summary>
        public LaunchAction LaunchAction { get; set; }

        /// <summary>
        /// A flag indicating whether or not the bundle is installed on the system
        /// </summary>
        public InstallState InstallState
        {
            get => _state;
            set => Set(ref _state, value);
        }

        /// <summary>
        /// The current state of the Wix BootstrapperApplication's engine
        /// </summary>
        public EngineState EngineState { get; set; } = EngineState.NotStarted;


        /// <summary>
        /// The current phase of the Wix BootstrapperApplication's progress
        /// </summary>
        public EnginePhase EnginePhase { get; set; } = EnginePhase.NotStarted;

        /// <summary>
        /// The text displayed in the SimpleUI's window title bar
        /// </summary>
        public string WindowTitle
        {
            get => _windowTitle;
            set => Set( ref _windowTitle, value );
        }

        /// <summary>
        /// Updates EngineState to EngineState.DetectionComplete. Sets InstallState to either InstallState.Present or 
        /// InstallState.NotPresent depending upon whether or not there are packages that need to be installed from
        /// the bundle.
        /// </summary>
        public virtual void OnDetectionComplete()
        {
            EngineState = EngineState.DetectionComplete;
            InstallState = BundleProperties.GetNumPackagesToInstall() == 0 ? InstallState.Present : InstallState.NotPresent;
        }

        /// <summary>
        /// If the WixProgress panel is being displayed, adds the specified message to its list of messages.
        /// </summary>
        /// <param name="mesg">a message to display</param>
        public virtual void ReportProgress( string mesg )
        {
            ProgressPanelViewModel vm = Current.PanelViewModel as ProgressPanelViewModel;

            if( vm != null ) WixApp.CrossThreadAction( vm.Messages.Add, mesg );
        }

        /// <summary>
        /// If the WixProgress panel is being displayed, updates the percent of completion
        /// numbers.
        /// 
        /// This method adjusts for the fact that during installations the Wix engine reports
        /// separate caching and execution phase percentages, each of which rise to 100.
        /// </summary>
        /// <param name="phasePct">the percent of the current phase's completion</param>
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

        /// <summary>
        /// Called when installation is complete. This base implementation does nothing.
        /// </summary>
        public virtual void OnInstallationComplete()
        {
        }

        /// <summary>
        /// Utility method for retrieving a ManifestResourceStream as text. Used primarily to retrieve
        /// things like license text from an installation assembly.
        /// </summary>
        /// <param name="fileName">the name of the embedded text file to retrieve</param>
        /// <param name="ns">the namespace containing the specified file; defaults to the Namespace
        /// of the class' Type</param>
        /// <returns>the text from an embedded resource, or null if the resource could not be found</returns>
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

        /// <summary>
        /// Utility method to check whether or not a particular process is running on the system. Used primarily to
        /// indicate when user intervention to shut down an app is required.
        /// 
        /// The check is done by comparing process names in a case-insensitive fashion.
        /// </summary>
        /// <param name="processName">the name of a process</param>
        /// <returns>true if the process is running, false otherwise</returns>
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