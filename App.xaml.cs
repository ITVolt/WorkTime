using System.Windows;
using WorkTime.Properties;
using WorkTime.ViewModels;
using WorkTime.WindowsEvents;
using TimerSettingsProvider = WorkTime.Properties.TimerSettingsFascade;

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

            var timerSettingsFascade = new TimerSettingsProvider(UserSettings.Default);
            var windowSettingsFascade = new WindowSettingsFascade(UserSettings.Default);
            var windowFocusChangedProvider = new WindowFocusChangedProvider(UserSettings.Default.IgnoredProcesses);
            UserSettings.Default.PropertyChanged += (_, _) => UserSettings.Default.Save();

            var settingsViewModel = new SettingsViewModel(timerSettingsFascade);
            var mainViewModel = new MainViewModel(timerSettingsFascade, windowSettingsFascade, windowFocusChangedProvider, settingsViewModel);

            var mainWindow = new MainWindow(mainViewModel);

            mainWindow.Show();
        }
    }
}
