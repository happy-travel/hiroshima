using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class AvailabilityIdGenerator : IAvailabilityIdGenerator
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
        
         
        private InternalRateDetailsSlim Convert(RateDetails rateDetails) => new InternalRateDetailsSlim
        {
            RoomId = rateDetails.Room.Id,
            AccommodationId = rateDetails.Room.AccommodationId,
            PaymentDetails = rateDetails.PaymentDetails,
            BoardBasis = rateDetails.BoardBasis,
            CancellationPolicies = rateDetails.CancellationPolicies,
            RoomType = rateDetails.RoomType,
            OccupationRequest = rateDetails.OccupationRequest
        };


        private readonly SHA256Managed _sha256Managed = new SHA256Managed();
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            IgnoreNullValues = true
        };
        
        
        private static readonly object Locker = new object();
        
        
        private class InternalRateDetailsSlim
        {
            public int RoomId { get; set; }
            public int AccommodationId { get; set; }
            public PaymentDetails PaymentDetails { get; set; }
            public BoardBasisTypes BoardBasis { get; set; }
            public List<CancellationPolicyDetails> CancellationPolicies { get; set; }
            public RoomTypes RoomType { get; set; }
            public RoomOccupationRequest OccupationRequest { get; set; }
        }
    }
}