using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopmart.Models;


namespace Shopmart.Data
{
    public class ApplicationDbContext : IdentityDbContext<ConfigUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Tạo khóa chính cho Prodcut và Category
            modelBuilder.Entity<Product>().HasKey(s => s.ProductID);
            modelBuilder.Entity<Category>().HasKey(s => s.CategoryID);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderID);
            modelBuilder.Entity<OrderDetail>().HasKey(od => od.OrderDetailID);

            //Tạo quan hệ cho Product và Category
            modelBuilder.Entity<Product>()
                .HasOne(s => s.Category)
                .WithMany(s => s.Products)
                .HasForeignKey(s => s.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
            //Tạo quan hệ cho Order và OrderDetail
            modelBuilder.Entity<OrderDetail>()
               .HasOne(od => od.Order)
               .WithMany(o => o.OrderDetails)
               .HasForeignKey(s => s.OrderID)
               .OnDelete(DeleteBehavior.Restrict);
            //Tạo quan hệ cho Order và ConfigUser
            modelBuilder.Entity<Order>()
               .HasOne(od => od.User)
               .WithMany(cf => cf.Orders)
               .HasForeignKey(od => od.Id)
               .OnDelete(DeleteBehavior.Restrict);


        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ConfigUser> ConfigUsers { get; set; }
    }
}
