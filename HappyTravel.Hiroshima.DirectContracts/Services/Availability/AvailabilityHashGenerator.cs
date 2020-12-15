using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Models.Availabilities;
using HappyTravel.Money.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class AvailabilityHashGenerator : IAvailabilityHashGenerator
    {
        public string Generate(List<RateDetails> rateDetails)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(rateDetails.Select(Convert), _jsonSerializerOptions);
            
            return ComputeSha256Hash(bytes);
        }

       
        private string ComputeSha256Hash(byte[] bytes)
        {
            byte[] hash;
            lock (Locker)
            {
                hash = _sha256Managed.ComputeHash(bytes);
            }
            var stringBuilder = new StringBuilder();  
            foreach (var b in hash)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }


        private InternalSlimRateDetails Convert(RateDetails rateDetails)
            => new InternalSlimRateDetails
            (
                rateDetails.Room.Id,
                rateDetails.Room.AccommodationId,
                rateDetails.PaymentDetails.TotalAmount,
                rateDetails.PaymentDetails.Discount.Percent,
                rateDetails.BoardBasis,
                rateDetails.CancellationPolicies.Select(cp => new InternalSlimCancellationPolicies
                (
                    cp.FromDate,
                    cp.ToDate,
                    cp.PenaltyAmount,
                    cp.Percent
                )).ToList(),
                rateDetails.RoomType,
                rateDetails.OccupationRequest
            );


        private readonly SHA256Managed _sha256Managed = new SHA256Managed();
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            IgnoreNullValues = true
        };
        
        
        private static readonly object Locker = new object();
        
        
        private readonly struct InternalSlimRateDetails
        {
            public InternalSlimRateDetails(int roomId, int accommodationId, MoneyAmount totalAmount, decimal discount, BoardBasisTypes boardBasis, List<InternalSlimCancellationPolicies> cancellationPolicies, RoomTypes roomType, RoomOccupationRequest occupationRequest)
            {
                RoomId = roomId;
                AccommodationId = accommodationId;
                TotalAmount = totalAmount;
                Discount = discount;
                BoardBasis = boardBasis;
                CancellationPolicies = cancellationPolicies;
                RoomType = roomType;
                OccupationRequest = occupationRequest;
            }


            public int RoomId { get; }
            public int AccommodationId { get; }
            public MoneyAmount TotalAmount { get; }
            public decimal Discount { get; }
            public BoardBasisTypes BoardBasis { get; }
            public List<InternalSlimCancellationPolicies> CancellationPolicies { get; }
            public RoomTypes RoomType { get; }
            public RoomOccupationRequest OccupationRequest { get; }
        }
        
        
        private readonly struct InternalSlimCancellationPolicies
        {
            public InternalSlimCancellationPolicies(DateTime fromDate, DateTime toDate, MoneyAmount penaltyAmount, decimal percent)
            {
                FromDate = fromDate;
                ToDate = toDate;
                PenaltyAmount = penaltyAmount;
                Percent = percent;
            }


            public DateTime FromDate { get; }
            public DateTime ToDate { get; }
            public MoneyAmount PenaltyAmount { get;  }
            public decimal Percent { get; }
        }
    }
}