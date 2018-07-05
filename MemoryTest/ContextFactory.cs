using System.Linq;
using Microsoft.EntityFrameworkCore.Design;

namespace MemoryTest
{
    class ContextFactory : IDesignTimeDbContextFactory<TestContext>
    {
        public TestContext CreateDbContext(string[] args)
        {
            return new TestContext(args.FirstOrDefault() ?? "Server=localhost;Database=memorytest");
        }
    }
}