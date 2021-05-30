using System;
using System.Diagnostics;
using BIGsort.Lib;

namespace BIGsort.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string path = @"C:\Users\Вакор\Desktop\Projects\ASD\ASD_lab7\BIGsort\SortFile\";
            string name = "Main1.txt";
            BigFiles bigFiles = new BigFiles(path, name);
            bigFiles.CreatedInitialFile();
            //bigFiles.NaturalSort();
            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds/1000} sec");
        }
    }
}