using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Core.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CurrencyPairEnum
    {
        /// <summary>
        /// BTC/USD
        /// </summary>
        btcusd = 0,
        /// <summary>
        /// ETH/USD
        /// </summary>
        ethusd = 1,
    }
}
