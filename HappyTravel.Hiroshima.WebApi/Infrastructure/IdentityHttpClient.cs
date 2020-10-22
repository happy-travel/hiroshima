using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using IdentityModel.Client;

namespace HappyTravel.Hiroshima.WebApi.Infrastructure
{
    public class IdentityHttpClient
    {
        public IdentityHttpClient(HttpClient httpClient, ITokenInfoAccessor tokenInfoAccessor)
        {
            _httpClient = httpClient;
            _tokenInfoAccessor = tokenInfoAccessor;
        }


        public async Task<string> GetEmail()
        {
            var doc = await _httpClient.GetDiscoveryDocumentAsync();
            var token = await _tokenInfoAccessor.GetAccessToken();

            var userInfo = await _httpClient.GetUserInfoAsync(new UserInfoRequest {Token = token, Address = doc.UserInfoEndpoint});
            var claims = userInfo.Claims;

            return claims != null
                ? claims.SingleOrDefault(c => c.Type == "email").Value
                : string.Empty;
        }

        
        private readonly HttpClient _httpClient;
        private readonly ITokenInfoAccessor _tokenInfoAccessor;
    }
}