using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.DirectContracts.Services;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.DirectManager.UnitTests.Mocks;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HappyTravel.Hiroshima.DirectManager.UnitTests.Tests.Services.AccommodationManagementServiceTests
{
    public class AccommodationManagementServiceTests
    {
        public AccommodationManagementServiceTests()
        {
            var dbContextMock = DirectContractsDbContextFactoryMock.Create();
            dbContextMock.Setup(x => x.ServiceSuppliers).Returns(DbSetMockProvider.GetDbSetMock(_serviceSuppliers));
            dbContextMock.Setup(x => x.Managers).Returns(DbSetMockProvider.GetDbSetMock(_managers));
            dbContextMock.Setup(x => x.ManagerServiceSupplierRelations).Returns(DbSetMockProvider.GetDbSetMock(_relations));
            //dbContextMock.Setup(x => x.Accommodations).Returns(DbSetMockProvider.GetDbSetMock(_accommodations));

            _accommodationManagementService = new AccommodationManagementService(
                new ManagerContextService(dbContextMock.Object, new TokenInfoAccessorMock(), new Sha256HashGenerator()), 
                new ServiceSupplierContextService(dbContextMock.Object),
                new ImageManagementServiceMock(),
                new AmenityService(dbContextMock.Object),
                dbContextMock.Object, 
                new GeometryFactory(),
                new DefaultDateTimeProvider());
        }


        [Fact]
        public async Task Manager_cannot_get_accommodation_from_another_service_supplier()
        {
            var (_, isFailure, _, _) = await _accommodationManagementService.Get(1);
            Assert.True(isFailure);
        }


        private readonly IEnumerable<ServiceSupplier> _serviceSuppliers = new[]
        {
            new ServiceSupplier
            {
                Id = 1,
                Name = "serviceSupplierName1",
                Address = "serviceSupplierAddress1",
                PostalCode = string.Empty,
                Phone = "+971 (123) 4567890",
                Website = string.Empty,
                Created = DateTime.Now,
                Modified = DateTime.Now
            },
            new ServiceSupplier
            {
                Id = 2,
                Name = "serviceSupplierName2",
                Address = "serviceSupplierAddress2",
                PostalCode = string.Empty,
                Phone = "+971 (321) 4567890",
                Website = string.Empty,
                Created = DateTime.Now,
                Modified = DateTime.Now
            }
        };

        private readonly IEnumerable<Manager> _managers = new[]
        {
            new Manager
            {
                Id = 1,
                IdentityHash = "identityHash1",
                FirstName = "firstName1",
                LastName = "lastName1",
                Title = "title1",
                Position = string.Empty,
                Email = "email1",
                Phone = "+971 (111) 1111111",
                Fax = string.Empty,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                IsActive = true
            },
            new Manager
            {
                Id = 2,
                IdentityHash = "identityHash2",
                FirstName = "firstName2",
                LastName = "lastName2",
                Title = "title2",
                Position = string.Empty,
                Email = "email2",
                Phone = "+971 (222) 2222222",
                Fax = string.Empty,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                IsActive = true
            },
            new Manager
            {
                Id = 3,
                IdentityHash = "identityHash3",
                FirstName = "firstName3",
                LastName = "lastName3",
                Title = "title3",
                Position = string.Empty,
                Email = "email3",
                Phone = "+971 (333) 3333333",
                Fax = string.Empty,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                IsActive = true
            },
            new Manager
            {
                Id = 4,
                IdentityHash = "identityHash4",
                FirstName = "firstName4",
                LastName = "lastName4",
                Title = "title4",
                Position = string.Empty,
                Email = "email4",
                Phone = "+971 (444) 4444444",
                Fax = string.Empty,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                IsActive = true
            }
        };

        private readonly IEnumerable<ManagerServiceSupplierRelation> _relations = new[]
        {
            new ManagerServiceSupplierRelation
            {
                ManagerId = 1,
                ManagerPermissions = Common.Models.Enums.ManagerPermissions.All,
                ServiceSupplierId = 1,
                IsMaster = true,
                IsActive = true
            },
            new ManagerServiceSupplierRelation
            {
                ManagerId = 2,
                ManagerPermissions = Common.Models.Enums.ManagerPermissions.All,
                ServiceSupplierId = 1,
                IsMaster = false,
                IsActive = true
            },
            new ManagerServiceSupplierRelation
            {
                ManagerId = 3,
                ManagerPermissions = Common.Models.Enums.ManagerPermissions.All,
                ServiceSupplierId = 1,
                IsMaster = false,
                IsActive = false
            },
            new ManagerServiceSupplierRelation
            {
                ManagerId = 4,
                ManagerPermissions = Common.Models.Enums.ManagerPermissions.All,
                ServiceSupplierId = 2,
                IsMaster = true,
                IsActive = true
            }
        };

        private readonly IEnumerable<Accommodation> _accommodations = new[]
{
            new Accommodation
            {
                Id = 1,
                Name = new MultiLanguage<string>(),
                Address = new MultiLanguage<string>(),
                TextualDescription = new MultiLanguage<TextualDescription>(),
                LeisureAndSports = new MultiLanguage<List<string>>(),
                Coordinates = new Point(0.0, 0.0),
                Rating = AccommodationStars.FiveStars,
                CheckInTime= string.Empty,
                CheckOutTime = string.Empty,
                ContactInfo = new ContactInfo(),
                PropertyType = PropertyTypes.Hotels,
                BuildYear = null,
                Floors = 2,
                PostalCode = string.Empty,
                AccommodationAmenities = new MultiLanguage<List<string>>(),
                AdditionalInfo = new MultiLanguage<string>(),
                OccupancyDefinition = new Common.Models.Accommodations.Rooms.OccupancyDefinitions.OccupancyDefinition(),
                ServiceSupplierId = 1,
                LocationId = 1,
                //public ServiceSupplier ServiceSupplier { get; set; }
                //public Locations.Location Location { get; set; }
                RateOptions = new RateOptions { SingleAdultAndChildBookings = SingleAdultAndChildBookings.ApplyAdultRate },
                Status = Status.Active,
                Created = DateTime.Now,
                Modified = DateTime.Now
            },
            new Accommodation
            {
                Id = 2,
                Name = new MultiLanguage<string>(),
                Address = new MultiLanguage<string>(),
                TextualDescription = new MultiLanguage<TextualDescription>(),
                LeisureAndSports = new MultiLanguage<List<string>>(),
                Coordinates = new Point(0.0, 0.0),
                Rating = AccommodationStars.FiveStars,
                CheckInTime= string.Empty,
                CheckOutTime = string.Empty,
                ContactInfo = new ContactInfo(),
                PropertyType = PropertyTypes.Hotels,
                BuildYear = null,
                Floors = 2,
                PostalCode = string.Empty,
                AccommodationAmenities = new MultiLanguage<List<string>>(),
                AdditionalInfo = new MultiLanguage<string>(),
                OccupancyDefinition = new Common.Models.Accommodations.Rooms.OccupancyDefinitions.OccupancyDefinition(),
                ServiceSupplierId = 1,
                LocationId = 1,
                //public ServiceSupplier ServiceSupplier { get; set; }
                //public Locations.Location Location { get; set; }
                RateOptions = new RateOptions { SingleAdultAndChildBookings = SingleAdultAndChildBookings.ApplyAdultRate },
                Status = Status.Active,
                Created = DateTime.Now,
                Modified = DateTime.Now
            },
            new Accommodation
            {
                Id = 3,
                Name = new MultiLanguage<string>(),
                Address = new MultiLanguage<string>(),
                TextualDescription = new MultiLanguage<TextualDescription>(),
                LeisureAndSports = new MultiLanguage<List<string>>(),
                Coordinates = new Point(0.0, 0.0),
                Rating = AccommodationStars.FiveStars,
                CheckInTime= string.Empty,
                CheckOutTime = string.Empty,
                ContactInfo = new ContactInfo(),
                PropertyType = PropertyTypes.Hotels,
                BuildYear = null,
                Floors = 2,
                PostalCode = string.Empty,
                AccommodationAmenities = new MultiLanguage<List<string>>(),
                AdditionalInfo = new MultiLanguage<string>(),
                OccupancyDefinition = new Common.Models.Accommodations.Rooms.OccupancyDefinitions.OccupancyDefinition(),
                ServiceSupplierId = 1,
                LocationId = 1,
                //public ServiceSupplier ServiceSupplier { get; set; }
                //public Locations.Location Location { get; set; }
                RateOptions = new RateOptions { SingleAdultAndChildBookings = SingleAdultAndChildBookings.ApplyAdultRate },
                Status = Status.Active,
                Created = DateTime.Now,
                Modified = DateTime.Now
            }
        };


        private readonly AccommodationManagementService _accommodationManagementService;
    }
}