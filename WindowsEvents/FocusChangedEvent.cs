namespace WorkTime.WindowsEvents
{
    internal class FocusChangedEvent
    {
        public string WindowTitle { get; }
        public string ProcessName { get; }

        public FocusChangedEvent(string windowTitle, string processName)
        {
            WindowTitle = windowTitle;
            ProcessName = processName;
        }
    }
}
