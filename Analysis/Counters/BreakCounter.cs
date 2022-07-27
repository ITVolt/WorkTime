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

    public override void AddProcess(object _, Process process)
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
        else if (remainingBreakTime > TimeSpan.Zero)
        {
            totalBreakTime += new TimeSpan(Math.Min(remainingBreakTime.Ticks, process.Duration.Ticks));
            remainingBreakTime -= process.Duration;
        }
    }

    public TimeSpan GetBreakTime()
    {
        return totalBreakTime;
    }

    public bool IsProcessBreak(Process process)
    {
        return !process.IsWork && remainingBreakTime > TimeSpan.Zero && remainingBreakTime >= process.Duration;
    }
}