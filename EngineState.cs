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