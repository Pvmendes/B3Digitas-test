using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.DTOs
{
    public class CurrencyData
    {
        public List<decimal> Prices { get; set; } = new List<decimal>();
        public List<float> Quantities { get; set; } = new List<float>();
        public decimal HighestPrice { get; set; } = decimal.MinValue;
        public decimal LowestPrice { get; set; } = decimal.MaxValue;
    }
}
