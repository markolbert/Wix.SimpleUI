using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;
using Olbert.Wix.messages;

namespace Olbert.Wix
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Olbert.Wix"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Olbert.Wix.buttons;assembly=Olbert.Wix.buttons"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:WixButton/>
    ///
    /// </summary>
    public class WixButton : Button
    {
        public static readonly DependencyProperty ButtonIDProperty =
            DependencyProperty.Register( nameof(ButtonID), typeof(string), typeof(WixButton),
                new PropertyMetadata( String.Empty ) );

        public static readonly DependencyProperty NormalBackgroundProperty =
            DependencyProperty.Register("NormalBackground", typeof(Brush), typeof(WixButton),
                new PropertyMetadata(Brushes.LightGray));

        public WixButton()
        {
            this.Click += WixButton_Click;
        }

        public string ButtonID
        {
            get => (string) GetValue( ButtonIDProperty );
            set => SetValue( ButtonIDProperty, value );
        }

        public Brush NormalBackground
        {
            get => (Brush) GetValue( NormalBackgroundProperty );
            set => SetValue( NormalBackgroundProperty, value );
        }

        private void WixButton_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<PanelButtonClick>( new PanelButtonClick( ButtonID ) );
        }

    }
}
