using System;
using System.Collections.Generic;
using WorkTime.Analysis.Counters;

namespace WorkTime.Analysis.Calculators;

internal class WorkTimeCalculator : TimeCalculator
{
    private readonly WorkCounter workCounter;

    public WorkTimeCalculator(IEnumerable<string> workProcesses) : base(workProcesses)
    {
        this.workCounter = new WorkCounter();
    }

    protected internal override void UpdateCounters(Process process)
    {
        this.workCounter.AddProcess(process);
    }

    protected internal override TimeSpan GetWorkTime()
    {
        return this.workCounter.GetWorkTime();
    }

    protected internal override FocusedOn GetFocus(bool currentFocusIsWork)
    {
        return currentFocusIsWork ? FocusedOn.Work : FocusedOn.NotWork;
    }
}
