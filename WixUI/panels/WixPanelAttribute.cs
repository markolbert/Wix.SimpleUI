
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System;
using System.Windows.Controls;
using Olbert.Wix.Buttons;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.Panels
{
    /// <summary>
    /// An Attribute for marking classes as being usable as Wix SimpleUI panels, and binding
    /// them to particular view models and button user controls.
    /// 
    /// Button user controls are collections of buttons used in the API to control movement
    /// through the various panels/steps.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited = false)]
    public class WixPanelAttribute : Attribute
    {
        /// <summary>
        /// Creates an instance of WixPanelAttribute
        /// </summary>
        /// <param name="panelID">the panel's ID, which must be unique</param>
        /// <param name="panelViewModelType">the Type which serves as the view model for the panel,
        /// which must be derived from PanelViewModel</param>
        /// <param name="buttonsType">the Type which serves as the buttons UserControl in the UI,
        /// which must be derived from UserControl; if not defined, WixStandardButtons is used as a default.</param>
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

        /// <summary>
        /// The panel's ID, which must be unique
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// The Type which serves as the panel's view model
        /// </summary>
        public Type ViewModelType { get; }

        /// <summary>
        /// The Type which serves as the panel's button control
        /// </summary>
        public Type ButtonsType { get; }
    }
}
