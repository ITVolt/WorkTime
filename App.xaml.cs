using System.Windows;
using WorkTime.Properties;

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

            // Make sure changes are saved
            UserSettings.Default.PropertyChanged += (_, _) => UserSettings.Default.Save();
        }
    }
}
