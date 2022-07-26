using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTime.Analysis.Calculators
{
    internal class Process
    {
        public TimeSpan Duration { get; }

        public bool IsWork { get; }

        public Process(TimeSpan duration, bool isWork)
        {
            Duration = duration;
            IsWork = isWork;
        }
    }
}
