using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WorkTime.Properties
{
    public interface ITimerSettingsFascade
    {
        event EventHandler<TimerSettingsDTO> SettingsChanged;

        public void UpdateSettings(TimerSettingsDTO settings);

        public TimerSettingsDTO GetSettings();
    }


    internal class TimerSettingsFascade : ITimerSettingsFascade
    {
        private readonly UserSettings _storedUserSettings;

        private TimerSettingsDTO _timerSettings;

        public event EventHandler<TimerSettingsDTO> SettingsChanged;

        public TimerSettingsFascade(UserSettings settings)
        {
            _storedUserSettings = settings;
            settings.PropertyChanged += OnUserSettingsChanged;
            _timerSettings = new TimerSettingsDTO(settings.WorkProcesses, settings.NrbOfMinutesBreakPerHour);

        }

        private void OnUserSettingsChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(UserSettings.WorkProcesses)){
                _timerSettings = _timerSettings with {WorkProcesses = _storedUserSettings.WorkProcesses};
                SettingsChanged?.Invoke(this, _timerSettings);
            }

            if (args.PropertyName == nameof(UserSettings.NrbOfMinutesBreakPerHour) 
                && _storedUserSettings.NrbOfMinutesBreakPerHour != _timerSettings.NbrOfMinutesBreakPerHour)
            {
                _timerSettings = _timerSettings with { WorkProcesses = _storedUserSettings.WorkProcesses };
                SettingsChanged?.Invoke(this, _timerSettings);
            }
        }

        public void UpdateSettings(TimerSettingsDTO settings)
        {
            if(settings.NbrOfMinutesBreakPerHour != _timerSettings.NbrOfMinutesBreakPerHour)
            {
                _storedUserSettings.NrbOfMinutesBreakPerHour = settings.NbrOfMinutesBreakPerHour;

            }

            if(settings.WorkProcesses != _timerSettings.WorkProcesses 
                && settings.WorkProcesses.Count != _timerSettings.WorkProcesses.Count 
                && settings.WorkProcesses.Any(x => !_timerSettings.WorkProcesses.Contains(x)))
            {
                _storedUserSettings.WorkProcesses = settings.WorkProcesses is List<string> listOfWorkProcesses
                                                    ? listOfWorkProcesses
                                                    : new List<string>(settings.WorkProcesses);
            }
        }

        public TimerSettingsDTO GetSettings() => _timerSettings;
    }
}
