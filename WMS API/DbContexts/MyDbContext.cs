using Microsoft.EntityFrameworkCore;
using WMS_API.Models.Events;
using WMS_API.Models;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.DbContexts
{
    public class MyDbContext : DbContext
    {
        public DbSet<WarehouseObject> WarehouseObjects { get; set; }
        public DbSet<EventType> EventTypes { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map entities to tables
            modelBuilder.Entity<WarehouseObject>().ToTable("WarehouseObjects");
            modelBuilder.Entity<EventType>().ToTable("EventTypes");

            // Configure Primary Keys
            modelBuilder.Entity<WarehouseObject>().HasKey(x => x.EventId).HasName("PK_WarehouseObjects");
            modelBuilder.Entity<EventType>().HasKey(x => x.Id).HasName("PK_EventTypes");

            // Configure indexes


            // Configure columns
            modelBuilder.Entity<WarehouseObject>().Property(x => x.EventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<WarehouseObject>().Property(x => x.ObjectType).HasColumnType("int").IsRequired();
            modelBuilder.Entity<WarehouseObject>().Property(x => x.ItemId).HasColumnType("char(36)");
            modelBuilder.Entity<WarehouseObject>().Property(x => x.ItemName).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<WarehouseObject>().Property(x => x.ItemDescription).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<WarehouseObject>().Property(x => x.LocationId).HasColumnType("char(36)");
            modelBuilder.Entity<WarehouseObject>().Property(x => x.LocationName).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<WarehouseObject>().Property(x => x.LocationDescription).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<WarehouseObject>().Property(x => x.ContainerId).HasColumnType("char(36)");
            modelBuilder.Entity<WarehouseObject>().Property(x => x.ContainerName).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<WarehouseObject>().Property(x => x.ContainerDescription).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<WarehouseObject>().Property(x => x.OrderId).HasColumnType("char(36)");
            modelBuilder.Entity<WarehouseObject>().Property(x => x.OrderName).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<WarehouseObject>().Property(x => x.OrderDescription).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<WarehouseObject>().Property(x => x.DateTimeStamp).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<WarehouseObject>().Property(x => x.Status).HasColumnType("int").IsRequired();
            modelBuilder.Entity<WarehouseObject>().Property(x => x.PreviousEventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<WarehouseObject>().Property(x => x.NextEventId).HasColumnType("char(36)").IsRequired();
            

            modelBuilder.Entity<EventType>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<EventType>().Property(x => x.EventTypeDescription).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<EventType>().HasData(
                new EventType { Id = Constants.LOCATION_REGISTERED_AS_UNOCCUPIED, EventTypeDescription = "Location Newly Registered, Unoccupied" },
                new EventType { Id = Constants.LOCATION_OCCUPIED, EventTypeDescription = "Location Occupied" },
                new EventType { Id = Constants.LOCATION_UNOCCUPIED, EventTypeDescription = "Location Unoccupied" },

                new EventType { Id = Constants.CONTAINER_REGISTERED_AS_NOT_IN_USE, EventTypeDescription = "Container Newly Registered, Not In Use" },
                new EventType { Id = Constants.CONTAINER_IN_USE, EventTypeDescription = "Container In Use" },
                new EventType { Id = Constants.CONTAINER_NOT_IN_USE, EventTypeDescription = "Container Not In Use" },

                new EventType { Id = Constants.ITEM_REGISTERED_WAITING_FOR_PUTAWAY_SELECTION, EventTypeDescription = "Item Newly Registered, Waiting To Be Selected For Putaway" },
                new EventType { Id = Constants.ITEM_SELECTED_FOR_PUTAWAY_PUTAWAY_IN_PROGRESS, EventTypeDescription = "Item Selected For Putaway, Putaway In Progress" },

                new EventType { Id = Constants.ITEM_PUTAWAY_INTO_LOCATION_COMPLETE, EventTypeDescription = "Item Putaway Into Location Complete" },

                new EventType { Id = Constants.ORDER_REGISTERED_WAITING_FOR_PICKING_SELECTION, EventTypeDescription = "Order Newly Registered, Waiting To Be Selected For Picking" },
                new EventType { Id = Constants.ITEM_ADDED_TO_ORDER, EventTypeDescription = "Item Added To Order" },
                new EventType { Id = Constants.ORDER_SELECTED_FOR_PICKING_PICKING_IN_PROGRESS, EventTypeDescription = "Order Selected For Picking, Picking In Progress" },
                new EventType { Id = Constants.CONTAINER_SELECTED_FOR_PICKING, EventTypeDescription = "Container Selected For Picking" },
                new EventType { Id = Constants.ITEM_PICKED_INTO_CONTAINER, EventTypeDescription = "Item Picked Into Container" }

                //new EventType { Id = Constants.ORDER_PICKING_COMPLETED_WAITING_FOR_PACKAGING_SELECTION, EventTypeDescription = "Order Picking Completed, Waiting Be To Selected For Packaging" },
                //new EventType { Id = Constants.ORDER_SELECTED_FOR_PACKAGING_PACKAGING_IN_PROGRESS, EventTypeDescription = "Order Selected For Packaging, Packaging In Progress" },

                //new EventType { Id = Constants.ORDER_PACKAGING_COMPLETED_WAITING_FOR_SHIPPING_SELECTION, EventTypeDescription = "Order Packaging Completed, Waiting To Be Selected For Shipping" },
                //new EventType { Id = Constants.ORDER_SELECTED_FOR_SHIPPING_SHIPPING_PREP_IN_PROGRESS, EventTypeDescription = "Order Selected For Shipping, Shipping Preparation In Progress" },

                //new EventType { Id = Constants.ORDER_SHIPPED, EventTypeDescription = "Order Shipped" }
                );

            // Configure relationships
            //modelBuilder.Entity<Container>().HasOne<Item>().WithOne().HasForeignKey<Container>(x => x.ItemId);
            //modelBuilder.Entity<Order>().HasMany(x => x.Items).WithOne().HasForeignKey(x => x.OrderEventId);
        }
    }
}