using System;
using System.Linq;
using System.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Olbert.JumpForJoy.WPF;

namespace Olbert.Wix
{
    public static class ProgrammingUtilities
    {
        public static void WaitForDebugger( this BootstrapperApplication wixApp )
        {
            if( wixApp == null || System.Diagnostics.Debugger.IsAttached ) return;

            bool debugBuild = false;

#if DEBUG
            debugBuild = true;
#endif

            var args = wixApp.Command.GetCommandLineArgs();

            if( !debugBuild && ( args == null
                                 || args.Length == 0
                                 || !args.Any( a => a.Equals( "/debug", StringComparison.OrdinalIgnoreCase ) ) ) )
                return;

            var response = new J4JMessageBox()
                .Title( "Ahoy There!" )
                .Message( "You're either running a debug build, or launched the app with the /debug flag. Click Okay to select a debugger." )
                .ButtonText( "Okay", "Skip" )
                .ShowMessageBox();

            if( response == 0 )
            {
                System.Diagnostics.Debugger.Launch();

                while( !System.Diagnostics.Debugger.IsAttached )
                {
                    Thread.Sleep( 100 );
                }
            }
        }
    }
}
