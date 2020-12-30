using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.Common.Infrastructure
{
    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow() => DateTime.UtcNow;

        public DateTime UtcTomorrow() => DateTime.UtcNow.AddDays(1).Date;

        public DateTime UtcToday() => DateTime.UtcNow.Date;
    }
}
