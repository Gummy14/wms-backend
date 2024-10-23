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
        public DbSet<EventStatus> Statuses { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<ItemContainerEvent> ItemContainerEvents { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map entities to tables
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<EventStatus>().ToTable("Statuses");
            modelBuilder.Entity<Container>().ToTable("Containers");
            modelBuilder.Entity<ItemContainerEvent>().ToTable("ItemContainerEvents");

            // Configure Primary Keys
            modelBuilder.Entity<Item>().HasKey(x => x.Id).HasName("PK_Items");
            modelBuilder.Entity<EventStatus>().HasKey(x => x.Id).HasName("PK_Statuses");
            modelBuilder.Entity<Container>().HasKey(x => x.Id).HasName("PK_Containers");
            modelBuilder.Entity<ItemContainerEvent>().HasKey(x => x.Id).HasName("PK_ItemContainerEvents");

            // Configure indexes

            // Configure columns
            modelBuilder.Entity<Item>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();

            modelBuilder.Entity<EventStatus>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<EventStatus>().Property(x => x.StatusType).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<EventStatus>().HasData(
                new EventStatus { Id = 1, StatusType = "Putaway" },
                new EventStatus { Id = 2, StatusType = "Picked" }
                );

            modelBuilder.Entity<Container>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();

            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.ItemId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.ContainerId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.EventType).HasColumnType("int").IsRequired();
            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.TimeStamp).HasColumnType("datetime").IsRequired();

            // Configure relationships
        }
    }
}
