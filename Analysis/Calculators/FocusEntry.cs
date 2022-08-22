using System;

namespace WorkTime.Analysis.Calculators;

public record FocusEntry
{
    public TimeSpan Duration { get; init; }
    
    public bool IsOnWork { get; init; }
    
    public FocusEntry(TimeSpan duration, bool isOnWork)
    {
        Duration = duration;
        IsOnWork = isOnWork;
    }
}
