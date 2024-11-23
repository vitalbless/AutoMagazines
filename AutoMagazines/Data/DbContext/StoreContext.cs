using AutoMagazines.Data.Models.Cart;
using AutoMagazines.Data.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AutoMagazines.Data.DbContext
{
    public class StoreContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=AutoMagazine;Trusted_Connection=True;TrustServerCertificate=True;");

        }
        public DbSet<Car> CarTable { get; set; }
        public DbSet<Category> CategoryTable { get; set; }
        public DbSet<ShopCartItem> ShopCartItemTable { get; set; }
        public DbSet<Order> OrderTable { get; set; }
        public DbSet<OrderDetail> OrderDetailTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetail)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Car)
                .WithMany()
                .HasForeignKey(od => od.CarId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Cars)
                .HasForeignKey(c => c.CategoryId);
        }
    }
}
