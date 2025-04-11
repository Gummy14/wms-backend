using Microsoft.EntityFrameworkCore;
using WMS_API.Models.Events;
using WMS_API.Models.Orders;
using WMS_API.Models;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Containers;
using WMS_API.Models.Boxes;

namespace WMS_API.DbContexts
{
    public class MyDbContext : DbContext
    {
        public DbSet<ItemData> ItemData { get; set; }
        public DbSet<Item> Items { get; set; }

        public DbSet<LocationData> LocationData { get; set; }
        public DbSet<Location> Locations { get; set; }

        public DbSet<OrderData> OrderData { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<ContainerData> ContainerData { get; set; }
        public DbSet<Container> Containers { get; set; }

        public DbSet<BoxData> BoxData { get; set; }
        public DbSet<Box> Boxes { get; set; }


        public DbSet<Address> Addresses { get; set; }
        public DbSet<EventType> EventTypes { get; set; }


        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map entities to tables
            modelBuilder.Entity<ItemData>().ToTable("ItemData");
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<LocationData>().ToTable("LocationData");
            modelBuilder.Entity<Location>().ToTable("Locations");
            modelBuilder.Entity<OrderData>().ToTable("OrderData");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<ContainerData>().ToTable("ContainerData");
            modelBuilder.Entity<Container>().ToTable("Containers");
            modelBuilder.Entity<BoxData>().ToTable("BoxData");
            modelBuilder.Entity<Box>().ToTable("Boxes");
            modelBuilder.Entity<Address>().ToTable("Addresses");
            modelBuilder.Entity<EventType>().ToTable("EventTypes");

            // Configure Primary Keys
            modelBuilder.Entity<ItemData>().HasKey(x => x.EventId).HasName("PK_ItemData");
            modelBuilder.Entity<LocationData>().HasKey(x => x.EventId).HasName("PK_LocationData");
            modelBuilder.Entity<OrderData>().HasKey(x => x.EventId).HasName("PK_OrderData");
            modelBuilder.Entity<ContainerData>().HasKey(x => x.EventId).HasName("PK_ContainerData");
            modelBuilder.Entity<BoxData>().HasKey(x => x.EventId).HasName("PK_BoxData");
            modelBuilder.Entity<Item>().HasKey(x => x.Id).HasName("PK_Items");
            modelBuilder.Entity<Location>().HasKey(x => x.Id).HasName("PK_Locations");
            modelBuilder.Entity<Order>().HasKey(x => x.Id).HasName("PK_Orders");
            modelBuilder.Entity<Container>().HasKey(x => x.Id).HasName("PK_Containers");
            modelBuilder.Entity<Box>().HasKey(x => x.Id).HasName("PK_Boxes");
            modelBuilder.Entity<Address>().HasKey(x => x.Id).HasName("PK_Addresses");
            modelBuilder.Entity<EventType>().HasKey(x => x.Id).HasName("PK_EventTypes");

            // Configure indexes


            // Configure columns
            modelBuilder.Entity<Item>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Location>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Box>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();

            modelBuilder.Entity<ItemData>().Property(x => x.EventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.DateTimeStamp).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.Status).HasColumnType("int").IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.LengthInCentimeters).HasColumnType("float").IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.WidthInCentimeters).HasColumnType("float").IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.HeightInCentimeters).HasColumnType("float").IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.WeightInKilograms).HasColumnType("float").IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.NextEventId).HasColumnType("char(36)");
            modelBuilder.Entity<ItemData>().Property(x => x.PrevEventId).HasColumnType("char(36)");

            modelBuilder.Entity<LocationData>().Property(x => x.EventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.DateTimeStamp).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.Status).HasColumnType("int").IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.LengthInCentimeters).HasColumnType("float").IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.WidthInCentimeters).HasColumnType("float").IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.HeightInCentimeters).HasColumnType("float").IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.MaxWeightInKilograms).HasColumnType("float").IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.NextEventId).HasColumnType("char(36)");
            modelBuilder.Entity<LocationData>().Property(x => x.PrevEventId).HasColumnType("char(36)");

            modelBuilder.Entity<ContainerData>().Property(x => x.EventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<ContainerData>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<ContainerData>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<ContainerData>().Property(x => x.DateTimeStamp).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<ContainerData>().Property(x => x.Status).HasColumnType("int").IsRequired();
            modelBuilder.Entity<ContainerData>().Property(x => x.NextEventId).HasColumnType("char(36)");
            modelBuilder.Entity<ContainerData>().Property(x => x.PrevEventId).HasColumnType("char(36)");

            modelBuilder.Entity<OrderData>().Property(x => x.EventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<OrderData>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<OrderData>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<OrderData>().Property(x => x.DateTimeStamp).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<OrderData>().Property(x => x.Status).HasColumnType("int").IsRequired();
            modelBuilder.Entity<OrderData>().Property(x => x.NextEventId).HasColumnType("char(36)");
            modelBuilder.Entity<OrderData>().Property(x => x.PrevEventId).HasColumnType("char(36)");

            modelBuilder.Entity<Address>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.FirstName).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.LastName).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.Street).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.City).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.State).HasColumnType("nvarchar(2)").IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.Zip).HasColumnType("nvarchar(5)").IsRequired();

            modelBuilder.Entity<BoxData>().Property(x => x.EventId).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.DateTimeStamp).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.Status).HasColumnType("int").IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.LengthInCentimeters).HasColumnType("float").IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.WidthInCentimeters).HasColumnType("float").IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.HeightInCentimeters).HasColumnType("float").IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.NextEventId).HasColumnType("char(36)");
            modelBuilder.Entity<BoxData>().Property(x => x.PrevEventId).HasColumnType("char(36)");

            modelBuilder.Entity<EventType>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<EventType>().Property(x => x.EventTypeDescription).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<EventType>().HasData(
                new EventType { Id = Constants.LOCATION_OCCUPIED, EventTypeDescription = "Location Occupied" },
                new EventType { Id = Constants.LOCATION_UNOCCUPIED, EventTypeDescription = "Location Unoccupied" },

                new EventType { Id = Constants.CONTAINER_IN_USE, EventTypeDescription = "Container In Use" },
                new EventType { Id = Constants.CONTAINER_NOT_IN_USE, EventTypeDescription = "Container Not In Use" },

                new EventType { Id = Constants.ITEM_REGISTERED_WAITING_FOR_PUTAWAY, EventTypeDescription = "Item Newly Registered, Waiting To Be Selected For Putaway" },

                new EventType { Id = Constants.ITEM_PUTAWAY_INTO_LOCATION_COMPLETE, EventTypeDescription = "Item Putaway Into Location Complete" },

                new EventType { Id = Constants.ORDER_REGISTERED_WAITING_FOR_ACKNOWLEDGEMENT, EventTypeDescription = "Order Newly Registered, Waiting To Be Selected For Picking" },
                new EventType { Id = Constants.ITEM_ADDED_TO_ORDER, EventTypeDescription = "Item Added To Order" },
                new EventType { Id = Constants.ORDER_ACKNOWLEDGED_PICKING_IN_PROGRESS, EventTypeDescription = "Order Selected For Picking, Picking In Progress" },
                new EventType { Id = Constants.CONTAINER_ADDED_TO_ORDER, EventTypeDescription = "Container Added To Order" },
                new EventType { Id = Constants.ITEM_PICKED_INTO_CONTAINER, EventTypeDescription = "Item Picked Into Container" }

                //new EventType { Id = Constants.ORDER_PICKING_COMPLETED_WAITING_FOR_PACKAGING_SELECTION, EventTypeDescription = "Order Picking Completed, Waiting Be To Selected For Packaging" },
                //new EventType { Id = Constants.ORDER_SELECTED_FOR_PACKAGING_PACKAGING_IN_PROGRESS, EventTypeDescription = "Order Selected For Packaging, Packaging In Progress" },

                //new EventType { Id = Constants.ORDER_PACKAGING_COMPLETED_WAITING_FOR_SHIPPING_SELECTION, EventTypeDescription = "Order Packaging Completed, Waiting To Be Selected For Shipping" },
                //new EventType { Id = Constants.ORDER_SELECTED_FOR_SHIPPING_SHIPPING_PREP_IN_PROGRESS, EventTypeDescription = "Order Selected For Shipping, Shipping Preparation In Progress" },

                //new EventType { Id = Constants.ORDER_SHIPPED, EventTypeDescription = "Order Shipped" }
                );

            // Configure relationships
        }
    }
}