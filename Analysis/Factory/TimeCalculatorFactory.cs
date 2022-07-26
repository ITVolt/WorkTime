using System;
using WorkTime.Analysis.Calculators;
using WorkTime.DataStorage;

namespace WorkTime.Analysis.Factory
{
    internal class TimeCalculatorFactory
    {
        public static TimeCalculator CreateCalculator(Settings settings)
        {
            if ( settings.NrbOfMinutesBreakPerHour >= 0 && settings.NrbOfMinutesBreakPerHour < 60 ){
                return new WorkWithBreakCalculator(TimeSpan.FromMinutes(settings.NrbOfMinutesBreakPerHour));
            } else {
                return new WorkTimeCalculator();
            }
        }
    }
}
