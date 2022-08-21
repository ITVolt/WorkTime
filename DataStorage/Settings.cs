using System.Collections.Generic;

namespace WorkTime.DataStorage
{
    public record Settings
    {
        public IList<string> WorkProcesses { get; init; }

        public int NrbOfMinutesBreakPerHour { get; init; }
    }
}
