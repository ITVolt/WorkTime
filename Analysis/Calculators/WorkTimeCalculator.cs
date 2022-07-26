using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkTime.Analysis.Calculators
{
    internal class WorkTimeCalculator : TimeCalculator
    {
        internal override (TimeSpan workTimeToday, FocusedOn focusedOn) CalculateWorkTime(IEnumerable<Process> processes)
        {
            return (processes.Aggregate(TimeSpan.Zero, (workTime, process) => process.IsWork ? workTime + process.Duration : workTime), 
                processes.Last().IsWork ? FocusedOn.Work : FocusedOn.NotWork);
        }
    }
}
