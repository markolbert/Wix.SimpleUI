using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    public class StandardButtonsViewModel: ViewModelBase
    {
        public const string PreviousButtonID = "previous";
        public const string NextButtonID = "next";
        public const string CancelButtonID = "cancel";

        private WixButtonViewModel _prevVM;
        private WixButtonViewModel _nextVM;
        private WixButtonViewModel _cancelVM;

        public StandardButtonsViewModel()
        {
            PreviousViewModel = new WixButtonViewModel
            {
                ButtonID = PreviousButtonID,
                Text = "< Previous",
                Visibility = Visibility.Visible,
                NormalBackground = (SolidColorBrush) new BrushConverter().ConvertFrom( "#bb911e" )
            };

            NextViewModel = new WixButtonViewModel
            {
                ButtonID = NextButtonID,
                Text = "Next >",
                Visibility = Visibility.Visible,
                NormalBackground = (SolidColorBrush) new BrushConverter().ConvertFrom( "#252315" )
            };

            CancelViewModel = new WixButtonViewModel
            {
                ButtonID = CancelButtonID,
                Text = "Cancel",
                Visibility = Visibility.Visible,
                NormalBackground = (SolidColorBrush) new BrushConverter().ConvertFrom( "#bc513e" )
            };
        }

        public WixButtonViewModel PreviousViewModel
        {
            get => _prevVM;
            set => Set<WixButtonViewModel>( ref _prevVM, value );
        }

        public WixButtonViewModel NextViewModel
        {
            get => _nextVM;
            set => Set<WixButtonViewModel>(ref _nextVM, value);
        }

        public WixButtonViewModel CancelViewModel
        {
            get => _cancelVM;
            set => Set<WixButtonViewModel>(ref _cancelVM, value);
        }

    }
}
