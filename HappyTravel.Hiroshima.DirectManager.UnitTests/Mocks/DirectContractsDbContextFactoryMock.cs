using HappyTravel.Hiroshima.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HappyTravel.Hiroshima.DirectManager.UnitTests.Mocks
{
    public class DirectContractsDbContextFactoryMock
    {
        public static Mock<DirectContractsDbContext> Create()
        {
            return new Mock<DirectContractsDbContext>(new DbContextOptions<DirectContractsDbContext>());
        }
    }
}