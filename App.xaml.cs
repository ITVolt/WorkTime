using System.Windows;
using WorkTime.DataStorage;
using WorkTime.ViewModels;
using SettingsProvider = WorkTime.DataStorage.SettingsProvider;

namespace WorkTime
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SettingsProvider settingsProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var loadedSettings = JsonHandler.Instance.GetSettings();

            this.settingsProvider = new SettingsProvider(loadedSettings);
            var settingsViewModel = new SettingsViewModel(settingsProvider);
            var mainViewModel = new MainViewModel(settingsProvider, settingsViewModel);

            var mainWindow = new MainWindow() { DataContext = mainViewModel };

            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            JsonHandler.Instance.SaveSettings(this.settingsProvider.GetSettings());
        }
    }
}
