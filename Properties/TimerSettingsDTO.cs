using System.Collections.Generic;

namespace WorkTime.Properties
{
    public sealed record TimerSettingsDTO(IList<string> WorkProcesses, int NbrOfMinutesBreakPerHour);
}
