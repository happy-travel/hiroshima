using System;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Booking;
using Hiroshima.DbData.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Location = Hiroshima.DbData.Models.Location;
using Room = Hiroshima.DbData.Models.Rooms.Room;

namespace Hiroshima.DbData
{
    public class DirectContractsDbContext : DbContext
    {
        public DirectContractsDbContext(DbContextOptions<DirectContractsDbContext> options) : base(options)
        { }


        [DbFunction("st_distance_sphere")]
        public static double GetDistance(Point from, Point to)
            => throw new Exception();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis")
                .HasPostgresExtension("uuid-ossp");
            modelBuilder.UseIdentityColumns();
            AddLocations(modelBuilder);
            AddAccommodations(modelBuilder);
            AddRooms(modelBuilder);
            AddRates(modelBuilder);
            AddRoomAvailabilityRestrictions(modelBuilder);
            AddPromotionalOffers(modelBuilder);
            AddRoomAllocationRequirements(modelBuilder);
            AddBooking(modelBuilder);
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
                e.Property(r => r.PermittedOccupancies).HasColumnType("jsonb").IsRequired();
                e.HasOne<Accommodation>().WithMany().HasForeignKey(r => r.AccommodationId).IsRequired();
            });
        }

        
        private void AddRates(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomRate>(e =>
            {
                e.ToTable("RoomRates");
                e.HasKey(rr=> rr.Id);
                e.Property(rr => rr.Price).IsRequired();
                e.Property(rr => rr.CurrencyCode).IsRequired();
                e.Property(rr => rr.MealPlan);
                e.Property(rr => rr.BoardBasis);
                e.Property(rr=> rr.StartsFromDate).IsRequired();
                e.Property(rr=> rr.EndsToDate).IsRequired();
                e.HasOne<Room>().WithMany().HasForeignKey(rr => rr.RoomId);
            });
        }

        
        private void AddRoomAvailabilityRestrictions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomAvailabilityRestrictions>(e =>
            {
                e.ToTable("RoomAvailabilityRestrictions");
                e.HasKey(rr => rr.Id);
                e.Property(rr => rr.Restrictions).HasColumnType("jsonb").IsRequired();
                e.Property(rr => rr.StartsFromDate).HasColumnType("jsonb").IsRequired();
                e.Property(rr => rr.EndsToDate).HasColumnType("jsonb").IsRequired();
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
                e.Property(rar => rar.StartsFromDate).IsRequired();
                e.Property(rar => rar.EndsToDate).IsRequired();
                e.Property(rar => rar.MinimumStayNights);
                e.Property(rar => rar.ReleasePeriod).HasColumnType("jsonb");
                e.Property(rar => rar.Allotment).HasDefaultValue(0);
                e.HasOne<Room>().WithMany().HasForeignKey(rar => rar.RoomId);
            });
        }
        
        
        private void AddBooking(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingOrder>(e =>
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
        
        
        public virtual DbSet<Location.Location> Locations { get; set; }
        public virtual DbSet<Accommodation> Accommodations { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomAvailabilityRestrictions> RoomAvailabilityRestrictions { get; set; }
        public virtual DbSet<RoomRate> RoomRates { get; set; }
        public virtual DbSet<RoomAllocationRequirement> RoomAllocationRequirements { get; set; }
        public virtual DbSet<RoomPromotionalOffer> RoomPromotionalOffers { get; set; }
        public virtual DbSet<BookingOrder>BookingOrders { get; set; }
    }
}