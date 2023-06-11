

namespace Assessment.Core.Domain.Models
{
    public class Product : ModelBase
    {
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<Order>? Orders { get; }
    }
}
