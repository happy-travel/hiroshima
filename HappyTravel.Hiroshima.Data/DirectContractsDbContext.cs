using System;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Common.Models.Images;
using HappyTravel.Hiroshima.Common.Models.Locations;
using HappyTravel.Hiroshima.Common.Models.Seasons;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Booking;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Location = HappyTravel.Hiroshima.Common.Models.Locations.Location;
using Room = HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.Room;

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

            AddContractManagers(modelBuilder);
            AddDocuments(modelBuilder);
            AddContracts(modelBuilder);
            AddLocations(modelBuilder);
            AddAccommodations(modelBuilder);
            AddImages(modelBuilder);
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
            AddPromotionalOffersStopSale(modelBuilder);
        }


        private void AddContractManagers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractManager>(e =>
            {
                e.ToTable("ContractManagers");
                e.HasKey(c => c.Id);
                e.Property(c => c.IdentityHash).IsRequired();
                e.Property(c => c.Email).IsRequired();
                e.Property(c => c.FirstName).IsRequired();
                e.Property(c => c.LastName).IsRequired();
                e.Property(c => c.Title).IsRequired();
                e.Property(c => c.Position).IsRequired();
                e.Property(c => c.Phone).IsRequired();
                e.Property(c => c.Fax);
                
                e.Property(c => c.Created).IsRequired().HasDefaultValueSql("now() at time zone 'utc'");
                e.Property(c => c.Updated).IsRequired().HasDefaultValueSql("now() at time zone 'utc'");
                e.Property(c => c.IsActive).IsRequired().HasDefaultValue(false);
                
                e.HasIndex(c => c.IdentityHash).IsUnique();
                e.HasIndex(c => c.Email).IsUnique();
            });
        }
        
        private void AddDocuments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>(e =>
            {
                e.ToTable("Documents");
                e.HasKey(c => c.Id);
                e.Property(c => c.Id).HasColumnType("uuid").HasDefaultValueSql("uuid_generate_v4()");
                e.Property(c => c.Name).IsRequired();
                e.Property(c => c.ContentType).IsRequired();
                e.Property(c => c.Key).IsRequired();
                e.Property(c => c.Created).IsRequired();
                e.Property(c => c.ContractManagerId).IsRequired();
                e.Property(c => c.ContractId).IsRequired();
                e.HasIndex(c => c.ContractManagerId);
                e.HasIndex(c => c.ContractId);
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
                e.Property(c => c.Created).IsRequired();
                e.Property(c => c.Modified).IsRequired();
                e.Property(c => c.Verified).IsRequired();
                e.HasIndex(c => c.ContractManagerId);
            });
        }
        
        
        private void AddLocations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>(e =>
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
                e.Property(a => a.LeisureAndSports).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.Status).IsRequired();
                e.Property(a => a.RateOptions).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.Created).IsRequired();
                e.Property(a => a.Modified).IsRequired();
                e.Property(a => a.Floors);
                e.Property(a => a.BuildYear);
                e.HasIndex(a=> a.Coordinates).HasMethod("GIST");
                e.HasIndex(a => a.LocationId);
                e.HasIndex(a => a.ContractManagerId);
            });
        }


        private void AddImages(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>(e =>
            {
                e.ToTable("Images");
                e.HasKey(i => i.Id);
                e.Property(i => i.Id).HasColumnType("uuid").HasDefaultValueSql("uuid_generate_v4()");
                e.Property(i => i.OriginalImageDetails).HasColumnType("jsonb").IsRequired();
                e.Property(i => i.MainImage).HasColumnType("jsonb").IsRequired();
                e.Property(i => i.SmallImage).HasColumnType("jsonb").IsRequired();
                e.Property(i => i.Created).IsRequired();
                e.Property(i => i.ContractManagerId).IsRequired();
                e.Property(i => i.AccommodationId).IsRequired();
                e.Property(i => i.Position).IsRequired();
                e.Property(i => i.Description).HasColumnType("jsonb").IsRequired();
                e.HasIndex(i => i.ContractManagerId);
                e.HasIndex(i => i.AccommodationId);
            });
        }


        private void AddRooms(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(e =>
            {
                e.ToTable("Rooms");
                e.HasKey(r=> r.Id);
                e.Property(r=> r.Amenities).HasColumnType("jsonb").IsRequired();
                e.Property(r => r.Description).HasColumnType("jsonb").IsRequired();
                e.Property(r => r.Name).HasColumnType("jsonb").IsRequired();
                e.Property(r => r.OccupancyConfigurations).HasColumnType("jsonb").IsRequired();
                e.Property(r => r.AccommodationId).IsRequired();
                e.Property(r => r.Created).IsRequired();
                e.Property(r => r.Modified).IsRequired();
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
                e.Property(rr => rr.MealPlan).IsRequired();
                e.Property(rr => rr.BoardBasis).IsRequired();
                e.Property(rr=> rr.SeasonId).IsRequired();
                e.Property(rr => rr.RoomId).IsRequired();
                e.Property(rr => rr.RoomType).IsRequired();
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
                e.Property(po => po.Description).HasColumnType("jsonb");
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
                e.Property(rcp => rcp.SeasonId).IsRequired();
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
        

        private void AddPromotionalOffersStopSale(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PromotionalOfferStopSale>(e =>
            {
                e.ToTable("PromotionalOffersStopSale");
                e.HasKey(pofs => pofs.Id);
                e.Property(pofs => pofs.RoomId).IsRequired();
                e.Property(pofs => pofs.FromDate).IsRequired();
                e.Property(pofs => pofs.ToDate).IsRequired();
                e.Property(pofs => pofs.ContractId).IsRequired();
                e.HasIndex(pofs => pofs.RoomId);
            });
        }

        
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Accommodation> Accommodations { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomAvailabilityRestriction> RoomAvailabilityRestrictions { get; set; }
        public virtual DbSet<RoomRate> RoomRates { get; set; }
        public virtual DbSet<RoomAllocationRequirement> RoomAllocationRequirements { get; set; }
        public virtual DbSet<RoomPromotionalOffer> PromotionalOffers { get; set; }
        public virtual DbSet<PromotionalOfferStopSale> PromotionalOfferStopSales { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<RoomCancellationPolicy> RoomCancellationPolicies { get; set; }
        public virtual DbSet<ContractManager> ContractManagers { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<ContractAccommodationRelation> ContractAccommodationRelations { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<SeasonRange> SeasonRanges { get; set; }
    }
}