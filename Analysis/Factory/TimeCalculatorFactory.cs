using System;
using WorkTime.DataStorage;

namespace WorkTime.Analysis.Factory
{
    internal class TimeCalculatorFactory
    {
        public static TimeCalculator CreateCalculator(Settings settings)
        {
            if ( settings.NrbOfMinutesBreakPerHour >= 0 && settings.NrbOfMinutesBreakPerHour < 60 ){
                return new WorkWithBreakCalculator(settings.WorkProcesses, TimeSpan.FromMinutes(settings.NrbOfMinutesBreakPerHour), TimeSpan.FromMinutes(60));
            } else {
                return new WorkTimeCalculator(settings.WorkProcesses);
            }
        }
    }
}
