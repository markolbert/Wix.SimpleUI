/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:LanHistory.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using Olbert.Wix.Models;
using Serilog;
using Serilog.Core;

namespace Olbert.Wix.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        private static readonly IContainer _container;

        static ViewModelLocator()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<WixApp>().SingleInstance();
            builder.RegisterType<WixModel>().SingleInstance();
            builder.RegisterType<WixViewModel>();

            // define shared rolling log files
            string localAppPath = System.Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );

            builder.Register<Logger>( ctx =>
                    new LoggerConfiguration()
                        .WriteTo
                        .RollingFile( pathFormat: $@"{localAppPath}\WixUI\log-{{Date}}.txt", shared: true )
                        .CreateLogger() )
                .As<ILogger>();

            _container = builder.Build();

            ServiceLocator.SetLocatorProvider( () => new AutofacServiceLocator( _container ) );
        }

        public WixModel Model => ServiceLocator.Current.GetInstance<WixModel>();
        public ILogger Logger => ServiceLocator.Current.GetInstance<ILogger>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}