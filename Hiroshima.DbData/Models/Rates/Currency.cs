﻿using System.Collections.Generic;

namespace Hiroshima.DbData.Models.Rates
{
    public class Currency
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
    }
}
