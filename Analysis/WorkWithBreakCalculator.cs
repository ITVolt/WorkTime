using System;
using System.Collections.Generic;
using System.Linq;
using WorkTime.DataStorage;

namespace WorkTime.Analysis
{
    internal class WorkWithBreakCalculator : TimeCalculator
    {
        private readonly HashSet<string> workProcesses;

        private readonly TimeSpan breakTimeAllowed;
        private readonly TimeSpan workTimeBeforeBreak;

        public WorkWithBreakCalculator(IEnumerable<string> workProcesses, TimeSpan breakTimeAllowed, TimeSpan workTimeBeforeBreak)
        {
            this.workProcesses = new HashSet<string>(workProcesses);
            this.breakTimeAllowed = breakTimeAllowed;
            this.workTimeBeforeBreak = workTimeBeforeBreak;
        }

        public override (TimeSpan workTimeToday, bool isCurrentlyWorking) CalculateWorkTimeOfDay(DayLogEntry day)
        {
            var focusChanges = day.FocusChangedLogEntries.OrderBy(x => x.Timestamp);

            var cumulativeWorkTime = new TimeSpan();
            var lastProcessWasWork = false;
            var lastFocusChange = DateTime.Today;

            var currentBreakTime = this.breakTimeAllowed;
            var currentWorkToBreak = this.workTimeBeforeBreak;

            foreach (var focusChange in focusChanges)
            {
                var duration = focusChange.Timestamp - lastFocusChange;

                if (lastProcessWasWork)
                {
                    if (duration >= currentWorkToBreak)
                    {
                        currentWorkToBreak = this.workTimeBeforeBreak;
                        currentBreakTime = this.breakTimeAllowed;
                    }
                    else
                    {
                        currentWorkToBreak -= duration;
                    }
                }
                else if (currentBreakTime > TimeSpan.Zero)
                {
                    var (remaningBreak, allowedBreak) = CalculateBreakTime(duration, currentBreakTime);
                    cumulativeWorkTime += allowedBreak;
                    currentBreakTime = remaningBreak;
                }

                if (lastProcessWasWork)
                {
                    cumulativeWorkTime += duration;
                }

                lastProcessWasWork = ProcessCountAsWork(focusChange);
                lastFocusChange = focusChange.Timestamp;
            }

            if (lastProcessWasWork)
            {
                cumulativeWorkTime += DateTime.Now - lastFocusChange;
            }
            else if (currentBreakTime > TimeSpan.Zero)
            {
                var (_, allowedBreak) = CalculateBreakTime(DateTime.Now - lastFocusChange, currentBreakTime);
                cumulativeWorkTime += allowedBreak;
            }

            return (workTimeToday: cumulativeWorkTime, isCurrentlyWorking: lastProcessWasWork);
        }

        private bool ProcessCountAsWork(FocusChangedLogEntry focusChange)
        {
            return workProcesses.Contains(focusChange.ProcessName);
        }

        private static (TimeSpan remainingBreakTime, TimeSpan allowedBreakTime) CalculateBreakTime(TimeSpan breakSpan, TimeSpan currentBreakTime)
        {
            return breakSpan > currentBreakTime ? (TimeSpan.Zero, currentBreakTime) : (currentBreakTime - breakSpan, breakSpan);
        }
    }
}
