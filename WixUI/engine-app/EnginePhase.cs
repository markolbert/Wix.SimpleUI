
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

namespace Olbert.Wix
{
    /// <summary>
    /// An enum defining the state in which the Wix installer is at any point in time
    /// </summary>
    public enum EnginePhase
    {
        NotStarted,
        Detect,
        Plan,
        Caching,
        Executing,
        Finished
    }
}