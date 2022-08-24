namespace WorkTime.Properties
{
    public interface IWindowSettingsProvider
    {
        public WindowSettingsDTO GetSettings();

        public void SaveSettings(WindowSettingsDTO settings);
    }

    internal class WindowSettingsProvider : IWindowSettingsProvider
    {
        private readonly UserSettings storedUserSettings;
        private WindowSettingsDTO settings;

        public WindowSettingsProvider(UserSettings userSettings)
        {
            this.storedUserSettings = userSettings;
            settings = new WindowSettingsDTO() 
            { 
                LastPosition = userSettings.LastPosition, 
                LastSize = userSettings.LastSize 
            };
        }

        public WindowSettingsDTO GetSettings() => settings;

        public void SaveSettings(WindowSettingsDTO settings)
        {
            this.settings = settings;
            storedUserSettings.LastPosition = settings.LastPosition;
            storedUserSettings.LastSize = settings.LastSize;
        }
    }
}
