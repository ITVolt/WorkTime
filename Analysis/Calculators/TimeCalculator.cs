using System;
using System.Linq;
using WorkTime.DataStorage;

namespace WorkTime.Analysis.Calculators;

internal abstract class TimeCalculator
{
    public (TimeSpan workTimeToday, FocusedOn focusedOn) CalculateWorkTimeOfDay(DayLogEntry day)
    {
        var processPublisher = new ProcessPublisher();

        SetupCounters(processPublisher);

        var focusChanges = day.FocusChangedLogEntries.OrderBy(x => x.Timestamp).ToList();

        var firstProcess = focusChanges.First();

        var lastProcessStart = firstProcess.Timestamp;
        var lastProcessWasWork = ProcessCountAsWork(firstProcess.ProcessName);

        foreach (var focusChange in focusChanges.Skip(1))
        {
            processPublisher.Update(new Process(focusChange.Timestamp - lastProcessStart, lastProcessWasWork));

            lastProcessStart = focusChange.Timestamp;
            lastProcessWasWork = ProcessCountAsWork(focusChange.ProcessName);
        }

        var lastProcess = new Process(DateTime.Now - lastProcessStart, lastProcessWasWork);

        var currentFocus = GetCurrentFocus(lastProcess);

        processPublisher.Update(lastProcess);

        var workTime = GetWorkTime();

        UnsubscribeCounters(processPublisher);

        return (workTime, currentFocus);
    }

    private static bool ProcessCountAsWork(string processName)
    {
        var settings = JsonHandler.Instance.GetSettings();

        return settings.WorkProcesses.Contains(processName);
    }

    internal abstract FocusedOn GetCurrentFocus(Process currentProcess);
    internal abstract TimeSpan GetWorkTime();

    internal abstract void SetupCounters(ProcessPublisher processPublisher);

    internal abstract void UnsubscribeCounters(ProcessPublisher processPublisher);
}