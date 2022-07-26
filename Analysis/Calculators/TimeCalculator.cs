using System;
using System.Collections.Generic;
using System.Linq;
using WorkTime.Analysis;
using WorkTime.Analysis.Calculators;
using WorkTime.DataStorage;

namespace WorkTime.Analysis { 
    internal abstract class TimeCalculator {

        public (TimeSpan workTimeToday, FocusedOn focusedOn) CalculateWorkTimeOfDay(DayLogEntry day)
        {
            var processes = ConvertToProcess(day);
            return this.CalculateWorkTime(processes);
        }

        internal abstract (TimeSpan workTimeToday, FocusedOn focusedOn) CalculateWorkTime(IEnumerable<Process> processes);

        private static IEnumerable<Process> ConvertToProcess(DayLogEntry day)
        {
            var processes = new List<Process>() { };
            
            var focusChanges = day.FocusChangedLogEntries.OrderBy(x => x.Timestamp);

            var lastProcessStart = focusChanges.First().Timestamp;
            var lastProcessWasWork = ProcessCountAsWork(focusChanges.First().ProcessName);

            foreach(var focusChange in focusChanges)
            {
                processes.Add(new Process(focusChange.Timestamp - lastProcessStart, lastProcessWasWork));
                lastProcessStart = focusChange.Timestamp;
                lastProcessWasWork = ProcessCountAsWork(focusChange.ProcessName);
            }

            processes.Add(new Process(DateTime.Now - lastProcessStart, lastProcessWasWork));

            return processes;
        }
        
        private static bool ProcessCountAsWork(string processName)
        {
            var settings = JsonHandler.Instance.GetSettings();

            return settings.WorkProcesses.Contains(processName);
        }
    }
}