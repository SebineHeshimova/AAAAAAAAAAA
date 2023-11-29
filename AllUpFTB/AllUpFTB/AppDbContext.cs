using AllUpFTB.Models;
using Microsoft.EntityFrameworkCore;

namespace AllUpFTB
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Brend> Brends { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
    }
}
