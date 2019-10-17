using System.Collections.Generic;
using System.Linq;

namespace Hiroshima.DirectContracts.Models
{
    public class Price
    {
        public decimal TotalPrice => _seasonsPrices.Sum(i => i.Price);
        public List<SeasonPrice> SeasonsPrices => _seasonsPrices ??= new List<SeasonPrice>();

        private List<SeasonPrice> _seasonsPrices;
    }
}
