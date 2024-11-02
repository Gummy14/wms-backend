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
            modelBuilder.Entity<EventType>().ToTable("EventTypes");

            // Configure Primary Keys
            modelBuilder.Entity<Item>().HasKey(x => x.ItemEventId).HasName("PK_Items");
            modelBuilder.Entity<Container>().HasKey(x => x.ContainerEventId).HasName("PK_Containers");
            modelBuilder.Entity<Order>().HasKey(x => x.OrderEventId).HasName("PK_Orders");
            modelBuilder.Entity<EventType>().HasKey(x => x.Id).HasName("PK_EventTypes");

            // Configure indexes


            // Configure columns
            modelBuilder.Entity<Item>().Property(x => x.ItemEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.ItemId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.ContainerEventId).HasColumnType("char(36)");
            modelBuilder.Entity<Item>().Property(x => x.OrderId).HasColumnType("char(36)");
            modelBuilder.Entity<Item>().Property(x => x.EventDateTime).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.EventType).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.PreviousItemEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.NextItemEventId).HasColumnType("char(36)").IsRequired();

            modelBuilder.Entity<Container>().Property(x => x.ContainerEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.ContainerId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.ItemEventId).HasColumnType("char(36)");
            modelBuilder.Entity<Container>().Property(x => x.EventDateTime).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.EventType).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.PreviousContainerEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.NextContainerEventId).HasColumnType("char(36)").IsRequired();

            modelBuilder.Entity<Order>().Property(x => x.OrderEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.OrderId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.EventType).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.EventDateTime).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.NumberOfItemsPickedForOrder).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.TotalNumberOfItemsInOrder).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.PreviousOrderEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.NextOrderEventId).HasColumnType("char(36)").IsRequired();

            modelBuilder.Entity<EventType>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<EventType>().Property(x => x.EventTypeDescription).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<EventType>().HasData(
                new EventType { Id = 1, EventTypeDescription = "Registered" },
                new EventType { Id = 2, EventTypeDescription = "Putaway" },
                new EventType { Id = 3, EventTypeDescription = "Added To Order" },
                new EventType { Id = 4, EventTypeDescription = "Picked" },
                new EventType { Id = 5, EventTypeDescription = "Order Created" },
                new EventType { Id = 6, EventTypeDescription = "Item In Order Picked" }
                );

            // Configure relationships
            //modelBuilder.Entity<Container>().HasOne<Item>().WithOne();
            //modelBuilder.Entity<Item>().HasOne<Container>().WithOne();
            //modelBuilder.Entity<Order>().HasMany(x => x.Items).WithOne().HasForeignKey("OrderId");
        }
    }
}