using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using WorkTime.Analysis;
using WorkTime.DataStorage;
using WorkTime.WindowsEvents;

namespace WorkTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly WindowFocusChangedProvider windowFocusChangedProvider = new();
        private readonly DispatcherTimer passiveUpdateTimer = new();
        private readonly TimeSpan passiveUpdateInterval = TimeSpan.FromMinutes(1);
        private readonly DayLogEntry currentDay = new(new List<FocusChangedLogEntry>());

        private string workTimeText;
        public string WorkTimeText
        {
            get => workTimeText;
            set
            {
                workTimeText = value;
                OnPropertyChanged();
            }
        }

        private SolidColorBrush workTimeBackground;
        public SolidColorBrush WorkTimeBackground
        {
            get => workTimeBackground;
            set
            {
                workTimeBackground = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            SetupPassiveUpdate();
            windowFocusChangedProvider.WindowFocusChanged += OnWindowFocusChanged;
        }

        private void SetupPassiveUpdate()
        {
            passiveUpdateTimer.Tick += (_, _) => UpdateWorkTimeText();
            passiveUpdateTimer.Interval = passiveUpdateInterval;
            passiveUpdateTimer.Start();
        }

        private void OnWindowFocusChanged(object sender, FocusChangedEvent focusChangedEvent)
        {
            currentDay.FocusChangedLogEntries.Add(new FocusChangedLogEntry(focusChangedEvent));
            UpdateWorkTimeText();

            UpdateLog(focusChangedEvent);
        }

        private void UpdateWorkTimeText()
        {
            var (workTimeToday, isCurrentlyWorking) = WorkTimeCalculator.CalculateWorkTimeOfDay(currentDay);
            WorkTimeText = workTimeToday.ToString(@"h\:mm");
            WorkTimeBackground = isCurrentlyWorking ? Brushes.ForestGreen : Brushes.Wheat;
        }

        private void UpdateLog(FocusChangedEvent focusChangedEvent)
        {
            LogTextBlock.Text += $"{focusChangedEvent.ProcessName} - {focusChangedEvent.WindowTitle}\r\n";
            LogScrollViewer.ScrollToEnd();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
