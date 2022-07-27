using System;

namespace WorkTime.Analysis.Calculators;

public class Process
{
    public TimeSpan Duration { get; }
    
    public bool IsWork { get; }
    
    public Process(TimeSpan duration, bool isWork)
    {
        Duration = duration;
        IsWork = isWork;
    }
}
