using System.Collections.Generic;
using Hiroshima.DbData.Models.Rates;

namespace Hiroshima.DbData.Models.Rooms
{
    public class MealPlan
    {
        public string Code { get; set;}
        public string Name { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
    }
}
