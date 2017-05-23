using System;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Olbert.Wix
{
    public interface IWixApp
    {
        void CrossThreadAction<T>( Action<T> action, T item );
        LaunchAction LaunchAction { get; }
        void CancelInstallation();
        void StartDetect();
        (bool, string) ExecuteAction( LaunchAction action );
        void Finish();
    }
}