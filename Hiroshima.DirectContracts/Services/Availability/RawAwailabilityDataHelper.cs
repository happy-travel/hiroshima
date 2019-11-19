using System.Collections.Generic;
using System.Linq;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Rates;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DirectContracts.Infrastructure.Utils;
using Hiroshima.DirectContracts.Models.RawAvailiability;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public static class RawAwailabilityDataHelper
    {
        public static List<Accommodation> OrganizeAvailabilityData(List<RawAvailabilityData> rawAvailabilityData)
        {
            var accommodationsDictionary = new Dictionary<int, Accommodation>();
            var seasonsDictionary = new Dictionary<int, Season>();
            var roomsDictionary = new Dictionary<int, Room>();
            var roomDetailsDictionary = new Dictionary<int, RoomDetails>();
            var contractRatesDictionary = new Dictionary<int, ContractedRate>();
            var discountRatesDictionary = new Dictionary<int, DiscountRate>();

            foreach (var availabilityData in rawAvailabilityData)
            {
                OrganizeAccommodations(availabilityData);
                OrganizeSeasons(availabilityData);
                OrganizeRooms(availabilityData);
                OrganizeRoomDetails(availabilityData);
                OrganizeContractRates(availabilityData);
                OrganizeDiscountRates(availabilityData);
            }

            return accommodationsDictionary.Values.ToList();


            void OrganizeAccommodations(RawAvailabilityData availabilityData)
            {
                var accommodation = availabilityData.Accommodation;

                if (!accommodationsDictionary.ContainsKey(accommodation.Id))
                {
                    var location = availabilityData.Location;
                    var locality = availabilityData.Locality;
                    var country = availabilityData.Country;

                    accommodation.Location = location;
                    location.Accommodation = accommodation;
                    location.Locality = locality;
                    locality.Country = country;

                    accommodationsDictionary.Add(accommodation.Id, accommodation);
                }
            }


            void OrganizeSeasons(RawAvailabilityData availabilityData)
            {
                var season = availabilityData.Season;

                if (!seasonsDictionary.ContainsKey(season.Id))
                {
                    var accommodation = accommodationsDictionary[season.AccommodationId];
                    season.Accommodation = accommodation;

                    if (accommodation.Seasons == null)
                        accommodation.Seasons = new List<Season> {season};
                    else
                        accommodation.Seasons.Add(season);

                    seasonsDictionary.Add(season.Id, season);

                    var cancelationPolicy = availabilityData.CancelationPolicy;
                    if (season.CancelationPolicy == null)
                        season.CancelationPolicy = cancelationPolicy;
                }
            }


            void OrganizeRooms(RawAvailabilityData availabilityData)
            {
                var room = availabilityData.Room;

                if (!roomsDictionary.ContainsKey(room.Id))
                {
                    var accommodation = accommodationsDictionary[room.AccommodationId];
                    room.Accommodation = accommodation;

                    if (accommodation.Rooms == null)
                        accommodation.Rooms = new List<Room> {room};
                    else
                        accommodation.Rooms.Add(room);

                    roomsDictionary.Add(room.Id, room);
                }
            }


            void OrganizeRoomDetails(RawAvailabilityData availabilityData)
            {
                var roomDetails = availabilityData.RoomDetails;

                if (!roomDetailsDictionary.ContainsKey(roomDetails.Id))
                {
                    var room = roomsDictionary[roomDetails.RoomId];
                    roomDetails.Room = room;

                    if (room.RoomDetails == null)
                        room.RoomDetails = new SortedSet<RoomDetails> {roomDetails};
                    else
                        room.RoomDetails.Add(roomDetails);

                    roomDetailsDictionary.Add(roomDetails.Id, roomDetails);
                }
            }


            void OrganizeContractRates(RawAvailabilityData availabilityData)
            {
                var contractedRate = availabilityData.ContractedRate;

                if (!contractRatesDictionary.ContainsKey(contractedRate.Id))
                {
                    var room = roomsDictionary[contractedRate.RoomId];
                    var season = seasonsDictionary[availabilityData.Season.Id];
                    contractedRate.Room = room;
                    contractedRate.Season = season;

                    if (room.ContractRates == null)
                        room.ContractRates = new SortedSet<ContractedRate>(Comparers.ContractedRate) {contractedRate};
                    else
                        room.ContractRates.Add(contractedRate);

                    contractRatesDictionary.Add(contractedRate.Id, contractedRate);
                }
            }


            void OrganizeDiscountRates(RawAvailabilityData availabilityData)
            {
                var discountRate = availabilityData.DiscountRate;

                if (!discountRatesDictionary.ContainsKey(discountRate.Id))
                {
                    var room = roomsDictionary[discountRate.RoomId];
                    discountRate.Room = room;

                    if (room.DiscountRates == null)
                        room.DiscountRates = new SortedSet<DiscountRate>(Comparers.DiscountRate) {discountRate};
                    else
                        room.DiscountRates.Add(discountRate);

                    discountRatesDictionary.Add(discountRate.Id, discountRate);
                }
            }
        }
    }
}