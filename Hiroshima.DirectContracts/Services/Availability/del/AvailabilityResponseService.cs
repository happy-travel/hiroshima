using System;
using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DbData.Models.Accommodation;

//using RoomDetails = HappyTravel.EdoContracts.Accommodations.Internals.RoomDetails;

namespace Hiroshima.DirectContracts.Services.Availability.del
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
           /* return new AvailabilityDetails(default,
                CalculateNumberOfNights(checkInDate, checkOutDate),
                checkInDate,
                checkOutDate,*
                EmptySlimAvailabilityResults);*/
           throw new NotImplementedException();
        }


       /* public AvailabilityDetails GetAvailabilityDetails(DateTime checkInDate,
            DateTime checkOutDate,
            List<RawAvailabilityData> rawAvailabilityData,
            Languages languages)
        {
            if (!rawAvailabilityData.Any())
                return GetEmptyAvailabilityDetails(checkInDate, checkOutDate);

            var accommodations = RawAwailabilityDataHelper.OrganizeAvailabilityData(rawAvailabilityData);
            return CreateAvailabilityDetails(accommodations, checkInDate, checkOutDate, languages);
        }
*/

        private AvailabilityDetails CreateAvailabilityDetails(ICollection<Accommodation> accommodations,
            DateTime checkInDate, DateTime checkOutDate,
            Languages languages)
        {
            /*var slimAvailabilityResult = new List<SlimAvailabilityResult>(accommodations.Count);

            foreach (var accommodation in accommodations)
            {
                var accommodationDetails = CreateSlimAccommodationDetails(accommodation, languages);
                var agreements = CreateAgreements(accommodation.Rooms, checkInDate, checkOutDate, languages);
                slimAvailabilityResult.Add(new SlimAvailabilityResult(accommodationDetails, agreements));
            }

            return new AvailabilityDetails(default,
                CalculateNumberOfNights(checkInDate, checkOutDate),
                checkInDate,
                checkOutDate,
                slimAvailabilityResult);*/
            throw new NotImplementedException();
        }

/*
        private List<Agreement> CreateAgreements(ICollection<Room> rooms, DateTime checkInDate, DateTime checkOutDate,
            Languages languages)
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
                var contractType = room.Name.TryGetValue(languages);
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
*/
/*
        private SlimAccommodationDetails CreateSlimAccommodationDetails(Accommodation accommodation, Languages languages)
        {
            var accommodationId = accommodation.Id.ToString();
            var accommodationAmenities = GetAccommodationAmenities(accommodation, languages);
            var additionalInfo = GetAdditionalInfo(accommodation.AdditionalInfo, languages);
            var featureInfo = GetFeaturesInfo(accommodation.Features);
            var locationInfo = GetLocationInfo(accommodation.Location, languages);
            var picture = GetPicture(accommodation.Picture, languages);
            var roomAmenities = GetRoomAmenities(accommodation.RoomAmenities, languages);
            var textualDescription = GetTextualDescription(accommodation.TextualDescription, languages);

            return new SlimAccommodationDetails(
                accommodationId,
                accommodationAmenities,
                additionalInfo,
                featureInfo,
                locationInfo,
                accommodation.Name.TryGetValue(languages),
                picture,
                accommodation.Rating,
                roomAmenities,
                textualDescription,
                accommodation.PropertyType);
        }


        private List<string> GetAccommodationAmenities(Accommodation accommodation, Languages languages)
        {
            return accommodation.Amenities == null ||
                   !accommodation.Amenities.Any()
                ? EmptyStringList
                : accommodation.Amenities.Select(i => i.TryGetValue(languages)).ToList();
        }


        private List<FeatureInfo> GetFeaturesInfo(List<Common.Models.Accommodation.FeatureInfo> featuresInfo)
        {
            return featuresInfo?.Select(i => new FeatureInfo(i.Name, FieldTypes.Boolean)).ToList();
        }


        private Dictionary<string, string> GetAdditionalInfo(Dictionary<string, MultiLanguage<string>> additionalInfo,
            Languages languages)
        {
            return additionalInfo == null ||
                   !additionalInfo.Any()
                ? EmptyStringDictionary
                : additionalInfo.ToDictionary(k => k.Key,
                    v => v.Value.TryGetValue(languages));
        }


        private SlimLocationInfo GetLocationInfo(Location location, Languages languages)
        {
            return new SlimLocationInfo(location.Address.TryGetValue(languages),
                location.Locality.Country.Name.TryGetValue(languages),
                location.Locality.Name.TryGetValue(languages),
                string.Empty,
                new GeoPoint(location.Coordinates.X, location.Coordinates.Y));
        }


        private Picture GetPicture(Common.Models.Accommodation.Picture picture, Languages languages)
        {
            return picture == null
                ? default
                : new Picture(picture.Source,
                    picture.Caption.TryGetValue(languages));
        }


        private static List<string> GetRoomAmenities(List<MultiLanguage<string>> roomAmenities, Languages languages)
        {
            return roomAmenities == null ||
                   !roomAmenities.Any()
                ? EmptyStringList
                : roomAmenities.Select(i => i.TryGetValue(languages)).ToList();
        }


        private int CalculateNumberOfNights(DateTime checkInDate, DateTime checkOutDate)
        {
            return checkInDate.Date <= checkOutDate.Date
                ? Convert.ToInt32(checkOutDate.Date.Subtract(checkInDate.Date).TotalDays)
                : 0;
        }


        private TextualDescription GetTextualDescription(
            Common.Models.Accommodation.TextualDescription textualDescription, Languages languages)
        {
            return Equals(textualDescription, null)
                ? default
                : new TextualDescription(textualDescription.Type, textualDescription.Description.TryGetValue(languages));
        }


        private List<RoomDetails> GetRoomDetails(List<DbData.Models.Rooms.RoomDetails> roomDetails)
        {
            return roomDetails.Select(i => new RoomDetails(i.AdultsNumber, i.ChildrenNumber)).ToList();
        }
*/

        private static readonly List<string> EmptyStringList = new List<string>(0);
        private static readonly Dictionary<string, string> EmptyStringDictionary = new Dictionary<string, string>(0);

       /* private static readonly List<SlimAvailabilityResult> EmptySlimAvailabilityResults =
            new List<SlimAvailabilityResult>(0);
*/
        private readonly ICancelationPoliciesService _cancelationPoliciesService;


        private readonly IPricesService _pricesService;
    }
}