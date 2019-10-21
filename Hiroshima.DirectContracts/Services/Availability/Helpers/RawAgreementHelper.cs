using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.EdoContracts.General;
using Hiroshima.Common.Utils.Languages;
using Hiroshima.DirectContracts.Models.Internal;
using Hiroshima.DirectContracts.Models.RawAvailiability;
using DailyPrice = HappyTravel.EdoContracts.General.DailyPrice;

namespace Hiroshima.DirectContracts.Services.Availability.Helpers
{
    public static class RawAgreementHelper
    {
        public static Agreement CreateAgreement(List<RawAvailability> rawAvailabilities, DateTime checkInDate, DateTime checkOutDate, Common.Models.Enums.Language language)
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

            var currencyCode = firstItem.Rate.CurrencyCode;

            var dailyPrice = GetDailyPrices(rawAvailabilities, checkInDate, checkOutDate);

            var totalPrice = dailyPrice.Sum(i => i.TotalPrice);
            var price = new Price(firstItem.Rate.CurrencyCode, totalPrice, totalPrice);
            var roomPrices = dailyPrice.Select(i =>
                new DailyPrice(i.StartDate, i.EndDate, currencyCode, i.TotalPrice, i.TotalPrice)).ToList();

            var roomDetails = GetRoomDetails(rawAvailabilities);
            
            var remarks = GetRemarks(rawAvailabilities);

            return new Agreement(agreementId, tariffCode, boardBasis, mealPlan, deadlineDate, contractTypeId,
                isAvailableImmediatly, isDynamic, isSpecial, price, roomPrices, roomDetails, roomName, remarks);
        }
        
        
        private static Dictionary<string, string> GetRemarks(List<RawAvailability> rawAvailabilities)
        {
            return new Dictionary<string, string>(0);
        }


        private static List<RoomDetails> GetRoomDetails(List<RawAvailability> rawAgreements)
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


        private static List<SeasonDailyPrice> GetDailyPrices(List<RawAvailability> rawAvailabilities, DateTime checkInDate, DateTime checkOutDate)
        {
            var price = PriceHelper.GetSeasonDailyPrices(
                 rawAvailabilities
                     .Select(i => new Season(i.Season.Name,
                         i.Season.StartDate,
                         i.Season.EndDate,
                         i.Rate.Price)).ToList(),
                 checkInDate,
                 checkOutDate);
            return price;
        }

    }
}
