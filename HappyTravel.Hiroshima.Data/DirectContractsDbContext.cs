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

            AddAccommodations(modelBuilder);
            AddBooking(modelBuilder);
            AddCancellationPolicies(modelBuilder);
            AddContractAccommodationRelation(modelBuilder);
            AddContractManagers(modelBuilder);
            AddContracts(modelBuilder);
            AddCountries(modelBuilder);
            AddDocuments(modelBuilder);
            AddImages(modelBuilder);
            AddLocations(modelBuilder);
            AddPromotionalOffers(modelBuilder);
            AddPromotionalOffersStopSale(modelBuilder);
            AddRates(modelBuilder);
            AddRoomAllocationRequirements(modelBuilder);
            AddRoomAvailabilityRestrictions(modelBuilder);
            AddRooms(modelBuilder);
            AddSeasonRanges(modelBuilder);
            AddSeasons(modelBuilder);
        }


        private void AddAccommodations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accommodation>(e =>
            {
                e.ToTable("Accommodations");
                e.HasKey(a => a.Id);
                e.Property(a => a.Address).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.ContactInfo).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.Coordinates).HasColumnType("geometry (point)").IsRequired();
                e.Property(a => a.Name).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.Pictures).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.Rating).IsRequired();
                e.Property(a => a.AccommodationAmenities).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.AdditionalInfo).HasColumnType("jsonb").HasDefaultValueSql("'{}'::json");
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
                e.HasIndex(a => a.Coordinates).HasMethod("GIST");
                e.HasIndex(a => a.LocationId);
                e.HasIndex(a => a.ContractManagerId);
                e.HasOne(a => a.ContractManager).WithMany(cm => cm.Accommodations).OnDelete(DeleteBehavior.SetNull);
                e.HasOne(a => a.Location).WithMany().OnDelete(DeleteBehavior.SetNull);
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
                e.HasOne(rcp => rcp.Room).WithMany(r => r.RoomCancellationPolicies).OnDelete(DeleteBehavior.SetNull);
                e.HasOne(rcp => rcp.Season).WithMany().OnDelete(DeleteBehavior.SetNull);
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


        private void AddContractManagers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractManager>(e =>
            {
                e.ToTable("ContractManagers");
                e.HasKey(cm => cm.Id);
                e.Property(cm => cm.IdentityHash).IsRequired();
                e.Property(cm => cm.Email).IsRequired();
                e.Property(cm => cm.FirstName).IsRequired();
                e.Property(cm => cm.LastName).IsRequired();
                e.Property(cm => cm.Title).IsRequired();
                e.Property(cm => cm.Position).IsRequired();
                e.Property(cm => cm.Phone).IsRequired();
                e.Property(cm => cm.Fax);
                
                e.Property(cm => cm.Created).IsRequired().HasDefaultValueSql("now() at time zone 'utc'");
                e.Property(cm => cm.Updated).IsRequired().HasDefaultValueSql("now() at time zone 'utc'");
                e.Property(cm => cm.IsActive).IsRequired().HasDefaultValue(false);
                
                e.HasIndex(cm => cm.IdentityHash).IsUnique();
                e.HasIndex(cm => cm.Email).IsUnique();
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
                e.HasOne(c => c.ContractManager).WithMany(cm => cm.Contracts).OnDelete(DeleteBehavior.SetNull);
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


        private void AddDocuments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>(e =>
            {
                e.ToTable("Documents");
                e.HasKey(d => d.Id);
                e.Property(d => d.Id).HasColumnType("uuid").HasDefaultValueSql("uuid_generate_v4()");
                e.Property(d => d.Name).IsRequired();
                e.Property(d => d.ContentType).IsRequired();
                e.Property(d => d.Key).IsRequired();
                e.Property(d => d.Created).IsRequired();
                e.Property(d => d.ContractManagerId).IsRequired();
                e.Property(d => d.ContractId).IsRequired();
                e.HasIndex(d => d.ContractManagerId);
                e.HasIndex(d => d.ContractId);
                e.HasOne(d => d.Contract).WithMany(c => c.Documents).OnDelete(DeleteBehavior.SetNull);
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
                e.HasOne(i => i.Accommodation).WithMany(a => a.Images).OnDelete(DeleteBehavior.SetNull);
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
                e.HasOne(l => l.Country).WithMany(c => c.Locations).OnDelete(DeleteBehavior.SetNull);
            });
        }


        private void AddPromotionalOffers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomPromotionalOffer>(e =>
            {
                e.ToTable("RoomPromotionalOffers");
                e.HasKey(rpo => rpo.Id);
                e.Property(rpo => rpo.DiscountPercent).IsRequired();
                e.Property(rpo => rpo.Description).HasColumnType("jsonb");
                e.Property(rpo => rpo.BookByDate).IsRequired();
                e.Property(rpo => rpo.ValidFromDate).IsRequired();
                e.Property(rpo => rpo.ValidToDate).IsRequired();
                e.Property(rpo => rpo.BookingCode);
                e.Property(rpo => rpo.RoomId).IsRequired();
                e.HasIndex(rpo => rpo.RoomId);
                e.HasOne(rpo => rpo.Contract).WithMany(c => c.PromotionalOffers).OnDelete(DeleteBehavior.SetNull);
                e.HasOne(rpo => rpo.Room).WithMany(r => r.RoomPromotionalOffers).OnDelete(DeleteBehavior.SetNull);
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


        private void AddRates(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomRate>(e =>
            {
                e.ToTable("RoomRates");
                e.HasKey(rr => rr.Id);
                e.Property(rr => rr.Price).IsRequired();
                e.Property(rr => rr.Currency).IsRequired();
                e.Property(rr => rr.MealPlan).IsRequired();
                e.Property(rr => rr.BoardBasis).IsRequired();
                e.Property(rr => rr.SeasonId).IsRequired();
                e.Property(rr => rr.RoomId).IsRequired();
                e.Property(rr => rr.RoomType).IsRequired();
                e.HasIndex(rr => rr.SeasonId);
                e.HasIndex(rr => rr.RoomId);
                e.HasOne(rr => rr.Room).WithMany(r => r.RoomRates).OnDelete(DeleteBehavior.SetNull);
                e.HasOne(rr => rr.Season).WithMany().OnDelete(DeleteBehavior.SetNull);
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
                e.HasOne(rar => rar.Room).WithMany(r => r.RoomAllocationRequirements).OnDelete(DeleteBehavior.SetNull);
                e.HasOne(rar => rar.SeasonRange).WithMany().OnDelete(DeleteBehavior.SetNull);
            });
        }


        private void AddRoomAvailabilityRestrictions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomAvailabilityRestriction>(e =>
            {
                e.ToTable("RoomAvailabilityRestrictions");
                e.HasKey(rar => rar.Id);
                e.Property(rar => rar.Restriction).IsRequired().HasDefaultValue(AvailabilityRestrictions.FreeSale);
                e.Property(rar => rar.FromDate).IsRequired();
                e.Property(rar => rar.ToDate).IsRequired();
                e.Property(rar => rar.RoomId).IsRequired();
                e.Property(rar => rar.ContractId).IsRequired();
                e.HasIndex(rar => rar.RoomId);
                e.HasIndex(rar => rar.ContractId);
                e.HasIndex(rar => rar.Restriction);
                e.HasOne(rar => rar.Contract).WithMany(c => c.RoomAvailabilityRestrictions).OnDelete(DeleteBehavior.SetNull);
                e.HasOne(rar => rar.Room).WithMany(r => r.RoomAvailabilityRestrictions).OnDelete(DeleteBehavior.SetNull);
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
                e.HasOne(r => r.Accommodation).WithMany(a => a.Rooms).OnDelete(DeleteBehavior.SetNull); 
            });
        }


        private void AddSeasonRanges(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeasonRange>(e =>
            {
                e.ToTable("SeasonRanges");
                e.HasKey(sr => sr.Id);
                e.Property(sr => sr.StartDate).IsRequired();
                e.Property(sr => sr.EndDate).IsRequired();
                e.Property(sr => sr.SeasonId).IsRequired();
                e.HasIndex(sr => sr.SeasonId);
                e.HasOne(sr => sr.Season).WithMany(s => s.SeasonRanges).OnDelete(DeleteBehavior.SetNull);
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
                e.HasOne(s => s.Contract).WithMany(c => c.Seasons).OnDelete(DeleteBehavior.SetNull);
            });
        }


        public virtual DbSet<Accommodation> Accommodations { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<RoomCancellationPolicy> RoomCancellationPolicies { get; set; }
        public virtual DbSet<ContractAccommodationRelation> ContractAccommodationRelations { get; set; }
        public virtual DbSet<ContractManager> ContractManagers { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<RoomPromotionalOffer> PromotionalOffers { get; set; }
        public virtual DbSet<PromotionalOfferStopSale> PromotionalOfferStopSales { get; set; }
        public virtual DbSet<RoomRate> RoomRates { get; set; }
        public virtual DbSet<RoomAllocationRequirement> RoomAllocationRequirements { get; set; }
        public virtual DbSet<RoomAvailabilityRestriction> RoomAvailabilityRestrictions { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<SeasonRange> SeasonRanges { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
    }
}