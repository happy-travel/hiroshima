using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace HappyTravel.Hiroshima.DirectManager.Infrastructure
{
    public class TokenInfoAccessor: ITokenInfoAccessor
    {
        public TokenInfoAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public string GetIdentityHash() 
            => GetClaimValue("sub");

        
        public string GetClientId() 
            => GetClaimValue("client_id");

        
        public async Task<string> GetAccessToken() 
            => await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");


        private string GetClaimValue(string claimType)
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims;
            return (claims != null
                ? claims.SingleOrDefault(claim => claim.Type == claimType)?.Value
                : string.Empty) ?? string.Empty;
        }


        private readonly IHttpContextAccessor _httpContextAccessor;
    }
}