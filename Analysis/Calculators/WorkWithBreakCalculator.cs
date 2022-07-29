using System;
using System.Collections.Generic;
using WorkTime.Analysis.Counters;

namespace WorkTime.Analysis.Calculators;

internal class WorkWithBreakCalculator : TimeCalculator
{
    private readonly BreakCounter breakCounter;
    private readonly WorkCounter workCounter;

    public WorkWithBreakCalculator(IEnumerable<string> workProcesses, TimeSpan breakTimeAllowedPerHour) : base(workProcesses)
    {
        this.breakCounter = new BreakCounter(breakTimeAllowedPerHour);
        this.workCounter = new WorkCounter();
    }

    protected internal override void UpdateCounters(Process process)
    {
        this.breakCounter.AddProcess(process);
        this.workCounter.AddProcess(process);
    }

    protected internal override TimeSpan GetWorkTime() => workCounter.GetWorkTime() + this.breakCounter.GetBreakTime();

    protected internal override FocusedOn GetFocus(bool currentFocusIsWork)
    {
        return currentFocusIsWork
            ? FocusedOn.Work
            : this.breakCounter.HasRemainingBreakTime ? FocusedOn.Break : FocusedOn.NotWork;
    }
    
}
