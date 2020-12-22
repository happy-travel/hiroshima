using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Locations;
using HappyTravel.Hiroshima.Common.Models.Seasons;
using HappyTravel.Hiroshima.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace HappyTravel.Hiroshima.DirectContracts.Extensions
{
    public static class AvailabilityQueryExtensions
    {
        public static IIncludableQueryable<ContractAccommodationRelation, SeasonRange>
            IncludeAllocationRequirements(this DbSet<ContractAccommodationRelation> contractAccommodationRelations, AvailabilityRequest availabilityRequest)
            => contractAccommodationRelations.Include(relation => relation.Accommodation)
                .ThenInclude(accommodation => accommodation.Rooms.Where(room => !room.AvailabilityRestrictions.Any(availabilityRestrictions
                    => availabilityRequest.CheckInDate <= availabilityRestrictions.ToDate &&
                    availabilityRestrictions.FromDate <= availabilityRequest.CheckOutDate &&
                    availabilityRestrictions.Restriction == AvailabilityRestrictions.StopSale)))
                .ThenInclude(room => room.AllocationRequirements.Where(allocationRequirements
                    => availabilityRequest.CheckInDate <= allocationRequirements.SeasonRange.EndDate &&
                    allocationRequirements.SeasonRange.StartDate <= availabilityRequest.CheckOutDate))
                .ThenInclude(allocationRequirements => allocationRequirements.SeasonRange);


        public static IIncludableQueryable<ContractAccommodationRelation, IEnumerable<RoomAvailabilityRestriction>>
            IncludeAvailabilityRestrictions(this IIncludableQueryable<ContractAccommodationRelation, SeasonRange> queryable,
                AvailabilityRequest availabilityRequest)
            => queryable.Include(relation => relation.Accommodation)
                .ThenInclude(accommodation => accommodation.Rooms)
                .ThenInclude(room => room.AvailabilityRestrictions.Where(availabilityRestrictions
                    => availabilityRequest.CheckInDate <= availabilityRestrictions.ToDate &&
                    availabilityRestrictions.FromDate <= availabilityRequest.CheckOutDate));


        public static IIncludableQueryable<ContractAccommodationRelation, IEnumerable<SeasonRange>> IncludeCancellationPolicies(
            this IIncludableQueryable<ContractAccommodationRelation, IEnumerable<RoomAvailabilityRestriction>> queryable,
            AvailabilityRequest availabilityRequest)
            => queryable.Include(accommodation => accommodation.Accommodation)
                .ThenInclude(accommodation => accommodation.Rooms)
                .ThenInclude(room => room.CancellationPolicies.Where(cancellationPolicy => cancellationPolicy.Season.SeasonRanges.Any(seasonRange
                    => availabilityRequest.CheckInDate <= seasonRange.EndDate && seasonRange.StartDate <= availabilityRequest.CheckOutDate)))
                .ThenInclude(cancellationPolicy => cancellationPolicy.Season)
                .ThenInclude(season => season.SeasonRanges.Where(seasonRange
                    => availabilityRequest.CheckInDate <= seasonRange.EndDate && seasonRange.StartDate <= availabilityRequest.CheckOutDate));


        public static IIncludableQueryable<ContractAccommodationRelation, IEnumerable<SeasonRange>> IncludeRates(
            this IIncludableQueryable<ContractAccommodationRelation, IEnumerable<SeasonRange>> queryable, AvailabilityRequest availabilityRequest,
            List<RoomTypes> roomTypes)
            => queryable.Include(relation => relation.Accommodation)
                .ThenInclude(accommodation => accommodation.Rooms)
                .ThenInclude(room => room.RoomRates.Where(rate => roomTypes.Contains(rate.RoomType) && rate.Season.SeasonRanges.Any(seasonRange
                    => availabilityRequest.CheckInDate <= seasonRange.EndDate && seasonRange.StartDate <= availabilityRequest.CheckOutDate)))
                .ThenInclude(rate => rate.Season)
                .ThenInclude(season => season.SeasonRanges.Where(seasonRange
                    => availabilityRequest.CheckInDate <= seasonRange.EndDate && seasonRange.StartDate <= availabilityRequest.CheckOutDate));


        public static IIncludableQueryable<ContractAccommodationRelation, IEnumerable<RoomPromotionalOffer>> IncludePromotionalOffers(
            this IIncludableQueryable<ContractAccommodationRelation, IEnumerable<SeasonRange>> queryable, AvailabilityRequest availabilityRequest)
            => queryable.Include(relation => relation.Accommodation)
                .ThenInclude(accommodation => accommodation.Rooms)
                .ThenInclude(room => room.RoomPromotionalOffers.Where(promotionalOffer
                    => availabilityRequest.CheckInDate <= promotionalOffer.ValidToDate && promotionalOffer.ValidFromDate <= availabilityRequest.CheckOutDate));


        public static IIncludableQueryable<ContractAccommodationRelation, IEnumerable<RoomOccupancy>> IncludeRoomOccupations(
            this IIncludableQueryable<ContractAccommodationRelation, IEnumerable<RoomPromotionalOffer>> queryable, AvailabilityRequest availabilityRequest)
            => queryable.Include(relation => relation.Accommodation)
                .ThenInclude(accommodation => accommodation.Rooms)
                .ThenInclude(room => room.RoomOccupations.Where(roomOccupation
                    => availabilityRequest.CheckInDate <= roomOccupation.FromDate && roomOccupation.ToDate <= availabilityRequest.CheckOutDate));


        public static IIncludableQueryable<ContractAccommodationRelation, Country> IncludeLocation(
            this IIncludableQueryable<ContractAccommodationRelation, IEnumerable<RoomOccupancy>> queryable)
            => queryable.Include(relation => relation.Accommodation)
                .ThenInclude(accommodation => accommodation.Location)
                .ThenInclude(location => location.Country);
    }
}