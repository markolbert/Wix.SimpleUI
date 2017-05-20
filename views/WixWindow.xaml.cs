using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Olbert.Wix.views
{
    /// <summary>
    /// Interaction logic for WixWindow.xaml
    /// </summary>
    public partial class WixWindow : Window
    {
        private const string ResourceDll = "Olbert.JumpForJoy.DefaultResources";

        public WixWindow()
        {
            InitializeComponent();

            try
            {
                var resDllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{ResourceDll}.dll");

                if( File.Exists( resDllPath ) )
                {
                    var resAssembly = Assembly.LoadFile( resDllPath );
                    var uriText =
                        $"pack://application:,,,/{resAssembly.GetName().Name};component/DefaultResources.xaml";

                    ResourceDictionary j4jRD =
                        new ResourceDictionary
                        {
                            Source = new Uri( uriText )
                        };

                    Resources.MergedDictionaries.Add(j4jRD);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
