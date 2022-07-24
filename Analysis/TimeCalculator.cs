using System;
using WorkTime.DataStorage;

internal abstract class TimeCalculator{

    public abstract (TimeSpan workTimeToday, bool isCurrentlyWorking) CalculateWorkTimeOfDay(DayLogEntry day);
}