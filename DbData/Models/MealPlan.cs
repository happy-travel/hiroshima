using System;
using System.Collections.Generic;
using System.Text;

namespace Hiroshima.DbData.Models
{
    public class MealPlan
    {
        public string Code { get; set;}
        public string Name { get; set; }
        public IEnumerable<Rate> Agreements { get; set; }
    }
}
