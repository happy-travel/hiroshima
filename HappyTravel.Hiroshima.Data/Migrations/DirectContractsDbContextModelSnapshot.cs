﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;
using HappyTravel.Hiroshima.Common.Models.Images;
using HappyTravel.Hiroshima.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    [DbContext(typeof(DirectContractsDbContext))]
    partial class DirectContractsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresExtension("postgis")
                .HasPostgresExtension("uuid-ossp")
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<JsonDocument>("AccommodationAmenities")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<JsonDocument>("AdditionalInfo")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("jsonb")
                        .HasDefaultValueSql("'{}'::jsonb");

                    b.Property<JsonDocument>("Address")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int?>("BuildYear")
                        .HasColumnType("integer");

                    b.Property<string>("CheckInTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CheckOutTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<ContactInfo>("ContactInfo")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<Point>("Coordinates")
                        .IsRequired()
                        .HasColumnType("geometry (point)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("Floors")
                        .HasColumnType("integer");

                    b.Property<List<SlimImage>>("Images")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("jsonb")
                        .HasDefaultValueSql("'[]'::jsonb");

                    b.Property<JsonDocument>("LeisureAndSports")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("LocationId")
                        .HasColumnType("integer");

                    b.Property<int>("ManagerId")
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

                    b.HasIndex("Coordinates")
                        .HasMethod("GIST");

                    b.HasIndex("LocationId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Accommodations");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Amenity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("LanguageCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LanguageCode");

                    b.ToTable("Amenities");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies.RoomCancellationPolicy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

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

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.PromotionalOfferStopSale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("ContractId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("RoomId");

                    b.ToTable("PromotionalOffersStopSale");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

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

                    b.Property<List<SlimImage>>("Images")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("jsonb")
                        .HasDefaultValueSql("'[]'::jsonb");

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
                        .UseIdentityByDefaultColumn();

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
                        .UseIdentityByDefaultColumn();

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
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("BookByDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("BookingCode")
                        .HasColumnType("text");

                    b.Property<int>("ContractId")
                        .HasColumnType("integer");

                    b.Property<JsonDocument>("Description")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<decimal>("DiscountPercent")
                        .HasColumnType("numeric");

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
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("BoardBasis")
                        .HasColumnType("integer");

                    b.Property<int>("Currency")
                        .HasColumnType("integer");

                    b.Property<JsonDocument>("Description")
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

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Bookings.BookingOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<JsonDocument>("AvailabilityRequest")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<JsonDocument>("AvailableRates")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<JsonDocument>("BookingRequest")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<DateTime>("CheckInDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("CheckOutDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("LanguageCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ManagerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Modified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("ReferenceCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("BookingOrders");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ManagerId")
                        .HasColumnType("integer");

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

                    b.HasIndex("ManagerId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ContractId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ManagerId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Images.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<JsonDocument>("Description")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("ImageType")
                        .HasColumnType("integer");

                    b.Property<ImageKeys>("Keys")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("jsonb")
                        .HasDefaultValueSql("'{}'::jsonb");

                    b.Property<int>("ManagerId")
                        .HasColumnType("integer");

                    b.Property<OriginalImageDetails>("OriginalImageDetails")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<int>("ReferenceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ImageType");

                    b.HasIndex("ManagerId");

                    b.HasIndex("ReferenceId");

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
                        .UseIdentityByDefaultColumn();

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

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Manager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdentityHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Updated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("IdentityHash")
                        .IsUnique();

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Seasons.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

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
                        .UseIdentityByDefaultColumn();

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

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.ContractAccommodationRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

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
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Locations.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Manager", "Manager")
                        .WithMany("Accommodations")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies.RoomCancellationPolicy", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", "Room")
                        .WithMany("RoomCancellationPolicies")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Seasons.Season", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("Season");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.PromotionalOfferStopSale", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Contract", "Contract")
                        .WithMany("PromotionalOffersStopSale")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", "Room")
                        .WithMany("PromotionalOffersStopSale")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Contract");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation", "Accommodation")
                        .WithMany("Rooms")
                        .HasForeignKey("AccommodationId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Accommodation");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.RoomAllocationRequirement", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", "Room")
                        .WithMany("RoomAllocationRequirements")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Seasons.SeasonRange", "SeasonRange")
                        .WithMany()
                        .HasForeignKey("SeasonRangeId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("SeasonRange");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.RoomAvailabilityRestriction", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Contract", "Contract")
                        .WithMany("RoomAvailabilityRestrictions")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", "Room")
                        .WithMany("RoomAvailabilityRestrictions")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Contract");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.RoomPromotionalOffer", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Contract", "Contract")
                        .WithMany("PromotionalOffers")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", "Room")
                        .WithMany("RoomPromotionalOffers")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Contract");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.RoomRate", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", "Room")
                        .WithMany("RoomRates")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Seasons.Season", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("Season");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Bookings.BookingOrder", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Manager", "Manager")
                        .WithMany("BookingOrders")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Contract", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Manager", "Manager")
                        .WithMany("Contracts")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Document", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Contract", "Contract")
                        .WithMany("Documents")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Manager", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Contract");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Images.Image", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Manager", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Locations.Location", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Locations.Country", "Country")
                        .WithMany("Locations")
                        .HasForeignKey("CountryCode")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Seasons.Season", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Contract", "Contract")
                        .WithMany("Seasons")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Seasons.SeasonRange", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Seasons.Season", "Season")
                        .WithMany("SeasonRanges")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Season");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Data.Models.ContractAccommodationRelation", b =>
                {
                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation", "Accommodation")
                        .WithMany()
                        .HasForeignKey("AccommodationId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("HappyTravel.Hiroshima.Common.Models.Contract", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Accommodation");

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room", b =>
                {
                    b.Navigation("PromotionalOffersStopSale");

                    b.Navigation("RoomAllocationRequirements");

                    b.Navigation("RoomAvailabilityRestrictions");

                    b.Navigation("RoomCancellationPolicies");

                    b.Navigation("RoomPromotionalOffers");

                    b.Navigation("RoomRates");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Contract", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("PromotionalOffers");

                    b.Navigation("PromotionalOffersStopSale");

                    b.Navigation("RoomAvailabilityRestrictions");

                    b.Navigation("Seasons");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Locations.Country", b =>
                {
                    b.Navigation("Locations");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Manager", b =>
                {
                    b.Navigation("Accommodations");

                    b.Navigation("BookingOrders");

                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("HappyTravel.Hiroshima.Common.Models.Seasons.Season", b =>
                {
                    b.Navigation("SeasonRanges");
                });
#pragma warning restore 612, 618
        }
    }
}
