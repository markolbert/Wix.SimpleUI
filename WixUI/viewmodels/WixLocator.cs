
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System;
using Serilog;

namespace Olbert.Wix.ViewModels
{
    public class WixLocator
    {
        private static ILogger _logger;

        static WixLocator()
        {
            // define shared rolling log files
            string localAppPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            _logger = new LoggerConfiguration()
                .WriteTo
                .RollingFile( pathFormat : $@"{localAppPath}\WixUI\log-{{Date}}.txt", shared : true )
                .CreateLogger();
        }

        public static ILogger Logger => _logger;

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}