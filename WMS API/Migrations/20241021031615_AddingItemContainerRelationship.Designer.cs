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
    [Migration("20241021031615_AddingItemContainerRelationship")]
    partial class AddingItemContainerRelationship
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("WMS_API.Models.Container", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ItemId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_Containers");

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.ToTable("Containers", (string)null);
                });

            modelBuilder.Entity("WMS_API.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_Items");

                    b.ToTable("Items", (string)null);
                });

            modelBuilder.Entity("WMS_API.Models.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("StatusType")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK_Statuses");

                    b.ToTable("Statuses", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            StatusType = "Registered"
                        },
                        new
                        {
                            Id = 2,
                            StatusType = "Putaway"
                        });
                });

            modelBuilder.Entity("WMS_API.Models.Container", b =>
                {
                    b.HasOne("WMS_API.Models.Item", "Item")
                        .WithOne("Container")
                        .HasForeignKey("WMS_API.Models.Container", "ItemId");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("WMS_API.Models.Item", b =>
                {
                    b.Navigation("Container");
                });
#pragma warning restore 612, 618
        }
    }
}