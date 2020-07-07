using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.DirectManager.Models.Responses;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AccommodationManagementService: IAccommodationManagementService
    {
        public Task<Result<Accommodation>> GetAccommodation(int accommodationId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<int>> AddAccommodation(HappyTravel.DirectManager.Models.Requests.Accommodation accommodation)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> DeleteAccommodation(string accommodationId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<Accommodation>> UpdateAccommodation(string accommodationId, HappyTravel.DirectManager.Models.Requests.Accommodation accommodation)
        {
            throw new System.NotImplementedException();
        }
    }
}