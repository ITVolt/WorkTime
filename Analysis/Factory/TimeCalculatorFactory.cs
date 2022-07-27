using System;
using WorkTime.Analysis.Calculators;
using WorkTime.DataStorage;

namespace WorkTime.Analysis.Factory;

internal class TimeCalculatorFactory
{
    public static TimeCalculator CreateCalculator(Settings settings)
    {
        if (settings.NrbOfMinutesBreakPerHour is >= 0 and < 60)
            return new WorkWithBreakCalculator(TimeSpan.FromMinutes(settings.NrbOfMinutesBreakPerHour));
        return new WorkTimeCalculator();
    }
}
