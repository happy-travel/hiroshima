using CSharpFunctionalExtensions;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Common.Models.Images;
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
    public class AccommodationManagementServiceTests : IDisposable
    {
        public AccommodationManagementServiceTests()
        {
            var dbContextMock = DirectContractsDbContextMockFactory.Create();
            dbContextMock.Setup(x => x.ServiceSuppliers).Returns(DbSetMockProvider.GetDbSetMock(_serviceSuppliers));
            dbContextMock.Setup(x => x.Managers).Returns(DbSetMockProvider.GetDbSetMock(_managers));
            dbContextMock.Setup(x => x.ManagerServiceSupplierRelations).Returns(DbSetMockProvider.GetDbSetMock(_relations));
            dbContextMock.Setup(x => x.Accommodations).Returns(DbSetMockProvider.GetDbSetMock(_accommodations));
            dbContextMock.Setup(x => x.Rooms).Returns(DbSetMockProvider.GetDbSetMock(_rooms));

            _accommodationManagementService = new AccommodationManagementService(
                new ManagerContextService(dbContextMock.Object, new TokenInfoAccessorMock(), new Sha256HashGenerator()), 
                new ServiceSupplierContextService(dbContextMock.Object),
                new ImageManagementServiceMock(),
                new AmenityService(dbContextMock.Object),
                dbContextMock.Object, 
                new GeometryFactory(),
                _dateTimeProvider);
        }


        [Fact]
        public async Task Found_accommodation_must_match()
        {
            var expectedAccommodation = new Models.Responses.Accommodation(1,
                new MultiLanguage<string> { En = "name1" },
                new MultiLanguage<string> { En = "address1" },
                new MultiLanguage<TextualDescription> { En = new TextualDescription { Type = EdoContracts.Accommodations.Enums.TextualDescriptionTypes.General, Description = "description1" } },
                new GeoPoint(1.0, 1.0),
                AccommodationStars.FourStars,
                "checkInTime1", 
                "checkOutTime1",
                new Models.Responses.ContactInfo(new List<string> { "email1" }, new List<string> { "phone1" }, new List<string> { "website1" }),
                PropertyTypes.Hotels,
                new MultiLanguage<List<string>> { En = new List<string> { "amenity1-1", "amenity1-2" } },
                new MultiLanguage<string> { En = "additionalInfo1" },
                new OccupancyDefinition
                {
                    Infant = new AgeRange { LowerBound = 0, UpperBound = 3 },
                    Child = new AgeRange { LowerBound = 3, UpperBound = 10 },
                    Teenager = new AgeRange { LowerBound = 10, UpperBound = 18 },
                    Adult = new AgeRange { LowerBound = 18, UpperBound = 140 }
                }, 
                1,
                new MultiLanguage<List<string>> { En = new List<string> { "leisure1", "sport1" } },
                Status.Active,
                new RateOptions { SingleAdultAndChildBookings = SingleAdultAndChildBookings.ApplyAdultRate }, 
                2,
                null, 
                "070000", 
                new List<Models.Responses.Room>());

            var (isSuccess, _, actualAccommodation, _) = await _accommodationManagementService.Get(accommodationId: 1);

            Assert.True(isSuccess);
            Assert.Equal(expectedAccommodation.Id, actualAccommodation.Id);
            Assert.Equal(expectedAccommodation.Name, actualAccommodation.Name);
            Assert.Equal(expectedAccommodation.Address, actualAccommodation.Address);
            Assert.Equal(expectedAccommodation.Description, actualAccommodation.Description);
            Assert.Equal(expectedAccommodation.LeisureAndSports, actualAccommodation.LeisureAndSports);
            Assert.Equal(expectedAccommodation.Coordinates, actualAccommodation.Coordinates);
            Assert.Equal(expectedAccommodation.Rating, actualAccommodation.Rating);
            Assert.Equal(expectedAccommodation.CheckInTime, actualAccommodation.CheckInTime);
            Assert.Equal(expectedAccommodation.CheckOutTime, actualAccommodation.CheckOutTime);
            //Assert.Equal(expectedAccommodation.ContactInfo, actualAccommodation.ContactInfo);
            Assert.Equal(expectedAccommodation.Type, actualAccommodation.Type);
            Assert.Equal(expectedAccommodation.BuildYear, actualAccommodation.BuildYear);
            Assert.Equal(expectedAccommodation.Floors, actualAccommodation.Floors);
            Assert.Equal(expectedAccommodation.PostalCode, actualAccommodation.PostalCode);
            Assert.Equal(expectedAccommodation.Amenities, actualAccommodation.Amenities);
            Assert.Equal(expectedAccommodation.AdditionalInfo, actualAccommodation.AdditionalInfo);
            //Assert.Equal(expectedAccommodation.OccupancyDefinition, actualAccommodation.OccupancyDefinition);
            Assert.Equal(expectedAccommodation.LocationId, actualAccommodation.LocationId);
            //Assert.Equal(expectedAccommodation.RateOptions, actualAccommodation.RateOptions);
            Assert.Equal(expectedAccommodation.Status, actualAccommodation.Status);
            Assert.Equal(expectedAccommodation.Rooms, actualAccommodation.Rooms);
        }


        [Fact]
        public async Task Manager_cannot_get_accommodation_from_another_service_supplier()
        {
            var (_, isFailure, _, _) = await _accommodationManagementService.Get(accommodationId: 3);
            
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
                Created = _dateTimeProvider.UtcNow(),
                Modified = _dateTimeProvider.UtcNow()
            },
            new ServiceSupplier
            {
                Id = 2,
                Name = "serviceSupplierName2",
                Address = "serviceSupplierAddress2",
                PostalCode = string.Empty,
                Phone = "+971 (321) 4567890",
                Website = string.Empty,
                Created = _dateTimeProvider.UtcNow(),
                Modified = _dateTimeProvider.UtcNow()
            }
        };

        private readonly IEnumerable<Manager> _managers = new[]
        {
            new Manager
            {
                Id = 1,
                IdentityHash = "7c2352a82f3275241bd574995259b4b691331b61d4c303f11af2940a6cb104cb",  // _identityHash = "94e53dea-313e-4e30-a8fb-6af1202a8ca6"
                FirstName = "firstName1",
                LastName = "lastName1",
                Title = "title1",
                Position = string.Empty,
                Email = "email1",
                Phone = "+971 (111) 1111111",
                Fax = string.Empty,
                Created = _dateTimeProvider.UtcNow(),
                Updated = _dateTimeProvider.UtcNow(),
                IsActive = true
            },
            new Manager
            {
                Id = 2,
                IdentityHash = "89df0261017f15699147f2172c6b40bae4971887b4085dc1e2ca2e3ba6bd9e5c",
                FirstName = "firstName2",
                LastName = "lastName2",
                Title = "title2",
                Position = string.Empty,
                Email = "email2",
                Phone = "+971 (222) 2222222",
                Fax = string.Empty,
                Created = _dateTimeProvider.UtcNow(),
                Updated = _dateTimeProvider.UtcNow(),
                IsActive = true
            },
            new Manager
            {
                Id = 3,
                IdentityHash = "50e8e915a487d1d36e5bc0ab27ff5641cb087aa55cd10fa11e70d207b8c8c9de",
                FirstName = "firstName3",
                LastName = "lastName3",
                Title = "title3",
                Position = string.Empty,
                Email = "email3",
                Phone = "+971 (333) 3333333",
                Fax = string.Empty,
                Created = _dateTimeProvider.UtcNow(),
                Updated = _dateTimeProvider.UtcNow(),
                IsActive = true
            },
            new Manager
            {
                Id = 4,
                IdentityHash = "0417fff728f9edd239bdbb8e80b8535919af8abe57a941d21ef2866d475b9009",
                FirstName = "firstName4",
                LastName = "lastName4",
                Title = "title4",
                Position = string.Empty,
                Email = "email4",
                Phone = "+971 (444) 4444444",
                Fax = string.Empty,
                Created = _dateTimeProvider.UtcNow(),
                Updated = _dateTimeProvider.UtcNow(),
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
                Name = new MultiLanguage<string> { En = "name1" },
                Address = new MultiLanguage<string> { En = "address1" },
                TextualDescription = new MultiLanguage<TextualDescription> { En = new TextualDescription { Type = EdoContracts.Accommodations.Enums.TextualDescriptionTypes.General, Description = "description1" } },
                LeisureAndSports = new MultiLanguage<List<string>> { En = new List<string> { "leisure1", "sport1" } },
                Coordinates = new Point(1.0, 1.0),
                Rating = AccommodationStars.FourStars,
                CheckInTime = "checkInTime1",
                CheckOutTime = "checkOutTime1",
                ContactInfo = new ContactInfo { Emails = new List<string> { "email1"}, Faxes = new List<string> { "fax1"}, Phones = new List<string> { "phone1" }, Websites = new List<string> { "website1" } },
                PropertyType = PropertyTypes.Hotels,
                BuildYear = null,
                Floors = 2,
                PostalCode = "070000",
                AccommodationAmenities = new MultiLanguage<List<string>> { En = new List<string> { "amenity1-1", "amenity1-2" } },
                AdditionalInfo = new MultiLanguage<string> { En = "additionalInfo1" },
                OccupancyDefinition = new OccupancyDefinition 
                { 
                    Infant = new AgeRange { LowerBound = 0, UpperBound = 3 },
                    Child = new AgeRange { LowerBound = 3, UpperBound = 10 },
                    Teenager = new AgeRange { LowerBound = 10, UpperBound = 18 },
                    Adult = new AgeRange { LowerBound = 18, UpperBound = 140 } 
                },
                ServiceSupplierId = 1,
                LocationId = 1,
                RateOptions = new RateOptions { SingleAdultAndChildBookings = SingleAdultAndChildBookings.ApplyAdultRate },
                Status = Status.Active,
                Created = _dateTimeProvider.UtcNow(),
                Modified = _dateTimeProvider.UtcNow()
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
                OccupancyDefinition = new OccupancyDefinition(),
                ServiceSupplierId = 1,
                LocationId = 1,
                RateOptions = new RateOptions { SingleAdultAndChildBookings = SingleAdultAndChildBookings.ApplyAdultRate },
                Status = Status.Active,
                Created = _dateTimeProvider.UtcNow(),
                Modified = _dateTimeProvider.UtcNow()
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
                OccupancyDefinition = new OccupancyDefinition(),
                ServiceSupplierId = 2,
                LocationId = 1,
                RateOptions = new RateOptions { SingleAdultAndChildBookings = SingleAdultAndChildBookings.ApplyAdultRate },
                Status = Status.Active,
                Created = _dateTimeProvider.UtcNow(),
                Modified = _dateTimeProvider.UtcNow()
            }
        };

        private readonly IEnumerable<Room> _rooms = new[]
        {
            new Room
            {
                Id = 1,
                AccommodationId = 1,
                Name = new MultiLanguage<string>(),
                Description = new MultiLanguage<string>(),
                Amenities = new MultiLanguage<List<string>> { En = new List<string>() },
                Created = _dateTimeProvider.UtcNow(),
                Modified = _dateTimeProvider.UtcNow(),
                OccupancyConfigurations = new List<OccupancyConfiguration>(),
                CancellationPolicies = new List<RoomCancellationPolicy>(),
                Images = new List<SlimImage>()
            },
            new Room
            {
                Id = 2,
                AccommodationId = 1,
                Name = new MultiLanguage<string>(),
                Description = new MultiLanguage<string>(),
                Amenities = new MultiLanguage<List<string>> { En = new List<string>() },
                Created = _dateTimeProvider.UtcNow(),
                Modified = _dateTimeProvider.UtcNow(),
                OccupancyConfigurations = new List<OccupancyConfiguration>(),
                CancellationPolicies = new List<RoomCancellationPolicy>(),
                Images = new List<SlimImage>()
            },
            new Room
            {
                Id = 3,
                AccommodationId = 2,
                Name = new MultiLanguage<string>(),
                Description = new MultiLanguage<string>(),
                Amenities = new MultiLanguage<List<string>> { En = new List<string>() },
                Created = _dateTimeProvider.UtcNow(),
                Modified = _dateTimeProvider.UtcNow(),
                OccupancyConfigurations = new List<OccupancyConfiguration>(),
                CancellationPolicies = new List<RoomCancellationPolicy>(),
                Images = new List<SlimImage>()
            }
        };


        public void Dispose() { }


        private static readonly IDateTimeProvider _dateTimeProvider = new DefaultDateTimeProvider();
        private readonly AccommodationManagementService _accommodationManagementService;
    }
}