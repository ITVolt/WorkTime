using System;
using WorkTime.WindowsEvents;

namespace WorkTime.DataStorage
{
    internal class JsonHandler
    {
        public static JsonHandler Instance { get; } = new JsonHandler();
        
        private const string LogPath = @"log.json";

        private JsonHandler()
        { }

        public void Log(FocusChangedEvent focusChangedEvent)
        {
            Console.WriteLine($"Log here: {LogPath}");
        }
    }
}
