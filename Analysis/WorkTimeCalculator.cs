using System;
using System.Linq;
using WorkTime.DataStorage;

namespace WorkTime.Analysis
{
    internal class WorkTimeCalculator
    {
        public static (TimeSpan workTimeToday, bool isCurrentlyWorking) CalculateWorkTimeOfDay(DayLogEntry day)
        {
            var focusChanges = day.FocusChangedLogEntries.OrderBy(x => x.Timestamp);

            var cumulativeWorkTime = new TimeSpan();
            var lastProcessWasWork = false;
            var lastFocusChange = DateTime.Today;
            foreach (var focusChange in focusChanges)
            {
                var currentProcessCountsAsWork = ProcessCountsAsWork(focusChange.ProcessName);
                
                if (lastProcessWasWork)
                {
                    cumulativeWorkTime += focusChange.Timestamp - lastFocusChange;
                }

                lastProcessWasWork = currentProcessCountsAsWork;
                lastFocusChange = focusChange.Timestamp;
            }

            if (lastProcessWasWork)
            {
                cumulativeWorkTime += DateTime.Now - lastFocusChange;
            }

            return (workTimeToday: cumulativeWorkTime, isCurrentlyWorking: lastProcessWasWork);
        }

        private static bool ProcessCountsAsWork(string processName)
        {
            var settings = JsonHandler.Instance.GetSettings();

            return settings.WorkProcesses.Contains(processName);
        }
    }
}
