using Library.Core.DTOs;
using Library.Core.Entities;
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

        public async Task<string> GetData()
        {
            var DataDTO = new CurrencyData();
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

            return JsonSerializer.Serialize(DataDTO);
        }

        private void ProcessCurrencyData(string message, string currencyPair)
        {
            //Console.WriteLine(message);

            // Deserialize the WebSocket message to an appropriate object
            var orderBook = ManualMappingOrderBookDTO(JsonSerializer.Deserialize<OrderBookJson>(message));

            if (orderBook != null)
            {
                var _currencyData = new Dictionary<string, CurrencyData>();

                if (!_currencyData.ContainsKey(currencyPair))
                {
                    _currencyData[currencyPair] = new CurrencyData();
                }

                var data = _currencyData[currencyPair];

                // Process each bid and ask in the order book
                if(orderBook.Bids is not null)
                    foreach (var bid in orderBook.Bids)
                    {
                        data.Prices.Add(bid.Price);
                        data.Quantities.Add(bid.Quantity);
                        data.HighestPrice = Math.Max(data.HighestPrice, bid.Price);
                        data.LowestPrice = Math.Min(data.LowestPrice, bid.Price);
                    }

                if (orderBook.Asks is not null)
                    foreach (var ask in orderBook.Asks)
                    {
                        data.Prices.Add(ask.Price);
                        data.Quantities.Add(ask.Quantity);
                        data.HighestPrice = Math.Max(data.HighestPrice, ask.Price);
                        data.LowestPrice = Math.Min(data.LowestPrice, ask.Price);
                    }

                CalculateAndDisplayMetrics(currencyPair, _currencyData);
            }
        }
        private OrderBook ManualMappingOrderBookDTO(OrderBookJson orderBookJson)
        {
            var orderBookDTO = new OrderBook() { Bids = new List<Order>(), Asks = new List<Order>() };

            if (orderBookJson.data.asks is not null)            
                orderBookDTO.Asks.AddRange(orderBookJson.data.asks.Select(x =>
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

        private void CalculateAndDisplayMetrics(string currencyPair, Dictionary<string, CurrencyData>  _currencyData)
        {
            var data = _currencyData[currencyPair];

            decimal averagePrice = data.Prices.Any() ? data.Prices.Average() : 0;
            float averageQuantity = data.Quantities.Any() ? data.Quantities.Average() : 0;

            if (data.Prices.Any() && data.Quantities.Any())
            {
                Console.WriteLine($"<<<<<<<<<<<<<<< {currencyPair.Replace("/USD","")} >>>>>>>>>>>>>>>>");
                Console.WriteLine($"Currency Pair: {currencyPair}");
                Console.WriteLine($"Highest Price: {data.HighestPrice}");
                Console.WriteLine($"Lowest Price: {data.LowestPrice}");
                Console.WriteLine($"Average Price: {averagePrice}");
                Console.WriteLine($"Average Quantity: {averageQuantity}");
                Console.WriteLine($"<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>");
            }           
        }
    }  
}
