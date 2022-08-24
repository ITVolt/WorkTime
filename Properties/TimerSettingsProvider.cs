using System;
using System.Collections.Generic;

namespace WorkTime.Properties
{
    public interface ITimerSettingsProvider
    {
        event EventHandler<TimerSettingsDTO> SettingsChanged;

        public void UpdateSettings(TimerSettingsDTO settings);

        public TimerSettingsDTO GetSettings();
    }


    internal class TimerSettingsProvider : ITimerSettingsProvider
    {
        private readonly UserSettings _storedUserSettings;

        private TimerSettingsDTO _timerSettings;

        public event EventHandler<TimerSettingsDTO> SettingsChanged;

        public TimerSettingsProvider(UserSettings settings)
        {
            _storedUserSettings = settings;
            _timerSettings = new TimerSettingsDTO { 
                WorkProcesses = settings.WorkProcesses, 
                NrbOfMinutesBreakPerHour = settings.NrbOfMinutesBreakPerHour
            };
        }

        public void UpdateSettings(TimerSettingsDTO settings)
        {
            _timerSettings = settings;

            _storedUserSettings.WorkProcesses = settings.WorkProcesses is List<string> listOfWorkProcesses 
                                                    ? listOfWorkProcesses 
                                                    : new List<string>(settings.WorkProcesses);
            _storedUserSettings.NrbOfMinutesBreakPerHour = settings.NrbOfMinutesBreakPerHour;

            SettingsChanged?.Invoke(this, settings);
        }

        public TimerSettingsDTO GetSettings() => _timerSettings;
    }
}
