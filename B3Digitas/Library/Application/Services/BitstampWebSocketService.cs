using Library.Core.DTOs;
using Library.Core.Entities;
using Library.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class BitstampWebSocketService
    {
        private readonly ClientWebSocket _webSocket = new ClientWebSocket();
        private string _latestData; // Stores the latest data received
        private readonly ICryptoCurrencyService _cryptoCurrencyService;

        public BitstampWebSocketService(ICryptoCurrencyService cryptoCurrencyService)
        {
            _cryptoCurrencyService = cryptoCurrencyService;
        }

        public async Task ConnectAsync(string uri)
        {
            if (_webSocket.State != WebSocketState.Open)
            {
                await _webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                await SubscribeToChannels();
            }
        }

        private async Task SubscribeToChannels()
        {
            var btcSubscription = new
            {
                @event = "bts:subscribe",
                data = new { channel = "order_book_btcusd" }
            };
            var ethSubscription = new
            {
                @event = "bts:subscribe",
                data = new { channel = "order_book_ethusd" }
            };

            await SendMessageAsync(JsonSerializer.Serialize(btcSubscription));
            await SendMessageAsync(JsonSerializer.Serialize(ethSubscription));
        }

        private async Task SendMessageAsync(string message)
        {
            var messageBuffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(messageBuffer);
            await _webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task GetData()
        {
            // Check if WebSocket is connected
            if (_webSocket.State == WebSocketState.Open)
            {
                _latestData = "";
                // Read data
                WebSocketReceiveResult result;
                var messageBuffer = new byte[4096];
                do
                {
                    result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(messageBuffer), CancellationToken.None);
                    var message = Encoding.UTF8.GetString(messageBuffer, 0, result.Count);
                                       
                    _latestData += message; // Update the latest data received
                }
                while (!result.EndOfMessage);

                // Process message based on channel
                if (_latestData.Contains("btcusd"))
                {
                    // Process BTC/USD data
                    ProcessCurrencyData(_latestData, "BTC/USD");
                }
                else if (_latestData.Contains("ethusd"))
                {
                    // Process ETH/USD data
                    ProcessCurrencyData(_latestData, "ETH/USD");
                }
            }
            else
            {
                throw new InvalidOperationException("WebSocket is not connected.");
            }
        }

        private void ProcessCurrencyData(string message, string currencyPairDescription)
        {
            // Deserialize the WebSocket message to an appropriate object
            var orderBook = ManualMappingOrderBookDTO(JsonSerializer.Deserialize<OrderBookJson>(message));

            if (orderBook != null && orderBook.Bids.Count > 0 && orderBook.Asks.Count > 0)
            {
                var _currencyData = new Dictionary<string, CurrencyData>();

                if (!_currencyData.ContainsKey(currencyPairDescription))
                    _currencyData[currencyPairDescription] = new CurrencyData();
                
                var data = _currencyData[currencyPairDescription];

                // Process each bid and ask in the order book
                foreach (var bid in orderBook.Bids)
                {
                    data.Prices.Add(bid.Price);
                    data.Quantities.Add(bid.Quantity);
                    data.HighestPrice = Math.Max(data.HighestPrice, bid.Price);
                    data.LowestPrice = Math.Min(data.LowestPrice, bid.Price);
                }

                foreach (var ask in orderBook.Asks)
                {
                    data.Prices.Add(ask.Price);
                    data.Quantities.Add(ask.Quantity);
                    data.HighestPrice = Math.Max(data.HighestPrice, ask.Price);
                    data.LowestPrice = Math.Min(data.LowestPrice, ask.Price);
                }

                var Metrics = CalculateAndDisplayMetrics(currencyPairDescription, _currencyData);

                _cryptoCurrencyService.SaveData(new CryptoCurrencyEntitie()
                {
                    OrderBook = orderBook,
                    CurrencyMetrics = Metrics,
                    RegisterDate = DateTime.UtcNow
                });
            }
        }
        private OrderBook ManualMappingOrderBookDTO(OrderBookJson orderBookJson)
        {
            var channelSliptArray = orderBookJson.channel.Split('_');
            var orderBookDTO = new OrderBook() { 
                CurrencyPair = channelSliptArray[2], 
                Bids = new List<Order>(), 
                Asks = new List<Order>() 
            };

            if (orderBookJson.data.asks is not null)            
                orderBookDTO.Asks.AddRange(
                    orderBookJson.data.asks.Select(x =>
                       new Order
                       {
                           Price = decimal.Parse(x[0]),
                           Quantity = float.Parse(x[1])
                       }));           

            if (orderBookJson.data.bids is not null)
                orderBookDTO.Bids.AddRange(
                    orderBookJson.data.bids.Select(x =>
                        new Order
                        {
                            Price = decimal.Parse(x[0]),
                            Quantity = float.Parse(x[1])                            
                        }));

            return orderBookDTO;
        }
        private CurrencyMetrics CalculateAndDisplayMetrics(string currencyPairDescription, Dictionary<string, CurrencyData>  _currencyData)
        {
            var data = _currencyData[currencyPairDescription];

            decimal averagePrice = data.Prices.Any() ? data.Prices.Average() : 0;
            float averageQuantity = data.Quantities.Any() ? data.Quantities.Average() : 0;

            if (data.Prices.Any() && data.Quantities.Any())
            {
                Console.WriteLine($"<<<<<<<<<<<<<<< {currencyPairDescription.Replace("/USD","")} >>>>>>>>>>>>>>>>");
                Console.WriteLine($"Currency Pair: {currencyPairDescription}");
                Console.WriteLine($"Highest Price: {data.HighestPrice}");
                Console.WriteLine($"Lowest Price: {data.LowestPrice}");
                Console.WriteLine($"Average Price: {averagePrice}");
                Console.WriteLine($"Average Quantity: {averageQuantity}");
                Console.WriteLine($"<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>");
            }      
            
            return new CurrencyMetrics {
                CurrencyPairDescription = currencyPairDescription,
                HighestPrice = data.HighestPrice,
                LowestPrice = data.LowestPrice,
                AveragePrice = averagePrice, 
                AverageQuantity = averageQuantity 
            };          
        }
    }  
}
