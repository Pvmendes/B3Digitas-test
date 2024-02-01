using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class CurrencyMetrics
    {
        public string CurrencyPairDescription { get; set; }
        public decimal HighestPrice { get; set; }
        public decimal LowestPrice { get; set; }
        public decimal AveragePrice { get; set; }
        public float AverageQuantity { get; set; }
    }
}
