using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using WorkTime.Analysis;
using WorkTime.Analysis.Calculators;
using WorkTime.Analysis.Factory;
using WorkTime.DataStorage;
using WorkTime.WindowsEvents;
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

        private TimeCalculator workTimeCalculator;

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
            workTimeCalculator = TimeCalculatorFactory.GetTimeCalculator(settings);
        }

        private void SetupPassiveUpdate()
        {
            passiveUpdateTimer.Tick += (_, _) => UpdateWorkTimeText();
            passiveUpdateTimer.Interval = passiveUpdateInterval;
            passiveUpdateTimer.Start();
        }

        private void OnWindowFocusChanged(object sender, FocusChangedEvent focusChangedEvent)
        {
            var focusChangedLogEntry = new FocusChangedLogEntry(focusChangedEvent);
            
            currentDay.FocusChangedLogEntries.Add(focusChangedLogEntry);
            this.workTimeCalculator.Update(focusChangedLogEntry);

            UpdateWorkTimeText();
            UpdateLog(focusChangedEvent);
        }
        private void OnSettingsChanged(Settings newSettings)
        {
            this.workTimeCalculator = TimeCalculatorFactory.UpdateTimeCalculatorWithNewSettings(
                currentCalculator: this.workTimeCalculator,
                settings: newSettings,
                focusChanges: currentDay.FocusChangedLogEntries);

            this.UpdateWorkTimeText();
        }

        private void UpdateWorkTimeText()
        {
            var (workTimeToday, currentFocusingOn) = workTimeCalculator.GetCurrentState();
            WorkTimeText = workTimeToday.ToString(@"h\:mm");
            UpdateBackground(currentFocusingOn);
        }
        
        private void UpdateBackground(FocusedOn focusedOn)
        {
            WorkTimeBackground = focusedOn switch
            {
                FocusedOn.NotWork => Brushes.Wheat,
                FocusedOn.Break => Brushes.CadetBlue,
                FocusedOn.Work => Brushes.ForestGreen,
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

            var currentScreen = Screen.FromPoint(new Point((int) Left, (int) Top));
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

        private void ReloadSettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            OnSettingsChanged(JsonHandler.Instance.GetSettings());
        }
    }
}
