using WorkTime.Analysis.Calculators;

namespace WorkTime.Analysis.Counters;

public abstract class Counter
{
    public abstract void AddProcess(Process process);
}
