﻿using System.Collections.Generic;

namespace WorkTime.DataStorage
{
    class Settings
    {
        public IList<string> WorkProcesses { get; set; }

        public int NrbOfMinutesBreakPerHour { get; set; }
    }
}
