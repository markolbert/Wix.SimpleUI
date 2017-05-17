using System.Windows.Controls;
using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    public class CurrentPanelInfo : ViewModelBase
    {
        private UserControl _panel;
        private UserControl _buttons;
        private PanelViewModel _panelVM;
        private ViewModelBase _btnVM;

        public string ID { get; set; }
        public string Stage { get; set; }

        public UserControl Panel
        {
            get => _panel;
            set => Set<UserControl>( ref _panel, value );
        }

        public UserControl Buttons
        {
            get => _buttons;
            set => Set<UserControl>( ref _buttons, value );
        }

        public PanelViewModel PanelViewModel
        {
            get => _panelVM;
            set => Set<PanelViewModel>( ref _panelVM, value );
        }

        public ViewModelBase ButtonsViewModel
        {
            get => _btnVM;
            set => Set<ViewModelBase>(ref _btnVM, value);
        }
    }
}