using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using Serilog;
using Serilog.Core;

namespace Olbert.Wix.ViewModels
{
    public class WixLocator
    {
        static WixLocator()
        {
            var builder = new ContainerBuilder();

            // define shared rolling log files
            string localAppPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var logger = new LoggerConfiguration()
                .WriteTo
                .RollingFile( pathFormat : $@"{localAppPath}\WixUI\log-{{Date}}.txt", shared : true )
                .CreateLogger();

            // find all the modules to register
            Type baseType = typeof(Autofac.Module);

            try
            {
                var junk = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany( x => x.GetTypes() )
                    .Where(
                        p =>
                            baseType.IsAssignableFrom( p )
                            && p.IsClass
                            && !p.IsAbstract
                            && !p.IsInterface
                            && p.GetConstructor( Type.EmptyTypes ) != null
                    )
                    .ToList();

                var viewModules = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany( x => x.GetTypes() )
                    .Where(
                        p =>
                            baseType.IsAssignableFrom( p )
                            && p.IsClass
                            && !p.IsAbstract
                            && !p.IsInterface
                            && p.GetConstructor( Type.EmptyTypes ) != null )
                    .ToList();

                string error = null;

                switch( viewModules.Count )
                {
                    case 0:
                        error = "No WixViewModel Autofac modules were found";
                        break;

                    case 1:
                        // no op; this is what we want
                        break;

                    default:
                        error = $"More than one WixViewModel Autofac modules defined";
                        break;
                }

                if ( !String.IsNullOrEmpty(error) )
                    logger.Fatal( error );

                var module = (Autofac.Module) Activator.CreateInstance( viewModules[ 0 ] );

                builder.RegisterModule( module );
            }
            catch( ReflectionTypeLoadException e )
            {
                var typeLoadException = e as ReflectionTypeLoadException;
                var loaderExceptions = typeLoadException.LoaderExceptions;

                foreach( Exception loaderExcep in loaderExceptions )
                {
                    logger.Fatal( $"WixLocator::static ctor() -- {loaderExcep.Message}" );
                }
            }

            builder.Register<Logger>( ctx =>
                    new LoggerConfiguration()
                        .WriteTo
                        .RollingFile( pathFormat: $@"{localAppPath}\WixUI\log-{{Date}}.txt", shared: true )
                        .CreateLogger() )
                .As<ILogger>();

            var container = builder.Build();

            ServiceLocator.SetLocatorProvider( () => new AutofacServiceLocator( container ) );
        }

        public static ILogger Logger => ServiceLocator.Current.GetInstance<ILogger>();
        public static IWixViewModel WixViewModel => ServiceLocator.Current.GetInstance<IWixViewModel>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}