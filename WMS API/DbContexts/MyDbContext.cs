using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS_API.Models.Containers;
using WMS_API.Models.Events;
using WMS_API.Models.Items;
using WMS_API.Models.Orders;
using WMS_API.Models;

namespace WMS_API.DbContexts
{
    public class MyDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<EventType> EventTypes { get; set; }


        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map entities to tables
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<Container>().ToTable("Containers");
            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetails");
            modelBuilder.Entity<EventType>().ToTable("EventTypes");

            // Configure Primary Keys
            modelBuilder.Entity<Item>().HasKey(x => x.ItemEventId).HasName("PK_Items");
            modelBuilder.Entity<Container>().HasKey(x => x.ContainerEventId).HasName("PK_Containers");
            modelBuilder.Entity<OrderDetail>().HasKey(x => x.OrderEventId).HasName("PK_OrderDetails");
            modelBuilder.Entity<EventType>().HasKey(x => x.Id).HasName("PK_EventTypes");

            // Configure indexes


            // Configure columns
            modelBuilder.Entity<Item>().Property(x => x.ItemEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.ItemId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.ContainerId).HasColumnType("char(36)");
            modelBuilder.Entity<Item>().Property(x => x.OrderId).HasColumnType("char(36)");
            modelBuilder.Entity<Item>().Property(x => x.EventDateTime).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.EventType).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.PreviousItemEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.NextItemEventId).HasColumnType("char(36)").IsRequired();

            modelBuilder.Entity<Container>().Property(x => x.ContainerEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.ContainerId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.ItemId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.EventDateTime).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.EventType).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.PreviousContainerEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.NextContainerEventId).HasColumnType("char(36)").IsRequired();

            modelBuilder.Entity<OrderDetail>().Property(x => x.OrderEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<OrderDetail>().Property(x => x.OrderId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<OrderDetail>().Property(x => x.OrderStatusDateTime).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<OrderDetail>().Property(x => x.OrderStatus).HasColumnType("int").IsRequired();
            modelBuilder.Entity<OrderDetail>().Property(x => x.PreviousOrderEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<OrderDetail>().Property(x => x.NextOrderEventId).HasColumnType("char(36)").IsRequired();

            modelBuilder.Entity<EventType>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<EventType>().Property(x => x.EventTypeDescription).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<EventType>().HasData(
                new EventType { Id = Constants.CONTAINER_REGISTERED, EventTypeDescription = "Container Registered" },

                new EventType { Id = Constants.ITEM_REGISTERED_ADDED_TO_PUTAWAY_QUEUE, EventTypeDescription = "Item Registered, Added To Putaway Queue" },
                new EventType { Id = Constants.ITEM_SELECTED_FROM_PUTAWAY_QUEUE_PUTAWAY_IN_PROGRESS, EventTypeDescription = "Item Selected From Putaway Queue, Putaway In Progress" },
                new EventType { Id = Constants.ITEM_PUTAWAY_INTO_CONTAINER_COMPLETE, EventTypeDescription = "Item Putaway Into Container Complete" },

                new EventType { Id = Constants.ORDER_ADDED_TO_NEW_ORDERS_QUEUE_WAITING_TO_BE_SELECTED, EventTypeDescription = "Order Received, Added To New Orders Queue, Waiting To Be Selected" },
                new EventType { Id = Constants.ITEM_ADDED_TO_ORDER, EventTypeDescription = "Item Added To Order" },

                new EventType { Id = Constants.ORDER_SELECTED_FROM_NEW_ORDERS_QUEUE_PICKING_IN_PROGRESS, EventTypeDescription = "Order Selected From New Orders Queue, Picking In Progress" },
                new EventType { Id = Constants.ITEM_PICKED_FROM_CONTAINER_BEFORE, EventTypeDescription = "Item Pick From Container Before" },
                new EventType { Id = Constants.ITEM_PICKED_FROM_CONTAINER_AFTER, EventTypeDescription = "Item Pick From Container After" },
                new EventType { Id = Constants.ORDER_PICKING_COMPLETED_MOVING_tO_PACKAGING_QUEUE, EventTypeDescription = "Order Picking Completed, Moving To Packaging Queue" },
                
                new EventType { Id = Constants.ORDER_ADDED_TO_PACKAGING_QUEUE_WAITING_TO_BE_SELECTED, EventTypeDescription = "Order Added To Packaging Queue, Waiting To Be Selected" },
                new EventType { Id = Constants.ORDER_SELECTED_FROM_PACKAGING_QUEUE_PACKAGING_IN_PROGRESS, EventTypeDescription = "Order Selected From Packaging Queue, Packaging In Progress" },
                new EventType { Id = Constants.ORDER_PACKAGING_COMPLETED_MOVING_TO_SHIPPING_QUEUE, EventTypeDescription = "Order Packaging Completed, Moving To Shipping Queue" },
                
                new EventType { Id = Constants.ORDER_ADDED_SHIPPING_QUEUE_WAITING_TO_BE_SELECTED, EventTypeDescription = "Order Added Shipping Queue, Waiting To Be Selected" },
                new EventType { Id = Constants.ORDER_SELECTED_FROM_SHIPPING_QUEUE_SHIPPING_PREPERATION_IN_PROGRESS, EventTypeDescription = "Order Seleted From Shipping Queue, Shipping Preparation In Progress" },
                new EventType { Id = Constants.ORDER_SHIPPED, EventTypeDescription = "Order Shipped" }
                );

            // Configure relationships
            //modelBuilder.Entity<Container>().HasOne<Item>().WithOne().HasForeignKey<Container>(x => x.ItemId);
            //modelBuilder.Entity<Order>().HasMany(x => x.Items).WithOne().HasForeignKey(x => x.OrderEventId);
        }
    }
}