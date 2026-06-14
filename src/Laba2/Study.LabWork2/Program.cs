using Study.LabWork2.Abstractions.Feature.Task1.SubTask1;
using Study.LabWork2.Feature.Task1.SubTask1;

namespace Study.LabWork2;

public static class Program
{
    public static void Main()
    {
        // todo: какая-то обертка для вызова классов заданий
        int threadCount = Environment.ProcessorCount;

        IPrimeCounter[] services =
        [
            new MonitorService(),
            new MutexService(),
            new SemaphoreService()
        ];

        foreach (var service in services)
        {
            Console.WriteLine($"\n=== {service.GetVersionName()} ===\n");
            var result = service.CountPrimes(1, 10_000, threadCount);
            Console.WriteLine($"\n{result}\n");
            Console.WriteLine(new string('-', 50));
        }
    }
}
