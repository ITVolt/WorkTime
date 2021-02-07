using System;
using System.Collections.Generic;
using WorkTime.WindowsEvents;

namespace WorkTime.DataStorage
{
    internal class DayLogEntry
    {
        public IList<FocusChangedLogEntry> FocusChangedLogEntries { get; }

        public DayLogEntry(IList<FocusChangedLogEntry> focusChangedLogEntries)
        {
            FocusChangedLogEntries = focusChangedLogEntries;
        }
    }

    internal class FocusChangedLogEntry
    {
        public DateTime Timestamp { get; }
        public string ProcessName { get; }
        public string WindowTitle { get; }

        public FocusChangedLogEntry(DateTime timestamp, string processName, string windowTitle)
        {
            Timestamp = timestamp;
            ProcessName = processName;
            WindowTitle = windowTitle;
        }

        public FocusChangedLogEntry(FocusChangedEvent focusChangedEvent) : this(timestamp: DateTime.Now, processName: focusChangedEvent.ProcessName, windowTitle: focusChangedEvent.WindowTitle) { }
    }
}
