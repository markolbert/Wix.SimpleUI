using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace Olbert.Wix.ViewModels
{
    //public class DependencyState : ViewModelBase
    //{
    //    private string _packageID;
    //    private InstallationState _state;
    //    private string _displayName;
    //    private PackageState _pkgState;

    //    public DependencyState()
    //    {
    //        InstallationState = InstallationState.Detecting;
    //    }

    //    public string DisplayName
    //    {
    //        get => _displayName;
    //        set => Set<string>( ref _displayName, value );
    //    }

    //    public string PackageID
    //    {
    //        get => _packageID;
    //        set => Set<string>( ref _packageID, value );
    //    }

    //    public InstallationState InstallationState
    //    {
    //        get => _state;
    //        set => Set<InstallationState>( ref _state, value );
    //    }

    //    public PackageState PackageState
    //    {
    //        get => _pkgState;
    //        set => Set<PackageState>( ref _pkgState, value );
    //    }
    //}

    public class DependencyPanelViewModel : PanelViewModel
    {
        public List<WixMbaPrereqInformation> Dependencies { get; set; }

        public override ViewModelBase GetButtonsViewModel()
        {
            return new StandardButtonsViewModel
            {
                NextViewModel = { Text = "Install" }
            };
        }

    }
}