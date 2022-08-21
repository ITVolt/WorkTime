using System;

namespace WorkTime.DataStorage
{
    internal class SettingsProvider
    {
        private Settings _settings;

        public Action<Settings> OnSettingsChange;

        public SettingsProvider(Settings settings)
        {
            _settings = settings;
        }

        public void UpdateSettings(Settings settings)
        {
            _settings = settings;
            OnSettingsChange?.Invoke(settings);
        }
        public Settings GetSettings() => _settings;
    }
}
