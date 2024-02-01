using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class CryptoCurrencyEntitie
    {
        public Guid Id { get; set; } // Unique identifier
        public required OrderBook OrderBook { get; set; }
        public required CurrencyMetrics CurrencyMetrics { get; set; }
        // ... other properties as per requirement
    }
}
