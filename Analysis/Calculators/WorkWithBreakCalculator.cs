using System;
using System.Collections.Generic;
using System.Linq;
using WorkTime.Analysis.Counter;

namespace WorkTime.Analysis.Calculators
{
    internal class WorkWithBreakCalculator : TimeCalculator
    {
        private readonly TimeSpan breakTimeAllowedPerHour;

        public WorkWithBreakCalculator(TimeSpan breakTimeAllowedPerHour)
        {
            this.breakTimeAllowedPerHour = breakTimeAllowedPerHour;
        }

        internal override (TimeSpan workTimeToday, FocusedOn focusedOn) CalculateWorkTime(IEnumerable<Process> processes)
        {
            var workTime = TimeSpan.Zero;
            var breakCounter = new BreakCounter(breakTimeAllowedPerHour);

            foreach (var process in processes.SkipLast(1))
            {
                workTime += process.IsWork ? process.Duration : TimeSpan.Zero;
                breakCounter.AddProcess(process);
            }

            var lastProccess = processes.Last();

            var currentFocus = lastProccess.IsWork ? FocusedOn.Work :
                                    breakCounter.IsProcessBreak(lastProccess) ?
                                        FocusedOn.Break :
                                        FocusedOn.NotWork;

            breakCounter.AddProcess(lastProccess);

            return (workTime + breakCounter.GetBreakTime(), currentFocus);
        }
    }
}
