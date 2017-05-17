using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Olbert.Wix.buttons;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.panels
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited = false)]
    public class WixPanelAttribute : Attribute
    {
        public WixPanelAttribute( string panelID, Type panelViewModelType, Type buttonsType = null )
        {
            if( String.IsNullOrEmpty( panelID ) )
                throw new NullReferenceException( nameof(panelID) );

            Type baseType = typeof(PanelViewModel);

            if( !baseType.IsAssignableFrom( panelViewModelType ) )
            {
                var mesg = $"Type {panelViewModelType.Name} does not derive from {baseType.Name}";

                throw new ArgumentOutOfRangeException( nameof(panelViewModelType), mesg );
            }

            baseType = typeof(UserControl);

            if( buttonsType != null && !baseType.IsAssignableFrom( buttonsType ) )
            {
                var mesg = $"Type {buttonsType.Name} does not derive from {baseType.Name}";

                throw new ArgumentOutOfRangeException( nameof(buttonsType), mesg );
            }

            ID = panelID;
            ViewModelType = panelViewModelType;
            ButtonsType = buttonsType == null ? typeof(WixStandardButtons) : buttonsType;
        }

        public string ID { get; }
        public Type ViewModelType { get; }
        public Type ButtonsType { get; }
    }
}
