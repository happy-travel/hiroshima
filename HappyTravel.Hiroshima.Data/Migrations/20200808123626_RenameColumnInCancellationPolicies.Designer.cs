﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models.Booking;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    [DbContext(typeof(DirectContractsDbContext))]
    [Migration("20200808123626_RenameColumnInCancellationPolicies")]
    partial class RenameColumnInCancellationPolicies
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:postgis", ",,")
                .HasAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.Accommodations.Accommodation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<JsonDocument>("AccommodationAmenities")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<JsonDocument>("AdditionalInfo")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("jsonb")
                        .HasDefaultValueSql("'{}'::json");

                    b.Property<JsonDocument>("Address")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("CheckInTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CheckOutTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<ContactInfo>("ContactInfo")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("ContractManagerId")
                        .HasColumnType("integer");

                    b.Property<Point>("Coordinates")
                        .IsRequired()
                        .HasColumnType("geometry (point)");

                    b.Property<int>("LocationId")
                        .HasColumnType("integer");

                    b.Property<JsonDocument>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<OccupancyDefinition>("OccupancyDefinition")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<JsonDocument>("Pictures")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("PropertyType")
                        .HasColumnType("integer");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<JsonDocument>("TextualDescription")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("ContractManagerId");

                    b.HasIndex("Coordinates")
                        .HasAnnotation("Npgsql:IndexMethod", "GIST");

                    b.HasIndex("LocationId");

                    b.ToTable("Accommodations");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.Booking.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("CheckInDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("CheckOutDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LanguageCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ReferenceCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Residency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<RoomGuests>>("Rooms")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("StatusCode")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("BookingOrders");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ContractManagerId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ContractManagerId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.ContractAccommodationRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AccommodationId")
                        .HasColumnType("integer");

                    b.Property<int>("ContractId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AccommodationId");

                    b.HasIndex("ContractId");

                    b.ToTable("ContractAccommodationRelations");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.ContractManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdentityHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ContractManagers");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.Location.Country", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<JsonDocument>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Code");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.Location.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<JsonDocument>("Locality")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<JsonDocument>("Zone")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("jsonb")
                        .HasDefaultValueSql("'{}'::json");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.Rooms.CancellationPolicies.RoomCancellationPolicy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<List<Policy>>("Policies")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.Property<int>("SeasonId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("SeasonId");

                    b.ToTable("CancellationPolicies");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.Rooms.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AccommodationId")
                        .HasColumnType("integer");

                    b.Property<JsonDocument>("Amenities")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<JsonDocument>("Description")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<JsonDocument>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<List<OccupancyConfiguration>>("OccupancyConfigurations")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<JsonDocument>("Pictures")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("AccommodationId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.Rooms.RoomAllocationRequirement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("Allotment")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("MinimumStayNights")
                        .HasColumnType("integer");

                    b.Property<ReleasePeriod>("ReleasePeriod")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomAllocationRequirements");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.Rooms.RoomAvailabilityRestrictions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Restrictions")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomAvailabilityRestrictions");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.Rooms.RoomPromotionalOffer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("BookByDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("BookingCode")
                        .HasColumnType("text");

                    b.Property<int>("ContractId")
                        .HasColumnType("integer");

                    b.Property<JsonDocument>("Details")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<double>("DiscountPercent")
                        .HasColumnType("double precision");

                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ValidFromDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ValidToDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomPromotionalOffers");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.Rooms.RoomRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("BoardBasis")
                        .HasColumnType("integer");

                    b.Property<int>("Currency")
                        .HasColumnType("integer");

                    b.Property<JsonDocument>("Details")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("MealPlan")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.Property<int>("SeasonId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("SeasonId");

                    b.ToTable("RoomRates");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ContractId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.ToTable("Seasons");
                });
#pragma warning restore 612, 618
        }
    }
}
