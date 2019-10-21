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


        public AvailabilityDetailsBuilder GetAvailability(DateTime checkIndDate, DateTime checkOutDate)
        {
            _checkInDate = checkIndDate;
            _checkOutDate = checkOutDate;
            _rawAvailabilities = AvailabilityDbRequests.GetAvailability(_dbContext,
                checkIndDate, checkOutDate);
            return this;
        }


        public AvailabilityDetailsBuilder FilterByCoordinates(Point point, double radius)
        {
            if (!point.Equals(default))
                _rawAvailabilities = _rawAvailabilities.Where(InDbExecutionPredicates.FilterByCoordinatesAndDistance(point, radius));
            return this;
        }


        public AvailabilityDetailsBuilder FilterByRoomDetails(RoomDetails roomDetails)
        {
            if (!roomDetails.Equals(default))
                _rawAvailabilities = _rawAvailabilities.Where(InDbExecutionPredicates.FilterByRoomDetails(roomDetails));
            return this;
        }


        public AvailabilityDetailsBuilder FilterByAccommodationName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                _rawAvailabilities = _rawAvailabilities.Where(InDbExecutionPredicates.SearchByAccommodationName(name));
            return this;
        }


        public AvailabilityDetailsBuilder FilterByAccommodationLocality(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                _rawAvailabilities = _rawAvailabilities.Where(InDbExecutionPredicates.SearchByAccommodationLocality(name));
            return this;
        }


        public AvailabilityDetailsBuilder FilterByAccommodationCountry(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                _rawAvailabilities = _rawAvailabilities.Where(InDbExecutionPredicates.SearchByAccommodationCountry(name));
            return this;
        }


        public async Task<AvailabilityDetails> Build()
        {
            if (!_rawAvailabilitiesData.Any())
                _rawAvailabilitiesData = await _rawAvailabilities.ToListAsync();
            return CreateAvailabilityDetails(_rawAvailabilitiesData);
        }


        private AvailabilityDetails CreateAvailabilityDetails(List<RawAvailability> rawAvailabilities)
        {
            if (rawAvailabilities == null || !rawAvailabilities.Any())
                return EmptyAvailabilityDetails;

            var groupedData = from queryRow in rawAvailabilities
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
                slimAvailabilities.Add(new SlimAvailabilityResult(
                     RawAvailabilityHelper.GetAccommodation(rawAvailability.First().First(), _language),
                     RawAvailabilityHelper.GetAgreements(rawAvailability, _checkInDate, _checkOutDate, _language)));
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
            Convert.ToInt32(checkOutDate.Date.Subtract(checkInDate.Date).TotalDays);


        private DateTime _checkInDate;
        private DateTime _checkOutDate;
        private readonly Language _language;
        private readonly DirectContractsDbContext _dbContext;
        private List<RawAvailability> _rawAvailabilitiesData = new List<RawAvailability>(0);
        private IQueryable<RawAvailability> _rawAvailabilities = Enumerable.Empty<RawAvailability>().AsQueryable();
        private static readonly List<SlimAvailabilityResult> EmptySlimAvailabilityResults = new List<SlimAvailabilityResult>(0);
    }

}
