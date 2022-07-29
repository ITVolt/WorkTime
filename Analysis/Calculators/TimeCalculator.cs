using System;
using System.Collections.Generic;
using WorkTime.DataStorage;

namespace WorkTime.Analysis.Calculators;

internal abstract class TimeCalculator
{
    public DateTime CalculationStart;

    private (DateTime time, bool wasWork) lastUpdate;

    private readonly ISet<string> workProcesses;

    protected TimeCalculator(IEnumerable<string> workProcesses)
    {
        this.workProcesses = new HashSet<string>(workProcesses);
    }

    public void StartCalculation(DateTime startTime)
    {
        CalculationStart = startTime;
        this.lastUpdate = new(startTime, false);
    }

    public void Update(FocusChangedLogEntry focusEntry)
    {
        UpdateCounters(new Process(focusEntry.Timestamp - this.lastUpdate.time, this.lastUpdate.wasWork));
        this.lastUpdate = (focusEntry.Timestamp, ProcessCountAsWork(focusEntry.ProcessName));
    }

    public (TimeSpan workTimeToday, FocusedOn focusedOn) GetCurrentState()
    {
        var currentTime = DateTime.Now;
        var currentFocusIsWork = lastUpdate.wasWork;

        UpdateCounters(new Process(currentTime - lastUpdate.time, lastUpdate.wasWork));
        this.lastUpdate = new(currentTime, currentFocusIsWork);

        return (GetWorkTime(), GetFocus(currentFocusIsWork));
    }
    private bool ProcessCountAsWork(string processName) => workProcesses.Contains(processName);

    protected internal abstract void UpdateCounters(Process process);

    protected internal abstract TimeSpan GetWorkTime();

    protected internal abstract FocusedOn GetFocus(bool currentFocusIsWork);
}
