using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.EdoContracts.General;
using HappyTravel.EdoContracts.General.Enums;
using HappyTravel.Geography;
using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Enums;
using Hiroshima.Common.Utils.Languages;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Location;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DirectContracts.Models.RawAvailiability;
using RoomDetails = HappyTravel.EdoContracts.Accommodations.Internals.RoomDetails;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public class AvailabilityResponseService : IAvailabilityResponseService
    {
        public AvailabilityResponseService(IPricesService pricesService,
            ICancelationPoliciesService cancelationPoliciesService)
        {
            _pricesService = pricesService;
            _cancelationPoliciesService = cancelationPoliciesService;
        }


        public AvailabilityDetails GetEmptyAvailabilityDetails(DateTime checkInDate, DateTime checkOutDate)
        {
            return new AvailabilityDetails(default,
                CalculateNumberOfNights(checkInDate, checkOutDate),
                checkInDate,
                checkOutDate,
                EmptySlimAvailabilityResults);
        }


        public AvailabilityDetails GetAvailabilityDetails(DateTime checkInDate,
            DateTime checkOutDate,
            List<RawAvailabilityData> rawAvailabilityData,
            Language language)
        {
            if (!rawAvailabilityData.Any())
                return GetEmptyAvailabilityDetails(checkInDate, checkOutDate);

            var accommodations = RawAwailabilityDataHelper.OrganizeAvailabilityData(rawAvailabilityData);
            return CreateAvailabilityDetails(accommodations, checkInDate, checkOutDate, language);
        }


        private AvailabilityDetails CreateAvailabilityDetails(ICollection<Accommodation> accommodations,
            DateTime checkInDate, DateTime checkOutDate,
            Language language)
        {
            var slimAvailabilityResult = new List<SlimAvailabilityResult>(accommodations.Count);

            foreach (var accommodation in accommodations)
            {
                var accommodationDetails = CreateSlimAccommodationDetails(accommodation, language);
                var agreements = CreateAgreements(accommodation.Rooms, checkInDate, checkOutDate, language);
                slimAvailabilityResult.Add(new SlimAvailabilityResult(accommodationDetails, agreements));
            }

            return new AvailabilityDetails(default,
                CalculateNumberOfNights(checkInDate, checkOutDate),
                checkInDate,
                checkOutDate,
                slimAvailabilityResult);
        }


        private List<Agreement> CreateAgreements(ICollection<Room> rooms, DateTime checkInDate, DateTime checkOutDate,
            Language language)
        {
            var agreements = new List<Agreement>();
            foreach (var room in rooms)
            {
                var firstContractRate = room.ContractRates.First();
                var currencyCode = firstContractRate.CurrencyCode;
                var mealPlanCode = firstContractRate.MealPlanCode;
                var boardBasisCode = firstContractRate.BoardBasisCode;

                var agreementId = Guid.Empty;
                var tariffCode = string.Empty;

                var deadlineDate = _cancelationPoliciesService.GetDeadline(room, checkInDate);

                var dailyPrices = _pricesService.GetSeasonPrices(room.ContractRates, room.DiscountRates, checkInDate,
                    checkOutDate);

                var totalPrice = dailyPrices.Sum(i => i.TotalPrice);
                var price = new Price(currencyCode, totalPrice, totalPrice);
                var roomPrices = dailyPrices.Select(i =>
                    new DailyPrice(i.StartDate, i.EndDate, currencyCode, i.TotalPrice, i.TotalPrice)).ToList();

                var roomsDetails = GetRoomDetails(room.RoomDetails.ToList());
                var contractType = room.Name.TryGetValue(language);
                var remarks = new Dictionary<string, string>(0);

                agreements.Add(new Agreement(
                    agreementId,
                    tariffCode,
                    boardBasisCode,
                    mealPlanCode,
                    deadlineDate,
                    default,
                    false,
                    false,
                    false,
                    price,
                    roomPrices,
                    roomsDetails,
                    contractType,
                    remarks
                ));
            }

            return agreements;
        }


        private SlimAccommodationDetails CreateSlimAccommodationDetails(Accommodation accommodation, Language language)
        {
            var accommodationId = accommodation.Id.ToString();
            var accommodationAmenities = GetAccommodationAmenities(accommodation, language);
            var additionalInfo = GetAdditionalInfo(accommodation.AdditionalInfo, language);
            var featureInfo = GetFeaturesInfo(accommodation.Features);
            var locationInfo = GetLocationInfo(accommodation.Location, language);
            var picture = GetPicture(accommodation.Picture, language);
            var roomAmenities = GetRoomAmenities(accommodation.RoomAmenities, language);
            var textualDescription = GetTextualDescription(accommodation.TextualDescription, language);

            return new SlimAccommodationDetails(
                accommodationId,
                accommodationAmenities,
                additionalInfo,
                featureInfo,
                locationInfo,
                accommodation.Name.TryGetValue(language),
                picture,
                accommodation.Rating,
                roomAmenities,
                textualDescription,
                accommodation.PropertyType);
        }


        private List<string> GetAccommodationAmenities(Accommodation accommodation, Language language)
        {
            return accommodation.Amenities == null ||
                   !accommodation.Amenities.Any()
                ? EmptyStringList
                : accommodation.Amenities.Select(i => i.TryGetValue(language)).ToList();
        }


        private List<FeatureInfo> GetFeaturesInfo(List<Common.Models.Accommodation.FeatureInfo> featuresInfo)
        {
            return featuresInfo?.Select(i => new FeatureInfo(i.Name, FieldTypes.Boolean)).ToList();
        }


        private Dictionary<string, string> GetAdditionalInfo(Dictionary<string, MultiLanguage<string>> additionalInfo,
            Language language)
        {
            return additionalInfo == null ||
                   !additionalInfo.Any()
                ? EmptyStringDictionary
                : additionalInfo.ToDictionary(k => k.Key,
                    v => v.Value.TryGetValue(language));
        }


        private SlimLocationInfo GetLocationInfo(Location location, Language language)
        {
            return new SlimLocationInfo(location.Address.TryGetValue(language),
                location.Locality.Country.Name.TryGetValue(language),
                location.Locality.Name.TryGetValue(language),
                string.Empty,
                new GeoPoint(location.Coordinates.X, location.Coordinates.Y));
        }


        private Picture GetPicture(Common.Models.Accommodation.Picture picture, Language language)
        {
            return picture == null
                ? default
                : new Picture(picture.Source,
                    picture.Caption.TryGetValue(language));
        }


        private static List<string> GetRoomAmenities(List<MultiLanguage<string>> roomAmenities, Language language)
        {
            return roomAmenities == null ||
                   !roomAmenities.Any()
                ? EmptyStringList
                : roomAmenities.Select(i => i.TryGetValue(language)).ToList();
        }


        private int CalculateNumberOfNights(DateTime checkInDate, DateTime checkOutDate)
        {
            return checkInDate.Date <= checkOutDate.Date
                ? Convert.ToInt32(checkOutDate.Date.Subtract(checkInDate.Date).TotalDays)
                : 0;
        }


        private TextualDescription GetTextualDescription(
            Common.Models.Accommodation.TextualDescription textualDescription, Language language)
        {
            return Equals(textualDescription, null)
                ? default
                : new TextualDescription(textualDescription.Type, textualDescription.Description.TryGetValue(language));
        }


        private List<RoomDetails> GetRoomDetails(List<DbData.Models.Rooms.RoomDetails> roomDetails)
        {
            return roomDetails.Select(i => new RoomDetails(i.AdultsNumber, i.ChildrenNumber)).ToList();
        }


        private static readonly List<string> EmptyStringList = new List<string>(0);
        private static readonly Dictionary<string, string> EmptyStringDictionary = new Dictionary<string, string>(0);

        private static readonly List<SlimAvailabilityResult> EmptySlimAvailabilityResults =
            new List<SlimAvailabilityResult>(0);

        private readonly ICancelationPoliciesService _cancelationPoliciesService;


        private readonly IPricesService _pricesService;
    }
}