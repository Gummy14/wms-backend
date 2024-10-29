using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS_API.Models.Containers;
using WMS_API.Models.Events;
using WMS_API.Models.Items;
using WMS_API.Models.Orders;

namespace WMS_API.DbContexts
{
    public class MyDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ItemContainerEvent> ItemContainerEventHistory { get; set; }
        public DbSet<EventType> EventTypes { get; set; }


        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map entities to tables
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<Container>().ToTable("Containers");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<ItemContainerEvent>().ToTable("ItemContainerEventHistory");
            modelBuilder.Entity<EventType>().ToTable("EventTypes");

            // Configure Primary Keys
            modelBuilder.Entity<Item>().HasKey(x => x.Id).HasName("PK_Items");
            modelBuilder.Entity<Container>().HasKey(x => x.Id).HasName("PK_Containers");
            modelBuilder.Entity<Order>().HasKey(x => x.Id).HasName("PK_Orders");
            modelBuilder.Entity<ItemContainerEvent>().HasKey(x => x.Id).HasName("PK_ItemContainerEventHistory");
            modelBuilder.Entity<EventType>().HasKey(x => x.Id).HasName("PK_EventTypes");

            // Configure indexes


            // Configure columns
            modelBuilder.Entity<Item>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.DateTimeRegistered).HasColumnType("datetime").IsRequired();

            modelBuilder.Entity<Container>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.DateTimeRegistered).HasColumnType("datetime").IsRequired();

            modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.DateTimeOrderRecieved).HasColumnType("datetime").IsRequired();

            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.EventType).HasColumnType("int").IsRequired();
            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.DateTimeStamp).HasColumnType("datetime").IsRequired();

            modelBuilder.Entity<EventType>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<EventType>().Property(x => x.EventTypeDescription).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<EventType>().HasData(
                new EventType { Id = 1, EventTypeDescription = "Putaway" },
                new EventType { Id = 2, EventTypeDescription = "Pick" }
                );

            // Configure relationships
            modelBuilder.Entity<Container>().HasOne(x => x.Item).WithOne().HasForeignKey<Container>("ItemId");
            modelBuilder.Entity<Order>().HasMany(x => x.OrderItems).WithOne().HasForeignKey("OrderId");
            modelBuilder.Entity<ItemContainerEvent>().HasOne(x => x.Item).WithOne().HasForeignKey<ItemContainerEvent>("ItemId");
            modelBuilder.Entity<ItemContainerEvent>().HasOne(x => x.Container).WithOne().HasForeignKey<ItemContainerEvent>("ContainerId");
        }
    }
}