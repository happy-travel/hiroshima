using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Infrastructure
{
    public interface ITokenInfoAccessor
    {
        string GetIdentityHash();

        string GetClientId();

        Task<string> GetAccessToken();
    }
}