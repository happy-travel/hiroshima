using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.EdoContracts.General;
using HappyTravel.EdoContracts.General.Enums;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Money.Helpers;
using HappyTravel.Money.Models;
using Accommodation = HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public class AvailabilityResponseService : IAvailabilityResponseService
    {
        public AvailabilityResponseService(IAccommodationResponseService accommodationResponseService)
        {
            _accommodationResponseService = accommodationResponseService;
        }


        public Availability Create(in AvailabilityRequest availabilityRequest, Dictionary<Accommodation, List<AvailableRates>> accommodationAvailableRatesStore, string languageCode)
        {
            var availabilityId = GenerateAvailabilityId();
            var numberOfNights = CalculateNumberOfNights(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate);
            var numberOfProcessedAccommodations = accommodationAvailableRatesStore.Count;
            var slimAccommodationAvailabilities = CreateSlimAccommodationAvailabilities(accommodationAvailableRatesStore, languageCode); 
            
            return new Availability(availabilityId, numberOfNights, availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, slimAccommodationAvailabilities, numberOfProcessedAccommodations);
        }

        
        public AccommodationAvailability Create(in AvailabilityRequest availabilityRequest, KeyValuePair<Accommodation, List<AvailableRates>> accommodationWithAvailableRates, string languageCode)
        {
            var availabilityId = GenerateAvailabilityId();
            var numberOfNights = CalculateNumberOfNights(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate);
            var accommodation = CreateSlimAccommodationAvailability(accommodationWithAvailableRates.Key, accommodationWithAvailableRates.Value, languageCode);
            
            return new AccommodationAvailability(availabilityId, availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, numberOfNights, accommodation.Accommodation, accommodation.RoomContractSets);
        }


        public RoomContractSetAvailability Create(in AccommodationAvailability accommodationAvailability, Guid roomContractSetId)
        {
            var availabilityId = GenerateAvailabilityId();
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
            var roomContractSets = CreateRoomContractSets(availableRates);
            var availabilityId = GenerateAvailabilityId();
                
            return new SlimAccommodationAvailability(slimAccommodation, roomContractSets, availabilityId);
        }
        

        private List<RoomContractSet> CreateRoomContractSets(List<AvailableRates> availableRates)
        {
            var roomContractSets = new List<RoomContractSet>();

            foreach (var availableRate in availableRates)
            {
                var roomContractSet = CreateRoomContractSet(availableRate.Rates);
                roomContractSets.Add(roomContractSet);
            }

            return roomContractSets;
        }
        

        private RoomContractSet CreateRoomContractSet(List<RateDetails> rateDetails)
        {
            var id = Guid.NewGuid();
            var roomContracts = CreateRoomContracts(rateDetails);
            var price = CreatePrice(rateDetails);
            var deadline = CreateDeadline(roomContracts);
            var advancePurchaseRate = false;
            
            return new RoomContractSet(id, price, deadline, roomContracts, advancePurchaseRate);
        }


        private List<RoomContract> CreateRoomContracts(List<RateDetails> rateDetails)
            => rateDetails.Select(CreateRoomContract).ToList();
        

        private RoomContract CreateRoomContract(RateDetails rateDetails)
        {
            var boardBasis = rateDetails.BoardBasis;
            var mealPlan = rateDetails.MealPlan;
            var contractTypeCode = default(int);
            var isAvailableImmediately = false;
            var idDynamic = false;
            var contractDescription = rateDetails.Description;
            var remarks = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string,string>($"{nameof(rateDetails.Amenities)}", string.Join(", ", rateDetails.Amenities))
            };
            var dailyPrices = CreateDailyPrices(rateDetails.PaymentDetails);
            var totalPrice = CreatePrice(rateDetails.PaymentDetails, PriceTypes.Room);
            var adultsNumber = rateDetails.OccupationRequest.AdultsNumber;
            var childrenAges = rateDetails.OccupationRequest.ChildrenAges;
            var isExtraBedNeeded = rateDetails.OccupationRequest.IsExtraBedNeeded;
            var roomType = rateDetails.RoomType;
            var deadline = CreateDeadline(rateDetails.CancellationPolicies);
            var isAdvancePurchaseRate = false;
            
            return new RoomContract(boardBasis, mealPlan, contractTypeCode, isAvailableImmediately, idDynamic, contractDescription, remarks, dailyPrices, totalPrice, adultsNumber, childrenAges, roomType, isExtraBedNeeded, deadline, isAdvancePurchaseRate);
        }


        private Deadline CreateDeadline(List<CancellationPolicyDetails> cancellationPolicies)
        {
            if (!cancellationPolicies.Any())
                return new Deadline();
            
            var policies = cancellationPolicies
                .Select(cancellationPolicyDetail => new CancellationPolicy(cancellationPolicyDetail.StartDate, Convert.ToDouble(cancellationPolicyDetail.Percent))).ToList();
            
            return new Deadline(policies.First().FromDate, policies);
        }
        
        
        private Deadline CreateDeadline(List<RoomContract> roomContracts)
            => roomContracts.Select(roomContract => roomContract.Deadline).OrderBy(roomContract => roomContract.Date).FirstOrDefault();
        
        
        private Price CreatePrice(PaymentDetails paymentDetails, PriceTypes priceType)
        {
            var discounts = new List<Discount> {paymentDetails.Discount};
            
            return new Price(paymentDetails.TotalAmount, paymentDetails.TotalAmount, discounts, priceType);           
        }
        
        
        private Price CreatePrice(List<RateDetails> rateDetails)
        {
            var firstRateDetails = rateDetails.First();
            var currency = firstRateDetails.PaymentDetails.TotalAmount.Currency;
            var totalPrice = rateDetails.Sum(rateDetailsItem => rateDetailsItem.PaymentDetails.TotalAmount.Amount);

            var totalPriceWithDiscount = rateDetails.Sum(rateDetailsItem
                => rateDetailsItem.PaymentDetails.TotalAmount.Amount - rateDetailsItem.PaymentDetails.TotalAmount.Amount * rateDetailsItem.PaymentDetails.Discount.Percent / 100);
            var totalDiscount = new Discount(MoneyRounder.Truncate(100 - totalPriceWithDiscount * 100 / totalPrice, currency));
            
            var moneyAmount = new MoneyAmount(totalPrice, currency);
            
            return new Price(moneyAmount, moneyAmount, new List<Discount>{totalDiscount}, PriceTypes.RoomContractSet);           
        }
        
        
        private List<DailyPrice> CreateDailyPrices(in PaymentDetails paymentDetails) 
            => paymentDetails.SeasonPrices.SelectMany(seasonPriceDetails => seasonPriceDetails.DailyPrices)
            .OrderBy(dailyPrice => dailyPrice.FromDate)
            .Select(dailyPrice => new DailyPrice(dailyPrice.FromDate, dailyPrice.ToDate, dailyPrice.DailyAmount, dailyPrice.DailyAmount))
            .ToList();


        private string GenerateAvailabilityId() => Guid.NewGuid().ToString("N");


        private int CalculateNumberOfNights(DateTime checkInDate, DateTime checkOutDate) => (checkOutDate.Date - checkInDate.Date).Days;

        
        private readonly IAccommodationResponseService _accommodationResponseService;
    }
}