using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WorkTime.WindowsEvents
{
    public class WindowFocusChangedProvider
    {
        public EventHandler<FocusChangedEvent> WindowFocusChanged;

        private const uint WindowEventOutOfContext = 0;
        private const uint EventSystemForeground = 3;

        public WindowFocusChangedProvider()
        {
            var winEventDelegate = new WinEventDelegate(WinEventProc);
            SetWinEventHook(EventSystemForeground, EventSystemForeground, IntPtr.Zero, winEventDelegate, 0, 0, WindowEventOutOfContext);
        }

        public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            var focusChangeEvent = GetActiveWindowInfo();

            WindowFocusChanged?.Invoke(this, focusChangeEvent);
        }

        private static FocusChangedEvent GetActiveWindowInfo()
        {
            var windowHandle = GetForegroundWindow();

            var windowText = "";
            const int nChars = 256;
            var strBuilder = new StringBuilder(nChars);
            if (GetWindowText(windowHandle, strBuilder, nChars) > 0)
            {
                windowText = strBuilder.ToString();
            }

            GetWindowThreadProcessId(windowHandle, out var processIdHandle);
            var process = Process.GetProcessById((int)processIdHandle);
            var processName = process.ProcessName;

            return new FocusChangedEvent(windowText, processName);
        }

        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    }
}
