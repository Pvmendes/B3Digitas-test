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
                    _cryptoCurrencyService.ProcessCurrencyDataAsync(_latestData, "BTC/USD");
                }
                else if (_latestData.Contains("ethusd"))
                {
                    // Process ETH/USD data
                    _cryptoCurrencyService.ProcessCurrencyDataAsync(_latestData, "ETH/USD");
                }
            }
            else
            {
                throw new InvalidOperationException("WebSocket is not connected.");
            }
        }
    }  
}
