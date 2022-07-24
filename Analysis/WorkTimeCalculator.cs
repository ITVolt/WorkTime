using System;
using System.Collections.Generic;
using System.Linq;
using WorkTime.DataStorage;

namespace WorkTime.Analysis
{
    internal class WorkTimeCalculator : TimeCalculator
    {

        private readonly HashSet<string> workProcesses;

        public WorkTimeCalculator(IEnumerable<string> workProcesses)
        {
            this.workProcesses = new HashSet<string>(workProcesses);
        }

        public override (TimeSpan workTimeToday, bool isCurrentlyWorking) CalculateWorkTimeOfDay(DayLogEntry day)
        {
            var focusChanges = day.FocusChangedLogEntries.OrderBy(x => x.Timestamp);

            var cumulativeWorkTime = new TimeSpan();
            var lastProcessWasWork = false;
            var lastFocusChange = DateTime.Today;

            foreach (var focusChange in focusChanges)
            {
                var currentProcessCountsAsWork = ProcessCountAsWork(focusChange);

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

        private bool ProcessCountAsWork(FocusChangedLogEntry focusChange)
        {
            return workProcesses.Contains(focusChange.ProcessName);
        }
    }
}
