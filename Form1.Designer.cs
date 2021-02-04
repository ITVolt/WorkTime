using System;
using System.Runtime.InteropServices;
using System.Text;
using WorkTime.WindowsEvents;

namespace WorkTime
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        //WinEventDelegate dele = null;

        ////public Form1()
        ////{
        ////    InitializeComponent();
        ////}

        //delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        //[DllImport("user32.dll")]
        //static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        //private const uint WINEVENT_OUTOFCONTEXT = 0;
        //private const uint EVENT_SYSTEM_FOREGROUND = 3;

        //[DllImport("user32.dll")]
        //static extern IntPtr GetForegroundWindow();

        //[DllImport("user32.dll")]
        //static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        //private string GetActiveWindowTitle()
        //{
        //    const int nChars = 256;
        //    IntPtr handle = IntPtr.Zero;
        //    StringBuilder Buff = new StringBuilder(nChars);
        //    handle = GetForegroundWindow();

        //    if (GetWindowText(handle, Buff, nChars) > 0)
        //    {
        //        return Buff.ToString();
        //    }
        //    return null;
        //}

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //    dele = new WinEventDelegate(WinEventProc);
        //    IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);
        //}

        public void SetEventListener(WindowFocusChangedProvider windowFocusChangedProvider)
        {
            windowFocusChangedProvider.WindowFocusChanged += OnWindowFocusChanged;
        }

        public void OnWindowFocusChanged(object sender, FocusChangedEvent focusChangedEvent)
        {
            logTextBox.Text += $"{focusChangedEvent.ProcessName} - {focusChangedEvent.WindowText}\r\n";
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // logTextBox
            // 
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logTextBox.Location = new System.Drawing.Point(0, 24);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(800, 426);
            this.logTextBox.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.logTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox logTextBox;
    }
}

