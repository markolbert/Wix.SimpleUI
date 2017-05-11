using System;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Olbert.Wix.views;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix
{
    public class WixApp : BootstrapperApplication
    {
        public static Dispatcher Dispatcher { get; private set; }

        public WixBundleProperties BundleProperties { get; private set; }

        protected override void Run()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;

            BundleProperties = WixBundleProperties.Load();
            if( BundleProperties == null ) throw new NullReferenceException( "failed to load WixBundleProperties" );

            var model = new ViewModelLocator().Model;
            var view = new WixWindow();

            this.Engine.Detect();

            view.Show();
            Dispatcher.Run();
            this.Engine.Quit( model.FinalResult );
        }
    }
}
