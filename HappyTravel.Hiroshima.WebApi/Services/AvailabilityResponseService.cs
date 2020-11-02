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


        public EdoContracts.Accommodations.Availability Create(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, Dictionary<Accommodation, List<AvailableRates>> accommodationAvailableRatesStore, string languageCode)
        {
            var availabilityId = CreateAvailabilityId();
            var numberOfNights = GetNumberOfNights(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate);
            var numberOfProcessedAccommodations = accommodationAvailableRatesStore.Count;
            var slimAccommodationAvailabilities = CreateSlimAccommodationAvailabilities(accommodationAvailableRatesStore, languageCode); 
            
            return new EdoContracts.Accommodations.Availability(availabilityId, numberOfNights, availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, slimAccommodationAvailabilities, numberOfProcessedAccommodations);
        }


        private List<EdoContracts.Accommodations.Internals.SlimAccommodationAvailability> CreateSlimAccommodationAvailabilities(Dictionary<Accommodation, List<AvailableRates>> accommodationAvailableRatesStore, string languageCode)
        {
            var slimAccommodationAvailabilities = new List<EdoContracts.Accommodations.Internals.SlimAccommodationAvailability>();
                
            foreach (var accommodationAvailableRate in accommodationAvailableRatesStore)
            {
                var slimAccommodationAvailability = CreateSlimAccommodationAvailability(accommodationAvailableRate.Key, accommodationAvailableRate.Value); 
                slimAccommodationAvailabilities.Add(slimAccommodationAvailability);
            }

            return slimAccommodationAvailabilities;


            EdoContracts.Accommodations.Internals.SlimAccommodationAvailability CreateSlimAccommodationAvailability(Accommodation accommodation, List<AvailableRates> availableRates)
            {
                var slimAccommodation = _accommodationResponseService.Create(accommodation, languageCode);
                var roomContractSets = Create(availableRates);
                var availabilityId = CreateAvailabilityId();
                
                return new EdoContracts.Accommodations.Internals.SlimAccommodationAvailability(slimAccommodation, roomContractSets, availabilityId);
            }
        }

        
        public List<RoomContractSet> Create(List<AvailableRates> availableRates)
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
                .Select(cancellationPolicyDetail => new CancellationPolicy(cancellationPolicyDetail.StartDate, cancellationPolicyDetail.Percent)).ToList();
            
            return new Deadline(policies.First().FromDate, policies);
        }
        
        
        private Deadline CreateDeadline(List<RoomContract> roomContracts)
            => roomContracts.Select(roomContract => roomContract.Deadline).OrderBy(roomContract => roomContract.Date).FirstOrDefault();
        
        
        private Price CreatePrice(PaymentDetails paymentDetails, PriceTypes priceType)
        {
            var moneyAmount = new MoneyAmount(paymentDetails.TotalPrice, paymentDetails.Currency);
            var discounts = new List<Discount> {new Discount(Convert.ToDecimal(paymentDetails.DiscountPercent))};
            
            return new Price(moneyAmount, moneyAmount, discounts, priceType);           
        }
        
        private Price CreatePrice(List<RateDetails> rateDetails)
        {
            var firstRateDetails = rateDetails.First();
            var currency = firstRateDetails.PaymentDetails.Currency;
            var totalPrice = rateDetails.Sum(rateDetailsItem => rateDetailsItem.PaymentDetails.TotalPrice);
            var discounts = rateDetails.Select(rateDetailsItem => new Discount(rateDetailsItem.PaymentDetails.DiscountPercent)).ToList();
            var moneyAmount = new MoneyAmount(totalPrice, currency);
            
            return new Price(moneyAmount, moneyAmount, discounts, PriceTypes.RoomContractSet);           
        }
        
        
        private List<DailyPrice> CreateDailyPrices(in PaymentDetails paymentDetails)
        {
            var currency = paymentDetails.Currency;
            
            return paymentDetails.SeasonPrices.SelectMany(seasonPriceDetails => seasonPriceDetails.DailyPrices)
                .OrderBy(dailyPrice => dailyPrice.FromDate).Select(dailyPrice =>
                {
                    var moneyAmount = new MoneyAmount(dailyPrice.Price, currency);
                    
                    return new DailyPrice(dailyPrice.FromDate, dailyPrice.ToDate, moneyAmount, moneyAmount, PriceTypes.Room, dailyPrice.Description);
                }).ToList();
        }
        
        
        
        private string CreateAvailabilityId() => Guid.NewGuid().ToString("N");


        private int GetNumberOfNights(DateTime checkInDate, DateTime checkOutDate) => (checkOutDate.Date - checkInDate.Date).Days;

        
        private readonly IAccommodationResponseService _accommodationResponseService;
    }
}