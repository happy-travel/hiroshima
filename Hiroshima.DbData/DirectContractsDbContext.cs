using System;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Booking;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DbData.Models.Rooms.CancellationPolicies;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Npgsql;
using Location = Hiroshima.DbData.Models.Location;
using Room = Hiroshima.DbData.Models.Rooms.Room;

namespace Hiroshima.DbData
{
    public class DirectContractsDbContext : DbContext
    {
        public DirectContractsDbContext(DbContextOptions<DirectContractsDbContext> options) : base(options)
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Location.LocationTypes>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<SaleRestrictions>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<AccommodationRating>();
        }


        [DbFunction("st_distance_sphere")]
        public static double GetDistance(Point from, Point to)
            => throw new Exception();

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis")
                .HasPostgresExtension("uuid-ossp");
            modelBuilder.UseIdentityColumns();
            AddEnums(modelBuilder);
            
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
        }

        
        private void AddEnums(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<Location.LocationTypes>();
            modelBuilder.HasPostgresEnum<SaleRestrictions>();
            modelBuilder.HasPostgresEnum<AccommodationRating>();
        }
       

        private void AddLocations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location.Location>(e =>
            {
                e.ToTable("Locations");
                e.HasKey(l => l.Id);
                e.Property(l => l.Name).HasColumnType("jsonb").IsRequired();
                e.Property(l => l.Type).IsRequired();
                e.Property(l => l.CountryCode);
                e.Property(l => l.ParentId);
                e.HasOne<Location.Country>().WithMany().HasForeignKey(l=> l.CountryCode).IsRequired();
            });
        }


        private void AddCountries(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location.Country>(e =>
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
                e.Property(a=> a.Contacts).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.Coordinates).HasColumnType("geometry (point)").IsRequired();
                e.Property(a=> a.Name).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.Pictures).HasColumnType("jsonb");
                e.Property(a => a.Rating).IsRequired();
                e.Property(a=> a.AccommodationAmenities).HasColumnType("jsonb");
                e.Property(a=> a.RoomAmenities).HasColumnType("jsonb");
                e.Property(a=> a.AdditionalInfo).HasColumnType("jsonb");
                e.Property(a => a.PropertyType);
                e.Property(a => a.TextualDescription).HasColumnType("jsonb");
                e.Property(a => a.CheckInTime);
                e.Property(a => a.CheckOutTime);
                e.Property(a => a.OccupancyDefinition).HasColumnType("jsonb");
                e.HasIndex(a=>a.Coordinates).HasMethod("GIST");
                e.HasOne<Location.Location>().WithMany().HasForeignKey(a=> a.LocationId).IsRequired();
            });
        }

        
        private void AddRooms(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(e =>
            {
                e.ToTable("Rooms");
                e.HasKey(r=> r.Id);
                e.Property(r=> r.Amenities).HasColumnType("jsonb");
                e.Property(r => r.Description).HasColumnType("jsonb");
                e.Property(r => r.Name).HasColumnType("jsonb").IsRequired();
                e.Property(r => r.OccupancyConfigurations).HasColumnType("jsonb").IsRequired();
                e.HasOne<Accommodation>().WithMany().HasForeignKey(r => r.AccommodationId).IsRequired();
            });
        }

        
        private void AddRates(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomRateData>(e =>
            {
                e.ToTable("RoomRates");
                e.HasKey(rr=> rr.Id);
                e.Property(rr => rr.Price).IsRequired();
                e.Property(rr => rr.CurrencyCode).IsRequired();
                e.Property(rr => rr.MealPlan);
                e.Property(rr => rr.BoardBasis);
                e.Property(rr=> rr.StartDate).IsRequired();
                e.Property(rr=> rr.EndDate).IsRequired();
                e.HasOne<Room>().WithMany().HasForeignKey(rr => rr.RoomId);
            });
        }

        
        private void AddRoomAvailabilityRestrictions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomAvailabilityRestrictions>(e =>
            {
                e.ToTable("RoomAvailabilityRestrictions");
                e.HasKey(rr => rr.Id);
                e.Property(rr => rr.Restrictions).IsRequired().HasDefaultValue(SaleRestrictions.StopSale);
                e.Property(rr => rr.StartDate).IsRequired();
                e.Property(rr => rr.EndDate).IsRequired();
                e.HasOne<Room>().WithMany().HasForeignKey(rr => rr.RoomId);
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
                e.Property(po => po.BookByDate);
                e.Property(po => po.ValidFromDate).IsRequired();
                e.Property(po => po.ValidToDate).IsRequired();
                e.Property(po => po.BookingCode).IsRequired();
                e.HasOne<Room>().WithMany().HasForeignKey(po => po.RoomId);
            });
        }


        private void AddRoomAllocationRequirements(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomAllocationRequirement>(e =>
            {
                e.ToTable("RoomAllocationRequirements");
                e.HasKey(rar => rar.Id);
                e.Property(rar => rar.StartDate).IsRequired();
                e.Property(rar => rar.EndDate).IsRequired();
                e.Property(rar => rar.MinimumStayNights);
                e.Property(rar => rar.ReleasePeriod).HasColumnType("jsonb");
                e.Property(rar => rar.Allotment);
                e.HasOne<Room>().WithMany().HasForeignKey(rar => rar.RoomId);
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
                e.HasKey(cp => cp.Id);
                e.Property(cp => cp.StartDate).IsRequired();
                e.Property(cp => cp.EndDate).IsRequired();
                e.Property(bo => bo.CancellationPolicyData).HasColumnType("jsonb").IsRequired();
                e.Property(bo => bo.RoomId).IsRequired();
            });
        }
        
        
        public virtual DbSet<Location.Location> Locations { get; set; }
        public virtual DbSet<Accommodation> Accommodations { get; set; }
        public virtual DbSet<Location.Country> Countries { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomAvailabilityRestrictions> RoomAvailabilityRestrictions { get; set; }
        public virtual DbSet<RoomRateData> RoomRates { get; set; }
        public virtual DbSet<RoomAllocationRequirement> RoomAllocationRequirements { get; set; }
        public virtual DbSet<RoomPromotionalOffer> RoomPromotionalOffers { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<RoomCancellationPolicy> CancellationPolicies { get; set; }
    }
}