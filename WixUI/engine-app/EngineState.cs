
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

namespace Olbert.Wix
{
    public enum EngineState
    {
        NotStarted,
        Detecting,
        DetectionComplete,
        Planning,
        PlanningComplete,
        Applying,
        ApplyingCaching,
        ApplyingCached,
        ApplyingExecuting,
        ApplyComplete
    }
}