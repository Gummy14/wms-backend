﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WMS_API.DbContexts;

#nullable disable

namespace WMS_API.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20241029015002_AddingSeparateOrderItemsTable")]
    partial class AddingSeparateOrderItemsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("WMS_API.Models.Containers.Container", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DateTimeRegistered")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("ItemId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK_Containers");

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.ToTable("Containers", (string)null);
                });

            modelBuilder.Entity("WMS_API.Models.Events.EventType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EventTypeDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK_EventTypes");

                    b.ToTable("EventTypes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EventTypeDescription = "Putaway"
                        },
                        new
                        {
                            Id = 2,
                            EventTypeDescription = "Pick"
                        });
                });

            modelBuilder.Entity("WMS_API.Models.Events.ItemContainerEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ContainerId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DateTimeStamp")
                        .HasColumnType("datetime");

                    b.Property<int>("EventType")
                        .HasColumnType("int");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id")
                        .HasName("PK_ItemContainerEventHistory");

                    b.HasIndex("ContainerId")
                        .IsUnique();

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.ToTable("ItemContainerEventHistory", (string)null);
                });

            modelBuilder.Entity("WMS_API.Models.Items.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DateTimeRegistered")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK_Items");

                    b.ToTable("Items", (string)null);
                });

            modelBuilder.Entity("WMS_API.Models.Orders.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DateTimeOrderRecieved")
                        .HasColumnType("datetime");

                    b.HasKey("Id")
                        .HasName("PK_Orders");

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("WMS_API.Models.Orders.OrderItem", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("char(36)");

                    b.HasKey("OrderId", "ItemId")
                        .HasName("PK_OrderItems");

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.ToTable("OrderItems", (string)null);
                });

            modelBuilder.Entity("WMS_API.Models.Containers.Container", b =>
                {
                    b.HasOne("WMS_API.Models.Items.Item", "Item")
                        .WithOne()
                        .HasForeignKey("WMS_API.Models.Containers.Container", "ItemId");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("WMS_API.Models.Events.ItemContainerEvent", b =>
                {
                    b.HasOne("WMS_API.Models.Containers.Container", "Container")
                        .WithOne()
                        .HasForeignKey("WMS_API.Models.Events.ItemContainerEvent", "ContainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WMS_API.Models.Items.Item", "Item")
                        .WithOne()
                        .HasForeignKey("WMS_API.Models.Events.ItemContainerEvent", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Container");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("WMS_API.Models.Orders.OrderItem", b =>
                {
                    b.HasOne("WMS_API.Models.Items.Item", "Item")
                        .WithOne()
                        .HasForeignKey("WMS_API.Models.Orders.OrderItem", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WMS_API.Models.Orders.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("WMS_API.Models.Orders.Order", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}