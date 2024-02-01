using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Library.Core.DTOs
{
    public class OrderBookJson
    {
        public Data data { get; set; }
        public string currencyPair { get; set; }
        public string channel { get; set; }
        public string @event { get; set; }
    }
    public class Data
    {
        public string timestamp { get; set; }
        public string microtimestamp { get; set; }
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }
    }
}
