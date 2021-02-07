using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WorkTime.WindowsEvents
{
    public class WindowFocusChangedProvider
    {
        internal EventHandler<FocusChangedEvent> WindowFocusChanged;

        private const uint WindowEventOutOfContext = 0;
        private const uint EventSystemForeground = 3;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable - If it is only local it will get garbage collected and crash the program
        private readonly WinEventDelegate winEventDelegate;

        public WindowFocusChangedProvider()
        {
            winEventDelegate = WinEventProc;
            SetWinEventHook(EventSystemForeground, EventSystemForeground, IntPtr.Zero, this.winEventDelegate, 0, 0, WindowEventOutOfContext);
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
