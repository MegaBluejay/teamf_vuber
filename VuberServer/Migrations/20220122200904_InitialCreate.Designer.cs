﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VuberServer.Data;

namespace VuberServer.Migrations
{
    [DbContext(typeof(VuberDbContext))]
    [Migration("20220122200904_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresExtension("postgis")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("VuberCore.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("PaymentCardId")
                        .HasColumnType("integer");

                    b.Property<int?>("RatingId")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PaymentCardId");

                    b.HasIndex("RatingId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("VuberCore.Entities.Driver", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Point>("LastKnownLocation")
                        .HasColumnType("geometry");

                    b.Property<DateTime>("LocationUpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("MaxRideLevel")
                        .HasColumnType("integer");

                    b.Property<int>("MinRideLevel")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("RatingId")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RatingId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("VuberCore.Entities.Mark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.HasKey("Id");

                    b.ToTable("Mark");
                });

            modelBuilder.Entity("VuberCore.Entities.PaymentCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CardData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PaymentCard");
                });

            modelBuilder.Entity("VuberCore.Entities.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("RidesNumber")
                        .HasColumnType("bigint");

                    b.Property<int?>("ValueId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ValueId");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("VuberCore.Entities.Ride", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("DriverId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Finished")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Found")
                        .HasColumnType("timestamp without time zone");

                    b.Property<LineString>("Path")
                        .IsRequired()
                        .HasColumnType("geometry");

                    b.Property<int>("PaymentType")
                        .HasColumnType("integer");

                    b.Property<int>("RideType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Started")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("DriverId");

                    b.ToTable("Rides");
                });

            modelBuilder.Entity("VuberCore.Entities.Client", b =>
                {
                    b.HasOne("VuberCore.Entities.PaymentCard", "PaymentCard")
                        .WithMany()
                        .HasForeignKey("PaymentCardId");

                    b.HasOne("VuberCore.Entities.Rating", "Rating")
                        .WithMany()
                        .HasForeignKey("RatingId");

                    b.Navigation("PaymentCard");

                    b.Navigation("Rating");
                });

            modelBuilder.Entity("VuberCore.Entities.Driver", b =>
                {
                    b.HasOne("VuberCore.Entities.Rating", "Rating")
                        .WithMany()
                        .HasForeignKey("RatingId");

                    b.Navigation("Rating");
                });

            modelBuilder.Entity("VuberCore.Entities.Rating", b =>
                {
                    b.HasOne("VuberCore.Entities.Mark", "Value")
                        .WithMany()
                        .HasForeignKey("ValueId");

                    b.Navigation("Value");
                });

            modelBuilder.Entity("VuberCore.Entities.Ride", b =>
                {
                    b.HasOne("VuberCore.Entities.Client", "Client")
                        .WithMany("Rides")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VuberCore.Entities.Driver", "Driver")
                        .WithMany("Rides")
                        .HasForeignKey("DriverId");

                    b.Navigation("Client");

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("VuberCore.Entities.Client", b =>
                {
                    b.Navigation("Rides");
                });

            modelBuilder.Entity("VuberCore.Entities.Driver", b =>
                {
                    b.Navigation("Rides");
                });
#pragma warning restore 612, 618
        }
    }
}
