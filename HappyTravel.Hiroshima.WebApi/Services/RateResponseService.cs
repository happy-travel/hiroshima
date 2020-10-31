using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.EdoContracts.General;
using HappyTravel.EdoContracts.General.Enums;
using HappyTravel.Hiroshima.DirectContracts.Models;
using HappyTravel.Money.Models;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public class RateResponseService : IRateResponseService
    {
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
            var price = CalculatePrice(rateDetails);
            var deadline = CalculateDeadline(roomContracts);
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
            var dailyPrices = CalculateDailyPrices(rateDetails.PaymentDetails);
            var totalPrice = CalculatePrice(rateDetails.PaymentDetails, PriceTypes.Room);
            var adultsNumber = rateDetails.OccupationRequest.AdultsNumber;
            var childrenAges = rateDetails.OccupationRequest.ChildrenAges;
            var isExtraBedNeeded = rateDetails.OccupationRequest.IsExtraBedNeeded;
            var roomType = rateDetails.RoomType;
            var deadline = CalculateDeadline(rateDetails.CancellationPolicies);
            var isAdvancePurchaseRate = false;
            
            return new RoomContract(boardBasis, mealPlan, contractTypeCode, isAvailableImmediately, idDynamic, contractDescription, remarks, dailyPrices, totalPrice, adultsNumber, childrenAges, roomType, isExtraBedNeeded, deadline, isAdvancePurchaseRate);
        }


        private Deadline CalculateDeadline(List<CancellationPolicyDetails> cancellationPolicies)
        {
            if (!cancellationPolicies.Any())
                return new Deadline();
            
            var policies = cancellationPolicies
                .Select(cancellationPolicyDetail => new CancellationPolicy(cancellationPolicyDetail.StartDate, cancellationPolicyDetail.Percent)).ToList();
            
            return new Deadline(policies.First().FromDate, policies);
        }
        
        
        private Deadline CalculateDeadline(List<RoomContract> roomContracts)
            => roomContracts.Select(roomContract => roomContract.Deadline).OrderBy(roomContract => roomContract.Date).FirstOrDefault();
        
        
        private Price CalculatePrice(PaymentDetails paymentDetails, PriceTypes priceType)
        {
            var moneyAmount = new MoneyAmount(paymentDetails.TotalPrice, paymentDetails.Currency);
            var discounts = new List<Discount> {new Discount(Convert.ToDecimal(paymentDetails.DiscountPercent))};
            
            return new Price(moneyAmount, moneyAmount, discounts, priceType);           
        }
        
        private Price CalculatePrice(List<RateDetails> rateDetails)
        {
            var firstRateDetails = rateDetails.First();
            var currency = firstRateDetails.PaymentDetails.Currency;
            var totalPrice = rateDetails.Sum(rateDetailsItem => rateDetailsItem.PaymentDetails.TotalPrice);
            var discounts = rateDetails.Select(rateDetailsItem => new Discount(Convert.ToDecimal(rateDetailsItem.PaymentDetails.DiscountPercent))).ToList();
            var moneyAmount = new MoneyAmount(totalPrice, currency);
            
            return new Price(moneyAmount, moneyAmount, discounts, PriceTypes.RoomContractSet);           
        }
        
        
        private List<DailyPrice> CalculateDailyPrices(in PaymentDetails paymentDetails)
        {
            var currency = paymentDetails.Currency;
            
            return paymentDetails.SeasonPrices.SelectMany(seasonPriceDetails => seasonPriceDetails.DailyPrices)
                .OrderBy(dailyPrice => dailyPrice.FromDate).Select(dailyPrice =>
                {
                    var moneyAmount = new MoneyAmount(dailyPrice.Price, currency);
                    
                    return new DailyPrice(dailyPrice.FromDate, dailyPrice.ToDate, moneyAmount, moneyAmount, PriceTypes.Room, dailyPrice.Description);
                }).ToList();
        }
    }
}