using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using WorkTime.Analysis;
using WorkTime.Analysis.Calculators;
using WorkTime.Analysis.Factory;
using WorkTime.DataStorage;
using WorkTime.WindowsEvents;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Drawing.Point;

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

        private TimeCalculator WorkTimeCalculator;

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

        public static double CollapsedWidth => 140;
        public static double CollapsedHeight => 110;

        public MainWindow()
        {
            InitializeComponent();

            SetupTimeCalculator();
            SetupPassiveUpdate();

            windowFocusChangedProvider.WindowFocusChanged += OnWindowFocusChanged;
        }

        private void SetupTimeCalculator(){
            var settings = JsonHandler.Instance.GetSettings();
            this.WorkTimeCalculator = TimeCalculatorFactory.CreateCalculator(settings);
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
            var (workTimeToday, currentFocusingOn) = WorkTimeCalculator.CalculateWorkTimeOfDay(currentDay);
            WorkTimeText = workTimeToday.ToString(@"h\:mm");
            UpdateBackground(currentFocusingOn);
        }
        
        private void UpdateBackground(FocusedOn focusedOn)
        {
            WorkTimeBackground = focusedOn switch
            {
                Analysis.FocusedOn.NotWork => Brushes.Wheat,
                Analysis.FocusedOn.Break => Brushes.CadetBlue,
                Analysis.FocusedOn.Work => Brushes.ForestGreen,
                _ => throw new ArgumentException("Does not recognize process with state " + Enum.GetName(focusedOn.GetType(), focusedOn.ToString())),
            };
        }

        private void UpdateLog(FocusChangedEvent focusChangedEvent)
        {
            LogTextBlock.Text += $"{focusChangedEvent.ProcessName} - {focusChangedEvent.WindowTitle}\r\n";
            LogScrollViewer.ScrollToEnd();
        }

        private void CollapseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Width = CollapsedWidth;
            Height = CollapsedHeight;

            var currentScreen = System.Windows.Forms.Screen.FromPoint(new Point((int) Left, (int) Top));
            Left = currentScreen.WorkingArea.Right - Width;
            Top = currentScreen.WorkingArea.Bottom - Height;
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
