﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PizzaMauiApp.API.Orders.DbOrders;

#nullable disable

namespace PizzaMauiApp.API.Orders.DbOrders.Migrations
{
    [DbContext(typeof(OrdersDbContext))]
    [Migration("20240223115858_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PizzaMauiApp.API.Orders.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("PizzaMauiApp.API.Orders.Models.OrderItems", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PizzaId")
                        .HasColumnType("uuid");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("PizzaMauiApp.API.Orders.Models.OrderStatusUpdate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrdersStatusHistory");
                });

            modelBuilder.Entity("PizzaMauiApp.API.Orders.Models.OrderItems", b =>
                {
                    b.HasOne("PizzaMauiApp.API.Orders.Models.Order", null)
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("PizzaMauiApp.API.Orders.Models.OrderStatusUpdate", b =>
                {
                    b.HasOne("PizzaMauiApp.API.Orders.Models.Order", null)
                        .WithMany("OrdersStatusHistory")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("PizzaMauiApp.API.Orders.Models.Order", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("OrdersStatusHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
