using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace MemoryTest
{
    class Program
    {
        private static void Main(string[] args)
        {
            var connectionString = args.FirstOrDefault() ?? "Server=localhost;Database=memorytest";
            var iterationCountStr = args.ElementAtOrDefault(1);
            var batchCountStr = args.ElementAtOrDefault(2);
            var iterationCount = iterationCountStr != null ? Int32.Parse(iterationCountStr) : 10;
            var batchCount = batchCountStr != null ? Int32.Parse(batchCountStr) : 50;

            InitDb(connectionString, batchCount * 2);

            Console.WriteLine("Starting");

            for (var i = 0; i < iterationCount; i++)
            {
                var sw = Stopwatch.StartNew();
                using (var context = new TestContext(connectionString))
                {
                    var entities = context.Entities.Take(batchCount).ToList();
                    var ms = DateTime.Now.Millisecond;
                    foreach (var testEntity in entities)
                    {
                        testEntity.Data = ms;
                    }
                    context.SaveChanges();
                }

                sw.Stop();
                Console.WriteLine($"Updated {batchCount} entities in {sw.Elapsed.TotalSeconds:F2} s");
                LogMemory();
                Thread.Sleep(500);
            }
        }

        private static double GetMemoryMb()
        {
            using (var proc = Process.GetCurrentProcess())
            {
                var memory = proc.PrivateMemorySize64;
                var memoryMb = memory / 1024d / 1024d;
                return memoryMb;
            }
        }

        private static void LogMemory()
        {
            var memoryMb = GetMemoryMb();
            Console.WriteLine($"Memory {memoryMb:F2} Mb");
        }

        private static void InitDb(string connectionString, int entityCount)
        {
            Console.WriteLine("Initialzing");
            var sw = Stopwatch.StartNew();

            using (var context = new TestContext(connectionString))
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();

                for (var i = 0; i < entityCount; ++i)
                {
                    context.Entities.Add(new TestEntity());
                }

                context.SaveChanges();
            }

            sw.Stop();
            Console.WriteLine($"Initialized in {sw.ElapsedMilliseconds / 1000d:F2} s");

            LogMemory();
        }
    }
}

