using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.Api.Services.AvailabilitySearch;

namespace HappyTravel.Hiroshima.Api.Services
{
    public class AccommodationDataService : IAccommodationDataService
    {
        public AccommodationDataService(IAccommodationManagementService accommodationManagementService, IAccommodationResponseService accommodationResponseService)
        {
            _accommodationManagementService = accommodationManagementService;
            _accommodationResponseService = accommodationResponseService;
        }
        
        
        public Task<Result<EdoContracts.Accommodations.Accommodation>> GetAccommodationDetails(int accommodationId, string languageCode)
            => _accommodationManagementService.GetInternal(accommodationId)
                .Map(accommodation => _accommodationResponseService.Create(accommodation, languageCode));


        private readonly IAccommodationManagementService _accommodationManagementService;
        private readonly IAccommodationResponseService _accommodationResponseService;
    }
}