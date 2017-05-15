using System;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Olbert.JumpForJoy.WPF;
using Olbert.Wix.messages;

namespace Olbert.Wix.ViewModels
{
    public class WixViewModel : ViewModelBase, IWixViewModel
    {
        public event EventHandler<EngineActionEventArgs> Action;
        public event EventHandler CancelAction;

        private InstallState _state;
        private string _windowTitle;
        private UserControl _curPanel;
        private UserControl _curButtons;
        private bool _bundleInstalled;

        protected WixViewModel()
        {
            WindowTitle = "Application Installer";

            BundleProperties = WixBundleProperties.Load() ??
                               throw new NullReferenceException( nameof(BundleProperties) );

            Messenger.Default.Register<PanelButtonClick>(this, PanelButtonClickHandler);
        }

        public WixBundleProperties BundleProperties { get; private set; }

        public InstallState InstallState
        {
            get => _state;
            set => Set<InstallState>(ref _state, value);
        }

        public EngineState EngineState { get; set; }

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

        public bool BundleInstalled
        {
            get => _bundleInstalled;
            set => Set<bool>( ref _bundleInstalled, value );
        }

        public CachingInfo CachingInfo { get; set; }
        public ExecutionInfo ExecutionInfo { get; set; }

        protected virtual void MoveNext()
        {
        }

        protected virtual void MovePrevious()
        {
        }

        protected virtual void OnAction( LaunchAction action )
        {
            EventHandler<EngineActionEventArgs> handler = Action;

            if( handler != null )
            {
                var args = new EngineActionEventArgs { Action = action };

                handler.Invoke( this, args );

                if( !args.Processed )
                    J4JMessageBox.Show( args.Message, "Problem Encountered", "Okay" );
            }
        }

        protected virtual void OnCancelInstallation()
        {
            EventHandler handler = CancelAction;

            if( handler != null )
            {
                handler.Invoke( this, EventArgs.Empty );

                J4JMessageBox.Show( "Cancellation requested", "Installation Message" );
            }
        }

        protected string GetEmbeddedTextFile(string fileName, string ns = null)
        {
            ns = ns ?? this.GetType().Namespace;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{ns}.{fileName}"))
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
                    if (J4JMessageBox.Show(
                            "Are you sure you want to cancel the installation?",
                            "Please Confirm",
                            "Yes", "No") == 1)
                        OnCancelInstallation();

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