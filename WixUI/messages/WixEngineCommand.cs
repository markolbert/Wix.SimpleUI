
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Olbert.Wix.messages
{
    public class WixEngineCommand
    {
        protected WixEngineCommand()
        {
        }

        protected WixEngineCommand( LaunchAction action )
        {
            Action = action;
        }

        public LaunchAction Action { get; }
        public bool CancelInstallation { get; protected set; }
    }

    public class WixEngineActionCommand : WixEngineCommand
    {
        public WixEngineActionCommand( LaunchAction action )
            : base( action )
        {
        }
    }

    public class WixEngineCancelCommand : WixEngineCommand
    {
        public WixEngineCancelCommand()
        {
            CancelInstallation = true;
        }
    }
}