using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Olbert.JumpForJoy.WPF;

namespace Olbert.Wix
{
    public static class ProgrammingUtilities
    {
        public static void WaitForDebugger( this BootstrapperApplication wixApp )
        {
            if( wixApp == null || System.Diagnostics.Debugger.IsAttached ) return;

            var args = wixApp.Command.GetCommandLineArgs();

            if( args == null 
                || args.Length == 0 
                || !args.Any( a => a.Equals( "/debug", StringComparison.OrdinalIgnoreCase ) ) )
                return;

            new J4JMessageBox().Title( "Ahoy There!" ).Message( "About to wait for debugger" ).ButtonText( "Okay" )
                .ShowMessageBox();

            System.Diagnostics.Debugger.Launch();

            while( !System.Diagnostics.Debugger.IsAttached )
            {
                Thread.Sleep( 100 );
            }
        }
    }
}
