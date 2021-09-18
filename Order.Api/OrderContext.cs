using Microsoft.EntityFrameworkCore;

namespace Order.Api
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options)
           : base(options)
        {

        }

        public DbSet<Models.Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
