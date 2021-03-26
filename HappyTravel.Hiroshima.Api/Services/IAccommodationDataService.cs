using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.Api.Services
{
    public interface IAccommodationDataService
    {
        Task<Result<EdoContracts.Accommodations.Accommodation>> GetAccommodationDetails(int accommodationId, string languageCode);
    }
}