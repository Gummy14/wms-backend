using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS_API.Models;

namespace WMS_API.DbContexts
{
    public class MyDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map entities to tables
            modelBuilder.Entity<Item>().ToTable("Items");

            // Configure Primary Keys
            modelBuilder.Entity<Item>().HasKey(x => x.Id).HasName("PK_Items");

            // Configure indexes

            // Configure columns
            modelBuilder.Entity<Item>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();

            // Configure relationships
        }
    }
}
