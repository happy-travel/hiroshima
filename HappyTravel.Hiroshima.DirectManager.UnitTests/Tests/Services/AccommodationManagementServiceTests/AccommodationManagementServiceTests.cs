using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HappyTravel.Hiroshima.DirectManager.UnitTests.Tests.Services.AccommodationManagementServiceTests
{
    public class AccommodationManagementServiceTests
    {
        [Fact]
        public async Task Agency_mismatch_must_fail_get_agent()
        {
            //var (_, isFailure, _, _) = await _agentService.GetAgent(4, AgentContext);
            var isFailure = false;
            Assert.True(isFailure);
        }
    }
}
