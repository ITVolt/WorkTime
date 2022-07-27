using WorkTime.Analysis.Calculators;

namespace WorkTime.Analysis.Counters;

public abstract class Counter
{
    public void Subscribe(ProcessPublisher publisher)
    {
        publisher.Publisher += AddProcess;
    }

    public void Unsubscribe(ProcessPublisher publisher)
    {
        publisher.Publisher -= AddProcess;
    }

    public abstract void AddProcess(object _, Process process);
}