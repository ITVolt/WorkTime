using System;
using WorkTime.Analysis;
using WorkTime.Analysis.Calculators;

namespace WorkTime.Analysis.Counter
{
    class BreakCounter
    {
        private readonly TimeSpan breakTimeAllowedPerHour;
        private readonly TimeSpan workTimeBeforeBreak;

        private TimeSpan remainingBreakTime;
        private TimeSpan reamainingWorkBeforeBreak;

        private TimeSpan totalBreakTime;

        public BreakCounter(TimeSpan breakTimeAllowedPerHour)
        {
            this.breakTimeAllowedPerHour = breakTimeAllowedPerHour;
            this.workTimeBeforeBreak = TimeSpan.FromHours(1) - breakTimeAllowedPerHour;

            this.remainingBreakTime = breakTimeAllowedPerHour;
            this.reamainingWorkBeforeBreak = workTimeBeforeBreak;
        }

        public void AddProcess(Process process)
        {
            if (process.IsWork)
            {
                reamainingWorkBeforeBreak -= process.Duration;
                if(reamainingWorkBeforeBreak <= TimeSpan.Zero)
                {
                    this.remainingBreakTime = breakTimeAllowedPerHour;
                    this.reamainingWorkBeforeBreak = workTimeBeforeBreak;
                }
            } else if(this.remainingBreakTime > TimeSpan.Zero)
            {
                totalBreakTime += new TimeSpan(Math.Min(remainingBreakTime.Ticks, process.Duration.Ticks));
                remainingBreakTime -= process.Duration;
            }
        }

        public TimeSpan GetBreakTime() => this.totalBreakTime;

        public bool IsProcessBreak(Process process)
        {
            return !process.IsWork && this.remainingBreakTime > TimeSpan.Zero && this.remainingBreakTime >= process.Duration;
        }
    }
}
