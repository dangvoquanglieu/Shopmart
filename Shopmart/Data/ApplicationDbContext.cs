using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopmart.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
            

            //Tạo quan hệ cho Product và Category
            modelBuilder.Entity<Product>()
                .HasOne(s => s.Category)
                .WithMany(s => s.Products)
                .HasForeignKey(s => s.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        
    }
}
