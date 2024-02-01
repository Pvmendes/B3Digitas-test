using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class OrderBook
    {
        public string CurrencyPair { get; set; }
        public List<Order> Bids { get; set; }
        public List<Order> Asks { get; set; }
    }

    public class Order
    {
        public decimal Price { get; set; }
        public float Quantity { get; set; }
    }
}
