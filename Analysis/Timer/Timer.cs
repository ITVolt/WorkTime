using WorkTime.Analysis.Calculators;

namespace WorkTime.Analysis.Timer;

public abstract class Timer
{
    public abstract void AddEntry(FocusEntry focusEntry);
}
