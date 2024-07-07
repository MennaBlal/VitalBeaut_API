using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
namespace EcommercePro.Models
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
         public DbSet<Payment> payments { get; set; }
        public DbSet<WishList> WishList { get; set; }//
        public DbSet<WebsiteReview> WebsiteReviews { get; set; }//
        public DbSet<ProductReview> ProductReviews { get; set; }
          public DbSet<Brand> Brands { get; set; }

        //public DbSet<AdminNotification> AdminNotifications { get; set; }

        public Context(DbContextOptions option) : base(option)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>()
             .HasMany(o => o.OrderItems)
             .WithOne(oi => oi.Order)
             .HasForeignKey(oi => oi.OrderId);

                    modelBuilder.Entity<OrderItem>()
                        .HasOne(oi => oi.Product)
                        .WithMany()
                        .HasForeignKey(oi => oi.ProductId);

            // Configure Price precision and scale for OrderItem model
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");
        }

    }
}
