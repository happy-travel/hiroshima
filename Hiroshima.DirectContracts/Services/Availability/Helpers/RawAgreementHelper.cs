using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.EdoContracts.General;
using Hiroshima.Common.Models.Enums;
using Hiroshima.Common.Utils.Languages;
using Hiroshima.DirectContracts.Models.Internal;
using Hiroshima.DirectContracts.Models.RawAvailiability;
using DailyPrice = HappyTravel.EdoContracts.General.DailyPrice;

namespace Hiroshima.DirectContracts.Services.Availability.Helpers
{
    public static class RawAgreementHelper
    {
        public static List<Agreement> CreateAgreements(IGrouping<int, IGrouping<int, RawAvailabilityData>> groupedAvailabilities, in DateTime checkInDate, in DateTime checkOutDate, Language language)
        {
            var agreements = new List<Agreement>(groupedAvailabilities.Count());

            foreach (var availabilities in groupedAvailabilities)
            {
                agreements.Add(CreateAgreement(availabilities.ToList(), checkInDate, checkOutDate, language));
            }

            return agreements;
        }


        private static Agreement CreateAgreement(List<RawAvailabilityData> rawAvailabilities, DateTime checkInDate, DateTime checkOutDate, Common.Models.Enums.Language language)
        {
            var firstItem = rawAvailabilities.First();

            var agreementId = Guid.Empty;

            var roomName = firstItem.Room.Name.TryGetValue(language);

            var tariffCode = string.Empty;

            var boardBasis = string.Empty;

            var mealPlan = string.Empty;

            var deadlineDate = default(DateTime);

            var contractTypeId = default(int);

            var isAvailableImmediatly = false;

            var isDynamic = false;

            var isSpecial = false;

            var currencyCode = firstItem.ContractRate.CurrencyCode;

            var dailyPrice = GetDailyPrices(rawAvailabilities, checkInDate, checkOutDate);

            var totalPrice = dailyPrice.Sum(i => i.TotalPrice);
            var price = new Price(firstItem.ContractRate.CurrencyCode, totalPrice, totalPrice);
            var roomPrices = dailyPrice.Select(i =>
                new DailyPrice(i.StartDate, i.EndDate, currencyCode, i.TotalPrice, i.TotalPrice)).ToList();

            var roomDetails = GetRoomDetails(rawAvailabilities);

            var remarks = GetRemarks(rawAvailabilities);

            return new Agreement(agreementId, tariffCode, boardBasis, mealPlan, deadlineDate, contractTypeId,
                isAvailableImmediatly, isDynamic, isSpecial, price, roomPrices, roomDetails, roomName, remarks);
        }


        private static Dictionary<string, string> GetRemarks(List<RawAvailabilityData> rawAvailabilities)
        {
            return new Dictionary<string, string>(0);
        }


        private static List<RoomDetails> GetRoomDetails(List<RawAvailabilityData> rawAgreements)
        {

            var roomDetails = new List<RoomDetails>();
            var permittedOccupancyGroups = from rawAgreement in rawAgreements
                                           group rawAgreement by new
                                           { rawAgreement.PermittedOccupancy.AdultsNumber, rawAgreement.PermittedOccupancy.ChildrenNumber };

            foreach (var group in permittedOccupancyGroups)
            {
                var permittedOccupancy = group.First().PermittedOccupancy;
                roomDetails.Add(new RoomDetails(

                    permittedOccupancy.AdultsNumber,
                    permittedOccupancy.ChildrenNumber
                ));
            }

            return roomDetails;
        }


        private static List<SeasonDailyPrice> GetDailyPrices(List<RawAvailabilityData> rawAvailabilities, DateTime checkInDate, DateTime checkOutDate)
        {
            var priceResult = PriceHelper.GetSeasonDailyPrices(
                rawAvailabilities
                    .Select(i => new Season(i.Season.Name,
                        i.Season.StartDate,
                        i.Season.EndDate,
                        i.ContractRate.SeasonPrice)).ToList(),
                checkInDate,
                checkOutDate);

            return priceResult.IsSuccess
                ? priceResult.Value
                : EmptyDailyPrices;
        }


        private static readonly List<SeasonDailyPrice> EmptyDailyPrices = new List<SeasonDailyPrice>(0);
    }
}
