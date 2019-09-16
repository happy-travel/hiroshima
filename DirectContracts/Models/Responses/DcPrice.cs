using System.Collections.Generic;
using Hiroshima.DbData.Models;

namespace Hiroshima.DirectContracts.Models.Responses
{
    public class DcPrice
    {
        public Price TotalPrice
        {
            get => _totalPrice ??= new Price();
            set => _totalPrice = value;
        }

        public List<DcSeasonPrice> SeasonsPrices
        {
            get => _seasonsPrices ??= new List<DcSeasonPrice>();
            set => _seasonsPrices = value;
        }

        private List<DcSeasonPrice> _seasonsPrices;
        private Price _totalPrice;
    }
}
