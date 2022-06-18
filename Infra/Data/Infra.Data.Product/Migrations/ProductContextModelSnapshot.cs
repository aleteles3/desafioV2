﻿// <auto-generated />
using System;
using Infra.Data.Product.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Data.Product.Migrations
{
    [DbContext(typeof(ProductContext))]
    partial class ProductContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Product.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Cat_CategoryId");

                    b.Property<DateTimeOffset>("DateAlter")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateInc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("Cat_Name");

                    b.HasKey("Id")
                        .HasName("PK_Cat_Category");

                    b.ToTable("Cat_Category", "Product");
                });

            modelBuilder.Entity("Domain.Product.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Ord_OrderId");

                    b.Property<DateTimeOffset>("DateAlter")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateInc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("integer")
                        .HasColumnName("Ord_OrderStatus");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("Use_UserId");

                    b.HasKey("Id")
                        .HasName("PK_Ord_Order");

                    b.HasIndex("UserId");

                    b.ToTable("Ord_Order", "Product");
                });

            modelBuilder.Entity("Domain.Product.Entities.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Ori_OrderItemId");

                    b.Property<DateTimeOffset>("DateAlter")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateInc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Discount")
                        .HasColumnType("numeric")
                        .HasColumnName("Ori_Discount");

                    b.Property<decimal>("ListPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("Ori_ListPrice");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("Ord_OrderId");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("Pro_ProductId");

                    b.HasKey("Id")
                        .HasName("PK_Ori_OrderItem");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("Ori_OrderItem", "Product");
                });

            modelBuilder.Entity("Domain.Product.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Pro_ProductId");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("Cat_CategoryId");

                    b.Property<DateTimeOffset>("DateAlter")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateInc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("Pro_Description");

                    b.Property<decimal>("ListPrice")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("Pro_ListPrice");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("Pro_Name");

                    b.Property<int>("Stock")
                        .HasColumnType("integer")
                        .HasColumnName("Pro_Stock");

                    b.HasKey("Id")
                        .HasName("PK_Pro_Product");

                    b.HasIndex("CategoryId");

                    b.ToTable("Pro_Product", "Product");
                });

            modelBuilder.Entity("Domain.Product.Shared.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("CascadeMode")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("DateAlter")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateInc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Domain.Product.Entities.Order", b =>
                {
                    b.HasOne("Domain.Product.Shared.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Ord_Order_Use_User_Use_UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Product.Entities.OrderItem", b =>
                {
                    b.HasOne("Domain.Product.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Ori_OderItem_Ord_Order_Ord_OrderId");

                    b.HasOne("Domain.Product.Entities.Product", "Product")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Ori_OrderItem_Pro_Product_Pro_ProductId");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Domain.Product.Entities.Product", b =>
                {
                    b.HasOne("Domain.Product.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Pro_Product_Cat_Category_Cat_CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Domain.Product.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Domain.Product.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Domain.Product.Entities.Product", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Domain.Product.Shared.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
