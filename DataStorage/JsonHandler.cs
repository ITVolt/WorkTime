using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using WorkTime.WindowsEvents;

namespace WorkTime.DataStorage
{
    internal class JsonHandler
    {
        public static JsonHandler Instance { get; } = new JsonHandler();

        private readonly string settingsPath = @"settings.json";
        private readonly string logPath = @"log.json";

        private FileSystemWatcher settingsWatcher;
        private EventHandler<Settings> onSettingsChanged;

        private const int MaxAccessRetries = 5;
        private const int AccessTimeOutInMilliseconds = 100;

        private readonly HashSet<string> settingsAccessedTimes = new HashSet<string>();

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
        
        public void SubscribeToSettingsChanged(EventHandler<Settings> listener)
        {
            if (this.settingsWatcher == null)
            {
                // ReSharper disable twice AssignNullToNotNullAttribute
                var fullPath = Path.GetFullPath(settingsPath);
                this.settingsWatcher = new FileSystemWatcher(Path.GetDirectoryName(fullPath), Path.GetFileName(fullPath));
                this.settingsWatcher.EnableRaisingEvents = true;
                this.settingsWatcher.NotifyFilter = NotifyFilters.LastWrite;
                this.settingsWatcher.Changed += OnFileWatcherSettingsChanged;
            }
            this.onSettingsChanged += listener;
        }

        private void OnFileWatcherSettingsChanged(object source, FileSystemEventArgs e)
        {
            // The set check is to prevent duplication as
            // FileSystemWatcher.Changed may be triggered several times for the same lastWrite event
            // https://docs.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=net-6.0
            var currentWriteTime = File.GetLastWriteTime(e.FullPath).ToString();

            if (settingsAccessedTimes.Add(currentWriteTime))
            {
                var iteration = 0;

                while (!CanReadSettings())
                {
                    Thread.Sleep(AccessTimeOutInMilliseconds);
                    iteration++;

                    if (MaxAccessRetries < iteration)
                    {
                        return;
                    }
                }

                this.onSettingsChanged.Invoke(source, GetSettings());
            }
        }

        private bool CanReadSettings(){
            try
            {
                File.Open(settingsPath, FileMode.Open, FileAccess.Read).Dispose();
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public void Log(FocusChangedEvent focusChangedEvent)
        {
            
        }
    }
}
