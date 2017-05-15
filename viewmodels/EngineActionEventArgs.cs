using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Olbert.Wix.ViewModels
{
    public class EngineActionEventArgs
    {
        public LaunchAction Action { get; set; }
        public bool Processed { get; set; }
        public string Message { get; set; }
    }
}