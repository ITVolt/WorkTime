using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkTime.WindowsEvents;

namespace WorkTime
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new Form1();
            mainForm.SetEventListener(new WindowFocusChangedProvider());

            Application.Run(mainForm);
        }
    }
}
