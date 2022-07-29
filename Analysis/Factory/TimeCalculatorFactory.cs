using System;
using System.Collections.Generic;
using System.Linq;
using WorkTime.Analysis.Calculators;
using WorkTime.DataStorage;
using WorkTime.WindowsEvents;

namespace WorkTime.Analysis.Factory;

internal class TimeCalculatorFactory
{
    public static TimeCalculator GetTimeCalculator(Settings settings)
    {
        var timeCalculator = CreateCalculator(settings);

        timeCalculator.StartCalculation(DateTime.Now);

        return timeCalculator;
    }

    public static TimeCalculator UpdateTimeCalculatorWithNewSettings(TimeCalculator currentCalculator, Settings settings, IList<FocusChangedLogEntry> focusChanges)
    {
        var newCalculator = CreateCalculator(settings);

        newCalculator.StartCalculation(currentCalculator.CalculationStart);

        var previousEntries = focusChanges.OrderBy(x => x.Timestamp);

        foreach (var entry in previousEntries)
        {
            newCalculator.Update(entry);
        }

        return newCalculator;
    }

    private static TimeCalculator CreateCalculator(Settings settings)
    {
        if (settings.NrbOfMinutesBreakPerHour is > 0 and < 60){
            return new WorkWithBreakCalculator(settings.WorkProcesses, TimeSpan.FromMinutes(settings.NrbOfMinutesBreakPerHour));
        }
        return new WorkTimeCalculator(settings.WorkProcesses);
    }
}
