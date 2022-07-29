﻿using System;
using WorkTime.Analysis.Calculators;

namespace WorkTime.Analysis.Counters;

internal class WorkCounter : Counter
{
    private TimeSpan workTime;

    public WorkCounter()
    {
        workTime = TimeSpan.Zero;
    }

    public override void AddProcess(Process process)
    {
        workTime += process.IsWork ? process.Duration : TimeSpan.Zero;
    }

    public TimeSpan GetWorkTime() => this.workTime;
}
