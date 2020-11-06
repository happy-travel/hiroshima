using System;
using System.Threading.Tasks;
using FloxDc.CacheFlow;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.EdoContracts.Accommodations;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public class AvailabilitySearchStore : IAvailabilitySearchStore
    {
        public AvailabilitySearchStore(IDistributedFlow distributedFlow)
        {
            _distributedFlow = distributedFlow;
        }


        public Task Add(in Availability availability)
            => _distributedFlow.SetAsync(BuildKey(availability.AvailabilityId), availability, CacheExpirationTime);


        public Task<Availability> Get(string availabilityId) => _distributedFlow.GetAsync<Availability>(BuildKey(availabilityId));


        public Task Remove(string availabilityId) => _distributedFlow.RemoveAsync(BuildKey(availabilityId));
        
        
        private string BuildKey(string availabilityId)
            => _distributedFlow.BuildKey(nameof(AvailabilitySearchStore), availabilityId);
        
        
        private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(15);
        private readonly IDistributedFlow _distributedFlow;
    }
}