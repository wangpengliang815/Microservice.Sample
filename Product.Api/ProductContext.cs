using Microsoft.EntityFrameworkCore;

namespace Product.Api
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
           : base(options)
        {

        }

        public DbSet<Models.Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //初始化种子数据
            modelBuilder.Entity<Models.Product>().HasData(new Models.Product
            {
                ID = 1,
                Name = "ThinkPad",
                Stock = 100
            },
            new Models.Product
            {
                ID = 2,
                Name = "Mac",
                Stock = 100
            });
        }
    }
}
