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
    public class MainViewModel : ViewModelBase
    {
        private readonly ITimerSettingsFascade timerSettings;
        private readonly IWindowSettingsFascade windowSettings;
        private readonly WindowFocusChangedProvider windowFocusChangedProvider;
        public SettingsViewModel SettingsViewModel { get; }

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

        private string focusedOnTitle;

        public string FocusedOnTitle
        {
            get => focusedOnTitle;
            set
            {
                focusedOnTitle = value;
                OnPropertyChanged();
            }
        }

        private SolidColorBrush workTimeTextColor;

        public SolidColorBrush WorkTimeTextColor
        {
            get => workTimeTextColor;
            set 
            {
                workTimeTextColor = value;
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

        private bool isCollapsed { get; set; }
        public bool IsCollapsed { 
            get => isCollapsed; 
            set
            {
                isCollapsed = value;
                OnPropertyChanged();
            }
        }

        public Command OnCollapseCommand { get; init; }

        public Command OnExpandCommand { get; init; }

        public MainViewModel(
            ITimerSettingsFascade timerSettings, 
            IWindowSettingsFascade windowSettings,
            WindowFocusChangedProvider windowFocusChangedProvider,
            SettingsViewModel settingsViewModel)
        {
            this.timerSettings = timerSettings;
            SetupTimeCalculator(timerSettings.GetSettings());

            this.windowSettings = windowSettings;

            this.windowFocusChangedProvider = windowFocusChangedProvider;

            isCollapsed = windowSettings.GetSettings().LastWasCollapsed;
            OnCollapseCommand = new Command(OnCollapse);
            OnExpandCommand = new Command(OnExpand);

            SettingsViewModel = settingsViewModel;

            SetupTimeCalculator(timerSettings.GetSettings());
            SetupPassiveUpdate();

            windowFocusChangedProvider.WindowFocusChanged += OnWindowFocusChanged;
            timerSettings.SettingsChanged += OnSettingsChanged;
        }

        public WindowSettingsDTO GetLastPositionAndPoint() => this.windowSettings.GetSettings();

        private void OnCollapse()
        {
            this.IsCollapsed = true;
        }

        private void OnExpand()
        {
            this.IsCollapsed = false;
        }

        public void StoreLastPositionAndPoint(Point lastPoint, Point lastCollapsedPosition, Size lastSize) {
            this.windowSettings.SaveSettings(new WindowSettingsDTO(lastPoint, lastCollapsedPosition, lastSize, isCollapsed));
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

        private void OnSettingsChanged(object _, TimerSettingsDTO newSettings)
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
            (FocusedOnTitle, WorkTimeTextColor, WorkTimeBackground) = GetFocusTagProperties(currentFocusingOn);
        }

        private static (string Title, SolidColorBrush TextColor, SolidColorBrush BackgroundColor) GetFocusTagProperties(Focus focusedOn)
        {
            return focusedOn switch
            {
                Focus.Idle => ("Idle", Brushes.Black, Brushes.Wheat),
                Focus.Work => ("Work", Brushes.White, Brushes.ForestGreen),
                Focus.Break => ("Break", Brushes.White, Brushes.CadetBlue),
                _ => throw new ArgumentException("Does not recognize process with state " + Enum.GetName(focusedOn.GetType(), focusedOn.ToString())),
            };
        }

        private void UpdateLog(FocusChangedEvent focusChangedEvent)
        {
            LogText += $"{focusChangedEvent.ProcessName} - {focusChangedEvent.WindowTitle}\r\n";
        }

        public override void Dispose()
        {
            windowFocusChangedProvider.WindowFocusChanged -= OnWindowFocusChanged;
            timerSettings.SettingsChanged -= OnSettingsChanged;
        }
    }
}
