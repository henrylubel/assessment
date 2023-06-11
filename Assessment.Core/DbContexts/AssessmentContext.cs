using Assessment.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Core.DbContexts
{
    public class AssessmentContext : DbContext
    {
        public AssessmentContext(DbContextOptions<AssessmentContext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }
        }
    }
}
