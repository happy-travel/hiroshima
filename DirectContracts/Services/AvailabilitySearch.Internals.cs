using System;
using System.Collections.Generic;
using System.Linq;
using Hiroshima.DirectContracts.Models.Common;
using Hiroshima.DirectContracts.Models.Internal;
using Hiroshima.DirectContracts.Models.Responses;

namespace Hiroshima.DirectContracts.Services
{
    public partial class AvailabilitySearch
    {
        private DcAvailability CreateEmptyAvailabilityResponse(DateTime checkInDate, DateTime checkOutDate) =>
            new DcAvailability
            {
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate
            };

        private DcAvailability CreateResponse(
            List<AvailabilityQueryRow> rawQueryData,
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
             *                      |- row agreement1 (season1)
             *                      |- row agreement1 (season2)
             *                      |- row agreement2 (season1)
             */

            foreach (var rawAgreements in groupedData)
            {
                var accommodation = GetAccommodationData(rawAgreements.First().First());
                accommodation.Agreements = GetAgreementsData(rawAgreements);
                dcResponse.Accommodations.Add(accommodation);
            }

            return dcResponse;

            List<DcAgreement> GetAgreementsData(IGrouping<int, IGrouping<int, AvailabilityQueryRow>> rawAgreements)
            {
                var agreements = new List<DcAgreement>(rawAgreements.Count());
                foreach (var rawAgreement in rawAgreements)
                {
                    agreements.Add(GetAgreementData(rawAgreement.ToList()));
                }
                return agreements;

                DcAgreement GetAgreementData(List<AvailabilityQueryRow> queryRows)
                {
                    var commonData = queryRows.First();
                    return new DcAgreement
                    {
                        RoomName = commonData.RoomName,
                        DcPrice = CalculatePrice(queryRows.Select(i => new DcSeasonPrice
                        {
                            SeasonName = i.Season.Name,
                            StartDate = i.Season.StartDate,
                            EndDate = i.Season.EndDate,
                            Price = i.Price
                        }).ToList(), checkInDate, checkOutDate),
                        DcPermittedOccupancy = new DcPermittedOccupancy
                        {
                            AdultsCount = commonData.PermittedOccupancy.AdultsCount,
                            ChildrenCount = commonData.PermittedOccupancy.ChildrenCount
                        }
                    };
                }
            }

            DcAccommodation GetAccommodationData(AvailabilityQueryRow queryRow)
            {
                return new DcAccommodation
                {
                    Id = queryRow.AccommodationId,
                    Name = queryRow.AccommodationName,
                    Description = queryRow.AccommodationDescription,
                    Amenities = queryRow.AccommodationAmenities,
                    DcLocation = new DcLocation
                    {
                        Coordinates = queryRow.Coordinates,
                        Country = queryRow.Country,
                        Address = queryRow.Address,
                        Locality = queryRow.Locality
                    }
                };
            }
        }

        private static DcPrice CalculatePrice(List<DcSeasonPrice> seasons, DateTime checkInDate, DateTime checkOutDate)
        {
            if (checkInDate >= checkOutDate)
                throw new ArgumentException(
                    $"{nameof(checkInDate)}({checkInDate}) must not be more than {nameof(checkOutDate)}(checkOutDate)");
            var price = new DcPrice();
            if (!seasons.Any())
                return price;

            var startDate = checkInDate;
            var endDate = startDate;
            var day = endDate;
            var currentSeasonPrice = 0m;
            var currentSeasonIndex = 0;
            bool startNewSeason = true;

            var seasonsPrices = new List<DcSeasonPrice>();
            var currentSeason = new DcSeasonPrice();

            while (day.Date < checkOutDate.Date)
            {
                while (currentSeasonIndex < seasons.Count)
                {
                    var season = seasons[currentSeasonIndex];

                    var nextDay = day.AddDays(1);
                    if (Contain(season, nextDay))
                    {
                        if (startNewSeason)
                        {
                            startDate = startDate == endDate ? day : endDate.AddDays(1);
                            startNewSeason = false;
                        }
                        price.TotalPrice.SglPrice += season.Price.SglPrice;
                        currentSeasonPrice += season.Price.SglPrice;
                        currentSeason.Price.SglPrice += season.Price.SglPrice;

                        //last step
                        if (nextDay == checkOutDate)
                        {
                            endDate = nextDay;
                            currentSeason.SeasonName = season.SeasonName;
                            currentSeason.StartDate = startDate;
                            currentSeason.EndDate = endDate;
                            seasonsPrices.Add(currentSeason);
                        }
                        break;
                    }

                    startNewSeason = true;
                    endDate = day;

                    //season ends
                    if (currentSeasonPrice != 0)
                    {
                        currentSeason.SeasonName = season.SeasonName;
                        currentSeason.StartDate = startDate;
                        currentSeason.EndDate = endDate;
                        currentSeason.Price.SglPrice = currentSeasonPrice;
                        seasonsPrices.Add(currentSeason);
                    }

                    //clear before checking a new season
                    currentSeasonPrice = 0;
                    currentSeason = new DcSeasonPrice();
                    currentSeasonIndex++;
                }

                day = day.AddDays(1);
            }
            price.SeasonsPrices = seasonsPrices;
            return price;

            static bool Contain(DcSeasonPrice season, in DateTime day)
            {
                return day.Date >= season.StartDate.Date && day.Date <= season.EndDate.Date;
            }
        }
    }
}
