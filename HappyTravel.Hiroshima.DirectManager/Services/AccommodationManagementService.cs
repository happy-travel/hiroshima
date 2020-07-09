using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AccommodationManagementService: IAccommodationManagementService
    {
        public AccommodationManagementService(DirectContracts.Services.Management.IAccommodationManagementRepository accommodationManagementRepository)
        {
            _accommodationManagementRepository = accommodationManagementRepository;
        }
        
        
        public Task<Result<Models.Responses.Accommodation>> Get(int accommodationId)
        {
            throw new System.NotImplementedException();
        }

        
        public Task<Result<Models.Responses.Accommodation>> Add(Models.Requests.Accommodation accommodation)
        {
            throw new System.NotImplementedException();
        }

        
        public Task<Result> Remove(string accommodationId)
        {
            throw new System.NotImplementedException();
        }

        
        public Task<Result> Update(string accommodationId, Models.Requests.Accommodation accommodation)
        {
            throw new System.NotImplementedException();
        }

        
        private readonly DirectContracts.Services.Management.IAccommodationManagementRepository _accommodationManagementRepository;
    }
}