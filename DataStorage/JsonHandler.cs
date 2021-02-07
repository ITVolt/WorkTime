using Newtonsoft.Json;
using System.IO;
using WorkTime.WindowsEvents;

namespace WorkTime.DataStorage
{
    internal class JsonHandler
    {
        public static JsonHandler Instance { get; } = new JsonHandler();

        private string settingsPath = @"D:\Development\Workspace\WorkTime\settings.json";
        private string logPath = @"D:\Development\Workspace\WorkTime\log.json";

        private JsonHandler()
        {

        }

        public void Log(FocusChangedEvent focusChangedEvent)
        {
            
        }

        public Settings GetSettings()
        {
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsPath));
        }
    }
}
