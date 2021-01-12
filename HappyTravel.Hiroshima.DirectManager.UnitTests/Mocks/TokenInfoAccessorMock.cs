using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using System;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.UnitTests.Mocks
{
    public class TokenInfoAccessorMock : ITokenInfoAccessor
    {
        public string GetIdentityHash()
        {
            throw new NotImplementedException();
        }


        public string GetClientId()
        {
            throw new NotImplementedException();
        }


        public async Task<string> GetAccessToken()
        {
            throw new NotImplementedException();
        }
    }
}
