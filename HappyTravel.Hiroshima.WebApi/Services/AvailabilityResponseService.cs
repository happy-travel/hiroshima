using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.EdoContracts.General;
using HappyTravel.EdoContracts.General.Enums;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Money.Models;
using Accommodation = HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public class AvailabilityResponseService : IAvailabilityResponseService
    {
        public AvailabilityResponseService(IAccommodationResponseService accommodationResponseService)
        {
            _accommodationResponseService = accommodationResponseService;
        }


        public Availability Create(AvailabilityRequest availabilityRequest, Dictionary<Accommodation, List<AvailableRates>> accommodationAvailableRatesStore, string languageCode)
        {
            var availabilityId = CreateAvailabilityId();
            var numberOfNights = GetNumberOfNights(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate);
            var numberOfProcessedAccommodations = accommodationAvailableRatesStore.Count;
            var slimAccommodationAvailabilities = CreateSlimAccommodationAvailabilities(accommodationAvailableRatesStore, languageCode); 
            
            return new Availability(availabilityId, numberOfNights, availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, slimAccommodationAvailabilities, numberOfProcessedAccommodations);
        }
        

        private List<SlimAccommodationAvailability> CreateSlimAccommodationAvailabilities(Dictionary<Accommodation, List<AvailableRates>> accommodationAvailableRatesStore, string languageCode)
        {
            var slimAccommodationAvailabilities = new List<SlimAccommodationAvailability>();
                
            foreach (var accommodationAvailableRate in accommodationAvailableRatesStore)
            {
                var slimAccommodationAvailability = CreateSlimAccommodationAvailability(accommodationAvailableRate.Key, accommodationAvailableRate.Value); 
                slimAccommodationAvailabilities.Add(slimAccommodationAvailability);
            }

            return slimAccommodationAvailabilities;


            SlimAccommodationAvailability CreateSlimAccommodationAvailability(Accommodation accommodation, List<AvailableRates> availableRates)
            {
                var slimAccommodation = _accommodationResponseService.Create(accommodation, languageCode);
                var roomContractSets = CreateRoomContractSets(availableRates);
                var availabilityId = CreateAvailabilityId();
                
                return new SlimAccommodationAvailability(slimAccommodation, roomContractSets, availabilityId);
            }
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
            var discounts = rateDetails.Select(rateDetailsItem => rateDetailsItem.PaymentDetails.Discount).ToList();
            var moneyAmount = new MoneyAmount(totalPrice, currency);
            
            return new Price(moneyAmount, moneyAmount, discounts, PriceTypes.RoomContractSet);           
        }
        
        
        private List<DailyPrice> CreateDailyPrices(in PaymentDetails paymentDetails) 
            => paymentDetails.SeasonPrices.SelectMany(seasonPriceDetails => seasonPriceDetails.DailyPrices)
            .OrderBy(dailyPrice => dailyPrice.FromDate)
            .Select(dailyPrice => new DailyPrice(dailyPrice.FromDate, dailyPrice.ToDate, dailyPrice.DailyAmount, dailyPrice.DailyAmount))
            .ToList();


        private string CreateAvailabilityId() => Guid.NewGuid().ToString("N");


        private int GetNumberOfNights(DateTime checkInDate, DateTime checkOutDate) => (checkOutDate.Date - checkInDate.Date).Days;

        
        private readonly IAccommodationResponseService _accommodationResponseService;
    }
}