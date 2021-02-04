namespace WorkTime.WindowsEvents
{
    public class FocusChangedEvent
    {
        public string WindowText { get; }
        public string ProcessName { get; }

        public FocusChangedEvent(string windowText, string processName)
        {
            WindowText = windowText;
            ProcessName = processName;
        }
    }
}
