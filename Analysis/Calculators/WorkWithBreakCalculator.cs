using System;
using System.Collections.Generic;
using WorkTime.Analysis.Timer;

namespace WorkTime.Analysis.Calculators;

internal class WorkWithBreakCalculator : TimeCalculator
{
    private readonly BreakTimer breakTimer;
    private readonly WorkTimer workTimer;

    public WorkWithBreakCalculator(IEnumerable<string> workProcesses, TimeSpan breakTimeAllowedPerHour) : base(workProcesses)
    {
        this.breakTimer = new BreakTimer(breakTimeAllowedPerHour);
        this.workTimer = new WorkTimer();
    }

    protected internal override void UpdateCounters(FocusEntry focusEntry)
    {
        this.breakTimer.AddEntry(focusEntry);
        this.workTimer.AddEntry(focusEntry);
    }

    protected internal override TimeSpan GetWorkTime() => workTimer.GetWorkTime() + this.breakTimer.GetBreakTime();

    protected internal override FocusedOn GetFocus(bool currentFocusIsWork)
    {
        return currentFocusIsWork
            ? FocusedOn.Work
            : this.breakTimer.HasRemainingBreakTime ? FocusedOn.Break : FocusedOn.NotWork;
    }
    
}
