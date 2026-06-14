using System.Diagnostics;
using Study.LabWork2.Abstractions.Feature.Task1.SubTask1;
using Study.LabWork2.Abstractions.Feature.Task1.SubTask1.DtoModels;

namespace Study.LabWork2.Feature.Task1.SubTask1;

/// <summary>
/// Версия 3. Использует Semaphore для синхронизации
/// </summary>
public sealed class SemaphoreService : IPrimeCounter
{
    public PrimeCountResultDto CountPrimes(int start, int end, int threadCount) => throw new NotImplementedException();
    private readonly Semaphore _semaphore = new(1, 1);
    private int _primeCount;
    private readonly List<int> _foundPrimes = [];

    public string GetVersionName() => "Semaphore";

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

            threads[i] = new Thread(() => ProcessRange(threadStart, threadEnd, threadIndex + 1));
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

    private void ProcessRange(int start, int end, int threadNumber)
    {
        for (int number = start; number <= end; number++)
        {
            Console.WriteLine($"Поток {threadNumber} проверяет число {number}");

            if (!IsPrime(number))
                continue;

            _semaphore.WaitOne();
            try
            {
                _primeCount++;
                _foundPrimes.Add(number);
            }
            finally
            {
                _semaphore.Release();
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

    public string GetVersionName() => throw new NotImplementedException();
        return true;
    }
}
