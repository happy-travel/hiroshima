using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using HappyTravel.Hiroshima.Data.Models.Rooms.CancellationPolicies;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityRepository
    {
        Task<List<AccommodationDetails>> GetAccommodations(string accommodationName, string languageCode);

        Task<List<AccommodationDetails>> GetAccommodations(string countryName, string locationName,
            string languageCode);

        Task<List<Room>> GetAvailableRooms(IEnumerable<int> accommodationIds, DateTime checkInDate,
            DateTime checkOutDate, string languageCode);

        Task<List<RoomRate>> GetRates(IEnumerable<int> roomIds, DateTime checkInDate, DateTime checkOutDate,
            string languageCode);

        Task<List<RoomPromotionalOffer>> GetPromotionalOffers(IEnumerable<int> roomIds, DateTime checkInDate, 
            DateTime checkOutDate, string languageCode);

        Task<List<RoomCancellationPolicy>> GetCancellationPolicies(IEnumerable<int> roomIds, DateTime checkInDate);
    }
}