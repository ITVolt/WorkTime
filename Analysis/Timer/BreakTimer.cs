using System;
using WorkTime.Analysis.Calculators;

namespace WorkTime.Analysis.Timer;

internal class BreakTimer : Timer
{
    private readonly TimeSpan breakTimeAllowedPerHour;
    private readonly TimeSpan workTimeBeforeBreak;

    private TimeSpan remainingBreakTime;
    private TimeSpan remainingWorkBeforeBreak;

    private TimeSpan totalBreakTime;

    public BreakTimer(TimeSpan breakTimeAllowedPerHour)
    {
        this.breakTimeAllowedPerHour = breakTimeAllowedPerHour;
        workTimeBeforeBreak = TimeSpan.FromHours(1) - this.breakTimeAllowedPerHour;

        remainingBreakTime = this.breakTimeAllowedPerHour;
        remainingWorkBeforeBreak = workTimeBeforeBreak;
    }

    public override void AddEntry(FocusEntry focusEntry)
    {
        if (focusEntry.IsOnWork)
        {
            remainingWorkBeforeBreak -= focusEntry.Duration;
            if (remainingWorkBeforeBreak <= TimeSpan.Zero)
            {
                remainingBreakTime = breakTimeAllowedPerHour;
                remainingWorkBeforeBreak = workTimeBeforeBreak;
            }
        }
        else if (HasRemainingBreakTime)
        {
            totalBreakTime += new TimeSpan(Math.Min(remainingBreakTime.Ticks, focusEntry.Duration.Ticks));
            remainingBreakTime -= focusEntry.Duration;
        }
    }

    public bool HasRemainingBreakTime => remainingBreakTime >= TimeSpan.Zero;

    public TimeSpan GetBreakTime() => this.totalBreakTime;
}
