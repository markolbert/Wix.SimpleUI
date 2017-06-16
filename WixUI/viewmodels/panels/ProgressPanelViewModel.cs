
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    //public static class AddOnUIThreadExtensions
    //{
    //    public static void AddOnUI<T>( this ICollection<T> collection, T item )
    //    {
    //        Action<T> addMethod = collection.Add;
    //        Application.Current.Dispatcher.BeginInvoke( addMethod, item );
    //    }
    //}

    /// <summary>
    /// Defines the view model used with the WixProgress panel
    /// </summary>
    public class ProgressPanelViewModel : PanelViewModel
    {
        private int _phasePct;
        private int _overallPct;

        /// <summary>
        /// A collection of messages displayed by the panel
        /// </summary>
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        /// <summary>
        /// The percentage completion of the current phase
        /// </summary>
        public int PhasePercent
        {
            get => _phasePct;
            set => Set<int>( ref _phasePct, value );
        }

        /// <summary>
        /// The percentage completion of the overall action
        /// </summary>
        public int OverallPercent
        {
            get => _overallPct;
            set => Set<int>(ref _overallPct, value);
        }

        /// <summary>
        /// Overrides the base implementation to return an instance of StandardButtonsViewModel where
        /// the Next and Previous buttons are collapsed/invisible. 
        /// </summary>
        /// <returns>Overrides the base implementation to return an instance of StandardButtonsViewModel where
        /// the Next and Previous buttons are collapsed/invisible. </returns>
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