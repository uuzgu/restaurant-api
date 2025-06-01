using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using RestaurantApi.Data;

namespace RestaurantApi.Migrations
{
    [DbContext(typeof(RestaurantContext))]
    [Migration("20250519000000_FixOrderCustomerInfoForeignKey")]
    partial class FixOrderCustomerInfoForeignKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("RestaurantApi.Models.Order", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<int?>("CustomerInfoId")
                    .HasColumnType("INTEGER");

                b.Property<string>("OrderMethod")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("OrderNumber")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("PaymentMethod")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("SpecialNotes")
                    .HasColumnType("TEXT");

                b.Property<string>("Status")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("StripeSessionId")
                    .HasColumnType("TEXT");

                b.Property<string>("Total")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<int?>("UserId")
                    .HasColumnType("INTEGER");

                b.HasKey("Id");

                b.HasIndex("CustomerInfoId");

                b.HasIndex("UserId");

                b.ToTable("orders");
            });

            modelBuilder.Entity("RestaurantApi.Models.Order", b =>
            {
                b.HasOne("RestaurantApi.Models.CustomerOrderInfo", "CustomerInfo")
                    .WithMany()
                    .HasForeignKey("CustomerInfoId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("RestaurantApi.Models.User", "User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
} 