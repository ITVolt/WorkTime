using System.Windows;
using WorkTime.WindowsEvents;

namespace WorkTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            windowFocusChangedProvider.WindowFocusChanged += OnWindowFocusChanged;
        }

        private readonly WindowFocusChangedProvider windowFocusChangedProvider = new WindowFocusChangedProvider();

        internal void OnWindowFocusChanged(object sender, FocusChangedEvent focusChangedEvent)
        {
            LogTextBlock.Text += $"{focusChangedEvent.ProcessName} - {focusChangedEvent.WindowTitle}\r\n";
        }
    }
}
