using System;
using WorkTime.Analysis.Counters;

namespace WorkTime.Analysis.Calculators;

internal class WorkTimeCalculator : TimeCalculator
{
    private WorkCounter workCounter;

    internal override void SetupCounters(ProcessPublisher processPublisher)
    {
        workCounter = new WorkCounter();
        workCounter.Subscribe(processPublisher);
    }

    internal override void UnsubscribeCounters(ProcessPublisher processPublisher)
    {
        workCounter.Unsubscribe(processPublisher);
    }

    internal override FocusedOn GetCurrentFocus(Process currentProcess)
    {
        return currentProcess.IsWork ? FocusedOn.Work : FocusedOn.Break;
    }

    internal override TimeSpan GetWorkTime()
    {
        return workCounter.GetWorkTime();
    }
}