using System;
using WorkTime.Analysis.Calculators;

namespace WorkTime.Analysis.Timer;

internal class WorkTimer : Timer
{
    private TimeSpan workTime;

    public WorkTimer()
    {
        workTime = TimeSpan.Zero;
    }

    public override void AddEntry(FocusEntry focusEntry)
    {
        workTime += focusEntry.IsOnWork ? focusEntry.Duration : TimeSpan.Zero;
    }

    public TimeSpan GetWorkTime() => this.workTime;
}
