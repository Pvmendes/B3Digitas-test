using Library.Application.Services;
using Library.Core.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3Digitas.Tests
{
    [TestFixture]
    public class BitstampWebSocketServiceTests
    {
        private Mock<ICryptoCurrencyService> _mockCryptoCurrencyService;
        private BitstampWebSocketService _webSocketService;

        [SetUp]
        public void Setup()
        {
            _webSocketService = new BitstampWebSocketService(_mockCryptoCurrencyService.Object);
        }

        [Test]
        public async Task ConnectAsync_CallsWebSocketConnect()
        {
            // Act
            await _webSocketService.ConnectAsync("wss://ws.bitstamp.net/");

            // Assert
            //Assert.ThatAsync(_webSocketService.ConnectAsync, It.);
            //_mockWebSocketClient.Verify(client => client.ConnectAsync(uri, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
