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
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<ItemContainerEvent> ItemContainerEvents { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map entities to tables
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<EventType>().ToTable("EventTypes");
            modelBuilder.Entity<Container>().ToTable("Containers");
            modelBuilder.Entity<ItemContainerEvent>().ToTable("ItemContainerEvents");

            // Configure Primary Keys
            modelBuilder.Entity<Item>().HasKey(x => x.Id).HasName("PK_Items");
            modelBuilder.Entity<EventType>().HasKey(x => x.Id).HasName("PK_Statuses");
            modelBuilder.Entity<Container>().HasKey(x => x.Id).HasName("PK_Containers");
            modelBuilder.Entity<ItemContainerEvent>().HasKey(x => x.Id).HasName("PK_ItemContainerEvents");

            // Configure indexes

            // Configure columns
            modelBuilder.Entity<Item>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Item>().Property(x => x.Description).HasColumnType("nvarchar(100)").IsRequired();

            modelBuilder.Entity<Container>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Container>().Property(x => x.ItemId).HasColumnType("char(36)");

            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.Id).HasColumnType("char(36)").IsRequired();
            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.ItemId).HasColumnType("char(36)");
            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.ContainerId).HasColumnType("char(36)");
            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.EventType).HasColumnType("int").IsRequired();
            modelBuilder.Entity<ItemContainerEvent>().Property(x => x.DateTimeStamp).HasColumnType("datetime").IsRequired();

            modelBuilder.Entity<EventType>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<EventType>().Property(x => x.EventTypeDescription).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<EventType>().HasData(
                new EventType { Id = 1, EventTypeDescription = "Registration" },
                new EventType { Id = 2, EventTypeDescription = "Putaway" },
                new EventType { Id = 3, EventTypeDescription = "Pick" }
                );

            // Configure relationships
            modelBuilder.Entity<Item>().HasOne(x => x.Container).WithOne(x => x.Item).HasForeignKey<Container>(x => x.ItemId);
        }
    }
}