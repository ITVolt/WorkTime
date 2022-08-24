using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WorkTime.Properties;

namespace WorkTime.ViewModels
{
    internal class SettingsViewModel : ViewModelBase
    {
        private readonly ITimerSettingsProvider settingsProvider;

        private string workProcesses;

        public string WorkProcesses
        {
            get => workProcesses;
            set
            {
                workProcesses = value;
                OnPropertyChanged();
            }
        }

        private Visibility workProcessErrorVisibility;

        public Visibility WorkProcessesErrorVisbility {
            get => workProcessErrorVisibility;
            set
            {
                workProcessErrorVisibility = value;
                OnPropertyChanged();
            } 
        }

        private string workProcessErrorMessage;
        
        public string WorkProcessesErrorMessage
        {
            get => workProcessErrorMessage;
            set
            {
                workProcessErrorMessage = value;
                OnPropertyChanged();
            }
        }


        private string nbrOfMinutesBreak;

        public string NbrOfMinutesBreak
        {
            get => nbrOfMinutesBreak.ToString();
            set
            {
                nbrOfMinutesBreak = value;
                OnPropertyChanged();
            }
        }

        private Visibility nbrOfMinutesErrorVisibility;

        public Visibility NbrOfMinutesErrorVisibility
        {
            get => nbrOfMinutesErrorVisibility;
            set
            {
                nbrOfMinutesErrorVisibility = value;
                OnPropertyChanged();
            }
        }

        private string nbrOfMinutesErrorMessage;

        public string NbrOfMinutesErrorMessage
        {
            get => nbrOfMinutesErrorMessage;
            set
            {
                nbrOfMinutesErrorMessage = value;
                OnPropertyChanged();
            }
        }


        public Command SettingsSaveCommand { get; init; }

        public SettingsViewModel(ITimerSettingsProvider settingsProvider)
        {
            this.settingsProvider = settingsProvider;
            UpdateSettingsProperties(settingsProvider.GetSettings());
            settingsProvider.SettingsChanged += OnSettingsChanged;

            SettingsSaveCommand = new Command(OnSettingsSaved);

            (WorkProcessesErrorVisbility, WorkProcessesErrorMessage) = (Visibility.Collapsed, "");
            (NbrOfMinutesErrorVisibility, NbrOfMinutesErrorMessage) = (Visibility.Collapsed, "");

        }

        private void OnSettingsChanged(object _, TimerSettingsDTO newSettings) => UpdateSettingsProperties(newSettings);

        private void UpdateSettingsProperties(TimerSettingsDTO newSettings) {
            WorkProcesses = FormatWorkProcesses(newSettings);
            NbrOfMinutesBreak = newSettings.NrbOfMinutesBreakPerHour.ToString();
        }

        private void OnSettingsSaved()
        {
            var workProcessValidationResult = ValidateWorkProcessInput(this.WorkProcesses);
            var nbrOfMinutesValidationResult = ValidateNbrOfMinutesInput(this.NbrOfMinutesBreak);

            if (!workProcessValidationResult.isValid)
            {
                (WorkProcessesErrorVisbility, WorkProcessesErrorMessage) = (Visibility.Visible, workProcessValidationResult.ErrorMessage);
            }

            if (!nbrOfMinutesValidationResult.isValid)
            {
                (NbrOfMinutesErrorVisibility, NbrOfMinutesErrorMessage) = (Visibility.Visible, nbrOfMinutesValidationResult.ErrorMessage);
            }

            if(workProcessValidationResult.isValid && nbrOfMinutesValidationResult.isValid)
            {
                (WorkProcessesErrorVisbility, WorkProcessesErrorMessage) = (Visibility.Collapsed, "");
                (NbrOfMinutesErrorVisibility, NbrOfMinutesErrorMessage) = (Visibility.Collapsed, "");

                settingsProvider.UpdateSettings(new TimerSettingsDTO()
                {
                    WorkProcesses = GetWorkProcessFromString(this.WorkProcesses),
                    NrbOfMinutesBreakPerHour = int.Parse(this.NbrOfMinutesBreak)
                });
            }
        }


        private static (bool isValid, string ErrorMessage) ValidateWorkProcessInput(string userInput)
        {
            return userInput.Length != 0 ? (true, "") : (false, "Workprocesses must contain at least one processes");
        }

        private static (bool isValid, string ErrorMessage) ValidateNbrOfMinutesInput(string userInput)
        {
            var isValid = int.TryParse(userInput, out int result);

            if (!isValid)
            {
                return (false, "Must be a digit between 0 and 59");
            }

            if(result < 0 || result > 59){
                return (false, "Must be between 0 and 59");
            }


            return (true, "");
        }

        public static string FormatWorkProcesses(TimerSettingsDTO settings)
        {
            return string.Join(",", settings.WorkProcesses);
        }

        private static IList<string> GetWorkProcessFromString(string input)
        {
            var inputWithNoNewLines = input.Replace("\n", "").Replace("\r", "");
            return inputWithNoNewLines.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
        }

        public override void Dispose()
        {
            settingsProvider.SettingsChanged -= OnSettingsChanged;
        }
    }
}
