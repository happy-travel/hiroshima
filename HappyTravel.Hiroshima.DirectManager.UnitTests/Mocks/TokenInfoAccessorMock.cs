using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.UnitTests.Mocks
{
    public class TokenInfoAccessorMock : ITokenInfoAccessor
    {
        public TokenInfoAccessorMock()
        {
            _identityHash = "94e53dea-313e-4e30-a8fb-6af1202a8ca6";
        }


        public string GetIdentityHash() 
            => _identityHash;



        public string GetClientId()
        {
            throw new NotImplementedException();
        }


        public async Task<string> GetAccessToken()
        {
            throw new NotImplementedException();
        }


        private readonly string _identityHash;
    }
}
