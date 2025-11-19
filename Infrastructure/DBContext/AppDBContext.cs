using API_ShoesShop.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Domain.Entities;

namespace API_ShoesShop.Infrastructure.DBContext
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductDetail>()
                .HasOne(pd => pd.Color)
                .WithMany()
                .HasForeignKey(pd => pd.ColorId);

            modelBuilder.Entity<ProductDetail>()
                .HasOne(pd => pd.Size)
                .WithMany()
                .HasForeignKey(pd => pd.SizeId);

            modelBuilder.Entity<Cart>()
               .HasOne(c => c.User)
               .WithMany()
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
               .HasOne(ci => ci.Cart)
               .WithMany(c => c.CartItems)
               .HasForeignKey(ci => ci.CartId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.ProductDetail)
                .WithMany()
                .HasForeignKey(ci => ci.ProductDetailId)
                .OnDelete(DeleteBehavior.Restrict);
           
            modelBuilder.Entity<Address>()
                .HasIndex(a => new { a.UserId, a.IsDefault })
                .IsUnique()
                .HasFilter("[IsDefault] = 1");
        }

    }


}
