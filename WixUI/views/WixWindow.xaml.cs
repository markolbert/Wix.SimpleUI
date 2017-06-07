
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Olbert.Wix.views
{
    /// <summary>
    /// Defines the SimpleUI's primary window, through which various panels are displayed
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
