using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FloxDc.CacheFlow;
using FloxDc.CacheFlow.Extensions;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class AvailabilityDataStorage : IAvailabilityDataStorage
    {
        public AvailabilityDataStorage(IDistributedFlow distributedFlow)
        {
            _distributedFlow = distributedFlow;
        }
        
        
        public Task Add(Common.Models.Availabilities.Availability availability)
        {
            var rateDetailsInternal = availability.AvailableRates.SelectMany(ar => ar.Value)
                .Select(r => new RateDetailsInternal{Id = r.Id, Hash = r.Hash}).ToList();

            var availabilityInternal = new AvailabilityInternal{ Id = availability.Id, RateDetails = rateDetailsInternal};
            
            return _distributedFlow.SetAsync(BuildKey(availability.Id), availabilityInternal, CacheExpirationTime);
        }


        public async Task<string> GetHash(string availabilityId, Guid availableRateId)
        {
            var availabilityInternal = await _distributedFlow.GetAsync<AvailabilityInternal>(BuildKey(availabilityId));
            if (string.IsNullOrEmpty(availabilityInternal.Id))
                return string.Empty;

            var rateDetails = availabilityInternal.RateDetails.SingleOrDefault(rd => rd.Id.Equals(availableRateId));

            return rateDetails.Id.Equals(default)
                ? string.Empty
                : rateDetails.Hash;
        }


        private string BuildKey(string availabilityId) => _distributedFlow.BuildKey(nameof(AvailabilityDataStorage), availabilityId);
        
        
        private readonly IDistributedFlow _distributedFlow;
        
        
        private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(15);

        
        private struct AvailabilityInternal
        {
            public string Id { get; set; }
            public List<RateDetailsInternal> RateDetails { get; set;}
        }
        
        
        private struct RateDetailsInternal
        {
            public Guid Id { get; set; }
            public string Hash { get; set; }
        }
    }
}