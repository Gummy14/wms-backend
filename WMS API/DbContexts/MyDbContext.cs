using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS_API.Models.Events;
using WMS_API.Models.Orders;
using WMS_API.Models;
using WMS_API.Models.WarehouseObjects;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Containers;

namespace WMS_API.DbContexts
{
    public class MyDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Location> Locations { get; set; }
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
            modelBuilder.Entity<Location>().ToTable("Locations");
            modelBuilder.Entity<Container>().ToTable("Containers");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<EventType>().ToTable("EventTypes");

            // Configure Primary Keys
            modelBuilder.Entity<Item>().HasKey(x => x.EventId).HasName("PK_Items");
            modelBuilder.Entity<Location>().HasKey(x => x.EventId).HasName("PK_Locations");
            modelBuilder.Entity<Container>().HasKey(x => x.EventId).HasName("PK_Containers");
            modelBuilder.Entity<Order>().HasKey(x => x.EventId).HasName("PK_Orders");
            modelBuilder.Entity<EventType>().HasKey(x => x.Id).HasName("PK_EventTypes");

            // Configure indexes


            // Configure columns
            modelBuilder.Entity<Item>().Property(x => x.EventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.DateTimeStamp).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Status).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.PreviousEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.NextEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.LocationId).HasColumnType("char(36)");
            modelBuilder.Entity<Item>().Property(x => x.ContainerId).HasColumnType("char(36)");
            modelBuilder.Entity<Item>().Property(x => x.OrderId).HasColumnType("char(36)");

            modelBuilder.Entity<Location>().Property(x => x.EventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Location>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Location>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Location>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Location>().Property(x => x.DateTimeStamp).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<Location>().Property(x => x.Status).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Location>().Property(x => x.PreviousEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Location>().Property(x => x.NextEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Location>().Property(x => x.ItemId).HasColumnType("char(36)");

            modelBuilder.Entity<Container>().Property(x => x.EventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.DateTimeStamp).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.Status).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.PreviousEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.NextEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.ItemId).HasColumnType("char(36)");

            modelBuilder.Entity<Order>().Property(x => x.EventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.DateTimeStamp).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.Status).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.PreviousEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.NextEventId).HasColumnType("char(36)").IsRequired();

            modelBuilder.Entity<EventType>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<EventType>().Property(x => x.EventTypeDescription).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<EventType>().HasData(
                new EventType { Id = Constants.CONTAINER_EMPTY, EventTypeDescription = "Container Registered" },
                new EventType { Id = Constants.CONTAINER_FULL, EventTypeDescription = "Container Declared Full" },

                new EventType { Id = Constants.ITEM_REGISTERED_WAITING_FOR_PUTAWAY_SELECTION, EventTypeDescription = "Item Registered, Waiting To Be Selected For Putaway" },
                new EventType { Id = Constants.ITEM_SELECTED_FOR_PUTAWAY_PUTAWAY_IN_PROGRESS, EventTypeDescription = "Item Selected For Putaway, Putaway In Progress" },

                new EventType { Id = Constants.ITEM_PUTAWAY_INTO_CONTAINER_COMPLETE, EventTypeDescription = "Item Putaway Into Container Complete" },

                new EventType { Id = Constants.ORDER_REGISTERED_WAITING_FOR_PICKING_SELECTION, EventTypeDescription = "Order Registered, Waiting To Be Selected For Picking" },
                new EventType { Id = Constants.ITEM_ADDED_TO_ORDER, EventTypeDescription = "Item Added To Order" },
                new EventType { Id = Constants.ORDER_SELECTED_FOR_PICKING_PICKING_IN_PROGRESS, EventTypeDescription = "Order Selected For Picking, Picking In Progress" },
                new EventType { Id = Constants.CONTAINER_SELECTED_FOR_PICKING, EventTypeDescription = "Container Selected For Picking" },
                new EventType { Id = Constants.ITEM_PICKED_FROM_CONTAINER_BEFORE, EventTypeDescription = "Item Pick From Container Before" },
                new EventType { Id = Constants.ITEM_PICKED_FROM_CONTAINER_AFTER, EventTypeDescription = "Item Pick From Container After" },

                new EventType { Id = Constants.ORDER_PICKING_COMPLETED_WAITING_FOR_PACKAGING_SELECTION, EventTypeDescription = "Order Picking Completed, Waiting Be To Selected For Packaging" },
                new EventType { Id = Constants.ORDER_SELECTED_FOR_PACKAGING_PACKAGING_IN_PROGRESS, EventTypeDescription = "Order Selected For Packaging, Packaging In Progress" },

                new EventType { Id = Constants.ORDER_PACKAGING_COMPLETED_WAITING_FOR_SHIPPING_SELECTION, EventTypeDescription = "Order Packaging Completed, Waiting To Be Selected For Shipping" },
                new EventType { Id = Constants.ORDER_SELECTED_FOR_SHIPPING_SHIPPING_PREP_IN_PROGRESS, EventTypeDescription = "Order Selected For Shipping, Shipping Preparation In Progress" },

                new EventType { Id = Constants.ORDER_SHIPPED, EventTypeDescription = "Order Shipped" }
                );

            // Configure relationships
            //modelBuilder.Entity<Container>().HasOne<Item>().WithOne().HasForeignKey<Container>(x => x.ItemId);
            //modelBuilder.Entity<Order>().HasMany(x => x.Items).WithOne().HasForeignKey(x => x.OrderEventId);
        }
    }
}