using System;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Olbert.Wix.Models
{
    public class WixModel
    {
        private IntPtr _hwnd = IntPtr.Zero;

        public WixModel( WixApp wixApp )
        {
            Application = wixApp ?? throw new NullReferenceException( nameof(wixApp) );
        }

        public WixApp Application { get; }
        public int FinalResult { get; set; }

        public void SetWindowHandle( Window view )
        {
            _hwnd = new WindowInteropHelper( view ).Handle;
        }

        public void PlanAction( LaunchAction action )
        {
            Application.Engine.Plan( action );
        }

        public void ApplyAction()
        {
            Application.Engine.Apply( _hwnd );
        }

        public void Log( string mesg, LogLevel level = LogLevel.Standard )
        {
            if( !String.IsNullOrEmpty( mesg ) )
                Application.Engine.Log( level, mesg );
        }
    }
}