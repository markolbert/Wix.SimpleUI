using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Olbert.Wix
{
    public interface IWixApp
    {
        void CancelInstallation();
        void StartDetect();
        (bool, string) ExecuteAction( LaunchAction action );
        void Finish();
    }
}