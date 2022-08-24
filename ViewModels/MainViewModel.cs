using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using WorkTime.Analysis;
using WorkTime.Analysis.Calculators;
using WorkTime.Analysis.Factory;
using WorkTime.DataStorage;
using WorkTime.Properties;
using WorkTime.WindowsEvents;

namespace WorkTime.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly TimerSettingsProvider timerSettingsProvider;
        private readonly IWindowSettingsProvider windowSettingsProvider;
        public SettingsViewModel SettingsViewModel { get; }

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

        private string logText;

        public string LogText {
            get => logText;
            set
            {
                logText = value;
                OnPropertyChanged();
            }
        }

        public Func<(Point, Size)> RestoreToPoint;

        public Command<Point, Size> SaveLastPositionAndSize;

        public MainViewModel(TimerSettingsProvider settingsProvider, WindowSettingsProvider windowSettingsProvider, SettingsViewModel settingsViewModel)
        {
            this.timerSettingsProvider = settingsProvider;
            SetupTimeCalculator(settingsProvider.GetSettings());
            
            this.windowSettingsProvider = windowSettingsProvider;
            this.RestoreToPoint = GetLastPositionAndPoint;
            SaveLastPositionAndSize = new Command<Point, Size>(StoreLastPositionAndPoint);
            
            SettingsViewModel = settingsViewModel;

            SetupPassiveUpdate();
            windowFocusChangedProvider.WindowFocusChanged += OnWindowFocusChanged;
            settingsProvider.OnSettingsChange += OnSettingsChanged;
        }

        public (Point lastPoint, Size lastSize) GetLastPositionAndPoint() {
            var windowSettings = this.windowSettingsProvider.GetSettings();
            return (windowSettings.LastPosition, windowSettings.LastSize);
        }


        public void StoreLastPositionAndPoint(Point point, Size size) { 
            this.windowSettingsProvider.SaveSettings(new WindowSettingsDTO() { LastPosition = point, LastSize = size });
        }
        private void SetupTimeCalculator(TimerSettingsDTO settings)
        {
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

        private void OnSettingsChanged(TimerSettingsDTO newSettings)
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

        private void UpdateBackground(Focus focusedOn)
        {
            WorkTimeBackground = focusedOn switch
            {
                Focus.NotWork => Brushes.Wheat,
                Focus.Break => Brushes.CadetBlue,
                Focus.Work => Brushes.ForestGreen,
                _ => throw new ArgumentException("Does not recognize process with state " + Enum.GetName(focusedOn.GetType(), focusedOn.ToString())),
            };
        }

        private void UpdateLog(FocusChangedEvent focusChangedEvent)
        {
            LogText += $"{focusChangedEvent.ProcessName} - {focusChangedEvent.WindowTitle}\r\n";
        }

        public override void Dispose()
        {
            timerSettingsProvider.OnSettingsChange -= OnSettingsChanged;
        }
    }
}
