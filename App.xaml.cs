using System.Windows;
using WorkTime.Properties;
using WorkTime.ViewModels;
using TimerSettingsProvider = WorkTime.Properties.TimerSettingsProvider;

namespace WorkTime
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var settingsProvider = new TimerSettingsProvider(UserSettings.Default);
            var windowSettingsProvider = new WindowSettingsProvider(UserSettings.Default);
            UserSettings.Default.PropertyChanged += (_, _) => UserSettings.Default.Save();

            var settingsViewModel = new SettingsViewModel(settingsProvider);
            var mainViewModel = new MainViewModel(settingsProvider, windowSettingsProvider, settingsViewModel);

            var mainWindow = new MainWindow() { DataContext = mainViewModel };

            mainWindow.Show();
        }
    }
}
