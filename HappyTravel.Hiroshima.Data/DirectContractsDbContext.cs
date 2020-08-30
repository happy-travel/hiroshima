﻿using System;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using HappyTravel.Hiroshima.Data.Models.Booking;
using HappyTravel.Hiroshima.Data.Models.Location;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using HappyTravel.Hiroshima.Data.Models.Seasons;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Room = HappyTravel.Hiroshima.Data.Models.Rooms.Room;

namespace HappyTravel.Hiroshima.Data
{
    public class DirectContractsDbContext : DbContext
    {
        public DirectContractsDbContext(DbContextOptions<DirectContractsDbContext> options) : base(options)
        {}


        [DbFunction("st_distance_sphere")]
        public static double GetDistance(Point from, Point to)
            => throw new Exception();

        
        [DbFunction("lang_from_jsonb")]
        public static JsonDocument GetLangFromJsonb(JsonDocument jsonb, string languageCode)
            => throw new Exception();
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis")
                .HasPostgresExtension("uuid-ossp");
            modelBuilder.UseIdentityColumns();

            AddContractManagers(modelBuilder);
            AddContracts(modelBuilder);
            AddLocations(modelBuilder);
            AddAccommodations(modelBuilder);
            AddRooms(modelBuilder);
            AddRates(modelBuilder);
            AddRoomAvailabilityRestrictions(modelBuilder);
            AddPromotionalOffers(modelBuilder);
            AddRoomAllocationRequirements(modelBuilder);
            AddBooking(modelBuilder);
            AddCountries(modelBuilder);
            AddCancellationPolicies(modelBuilder);
            AddContractAccommodationRelation(modelBuilder);
            AddSeasons(modelBuilder);
            AddSeasonRanges(modelBuilder);
        }


        private void AddContractManagers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractManager>(e =>
            {
                e.ToTable("ContractManagers");
                e.HasKey(c => c.Id);
                e.Property(c => c.IdentityHash);
                e.Property(c => c.Email).IsRequired();
                e.Property(c => c.Name).IsRequired();
                e.Property(c => c.Title);
            });
        }
        
        
        private void AddContracts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contract>(e =>
            {
                e.ToTable("Contracts");
                e.HasKey(c => c.Id);
                e.Property(c => c.ValidFrom).IsRequired();
                e.Property(c => c.ValidTo).IsRequired();
                e.Property(c => c.Name).IsRequired();
                e.Property(c => c.Description);
                e.Property(c => c.ContractManagerId).IsRequired();
                e.HasIndex(c => c.ContractManagerId);
            });
        }
        
        
        private void AddLocations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Location.Location>(e =>
            {
                e.ToTable("Locations");
                e.HasKey(l => l.Id);
                e.Property(l => l.Locality ).HasColumnType("jsonb").IsRequired();
                e.Property(l => l.Zone).HasColumnType("jsonb").HasDefaultValueSql("'{}'::json");
                e.Property(l => l.CountryCode).IsRequired();
                e.HasIndex(l => l.CountryCode);
            });
        }


        private void AddCountries(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(e =>
            {
                e.ToTable("Countries");
                e.HasKey(c => c.Code);
                e.Property(c => c.Name).HasColumnType("jsonb");
            });
        }
        

        private void AddAccommodations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accommodation>(e =>
            {
                e.ToTable("Accommodations");
                e.HasKey(a => a.Id);
                e.Property(a => a.Address).HasColumnType("jsonb").IsRequired();
                e.Property(a=> a.ContactInfo).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.Coordinates).HasColumnType("geometry (point)").IsRequired();
                e.Property(a=> a.Name).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.Pictures).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.Rating).IsRequired();
                e.Property(a=> a.AccommodationAmenities).HasColumnType("jsonb").IsRequired();
                e.Property(a=> a.AdditionalInfo).HasColumnType("jsonb").HasDefaultValueSql("'{}'::json");
                e.Property(a => a.PropertyType).IsRequired();
                e.Property(a => a.TextualDescription).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.CheckInTime).IsRequired();
                e.Property(a => a.CheckOutTime).IsRequired();
                e.Property(a => a.OccupancyDefinition).HasColumnType("jsonb");
                e.Property(a => a.ContractManagerId).IsRequired();
                e.HasIndex(a=> a.Coordinates).HasMethod("GIST");
                e.HasIndex(a => a.LocationId);
                e.HasIndex(a => a.ContractManagerId);
            });
        }

        
        private void AddRooms(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(e =>
            {
                e.ToTable("Rooms");
                e.HasKey(r=> r.Id);
                e.Property(r=> r.Amenities).HasColumnType("jsonb").IsRequired();
                e.Property(r => r.Description).HasColumnType("jsonb").IsRequired();;
                e.Property(r => r.Name).HasColumnType("jsonb").IsRequired();
                e.Property(r => r.OccupancyConfigurations).HasColumnType("jsonb").IsRequired();
                e.Property(r => r.AccommodationId).IsRequired();
                e.HasIndex(r => r.AccommodationId);
            });
        }

        
        private void AddRates(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomRate>(e =>
            {
                e.ToTable("RoomRates");
                e.HasKey(rr=> rr.Id);
                e.Property(rr => rr.Price).IsRequired();
                e.Property(rr => rr.Currency).IsRequired();
                e.Property(rr => rr.MealPlan).IsRequired();;
                e.Property(rr => rr.BoardBasis).IsRequired();;
                e.Property(rr=> rr.SeasonId).IsRequired();
                e.Property(rr => rr.RoomId).IsRequired();
                e.HasIndex(rr => rr.SeasonId);
                e.HasIndex(rr => rr.RoomId);
            });
        }

        
        private void AddRoomAvailabilityRestrictions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomAvailabilityRestriction>(e =>
            {
                e.ToTable("RoomAvailabilityRestrictions");
                e.HasKey(rr => rr.Id);
                e.Property(rr => rr.Restriction).IsRequired().HasDefaultValue(AvailabilityRestrictions.FreeSale);
                e.Property(rr => rr.FromDate).IsRequired();
                e.Property(rr => rr.ToDate).IsRequired();
                e.Property(rr => rr.RoomId).IsRequired();
                e.Property(rr => rr.ContractId).IsRequired();
                e.HasIndex(rr => rr.RoomId);
                e.HasIndex(rr => rr.ContractId);
                e.HasIndex(rr => rr.Restriction);
            });
        }


        private void AddPromotionalOffers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomPromotionalOffer>(e =>
            {
                e.ToTable("RoomPromotionalOffers");
                e.HasKey(po => po.Id);
                e.Property(po => po.DiscountPercent).IsRequired();
                e.Property(po => po.Details).HasColumnType("jsonb");
                e.Property(po => po.BookByDate).IsRequired();
                e.Property(po => po.ValidFromDate).IsRequired();
                e.Property(po => po.ValidToDate).IsRequired();
                e.Property(po => po.BookingCode);
                e.Property(rr => rr.RoomId).IsRequired();
                e.HasIndex(rr => rr.RoomId);
            });
        }


        private void AddRoomAllocationRequirements(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomAllocationRequirement>(e =>
            {
                e.ToTable("RoomAllocationRequirements");
                e.HasKey(rar => rar.Id);
                e.Property(rar => rar.SeasonRangeId).IsRequired();
                e.Property(rar => rar.ReleaseDays).IsRequired();
                e.Property(rar => rar.MinimumLengthOfStay);
                e.Property(rar => rar.Allotment);
                e.Property(rar => rar.RoomId).IsRequired();
                e.HasIndex(rar => rar.RoomId);
            });
        }
        
        
        private void AddBooking(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(e =>
            {
                e.ToTable("BookingOrders");
                e.HasKey(bo => bo.Id);
                e.Property(bo => bo.ReferenceCode).IsRequired();
                e.Property(bo => bo.StatusCode).IsRequired();
                e.Property(bo => bo.BookingDate).IsRequired();
                e.Property(bo => bo.CheckInDate).IsRequired();
                e.Property(bo => bo.CheckOutDate).IsRequired();
                e.Property(bo => bo.Rooms).HasColumnType("jsonb").IsRequired();
                e.Property(bo => bo.Nationality).IsRequired();
                e.Property(bo => bo.Residency).IsRequired();
                e.Property(bo => bo.LanguageCode).IsRequired();
            });
        }

        
        private void AddCancellationPolicies(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomCancellationPolicy>(e =>
            {
                e.ToTable("CancellationPolicies");
                e.HasKey(rcp => rcp.Id);
                e.Property(rcp => rcp.Policies).HasColumnType("jsonb").IsRequired();
                e.Property(rcp => rcp.RoomId).IsRequired();
                e.Property(rcp => rcp.SeasonId).IsRequired();;
                e.HasIndex(rcp => rcp.RoomId);
                e.HasIndex(rcp => rcp.SeasonId);
            });
        }


        private void AddContractAccommodationRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractAccommodationRelation>(e =>
            {
                e.ToTable("ContractAccommodationRelations");
                e.HasKey(car => car.Id);
                e.Property(car => car.AccommodationId).IsRequired();
                e.Property(car => car.ContractId).IsRequired();
                e.HasIndex(car => car.AccommodationId);
                e.HasIndex(car => car.ContractId);
            });
        }
        
        
        private void AddSeasons(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Season>(e =>
            {
                e.ToTable("Seasons");
                e.HasKey(s  => s.Id);
                e.Property(s => s.Name).IsRequired();
                e.Property(s => s.ContractId).IsRequired();
                e.HasIndex(s => s.ContractId);
            });
        }


        private void AddSeasonRanges(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeasonRange>(e =>
            {
                e.ToTable("SeasonRanges");
                e.HasKey(s => s.Id);
                e.Property(s => s.StartDate).IsRequired();
                e.Property(s => s.EndDate).IsRequired();
                e.Property(s => s.SeasonId).IsRequired();
                e.HasIndex(s => s.SeasonId);
            });
        }


        public virtual DbSet<Models.Location.Location> Locations { get; set; }
        public virtual DbSet<Accommodation> Accommodations { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomAvailabilityRestriction> RoomAvailabilityRestrictions { get; set; }
        public virtual DbSet<RoomRate> RoomRates { get; set; }
        public virtual DbSet<RoomAllocationRequirement> RoomAllocationRequirements { get; set; }
        public virtual DbSet<RoomPromotionalOffer> RoomPromotionalOffers { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<RoomCancellationPolicy> RoomCancellationPolicies { get; set; }
        public virtual DbSet<ContractManager> ContractManagers { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<ContractAccommodationRelation> ContractAccommodationRelations { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<SeasonRange> SeasonRanges { get; set; }
    }
}