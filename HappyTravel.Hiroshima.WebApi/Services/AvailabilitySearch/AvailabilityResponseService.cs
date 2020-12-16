using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.EdoContracts.General;
using HappyTravel.EdoContracts.General.Enums;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models.Availabilities;
using Accommodation = HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation;
using Availability = HappyTravel.EdoContracts.Accommodations.Availability;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public class AvailabilityResponseService : IAvailabilityResponseService
    {
        public AvailabilityResponseService(IAccommodationResponseService accommodationResponseService)
        {
            _accommodationResponseService = accommodationResponseService;
        }


        public Availability Create(in AvailabilityRequest availabilityRequest, Common.Models.Availabilities.Availability availability, string languageCode)
        {
            var numberOfNights = CalculateNumberOfNights(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate);
            var numberOfProcessedAccommodations = availability.AvailableRates.Count;
            var slimAccommodationAvailabilities = CreateSlimAccommodationAvailabilities(availability.AvailableRates, languageCode); 
            
            return new Availability(availability.Id, numberOfNights, availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, slimAccommodationAvailabilities, numberOfProcessedAccommodations);
        }

        
        public AccommodationAvailability CreateAccommodationAvailability(in AvailabilityRequest availabilityRequest, Common.Models.Availabilities.Availability availability, string languageCode)
        {
            var accommodationAvailability = availability.AvailableRates.FirstOrDefault();
            var availabilityId = availability.Id;
            var numberOfNights = CalculateNumberOfNights(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate);
            var accommodation = CreateSlimAccommodationAvailability(accommodationAvailability.Key, accommodationAvailability.Value, languageCode);
            
            return new AccommodationAvailability(availabilityId, availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, numberOfNights, accommodation.Accommodation, accommodation.RoomContractSets);
        }


        public RoomContractSetAvailability Create(in AccommodationAvailability accommodationAvailability, Guid roomContractSetId)
        {
            var availabilityId = accommodationAvailability.AvailabilityId;
            var checkInDate = accommodationAvailability.CheckInDate;
            var checkOutDate = accommodationAvailability.CheckOutDate;
            var numberOfNights = CalculateNumberOfNights(checkInDate, checkOutDate);
            var accommodation = accommodationAvailability.Accommodation;
            var requiredRoomContractSet = accommodationAvailability.RoomContractSets.SingleOrDefault(roomContractSet => roomContractSet.Id.Equals(roomContractSetId));
            
            return new RoomContractSetAvailability(availabilityId, checkInDate, checkOutDate, numberOfNights, accommodation, requiredRoomContractSet);
        }
        
        
        public Availability CreateEmptyAvailability(in AvailabilityRequest availabilityRequest)
            => new Availability(string.Empty, CalculateNumberOfNights(availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date), availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, new List<SlimAccommodationAvailability>(), 0);
        
        
        public AccommodationAvailability CreateEmptyAccommodationAvailability(in AvailabilityRequest availabilityRequest) 
            => new AccommodationAvailability(string.Empty, availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, CalculateNumberOfNights(availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date), new SlimAccommodation(), new List<RoomContractSet>());
        

        private List<SlimAccommodationAvailability> CreateSlimAccommodationAvailabilities(Dictionary<Accommodation, List<AvailableRates>> accommodationAvailableRatesStore, string languageCode)
        {
            var slimAccommodationAvailabilities = new List<SlimAccommodationAvailability>();
                
            foreach (var accommodationAvailableRate in accommodationAvailableRatesStore)
            {
                var slimAccommodationAvailability = CreateSlimAccommodationAvailability(accommodationAvailableRate.Key, accommodationAvailableRate.Value, languageCode); 
                slimAccommodationAvailabilities.Add(slimAccommodationAvailability);
            }

            return slimAccommodationAvailabilities;
        }

        
        private SlimAccommodationAvailability CreateSlimAccommodationAvailability(Accommodation accommodation, List<AvailableRates> availableRates, string languageCode)
        {
            var slimAccommodation = _accommodationResponseService.Create(accommodation, languageCode);
            var roomContractSets = CreateRoomContractSets(availableRates, languageCode);
            var availabilityId = GenerateAvailabilityId();
                
            return new SlimAccommodationAvailability(slimAccommodation, roomContractSets, availabilityId);
        }
        

        private List<RoomContractSet> CreateRoomContractSets(List<AvailableRates> availableRates, string languageCode)
        {
            var roomContractSets = new List<RoomContractSet>();

            foreach (var availableRate in availableRates)
            {
                var roomContractSet = CreateRoomContractSet(availableRate, languageCode);
                roomContractSets.Add(roomContractSet);
            }

            return roomContractSets;
        }
        

        private RoomContractSet CreateRoomContractSet(AvailableRates availableRates, string languageCode)
        {
            var id = availableRates.Id;
            var roomContracts = CreateRoomContracts(availableRates.Rates, languageCode);
            var contractRate = CreateContractRate(availableRates.Rates);
            var deadline = CreateDeadline(roomContracts);
            var advancePurchaseRate = false;
            
            return new RoomContractSet(id, contractRate, deadline, roomContracts, advancePurchaseRate);
        }


        private List<RoomContract> CreateRoomContracts(List<RateDetails> rateDetails, string languageCode)
            => rateDetails.Select(rd => CreateRoomContract(rd, languageCode)).ToList();
        

        private RoomContract CreateRoomContract(in RateDetails rateDetails, string languageCode)
        {
            var boardBasis = rateDetails.BoardBasis;
            var mealPlan = rateDetails.MealPlan;
            var contractTypeCode = default(int);
            var isAvailableImmediately = false;
            var idDynamic = false;
            var contractDescription = rateDetails.Description;
            rateDetails.Room.Amenities.TryGetValue(languageCode, out var roomAmenities);
            var remarks = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string,string>($"{nameof(rateDetails.Room.Amenities)}", string.Join(", ", roomAmenities))
            };
            var dailyContractRates = CreateDailyContractRates(rateDetails.PaymentDetails);
            var totalContractRate = CreateContractRate(rateDetails.PaymentDetails, PriceTypes.Room);
            var adultsNumber = rateDetails.OccupationRequest.AdultsNumber;
            var childrenAges = rateDetails.OccupationRequest.ChildrenAges;
            var isExtraBedNeeded = rateDetails.OccupationRequest.IsExtraBedNeeded;
            var roomType = rateDetails.RoomType;
            var deadline = CreateDeadline(rateDetails.CancellationPolicies);
            var isAdvancePurchaseRate = false;
            
            return new RoomContract(boardBasis, mealPlan, contractTypeCode, isAvailableImmediately, idDynamic, contractDescription, remarks, dailyContractRates, totalContractRate, adultsNumber, childrenAges, roomType, isExtraBedNeeded, deadline, isAdvancePurchaseRate);
        }


        private Deadline CreateDeadline(List<CancellationPolicyDetails> cancellationPolicies)
        {
            if (!cancellationPolicies.Any())
                return new Deadline();
            
            var policies = cancellationPolicies
                .Select(cancellationPolicyDetail => new CancellationPolicy(cancellationPolicyDetail.FromDate, Convert.ToDouble(cancellationPolicyDetail.Percent))).ToList();
            
            return new Deadline(policies.First().FromDate, policies);
        }
        
        
        private Deadline CreateDeadline(List<RoomContract> roomContracts)
            => roomContracts.Select(roomContract => roomContract.Deadline).OrderBy(roomContract => roomContract.Date).FirstOrDefault();
        
        
        private Rate CreateContractRate(PaymentDetails paymentDetails, PriceTypes priceType)
        {
            var discounts = new List<Discount> {paymentDetails.Discount};
            
            return new Rate(paymentDetails.TotalAmount, paymentDetails.TotalAmount, discounts, priceType);           
        }
        
        
        private Rate CreateContractRate(List<RateDetails> rateDetails)
        {
            var price = PriceHelper.GetPrice(rateDetails);
            
            return new Rate(price.amount, price.amount, new List<Discount>{price.discount}, PriceTypes.RoomContractSet);           
        }
        
        
        private List<DailyRate> CreateDailyContractRates(in PaymentDetails paymentDetails) 
            => paymentDetails.SeasonPrices.SelectMany(seasonPriceDetails => seasonPriceDetails.DailyPrices)
            .OrderBy(dailyPrice => dailyPrice.FromDate)
            .Select(dailyPrice => new DailyRate(dailyPrice.FromDate, dailyPrice.ToDate, dailyPrice.DailyAmount, dailyPrice.DailyAmount))
            .ToList();


        private string GenerateAvailabilityId() => Guid.NewGuid().ToString("N");


        private int CalculateNumberOfNights(DateTime checkInDate, DateTime checkOutDate) => (checkOutDate.Date - checkInDate.Date).Days;

        
        private readonly IAccommodationResponseService _accommodationResponseService;
    }
}