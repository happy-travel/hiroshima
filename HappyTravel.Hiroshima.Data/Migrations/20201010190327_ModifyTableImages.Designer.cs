﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    [DbContext(typeof(DirectContractsDbContext))]
    [Migration("20201010190327_ModifyTableImages")]
    partial class ModifyTableImages
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:postgis", ",,")
                .HasAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation", b =>
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

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<JsonDocument>("LeisureAndSports")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("LocationId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp without time zone");

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

                    b.Property<RateOptions>("RateOptions")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
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

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies.RoomCancellationPolicy", b =>
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

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", b =>
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

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<JsonDocument>("Description")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp without time zone");

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

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.RoomAllocationRequirement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("Allotment")
                        .HasColumnType("integer");

                    b.Property<int?>("MinimumLengthOfStay")
                        .HasColumnType("integer");

                    b.Property<int>("ReleaseDays")
                        .HasColumnType("integer");

                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.Property<int>("SeasonRangeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("SeasonRangeId");

                    b.ToTable("RoomAllocationRequirements");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.RoomAvailabilityRestriction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ContractId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Restriction")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(4);

                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("Restriction");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomAvailabilityRestrictions");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.RoomPromotionalOffer", b =>
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

                    b.HasIndex("ContractId");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomPromotionalOffers");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.RoomRate", b =>
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

                    b.Property<int>("RoomType")
                        .HasColumnType("integer");

                    b.Property<int>("SeasonId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("SeasonId");

                    b.ToTable("RoomRates");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ContractManagerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Verified")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("ContractManagerId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.ContractManager", b =>
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

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Document", b =>
                {
                    b.Property<Guid>("UniqueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("ContractId")
                        .HasColumnType("integer");

                    b.Property<int>("ContractManagerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UniqueId");

                    b.HasIndex("ContractId");

                    b.HasIndex("ContractManagerId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccommodationId")
                        .HasColumnType("integer");

                    b.Property<int>("ContractManagerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccommodationId");

                    b.HasIndex("ContractManagerId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Locations.Country", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<JsonDocument>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Code");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Locations.Location", b =>
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

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Seasons.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ContractId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Seasons.SeasonRange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("SeasonId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.ToTable("SeasonRanges");
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

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.ContractManager", "ContractManager")
                        .WithMany("Accommodations")
                        .HasForeignKey("ContractManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Locations.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies.RoomCancellationPolicy", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", "Room")
                        .WithMany("RoomCancellationPolicies")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Seasons.Season", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation", "Accommodation")
                        .WithMany("Rooms")
                        .HasForeignKey("AccommodationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.RoomAllocationRequirement", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", "Room")
                        .WithMany("RoomAllocationRequirements")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Seasons.SeasonRange", "SeasonRange")
                        .WithMany()
                        .HasForeignKey("SeasonRangeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.RoomAvailabilityRestriction", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Contract", "Contract")
                        .WithMany("RoomAvailabilityRestriction")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", "Room")
                        .WithMany("RoomAvailabilityRestrictions")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.RoomPromotionalOffer", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Contract", "Contract")
                        .WithMany("PromotionalOffers")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", "Room")
                        .WithMany("RoomPromotionalOffers")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.RoomRate", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", "Room")
                        .WithMany("RoomRates")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Seasons.Season", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Contract", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.ContractManager", "ContractManager")
                        .WithMany("Contracts")
                        .HasForeignKey("ContractManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Seasons.Season", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Contract", "Contract")
                        .WithMany("Seasons")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Seasons.SeasonRange", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Seasons.Season", "Season")
                        .WithMany("SeasonRanges")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
