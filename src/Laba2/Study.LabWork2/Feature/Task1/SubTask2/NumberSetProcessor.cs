namespace Study.LabWork2.UnitTests.Feature.Task1.SubTask2;

using System.Diagnostics;
using global::Study.LabWork2.Abstractions.Feature.Task1.SubTask2;
using global::Study.LabWork2.Abstractions.Feature.Task1.SubTask2.DtoModels;
using Study.LabWork2.Abstractions.Feature.Task1.SubTask2;
using Study.LabWork2.Abstractions.Feature.Task1.SubTask2.DtoModels;

[TestFixture]
public sealed class NumberSetProcessorTests
namespace Study.LabWork2.Feature.Task1.SubTask2;

public sealed class NumberSetProcessor : INumberSetProcessor
{
    private readonly List<int[]> _dataSets;
    private readonly int _maxConcurrentThreads;
    private readonly List<ResultEntryDto> _results = new();
    private int _totalSum;
    private readonly object _resultsLock = new();
    private readonly Mutex _totalSumMutex = new();
    private readonly SemaphoreSlim _semaphore;
    private TimeSpan _executionTime;

    public NumberSetProcessor() : this(3)
    {
    }

    public NumberSetProcessor(int maxConcurrentThreads)
    {
        _maxConcurrentThreads = maxConcurrentThreads;
        _semaphore = new SemaphoreSlim(maxConcurrentThreads, maxConcurrentThreads);
        _dataSets = GenerateDataSets();
    }

    internal NumberSetProcessor(List<int[]> testDataSets, int maxConcurrentThreads = 3)
    {
        _maxConcurrentThreads = maxConcurrentThreads;
        _semaphore = new SemaphoreSlim(maxConcurrentThreads, maxConcurrentThreads);
        _dataSets = testDataSets;
    }

    private List<int[]> GenerateDataSets()
    {
        var random = new Random();
        var sets = new List<int[]>();
        for (int i = 0; i < 15; i++)
        {
            var numbers = new int[100];
            for (int j = 0; j < 100; j++)
                numbers[j] = random.Next(1, 101);
            sets.Add(numbers);
        }
        return sets;
    }

    public void Process()
    {
        var sw = Stopwatch.StartNew();
        var threads = new List<Thread>();

        for (int i = 0; i < _dataSets.Count; i++)
        {
            int index = i;
            var thread = new Thread(() => ProcessSet(index));
            threads.Add(thread);
            thread.Start();
        }

        foreach (var t in threads) t.Join();
        sw.Stop();
        _executionTime = sw.Elapsed;
    }

    private void ProcessSet(int setIndex)
    {
        _semaphore.Wait();
        try
        {
            int sum = _dataSets[setIndex].Sum();
            int threadId = Thread.CurrentThread.ManagedThreadId;

            lock (_resultsLock)
            {
                _results.Add(new ResultEntryDto
                {
                    SetNumber = setIndex + 1,
                    Sum = sum,
                    ThreadId = threadId
                });
            }

            _totalSumMutex.WaitOne();
            try
            {
                _totalSum += sum;
            }
            finally
            {
                _totalSumMutex.ReleaseMutex();
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public ProcessingResultDto GetResult()
    {
        return new ProcessingResultDto
        {
            Results = _results.OrderBy(r => r.SetNumber).ToList(),
            TotalSum = _totalSum,
            ExecutionTime = _executionTime,
            ProcessedSetsCount = _results.Count
        };
    }
}
