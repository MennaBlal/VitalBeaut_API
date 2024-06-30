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
        public DbSet<Order> Orders { get; set; }
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
    }
}
