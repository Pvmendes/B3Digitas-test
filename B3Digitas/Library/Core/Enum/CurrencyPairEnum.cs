using System.Text.Json.Serialization;

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
