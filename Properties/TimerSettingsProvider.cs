﻿using System;
using System.Collections.Generic;

namespace WorkTime.Properties
{
    public interface ITimerSettingsProvider
    {
        Action<TimerSettingsDTO> OnSettingsChange { get; set; }

        public void UpdateSettings(TimerSettingsDTO settings);

        public TimerSettingsDTO GetSettings();
    }


    internal class TimerSettingsProvider : ITimerSettingsProvider
    {
        private readonly UserSettings _storedUserSettings;

        private TimerSettingsDTO _timerSettings;

        Action<TimerSettingsDTO> OnSettingsChange;

        Action<TimerSettingsDTO> ITimerSettingsProvider.OnSettingsChange {
            get => OnSettingsChange;
            set => OnSettingsChange = value; 
        }

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

            OnSettingsChange?.Invoke(settings);
        }

        public TimerSettingsDTO GetSettings() => _timerSettings;
    }
}
