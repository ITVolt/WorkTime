using System;
using WorkTime.Analysis.Counters;

namespace WorkTime.Analysis.Calculators;

internal class WorkWithBreakCalculator : TimeCalculator
{
    private readonly TimeSpan breakTimeAllowedPerHour;

    private BreakCounter breakCounter;
    private WorkCounter workCounter;

    public WorkWithBreakCalculator(TimeSpan breakTimeAllowedPerHour)
    {
        this.breakTimeAllowedPerHour = breakTimeAllowedPerHour;
    }

    internal override void SetupCounters(ProcessPublisher processPublisher)
    {
        workCounter = new WorkCounter();
        breakCounter = new BreakCounter(breakTimeAllowedPerHour);
        workCounter.Subscribe(processPublisher);
        breakCounter.Subscribe(processPublisher);
    }

    internal override void UnsubscribeCounters(ProcessPublisher processPublisher)
    {
        workCounter.Unsubscribe(processPublisher);
        breakCounter.Unsubscribe(processPublisher);
    }

    internal override FocusedOn GetCurrentFocus(Process currentProcess)
    {
        return currentProcess.IsWork ? FocusedOn.Work :
            breakCounter.IsProcessBreak(currentProcess) ? FocusedOn.Break :
            FocusedOn.NotWork;
    }

    internal override TimeSpan GetWorkTime()
    {
        return workCounter.GetWorkTime() + breakCounter.GetBreakTime();
    }
}