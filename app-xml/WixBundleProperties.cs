using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Serilog;

namespace Olbert.Wix
{
    internal static class XmlExtensions
    {
        public static T GetAttribute<T>( this XElement element, string attrName )
        {
            if( element == null
                || String.IsNullOrEmpty( attrName )
                || element.Attribute( attrName ) == null ) return default(T);

            string text = element.Attribute( attrName ).Value;

            Type targetType = typeof(T);
            object retVal = null;

            if( targetType == typeof(bool) )
                retVal = text.Equals( "yes", StringComparison.OrdinalIgnoreCase );

            if ( targetType == typeof(YesNoAlways) )
                retVal = Enum.TryParse( text, true, out YesNoAlways yna ) ? yna : YesNoAlways.No;

            if (targetType == typeof(PackageType))
                retVal = Enum.TryParse(text, true, out PackageType pt) ? pt : PackageType.Msi;

            if( targetType == typeof(Version) )
                retVal = Version.TryParse( text, out Version version ) ? version : new Version();

            if( targetType == typeof(Display) )
            {
                if( int.TryParse( text, out int displayVal ) ) retVal = (Display) displayVal;
                else retVal = Display.Unknown;
            }

            if ( retVal == null ) retVal = Convert.ChangeType( text, typeof(T) );

            return (T) retVal;
        }
    }

    public class WixBundleProperties
    {
        public const string BootstrapperAppDataName = "BootstrapperApplicationData";

        public static XNamespace ManifestNamespace =
            "http://schemas.microsoft.com/wix/2010/BootstrapperApplicationData";

        public static XElement GetAppXml( string xmlPath = null, ILogger logger = null )
        {
            if( String.IsNullOrEmpty( xmlPath ) )
            {
                logger?.Information( $"checking default location for {BootstrapperAppDataName}.xml file" );

                var workingFolder = Path.GetDirectoryName( typeof(WixBundleProperties).Assembly.Location );

                if( workingFolder == null )
                {
                    logger?.Error( "couldn't find default assembly location" );
                    return null;
                }

                xmlPath = Path.Combine( workingFolder, $"{BootstrapperAppDataName}.xml" );
                logger?.Information( $"{BootstrapperAppDataName}.xml file found at {xmlPath}" );
            }

            XElement retVal = null;

            try
            {
                using( var reader = new StreamReader( xmlPath ) )
                {
                    var xml = reader.ReadToEnd();
                    var xDoc = XDocument.Parse( xml );

                    retVal = xDoc.Element( ManifestNamespace + BootstrapperAppDataName );
                    logger?.Information( $"parsed {BootstrapperAppDataName}.xml file" );
                }
            }
            catch( Exception e )
            {
                logger?.Error( e, "Failed to parse " + BootstrapperAppDataName + ".xml file; message was {Exception}" );
            }

            return retVal;
        }

        public static WixBundleProperties Load( string xmlPath = null, ILogger logger = null )
        {
            var xml = GetAppXml( xmlPath );
            if( xml == null ) return null;

            var xBundleProp = xml.Element( ManifestNamespace + nameof(WixBundleProperties) );
            if( xBundleProp == null )
            {
                logger?.Error( $"Couldn't find WixBundleProperties element in {BootstrapperAppDataName}.xml file" );
                return null;
            }

            WixBundleProperties retVal = new WixBundleProperties()
            {
                DisplayName = xBundleProp.GetAttribute<string>( "DisplayName" ),
                LogPathVariable = xBundleProp.GetAttribute<string>( "LogPathVariable" ),
                Compressed = xBundleProp.GetAttribute<bool>("Compressed"),
                Id = xBundleProp.GetAttribute<string>("Id"),
                UpgradeCode = xBundleProp.GetAttribute<Guid>("UpgradeCode"),
                PerMachine = xBundleProp.GetAttribute<bool>("PerMachine")
            };

            retVal.Prerequisites.AddRange( xml.Descendants( ManifestNamespace + nameof(WixMbaPrereqInformation) )
                .Select( pri => new WixMbaPrereqInformation()
                {
                    LicenseUrl = pri.GetAttribute<string>("LicenseUrl"),
                    PackageID = pri.GetAttribute<string>("PackageID")
                } ) );

            retVal.Packages.AddRange( xml.Descendants( ManifestNamespace + nameof(WixPackageProperties))
                .Select( pp => new WixPackageProperties()
                {
                    Package = pp.GetAttribute<string>( "Package" ),
                    Vital = pp.GetAttribute<bool>( "Vital" ),
                    DisplayName = pp.GetAttribute<string>( "DisplayName" ),
                    Description = pp.GetAttribute<string>( "Description" ),
                    DownloadSize = pp.GetAttribute<long>( "DownloadSize" ),
                    PackageSize = pp.GetAttribute<long>( "PackageSize" ),
                    InstalledSize = pp.GetAttribute<long>( "InstalledSize" ),
                    PackageType = pp.GetAttribute<PackageType>( "PackageType" ),
                    Permanent = pp.GetAttribute<bool>( "Permanent" ),
                    LogPathVariable = pp.GetAttribute<string>( "LogPathVariable" ),
                    RollbackLogPathVariable = pp.GetAttribute<string>( "RollbackLogPathVariable" ),
                    Compressed = pp.GetAttribute<bool>( "Compressed" ),
                    DisplayInternalUI = pp.GetAttribute<bool>( "DisplayInternalUI" ),
                    ProductCode = pp.GetAttribute<Guid>( "ProductCode" ),
                    UpgradeCode = pp.GetAttribute<Guid>( "UpgradeCode" ),
                    Version = pp.GetAttribute<Version>( "Version" ),
                    InstallCondition = pp.GetAttribute<string>( "InstallCondition" ),
                    Cache = pp.GetAttribute<YesNoAlways>( "Cache" )
                } ) );

            List<WixPackageFeatureInfo> features = xml.Descendants( ManifestNamespace + nameof(WixPackageFeatureInfo) )
                .Select( pfi => new WixPackageFeatureInfo()
                {
                    Attributes = pfi.GetAttribute<int>( "Attributes" ),
                    Description = pfi.GetAttribute<string>( "Description" ),
                    Directory = pfi.GetAttribute<string>( "Directory" ),
                    Display = pfi.GetAttribute<Display>( "Display" ),
                    Feature = pfi.GetAttribute<string>( "Feature" ),
                    Level = pfi.GetAttribute<int>( "Level" ),
                    Size = pfi.GetAttribute<long>( "Size" ),
                    Title = pfi.GetAttribute<string>( "Title" ),
                    PackageID = pfi.GetAttribute<string>( "Package" ),
                    Parent = pfi.GetAttribute<string>( "Parent" )
                } )
                .ToList();

            features.ForEach( pfi =>
            {
                var pkg = retVal.Packages.SingleOrDefault(
                    x => x.Package.Equals( pfi.PackageID, StringComparison.OrdinalIgnoreCase ) );

                if( pkg == null )
                    logger?.Error( $"Couldn't find package '{pfi.PackageID}' for feature '{pfi.Feature}'" );
                else pkg.Features.Add( pfi );
            } );

            int lastChildCount = -1;
            int repeats = 0;

            while( true )
            {
                var childFeatures = features.Where( f => !String.IsNullOrEmpty( f.Parent ) ).ToList();

                var curChildren = childFeatures.Count;
                if( curChildren == 0 ) break;

                if( curChildren == lastChildCount ) repeats++;
                else repeats = 0;

                if( repeats > 10 )
                {
                    logger?.Error($"Exceeded 10 loops trying to assign child features to parents; aborting assignment");
                    break;
                }

                childFeatures.ForEach( cf =>
                {
                    var parentFeature =
                        features.SingleOrDefault(
                            f => f.Feature.Equals( cf.Parent, StringComparison.OrdinalIgnoreCase ) );

                    if( parentFeature == null )
                        logger?.Error(
                            $"Couldn't find parent feature '{cf.Parent}' for child feature '{cf.PackageID}'" );
                    else
                    {
                        cf.ParentFeature = parentFeature;
                        parentFeature.Features.Add( cf );
                    }
                });
            }

            return retVal;
        }

        public string DisplayName { get; set; }
        public string LogPathVariable { get; set; }
        public bool Compressed { get; set; }
        public string Id { get; set; }
        public Guid UpgradeCode { get; set; }
        public bool PerMachine { get; set; }
        public List<WixMbaPrereqInformation> Prerequisites { get; } = new List<WixMbaPrereqInformation>();
        public List<WixPackageProperties> Packages { get; } = new List<WixPackageProperties>();
    }
}
