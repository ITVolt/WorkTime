﻿using System;
using System.Collections.Generic;
using WorkTime.Analysis.Timer;

namespace WorkTime.Analysis.Calculators;

internal class WorkTimeCalculator : TimeCalculator
{
    private readonly WorkTimer workTimer;

    public WorkTimeCalculator(IEnumerable<string> workProcesses) : base(workProcesses)
    {
        this.workTimer = new WorkTimer();
    }

    protected internal override void UpdateCounters(FocusEntry focusEntry)
    {
        this.workTimer.AddEntry(focusEntry);
    }

    protected internal override TimeSpan GetWorkTime()
    {
        return this.workTimer.GetWorkTime();
    }

    protected internal override Focus GetFocus(bool currentFocusIsWork)
    {
        return currentFocusIsWork ? Focus.Work : Focus.Idle;
    }
}
