using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data.Models.Accommodations;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public class AccommodationManagementService: IAccommodationManagementService
    {
        public Task<int> AddAccommodation(Accommodation accommodation)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteAccommodation(int accommodationId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateAccommodation(Accommodation accommodation)
        {
            throw new System.NotImplementedException();
        }
    }
}