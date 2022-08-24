using System.Collections.Generic;

namespace WorkTime.Properties
{
    public sealed record TimerSettingsDTO
    {
        public IList<string> WorkProcesses { get; init; }
        public int NrbOfMinutesBreakPerHour { get; init; }
    }
}
