using Microsoft.EntityFrameworkCore;

namespace MemoryTest
{
    class TestContext : DbContext
    {
        private readonly string _connectionString;

        public TestContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<TestEntity> Entities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString);
        }
    }
}