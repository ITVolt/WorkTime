using System;

namespace WorkTime.Analysis.Calculators;

public class ProcessPublisher
{
    public event EventHandler<Process> Publisher;

    public void Update(Process process)
    {
        Publisher?.Invoke(this, process);
    }
}
