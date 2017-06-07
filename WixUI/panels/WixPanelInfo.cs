using System;

namespace Olbert.Wix.Panels
{
    /// <summary>
    /// Describes a UserControl which can be used as a panel in the UI
    /// </summary>
    public class WixPanelInfo
    {
        /// <summary>
        /// The panel's ID, which must be unique
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The Type which defines the panel
        /// </summary>
        public Type UserControlType { get; set; }

        /// <summary>
        /// The Type which serves as the panel's view model
        /// </summary>
        public Type ViewModelType { get; set; }

        /// <summary>
        /// The Type which serves as the panel's button control
        /// </summary>
        public Type ButtonsType { get; set; }
    }
}