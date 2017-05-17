using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Olbert.Wix.panels
{
    public class WixPanelInfo
    {
        public string ID { get; set; }
        public Type UserControlType { get; set; }
        public Type ViewModelType { get; set; }
        public Type ButtonsType { get; set; }
    }

    public class WixPanels : KeyedCollection<string, WixPanelInfo>
    {
        public static WixPanels Instance { get; } = new WixPanels();

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

        protected override string GetKeyForItem( WixPanelInfo item )
        {
            return item.ID.ToLower();
        }
    }
}
