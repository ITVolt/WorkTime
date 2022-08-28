namespace WorkTime.Properties
{
    public interface IWindowSettingsFascade
    {
        public WindowSettingsDTO GetSettings();

        public void SaveSettings(WindowSettingsDTO settings);
    }

    internal class WindowSettingsFascade : IWindowSettingsFascade
    {
        private readonly UserSettings storedUserSettings;
        private WindowSettingsDTO settings;

        public WindowSettingsFascade(UserSettings userSettings)
        {
            this.storedUserSettings = userSettings;

            settings = new WindowSettingsDTO(
                LastPosition: userSettings.LastPosition, 
                LastCollapsedPosition: userSettings.LastCollapsedPosition, 
                LastSize: userSettings.LastSize, 
                LastWasCollapsed: userSettings.LastWasCollapsed);
        }

        public WindowSettingsDTO GetSettings() => settings;

        public void SaveSettings(WindowSettingsDTO settings)
        {
            this.settings = settings;

            storedUserSettings.LastPosition = settings.LastPosition;
            storedUserSettings.LastCollapsedPosition = settings.LastCollapsedPosition;
            storedUserSettings.LastSize = settings.LastSize;
            storedUserSettings.LastWasCollapsed = settings.LastWasCollapsed;
        }
    }
}
