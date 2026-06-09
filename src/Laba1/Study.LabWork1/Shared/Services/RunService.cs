using System;
using Study.LabWork1.Features.Task1;


namespace Study.LabWork1.Shared.Services
{
    /// <summary>
    /// Сервис для выполнения заданий лабораторной работы
    /// </summary>
    public class LabRunnerService
    {
        /// <summary>
        /// Задание 1 - RGBA пиксели
        /// </summary>
        public void RunTask1()
        {
            var pixel1 = new RGBA(100, 150, 200, 0.5);
            var pixel2 = new RGBA(80, 120, 90, 0.3);

            var sum = pixel1 + pixel2;
            var diff = pixel1 - pixel2;
            var multiplied = pixel1 * pixel2;
            var scaled = pixel1 * 1.5;
            var divided = pixel1 / 2;

            Console.WriteLine("\n=== Task 1: RGBA Pixel ===\n");
            Console.WriteLine($"pixel1 = {pixel1}");
            Console.WriteLine($"pixel2 = {pixel2}");
            Console.WriteLine($"pixel1 + pixel2 = {sum}");
            Console.WriteLine($"pixel1 - pixel2 = {diff}");
            Console.WriteLine($"pixel1 * pixel2 = {multiplied}");
            Console.WriteLine($"pixel1 * 1.5 = {scaled}");
            Console.WriteLine($"pixel1 / 2 = {divided}");
            Console.WriteLine($"pixel1 HEX = {pixel1.ToHex()}");
            Console.WriteLine($"pixel1 HEX with alpha = {pixel1.ToHexWithAlpha()}");
            Console.WriteLine($"pixel1 == pixel2 -> {pixel1 == pixel2}");
        }
    }
}
