using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    public abstract class PanelViewModel : ViewModelBase
    {
        public abstract ViewModelBase GetButtonsViewModel();
    }
}
