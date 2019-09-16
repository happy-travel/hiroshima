using System;
using Hiroshima.DbData.Models;

namespace Hiroshima.DirectContracts.Models.Responses
{
    public class DcSeasonPrice
    {
        private Price _price;
        public string SeasonName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Price Price
        {
            get => _price ??= new Price();
            set => _price = value;
        }
    }
}
