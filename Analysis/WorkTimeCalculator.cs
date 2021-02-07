using System;
using System.Linq;
using WorkTime.DataStorage;

namespace WorkTime.Analysis
{
    internal class WorkTimeCalculator
    {
        public static long CalculateWorkTimeOfDay(DayLogEntry day)
        {
            var focusChanges = day.FocusChangedLogEntries.OrderBy(x => x.Timestamp);

            long cumulativeWorkTime = 0;
            var lastProcessWasWork = false;
            var lastFocusChange = DateTime.Today;
            foreach (var focusChange in focusChanges)
            {
                if (ProcessCountsAsWork(focusChange.ProcessName))
                {
                    if (lastProcessWasWork)
                    {
                        cumulativeWorkTime += (focusChange.Timestamp - lastFocusChange).Milliseconds;
                    }

                    lastProcessWasWork = true;
                }
                else
                {
                    lastProcessWasWork = false;
                }

                lastFocusChange = focusChange.Timestamp;
            }

            return cumulativeWorkTime;
        }

        public static bool ProcessCountsAsWork(string processName)
        {
            switch (processName)
            {
                case "slack": return true;
                case "teams": return true;
                default: return false;
            }
        }
    }
}
