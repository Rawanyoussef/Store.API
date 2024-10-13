﻿using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntity;
using System.Reflection;

namespace Store.Data.Contexts
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }  
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethodS {  get; set; }
        public DbSet<Order>Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }



    }
}
