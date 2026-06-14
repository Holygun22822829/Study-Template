using System.Diagnostics;
using Study.LabWork2.Abstractions.Feature.Task1.SubTask1;
using Study.LabWork2.Abstractions.Feature.Task1.SubTask1.DtoModels;

namespace Study.LabWork2.Feature.Task1.SubTask1;

/// <summary>
/// Версия 1. Использует Monitor (lock) для синхронизации
/// </summary>
public sealed class MonitorService : IPrimeCounter
{
    public PrimeCountResultDto CountPrimes(int start, int end, int threadCount) => throw new NotImplementedException();
    private readonly object _lock = new();
    private int _primeCount;
    private readonly List<int> _foundPrimes = [];

    public string GetVersionName() => throw new NotImplementedException();
    public string GetVersionName() => "Monitor (lock)";

    public PrimeCountResultDto CountPrimes(int start, int end, int threadCount)
    {
        _primeCount = 0;
        _foundPrimes.Clear();

        var stopwatch = Stopwatch.StartNew();

        int rangeSize = (end - start + 1) / threadCount;
        var threads = new Thread[threadCount];

        for (int i = 0; i < threadCount; i++)
        {
            int threadIndex = i;
            int threadStart = start + threadIndex * rangeSize;
            int threadEnd = threadIndex == threadCount - 1 ? end : threadStart + rangeSize - 1;

            threads[i] = new Thread(() => ProcessRange(threadStart, threadEnd));
            threads[i].Start();
        }

        foreach (var thread in threads)
            thread.Join();

        stopwatch.Stop();

        return new PrimeCountResultDto
        {
            PrimeCount = _primeCount,
            ExecutionTime = stopwatch.Elapsed,
            ThreadCount = threadCount,
            SynchronizationType = GetVersionName(),
            FoundPrimes = [.. _foundPrimes]
        };
    }

    private void ProcessRange(int start, int end)
    {
        for (int number = start; number <= end; number++)
        {
            if (!IsPrime(number))
                continue;

            lock (_lock)
            {
                _primeCount++;
                _foundPrimes.Add(number);
            }
        }
    }

    private static bool IsPrime(int number)
    {
        if (number < 2) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        for (int i = 3; i * i <= number; i += 2)
            if (number % i == 0)
                return false;

        return true;
    }
}
