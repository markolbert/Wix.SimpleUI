
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace Olbert.Wix.Panels
{
    /// <summary>
    /// Defines a collection of UserControls which can be used as panels in the SimpleUI
    /// </summary>
    public class WixPanels : KeyedCollection<string, WixPanelInfo>
    {
        /// <summary>
        /// The collection of SimpleUI panels defined in the solution; this is set when 
        /// the application launches.
        /// </summary>
        public static WixPanels Instance { get; } = new WixPanels();

        /// <summary>
        /// Creates an instance of the collection.
        /// 
        /// An ArgumentException is thrown if a duplicate panel ID -- defined in the panel class's 
        /// WixPanelAttribute -- is encountered.
        /// </summary>
        protected WixPanels()
        {
            Type baseType = typeof(UserControl);

            foreach( var panelType in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany( a => a.GetTypes() )
                .Where( t => t.IsClass
                             && baseType.IsAssignableFrom( t )
                             && t.GetCustomAttribute<WixPanelAttribute>() != null ) )
            {
                WixPanelAttribute attr = panelType.GetCustomAttribute<WixPanelAttribute>();

                if( this.Contains( attr.ID.ToLower() ) )
                    throw new ArgumentException( $"Duplicate WixPanelAttribute ID '{attr.ID}'" );

                Add(
                    new WixPanelInfo
                    {
                        ID = attr.ID,
                        UserControlType = panelType,
                        ViewModelType = attr.ViewModelType,
                        ButtonsType = attr.ButtonsType
                    } );
            }
        }

        /// <summary>
        /// Gets the panel's ID, in lower case form, as a key
        /// </summary>
        /// <param name="item">the WixPanelInfo item whose key is needed</param>
        /// <returns>the panel's ID, in lower case form</returns>
        protected override string GetKeyForItem( WixPanelInfo item )
        {
            return item.ID.ToLower();
        }
    }
}
