using System;
using WorkTime.Analysis.Calculators;

namespace WorkTime.Analysis.Counters;

internal class BreakCounter : Counter
{
    private readonly TimeSpan breakTimeAllowedPerHour;
    private readonly TimeSpan workTimeBeforeBreak;

    private TimeSpan remainingBreakTime;
    private TimeSpan remainingWorkBeforeBreak;

    private TimeSpan totalBreakTime;

    public BreakCounter(TimeSpan breakTimeAllowedPerHour)
    {
        this.breakTimeAllowedPerHour = breakTimeAllowedPerHour;
        workTimeBeforeBreak = TimeSpan.FromHours(1) - this.breakTimeAllowedPerHour;

        remainingBreakTime = this.breakTimeAllowedPerHour;
        remainingWorkBeforeBreak = workTimeBeforeBreak;
    }

    public override void AddProcess(Process process)
    {
        if (process.IsWork)
        {
            remainingWorkBeforeBreak -= process.Duration;
            if (remainingWorkBeforeBreak <= TimeSpan.Zero)
            {
                remainingBreakTime = breakTimeAllowedPerHour;
                remainingWorkBeforeBreak = workTimeBeforeBreak;
            }
        }
        else if (HasRemainingBreakTime)
        {
            totalBreakTime += new TimeSpan(Math.Min(remainingBreakTime.Ticks, process.Duration.Ticks));
            remainingBreakTime -= process.Duration;
        }
    }

    public bool HasRemainingBreakTime => remainingBreakTime >= TimeSpan.Zero;

    public TimeSpan GetBreakTime() => this.totalBreakTime;
}
