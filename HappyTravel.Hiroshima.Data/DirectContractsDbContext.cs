using System;
using System.Text.Json;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Common.Models.Bookings;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Common.Models.Images;
using HappyTravel.Hiroshima.Common.Models.Locations;
using HappyTravel.Hiroshima.Common.Models.Seasons;
using HappyTravel.Hiroshima.Data.Models;
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
            AddAmenities(modelBuilder);
            AddBooking(modelBuilder);
            AddCancellationPolicies(modelBuilder);
            AddServiceSuppliers(modelBuilder);
            AddContractAccommodationRelation(modelBuilder);
            AddManagers(modelBuilder);
            AddManagerServiceSupplierRelations(modelBuilder);
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
            AddRoomOccupancies(modelBuilder);
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
                e.Property(a => a.AdditionalInfo).HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");
                e.Property(a => a.PropertyType).IsRequired();
                e.Property(a => a.TextualDescription).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.CheckInTime).IsRequired();
                e.Property(a => a.CheckOutTime).IsRequired();
                e.Property(a => a.OccupancyDefinition).HasColumnType("jsonb");
                e.Property(a => a.ServiceSupplierId).IsRequired();
                e.Property(a => a.LeisureAndSports).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.Status).IsRequired();
                e.Property(a => a.RateOptions).HasColumnType("jsonb").IsRequired();
                e.Property(a => a.Created).IsRequired();
                e.Property(a => a.Modified).IsRequired();
                e.Property(a => a.Floors);
                e.Property(a => a.BuildYear);
                e.Property(a => a.Images).HasColumnType("jsonb").IsRequired().HasDefaultValueSql("'[]'::jsonb");
                e.HasIndex(a => a.Coordinates).HasMethod("GIST");
                e.HasIndex(a => a.LocationId);
                e.HasIndex(a => a.ServiceSupplierId);
                e.HasOne(a => a.ServiceSupplier).WithMany(cm => cm.Accommodations).OnDelete(DeleteBehavior.SetNull);
                e.HasOne(a => a.Location).WithMany().OnDelete(DeleteBehavior.SetNull);
            });
        }

        
        private void AddAmenities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Amenity>(e =>
            {
                e.ToTable("Amenities");
                e.HasKey(a => a.Id);
                e.Property(a => a.LanguageCode).IsRequired();
                e.Property(a => a.Name).IsRequired();
                e.HasIndex(a => a.LanguageCode);
            });
        }


        private void AddBooking(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingOrder>(e =>
            {
                e.ToTable("BookingOrders");
                e.HasKey(b => b.Id);
                e.Property(b=> b.Id).HasDefaultValueSql("uuid_generate_v4()").IsRequired();
                e.Property(b => b.Status).IsRequired();
                e.Property(b => b.ReferenceCode).IsRequired();
                e.Property(b => b.CheckInDate).IsRequired();
                e.Property(b => b.CheckOutDate).IsRequired();
                e.Property(b => b.LanguageCode).IsRequired();
                e.Property(b => b.Created).IsRequired().HasDefaultValueSql("now() at time zone 'utc'");
                e.Property(b => b.Modified).IsRequired().HasDefaultValueSql("now() at time zone 'utc'");
                e.Property(b => b.AvailabilityRequest).IsRequired().HasColumnType("jsonb");
                e.Property(b => b.BookingRequest).IsRequired().HasColumnType("jsonb");
                e.Property(b => b.AvailableRates).IsRequired().HasColumnType("jsonb");
                e.HasIndex(b => b.ServiceSupplierId);
                e.HasOne(b => b.ServiceSupplier).WithMany(cm => cm.BookingOrders).OnDelete(DeleteBehavior.SetNull);
                e.HasMany(b => b.RoomOccupancies).WithOne().OnDelete(DeleteBehavior.SetNull);
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
                e.HasOne(rcp => rcp.Room).WithMany(r => r.CancellationPolicies).OnDelete(DeleteBehavior.SetNull);
                e.HasOne(rcp => rcp.Season).WithMany().OnDelete(DeleteBehavior.SetNull);
            });
        }


        private void AddServiceSuppliers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceSupplier>(e =>
            {
                e.ToTable("ServiceSuppliers");
                e.HasKey(ss => ss.Id);
                e.Property(ss => ss.Name).IsRequired();
                e.Property(ss => ss.Address).IsRequired();
                e.Property(ss => ss.PostalCode).IsRequired();
                e.Property(ss => ss.Phone).IsRequired();
                e.Property(ss => ss.Website).IsRequired();
                e.Property(ss => ss.Created).IsRequired();
                e.Property(ss => ss.Modified).IsRequired();
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
                e.HasOne(car => car.Accommodation).WithMany().OnDelete(DeleteBehavior.SetNull);
                e.HasOne(car => car.Contract).WithMany().OnDelete(DeleteBehavior.SetNull);
            });
        }


        private void AddManagers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manager>(e =>
            {
                e.ToTable("Managers");
                e.HasKey(m => m.Id);
                e.Property(m => m.IdentityHash).IsRequired();
                e.Property(m => m.Email).IsRequired();
                e.Property(m => m.FirstName).IsRequired();
                e.Property(m => m.LastName).IsRequired();
                e.Property(m => m.Title).IsRequired();
                e.Property(m => m.Position).IsRequired();
                e.Property(m => m.Phone).IsRequired();
                e.Property(m => m.Fax);
                e.Property(m => m.Created).IsRequired().HasDefaultValueSql("now() at time zone 'utc'");
                e.Property(m => m.Updated).IsRequired().HasDefaultValueSql("now() at time zone 'utc'");
                e.Property(m => m.IsActive).IsRequired().HasDefaultValue(false);
                e.Property(m => m.ServiceSupplierId).IsRequired();

                e.HasIndex(m => m.IdentityHash).IsUnique();
                e.HasIndex(m => m.Email).IsUnique();
                e.HasIndex(m => m.ServiceSupplierId);
                e.HasOne(m => m.ServiceSupplier).WithMany(c => c.Managers).OnDelete(DeleteBehavior.SetNull);
            });
        }


        private void AddManagerServiceSupplierRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ManagerServiceSupplierRelation>(e =>
            {
                e.ToTable("ManagerServiceSupplierRelations");
                e.HasKey(mssr => new { mssr.ManagerId, mssr.ServiceSupplierId });
                e.Property(mssr => mssr.ManagerPermissions).IsRequired().HasDefaultValue(ManagerPermissions.All);   // All permissions
                e.Property(mssr => mssr.IsMaster).IsRequired().HasDefaultValue(false); 
                e.Property(mssr => mssr.IsActive).IsRequired().HasDefaultValue(true);
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
                e.Property(c => c.ServiceSupplierId).IsRequired();
                e.Property(c => c.Created).IsRequired();
                e.Property(c => c.Modified).IsRequired();
                e.Property(c => c.Verified).IsRequired();
                e.HasIndex(c => c.ServiceSupplierId);
                e.HasOne(c => c.ServiceSupplier).WithMany(cm => cm.Contracts).OnDelete(DeleteBehavior.SetNull);
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
                e.Property(d => d.ServiceSupplierId).IsRequired();
                e.Property(d => d.ContractId).IsRequired();
                e.HasIndex(d => d.ServiceSupplierId);
                e.HasIndex(d => d.ContractId);
                e.HasOne(d => d.ServiceSupplier).WithMany().OnDelete(DeleteBehavior.SetNull);
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
                e.Property(i => i.Keys).HasColumnType("jsonb").IsRequired().HasDefaultValueSql("'{}'::jsonb");
                e.Property(i => i.Created).IsRequired();
                e.Property(i => i.ServiceSupplierId).IsRequired();
                e.Property(i => i.ReferenceId).IsRequired();
                e.Property(i => i.ImageType).IsRequired();
                e.Property(i => i.Position).IsRequired();
                e.Property(i => i.Description).HasColumnType("jsonb").IsRequired();
                e.HasIndex(i => i.ServiceSupplierId);
                e.HasIndex(i => i.ReferenceId);
                e.HasIndex(i => i.ImageType);
                e.HasOne(i => i.ServiceSupplier).WithMany().OnDelete(DeleteBehavior.SetNull);
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
                e.HasKey(poss => poss.Id);
                e.Property(poss => poss.RoomId).IsRequired();
                e.Property(poss => poss.FromDate).IsRequired();
                e.Property(poss => poss.ToDate).IsRequired();
                e.Property(poss => poss.ContractId).IsRequired();
                e.HasIndex(poss => poss.RoomId);
                e.HasOne(poss => poss.Contract).WithMany(c => c.PromotionalOffersStopSale).OnDelete(DeleteBehavior.SetNull);
                e.HasOne(poss => poss.Room).WithMany(r => r.PromotionalOffersStopSale).OnDelete(DeleteBehavior.SetNull);
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
                e.HasOne(rar => rar.Room).WithMany(r => r.AllocationRequirements).OnDelete(DeleteBehavior.SetNull);
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
                e.HasOne(rar => rar.Room).WithMany(r => r.AvailabilityRestrictions).OnDelete(DeleteBehavior.SetNull);
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
                e.Property(r => r.Images).HasColumnType("jsonb").IsRequired().HasDefaultValueSql("'[]'::jsonb");
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

        
        private void AddRoomOccupancies(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomOccupancy>(e =>
            {
                e.ToTable("RoomOccupancies");
                e.HasKey(a  => a.Id);
                e.Property(a => a.RoomId).IsRequired();
                e.Property(a => a.FromDate).IsRequired();
                e.Property(a => a.ToDate).IsRequired();
                e.Property(a => a.BookingOrderId).IsRequired();
                e.Property(a => a.Created).HasDefaultValueSql("now() at time zone 'utc'").IsRequired();
                e.HasOne(a => a.Room).WithMany(r => r.RoomOccupations).OnDelete(DeleteBehavior.SetNull);
            });
        }
        

        public virtual DbSet<Accommodation> Accommodations { get; set; }
        public virtual DbSet<BookingOrder> BookingOrders { get; set; }
        public virtual DbSet<Amenity> Amenities { get; set; }
        public virtual DbSet<RoomCancellationPolicy> RoomCancellationPolicies { get; set; }
        public virtual DbSet<ServiceSupplier> Companies { get; set; }
        public virtual DbSet<ContractAccommodationRelation> ContractAccommodationRelations { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<ManagerServiceSupplierRelation> ManagerServiceSupplierRelations { get; set; }
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
        public virtual DbSet<RoomOccupancy> RoomOccupancies { get; set; }
    }
}