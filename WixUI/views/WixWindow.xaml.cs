using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Olbert.Wix.views
{
    /// <summary>
    /// Interaction logic for WixWindow.xaml
    /// </summary>
    public partial class WixWindow : Window
    {
        /// <summary>
        /// The name of the resource DLL used to customize the message box's appearance
        /// </summary>
        public const string ResourceID = "Olbert.J4JResources";

        public WixWindow()
        {
            InitializeComponent();

            // search for a custom resource directory in the file system
            ResourceDictionary j4jRD = null;

            try
            {
                // check the file system
                var resDllPath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, $"{ResourceID}.dll" );

                if( File.Exists( resDllPath ) )
                {
                    var resAssembly = Assembly.LoadFile( resDllPath );

                    var uriText =
                        $"pack://application:,,,/{ResourceID};component/DefaultResources.xaml";

                    j4jRD = new ResourceDictionary { Source = new Uri( uriText ) };
                }
            }
            catch( Exception ex )
            {
            }

            if (j4jRD != null) Resources.MergedDictionaries.Add(j4jRD);
        }
    }
}
