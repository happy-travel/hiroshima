using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.WebApi.Infrastructure
{
    public interface ITokenInfoAccessor
    {
        string GetIdentity();

        string GetClientId();

        Task<string> GetAccessToken();
    }
}