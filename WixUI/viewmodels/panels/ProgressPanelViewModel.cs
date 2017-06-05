
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    public static class AddOnUIThreadExtensions
    {
        public static void AddOnUI<T>( this ICollection<T> collection, T item )
        {
            Action<T> addMethod = collection.Add;
            Application.Current.Dispatcher.BeginInvoke( addMethod, item );
        }
    }

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