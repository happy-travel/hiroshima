using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DbData;
using Hiroshima.DirectContracts.Models.RawAvailiability;
using Hiroshima.DirectContracts.Services.Availability.Helpers;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public class AvailabilityDetailsBuilder
    {
        public AvailabilityDetailsBuilder(DirectContractsDbContext dbContext, Language language)
        {
            _language = language;
            _dbContext = dbContext;
        }


        public AvailabilityDetailsBuilder GetQueryableAvailability(DateTime checkIndDate, DateTime checkOutDate)
        {
            _checkInDate = checkIndDate;
            _checkOutDate = checkOutDate;
            _rawAvailabilities = AvailabilityDbRequests.GetAvailability(_dbContext,
                checkIndDate, checkOutDate);
            return this;
        }


        public AvailabilityDetailsBuilder WithCoordinatesAndRadius(Point point, double radius)
        {
            if (!point.Equals(default))
                _rawAvailabilities = _rawAvailabilities.Where(InDbExecutionPredicates.FilterByCoordinatesAndDistance(point, radius));
            return this;
        }


        public AvailabilityDetailsBuilder WithRoomDetails(RoomDetails roomDetails)
        {
            if (!roomDetails.Equals(default))
                _rawAvailabilities = _rawAvailabilities.Where(InDbExecutionPredicates.FilterByRoomDetails(roomDetails));
            return this;
        }


        public AvailabilityDetailsBuilder WithAccommodationName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                _rawAvailabilities = _rawAvailabilities.Where(InDbExecutionPredicates.FilterByAccommodationName(name));
            return this;
        }


        public AvailabilityDetailsBuilder WithAccommodationLocality(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                _rawAvailabilities = _rawAvailabilities.Where(InDbExecutionPredicates.FilterByAccommodationLocality(name));
            return this;
        }


        public AvailabilityDetailsBuilder WithAccommodationCountry(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                _rawAvailabilities = _rawAvailabilities.Where(InDbExecutionPredicates.FilterByAccommodationCountry(name));
            return this;
        }


        public async Task<AvailabilityDetails> Build()
        {
            var rawAvailabilitiesData = await _rawAvailabilities.ToListAsync();

            if (rawAvailabilitiesData == null || !rawAvailabilitiesData.Any())
                return EmptyAvailabilityDetails;

            var groupedData = from queryRow in rawAvailabilitiesData
                              group queryRow by queryRow.Accommodation.Id
                into accommodations
                              from room in from accomItem in accommodations
                                           group accomItem by accomItem.Room.Id
                              group room by accommodations.Key;

            /* grouping result:
             * accommodation1-
             *               |-room1-
             *                      |- rawAvailabilityData1 (season1)
             *                      |- rawAvailabilityData2 (season2)
             *                      |- rawAvailabilityData3 (season1)
             */
            var slimAvailabilities = new List<SlimAvailabilityResult>();

            foreach (var rawAvailability in groupedData)
            {
                // rawAvailability it's a group with rooms for the current accommodation
                slimAvailabilities.Add(new SlimAvailabilityResult(
                     //get a first group than get the first item from the group
                     RawSlimAccommodationDetailsHelper.CreateSlimAccommodationDetails(rawAvailability.First().First(), _language),
                     RawAgreementHelper.CreateAgreements(rawAvailability, _checkInDate, _checkOutDate, _language)));
            }

            return new AvailabilityDetails(default,
                CalculateCountOfNights(_checkInDate, _checkOutDate),
                _checkInDate,
                _checkOutDate,
                slimAvailabilities);
        }


        public AvailabilityDetails EmptyAvailabilityDetails => new AvailabilityDetails(default,
            CalculateCountOfNights(_checkInDate, _checkOutDate),
            _checkInDate,
            _checkOutDate,
            EmptySlimAvailabilityResults);


        private static int CalculateCountOfNights(DateTime checkInDate, DateTime checkOutDate) =>
            checkInDate.Date <= checkOutDate.Date
                ? Convert.ToInt32(checkOutDate.Date.Subtract(checkInDate.Date).TotalDays)
                : 0;


        private DateTime _checkInDate;
        private DateTime _checkOutDate;
        private readonly Language _language;
        private readonly DirectContractsDbContext _dbContext;
        private IQueryable<RawAvailabilityData> _rawAvailabilities = Enumerable.Empty<RawAvailabilityData>().AsQueryable();
        private static readonly List<SlimAvailabilityResult> EmptySlimAvailabilityResults = new List<SlimAvailabilityResult>(0);
    }

}
