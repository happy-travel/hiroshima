using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AccommodationManagementService: IAccommodationManagementService
    {
        public AccommodationManagementService(DirectContracts.Services.Management.IAccommodationManagementService accommodationManagement)
        {
            _accommodationManagement = accommodationManagement;
        }
        
        
        public Task<Result<Models.Responses.Accommodation>> GetAccommodation(int accommodationId)
        {
            throw new System.NotImplementedException();
        }

        
        public Task<Result<Models.Responses.Accommodation>> AddAccommodation(Models.Requests.Accommodation accommodation)
        {
            throw new System.NotImplementedException();
        }

        
        public Task<Result> DeleteAccommodation(string accommodationId)
        {
            throw new System.NotImplementedException();
        }

        
        public Task<Result> UpdateAccommodation(string accommodationId, Models.Requests.Accommodation accommodation)
        {
            throw new System.NotImplementedException();
        }

        private readonly DirectContracts.Services.Management.IAccommodationManagementService _accommodationManagement;
    }
}