using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.DTOs
{
    public class CryptoCurrencyDTO
    {
        public string Symbol { get; set; } // e.g., BTC/USD
        public decimal LastPrice { get; set; } // Last traded price
        // ... other relevant data you want to expose
    }
}
