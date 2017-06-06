using System;
using System.Xml.Linq;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Olbert.Wix
{
    /// <summary>
    /// Extensions for extracting attribute values, in a Type-specific manner, from
    /// an XElement
    /// </summary>
    internal static class XmlExtensions
    {
        /// <summary>
        /// Generic method to retrieve a named attribute from an XElement
        /// </summary>
        /// <typeparam name="T">the Type of the attribute to retrieve</typeparam>
        /// <param name="element">the XElement whose attribute is being retrieved</param>
        /// <param name="attrName">the name of the attribute being retrieved</param>
        /// <returns>The value of the specified attribute. This is determined by
        /// first checking to see if T is a specially-supported Type (one of bool, YesNoAlways,
        /// PackageType, Version, Guid or Display). If it isn't, the value is returned by
        /// converting the attribute's text value with Convert.ChangeType().
        /// 
        /// Specially supported Types are converted as follows:
        /// 
        /// - bool: "yes" (case insensitive) => true, false otherwise
        /// - YesNoAlways: based on the textual enum values (mismatches return YesNoAlways.No)
        /// - PackageType: based on the textual enum values (mismatches return PackageType.Msi)
        /// - Version: based on the value returned by Version.TryParse(), or new Version() if that fails
        /// - Guid: based on the value returned by Guid.TryParse(), or new Guid() if that fails
        /// - Display: based on the value returned by int.TryParse, or Display.Unknown if that fails
        /// 
        /// The default value of T is returned if element is null, or attrName is null or empty, 
        /// or there is no value for the specified attribute.
        /// </returns>
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

            if( targetType == typeof(Guid) )
                retVal = Guid.TryParse( text, out Guid guid ) ? guid : new Guid();
                    
            if( targetType == typeof(Display) )
            {
                if( int.TryParse( text, out int displayVal ) ) retVal = (Display) displayVal;
                else retVal = Display.Unknown;
            }

            if ( retVal == null ) retVal = Convert.ChangeType( text, typeof(T) );

            return (T) retVal;
        }
    }
}