using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkTime.WindowsEvents;

namespace WorkTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WindowFocusChangedProvider windowFocusChangedProvider = new WindowFocusChangedProvider();

        public MainWindow()
        {
            InitializeComponent();

            windowFocusChangedProvider.WindowFocusChanged += OnWindowFocusChanged;
        }

        //public void SetEventListener(WindowFocusChangedProvider windowFocusChangedProvider)
        //{
        //    windowFocusChangedProvider.WindowFocusChanged += OnWindowFocusChanged;
        //}

        public void OnWindowFocusChanged(object sender, FocusChangedEvent focusChangedEvent)
        {
            LogTextBlock.Text += $"{focusChangedEvent.ProcessName} - {focusChangedEvent.WindowText}\r\n";
        }
    }
}
