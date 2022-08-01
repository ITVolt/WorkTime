using System;

namespace WorkTime.Analysis.Calculators;

public class FocusEntry
{
    public TimeSpan Duration { get; }
    
    public bool IsOnWork { get; }
    
    public FocusEntry(TimeSpan duration, bool isOnWork)
    {
        Duration = duration;
        IsOnWork = isOnWork;
    }
}
