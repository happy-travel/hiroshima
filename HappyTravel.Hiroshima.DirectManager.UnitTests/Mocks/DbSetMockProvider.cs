using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace HappyTravel.Hiroshima.DirectManager.UnitTests.Mocks
{
    public class DbSetMockProvider
    {
        public static DbSet<T> GetDbSetMock<T>(IEnumerable<T> enumerable) where T : class
        {
            var list = enumerable is List<T> enumerableList ? enumerableList : enumerable.ToList();
            var mock = list.AsQueryable().BuildMockDbSet();

            mock.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(list.Add);

            return mock.Object;
        }
    }
}
