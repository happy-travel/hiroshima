using System;
using System.Collections.Generic;
using System.Linq;
using Hiroshima.Common.Models;
using Hiroshima.DirectContracts.Models;
using Hiroshima.DirectContracts.Models.Internal;

namespace Hiroshima.DirectContracts.Services.Availability.Helpers
{
    class ResponseCreator
    {
        public AvailabilityResponse CreateAvailabilityResponse(
           List<RawAgreementData> rawQueryData,
           DateTime checkInDate,
           DateTime checkOutDate)
        {
            var dcResponse = CreateEmptyAvailabilityResponse(checkInDate, checkOutDate);
            if (rawQueryData == null || !rawQueryData.Any())
                return dcResponse;

            var groupedData = from queryRow in rawQueryData
                              group queryRow by queryRow.AccommodationId
                                        into accommodations
                              from room in from accomItem in accommodations
                                           group accomItem by accomItem.RoomId
                              group room by accommodations.Key;

            /* grouping result:
             * accommodation1-
             *               |-room1-
             *                      |- rawAgreementData1 (season1)
             *                      |- rawAgreementData2 (season2)
             *                      |- rawAgreementData3 (season1)
             */

            foreach (var rawAgreements in groupedData)
            {
                var accommodation = GetAccommodationData(rawAgreements.First().First());
                accommodation.Agreements = GetAgreementsData(rawAgreements);
                dcResponse.Accommodations.Add(accommodation);
            }
            return dcResponse;

            Accommodation GetAccommodationData(RawAgreementData queryRow)
            {
                return new Accommodation
                {
                    Id = queryRow.AccommodationId,
                    Name = queryRow.AccommodationName,
                    Description = queryRow.AccommodationDescription,
                    Amenities = queryRow.AccommodationAmenities,
                    Location = new Location
                    {
                        Coordinates = queryRow.Coordinates,
                        Country = queryRow.Country,
                        Address = queryRow.Address,
                        Locality = queryRow.Locality
                    }
                };
            }

            List<Agreement> GetAgreementsData(IGrouping<int, IGrouping<int, RawAgreementData>> rawAgreements)
            {
                var agreements = new List<Agreement>(rawAgreements.Count());
                foreach (var rawAgreement in rawAgreements)
                {
                    agreements.Add(GetAgreementData(rawAgreement.ToList()));
                }
                return agreements;
            }

            Agreement GetAgreementData(List<RawAgreementData> queryRows)
            {
                var commonData = queryRows.First();
                return new Agreement
                {
                    RoomName = commonData.RoomName,
                    Price = ResponseHelper.GetPrice(queryRows.Select(i => new SeasonPrice
                    {
                        SeasonName = i.Season.Name,
                        StartDate = i.Season.StartDate,
                        EndDate = i.Season.EndDate,
                        Price = i.Price
                    }).ToList(), checkInDate, checkOutDate),
                    PermittedOccupancy = new PermittedOccupancy
                    {
                        AdultsNumber = commonData.PermittedOccupancy.AdultsNumber,
                        ChildrenNumber = commonData.PermittedOccupancy.ChildrenNumber
                    }
                };
            }
        }


        private AvailabilityResponse CreateEmptyAvailabilityResponse(DateTime checkInDate, DateTime checkOutDate) =>
            new AvailabilityResponse
            {
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate
            };
    }
}
