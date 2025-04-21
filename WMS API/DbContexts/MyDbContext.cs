using Microsoft.EntityFrameworkCore;
using WMS_API.Models.Orders;
using WMS_API.Models;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Containers;
using WMS_API.Models.Boxes;
using WMS_API.Models.Shipments;
using WMS_API.Models.Trucks;

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

        public DbSet<ShipmentData> ShipmentData { get; set; }
        public DbSet<Shipment> Shipments { get; set; }

        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Address> Addresses { get; set; }


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
            modelBuilder.Entity<ShipmentData>().ToTable("ShipmentData");
            modelBuilder.Entity<Shipment>().ToTable("Shipments");
            modelBuilder.Entity<Address>().ToTable("Addresses");
            modelBuilder.Entity<Truck>().ToTable("Trucks");

            // Configure Primary Keys
            modelBuilder.Entity<ItemData>().HasKey(x => x.EventId).HasName("PK_ItemData");
            modelBuilder.Entity<LocationData>().HasKey(x => x.EventId).HasName("PK_LocationData");
            modelBuilder.Entity<OrderData>().HasKey(x => x.EventId).HasName("PK_OrderData");
            modelBuilder.Entity<ContainerData>().HasKey(x => x.EventId).HasName("PK_ContainerData");
            modelBuilder.Entity<BoxData>().HasKey(x => x.EventId).HasName("PK_BoxData");
            modelBuilder.Entity<ShipmentData>().HasKey(x => x.EventId).HasName("PK_ShipmentData");
            modelBuilder.Entity<Item>().HasKey(x => x.Id).HasName("PK_Items");
            modelBuilder.Entity<Location>().HasKey(x => x.Id).HasName("PK_Locations");
            modelBuilder.Entity<Order>().HasKey(x => x.Id).HasName("PK_Orders");
            modelBuilder.Entity<Container>().HasKey(x => x.Id).HasName("PK_Containers");
            modelBuilder.Entity<Box>().HasKey(x => x.Id).HasName("PK_Boxes");
            modelBuilder.Entity<Shipment>().HasKey(x => x.Id).HasName("PK_Shipments");
            modelBuilder.Entity<Address>().HasKey(x => x.Id).HasName("PK_Addresses");
            modelBuilder.Entity<Truck>().HasKey(x => x.Id).HasName("PK_Addresses");

            // Configure indexes


            // Configure columns
            modelBuilder.Entity<Item>().Property(x => x.Id).IsRequired();
            modelBuilder.Entity<Location>().Property(x => x.Id).IsRequired();
            modelBuilder.Entity<Order>().Property(x => x.Id).IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.Id).IsRequired();
            modelBuilder.Entity<Box>().Property(x => x.Id).IsRequired();
            modelBuilder.Entity<Shipment>().Property(x => x.Id).IsRequired();

            modelBuilder.Entity<ItemData>().Property(x => x.EventId).IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.Description).IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.DateTimeStamp).IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.LengthInCentimeters).IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.WidthInCentimeters).IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.HeightInCentimeters).IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.WeightInKilograms).IsRequired();
            modelBuilder.Entity<ItemData>().Property(x => x.NextEventId);
            modelBuilder.Entity<ItemData>().Property(x => x.PrevEventId);

            modelBuilder.Entity<LocationData>().Property(x => x.EventId).IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.Description).IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.DateTimeStamp).IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.LengthInCentimeters).IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.WidthInCentimeters).IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.HeightInCentimeters).IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.MaxWeightInKilograms).IsRequired();
            modelBuilder.Entity<LocationData>().Property(x => x.NextEventId);
            modelBuilder.Entity<LocationData>().Property(x => x.PrevEventId);

            modelBuilder.Entity<ContainerData>().Property(x => x.EventId).IsRequired();
            modelBuilder.Entity<ContainerData>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<ContainerData>().Property(x => x.Description).IsRequired();
            modelBuilder.Entity<ContainerData>().Property(x => x.DateTimeStamp).IsRequired();
            modelBuilder.Entity<ContainerData>().Property(x => x.NextEventId);
            modelBuilder.Entity<ContainerData>().Property(x => x.PrevEventId);

            modelBuilder.Entity<OrderData>().Property(x => x.EventId).IsRequired();
            modelBuilder.Entity<OrderData>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<OrderData>().Property(x => x.Description).IsRequired();
            modelBuilder.Entity<OrderData>().Property(x => x.DateTimeStamp).IsRequired();
            modelBuilder.Entity<OrderData>().Property(x => x.NextEventId);
            modelBuilder.Entity<OrderData>().Property(x => x.PrevEventId);

            modelBuilder.Entity<Address>().Property(x => x.Id).IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.FirstName).IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.LastName).IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.Street).IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.City).IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.State).IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.Zip).IsRequired();

            modelBuilder.Entity<BoxData>().Property(x => x.EventId).IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.Description).IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.DateTimeStamp).IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.LengthInCentimeters).IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.WidthInCentimeters).IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.HeightInCentimeters).IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.IsSealed).IsRequired();
            modelBuilder.Entity<BoxData>().Property(x => x.NextEventId);
            modelBuilder.Entity<BoxData>().Property(x => x.PrevEventId);

            modelBuilder.Entity<ShipmentData>().Property(x => x.EventId);
            modelBuilder.Entity<ShipmentData>().Property(x => x.Name);
            modelBuilder.Entity<ShipmentData>().Property(x => x.Description);
            modelBuilder.Entity<ShipmentData>().Property(x => x.DateTimeStamp);
            modelBuilder.Entity<ShipmentData>().Property(x => x.NextEventId);
            modelBuilder.Entity<ShipmentData>().Property(x => x.PrevEventId);

            modelBuilder.Entity<Truck>().Property(x => x.Id).IsRequired();
            modelBuilder.Entity<Truck>().Property(x => x.LicensePlate).IsRequired();
            modelBuilder.Entity<Truck>().Property(x => x.ArrivalDateTimeStamp).IsRequired();
            modelBuilder.Entity<Truck>().Property(x => x.DepartureDateTimeStamp);

            // Configure relationships
        }
    }
}