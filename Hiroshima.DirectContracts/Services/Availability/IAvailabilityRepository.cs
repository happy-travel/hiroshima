using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hiroshima.DbData.Models;
using Hiroshima.DbData.Models.Room;
using Hiroshima.DbData.Models.Room.CancellationPolicies;

namespace Hiroshima.DirectContracts.Services.Availability
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
            string languageCode);

        Task<List<RoomCancellationPolicy>> GetCancellationPolicies(IEnumerable<int> roomIds, DateTime checkInDate);
    }
}