using WorkTime.WindowsEvents;

namespace WorkTime.DataStorage
{
    internal class JsonLogger
    {
        public static JsonLogger Instance { get; } = new JsonLogger();
        
        private JsonLogger()
        {

        }

        public void Log(FocusChangedEvent focusChangedEvent)
        {
            
        }
    }
}
