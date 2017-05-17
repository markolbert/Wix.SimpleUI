using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    public class ProgressPanelViewModel : PanelViewModel
    {
        private int _phasePct;
        private int _overallPct;

        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        public int PhasePercent
        {
            get => _phasePct;
            set => Set<int>( ref _phasePct, value );
        }

        public int OverallPercent
        {
            get => _overallPct;
            set => Set<int>(ref _overallPct, value);
        }

        public override ViewModelBase GetButtonsViewModel()
        {
            return new StandardButtonsViewModel
            {
                NextViewModel = { Visibility = Visibility.Collapsed },
                PreviousViewModel = { Visibility = Visibility.Collapsed }
            };
        }
    }
}