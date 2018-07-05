using System.ComponentModel.DataAnnotations;

namespace MemoryTest
{
    class TestEntity
    {
        [Key]
        public int Id { get; set; }

        public int Data { get; set; }
    }
}