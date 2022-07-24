using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using WorkTime.WindowsEvents;

namespace WorkTime.DataStorage
{
    internal class JsonHandler
    {
        public static JsonHandler Instance { get; } = new JsonHandler();

        private readonly string settingsPath = @"settings.json";
        private readonly string logPath = @"log.json";

        private JsonHandler()
        {
            if (!File.Exists(settingsPath))
            {
                var settings = new Settings {
                    WorkProcesses = new List<string> {"mstsc", "Teams", "slack"}, 
                    NrbOfMinutesBreakPerHour = 0 
                };
                    
                File.WriteAllText(settingsPath, JsonConvert.SerializeObject(settings, Formatting.Indented));
            }
        }

        public Settings GetSettings()
        {
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsPath));
        }

        public void Log(FocusChangedEvent focusChangedEvent)
        {
            
        }
    }
}
