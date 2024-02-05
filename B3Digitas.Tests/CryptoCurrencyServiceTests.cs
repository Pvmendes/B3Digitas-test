using Library.Application.Services;
using Library.Core.Entities;
using Library.Core.Enum;
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
    public class CryptoCurrencyServiceTests
    {
        private Mock<ICryptoCurrencyRepository> _mockRepository;
        private CryptoCurrencyService _service;

        private OrderBook sampleOrderBook; 
        private CurrencyMetrics currencyMetrics;
        private float quantity;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ICryptoCurrencyRepository>();
            _service = new CryptoCurrencyService(_mockRepository.Object);

            sampleOrderBook = new OrderBook
            {
                CurrencyPair = "",
                Asks = new List<Order>
                {
                    new Order { Price = 10000, Quantity = 2 },
                    new Order { Price = 10100, Quantity = 3 }
                },
                Bids = new List<Order>
                {
                    new Order { Price = 10000, Quantity = 2 },
                    new Order { Price = 10100, Quantity = 3 }
                }
            };

            currencyMetrics = new CurrencyMetrics() 
            {
                CurrencyPairDescription = "",
                HighestPrice = 0,
                LowestPrice =0,
                AveragePrice = 0,
                AverageQuantity = 0
            };

            quantity = 5;
        }

        #region smoke tests

        [Test]
        public async Task CalculateBestPrice_ReturnsCorrectTotalCost_ForBuyOperation()
        {
            sampleOrderBook.CurrencyPair = CurrencyPairEnum.btcusd.ToString();

            _mockRepository.Setup(r => r.GetLatestBySymbolAsync(CurrencyPairEnum.btcusd))
                           .ReturnsAsync(new CryptoCurrencyEntitie { 
                               OrderBook = sampleOrderBook,
                               CurrencyMetrics = currencyMetrics,
                           });

            // Act
            var result = await _service.CalculateBestPrice(CurrencyPairEnum.btcusd, quantity, OperationEnum.Buy);

            // Assert
            Assert.That(result.TotalCost, Is.EqualTo(50300));
            Assert.That(result.RequestedQuantity, Is.EqualTo(5));
            Assert.That(result.OperationType, Is.EqualTo(OperationEnum.Buy.ToString()));
            Assert.That(result.UsedOrders.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task CalculateBestPrice_ReturnsCorrectTotalCost_ForSellOperation()
        {
            sampleOrderBook.CurrencyPair = CurrencyPairEnum.btcusd.ToString();

            _mockRepository.Setup(r => r.GetLatestBySymbolAsync(CurrencyPairEnum.btcusd))
                           .ReturnsAsync(new CryptoCurrencyEntitie
                           {
                               OrderBook = sampleOrderBook,
                               CurrencyMetrics = currencyMetrics,
                           });

            // Act
            var result = await _service.CalculateBestPrice(CurrencyPairEnum.btcusd, quantity, OperationEnum.Sell);

            // Assert
            Assert.That(result.TotalCost, Is.EqualTo(50300));
            Assert.That(result.RequestedQuantity, Is.EqualTo(5));
            Assert.That(result.OperationType, Is.EqualTo(OperationEnum.Sell.ToString()));
            Assert.That(result.UsedOrders.Count, Is.EqualTo(2));
        }

        [Test]
        public void CalculateBestPrice_ThrowsInvalidOperationException_WhenInsufficientLiquidity()
        {
            _mockRepository.Setup(r => r.GetLatestBySymbolAsync(CurrencyPairEnum.btcusd))
                           .ReturnsAsync(new CryptoCurrencyEntitie
                           {
                               OrderBook = sampleOrderBook,
                               CurrencyMetrics = currencyMetrics,
                           });

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.CalculateBestPrice(CurrencyPairEnum.btcusd, 10, OperationEnum.Sell));
        }
        #endregion

        [Test]
        public async Task CalculateBestPrice_AccuratelyCalculatesTotalCost_ForLargeBuyOrder()
        {
            // Arrange            
            float largeQuantity = 100; // Large quantity for the test

            var sampleOrderBook2 = new OrderBook
            {
                CurrencyPair = "",
                Asks = new List<Order>
                {
                    new Order { Price = 10000, Quantity = 30 },
                    new Order { Price = 10100, Quantity = 30 },
                    new Order { Price = 10200, Quantity = 40 }
                },
                Bids = new List<Order>
                {
                    new Order { Price = 10000, Quantity = 2 },
                    new Order { Price = 10100, Quantity = 3 }
                }
            };

            _mockRepository.Setup(r => r.GetLatestBySymbolAsync(CurrencyPairEnum.btcusd))
                           .ReturnsAsync(new CryptoCurrencyEntitie
                           {
                               OrderBook = sampleOrderBook2,
                               CurrencyMetrics = currencyMetrics,
                           });

            decimal expectedTotalCost = (30 * 10000) + (30 * 10100) + (40 * 10200); // Expected cost calculation

            // Act
            var result = await _service.CalculateBestPrice(CurrencyPairEnum.btcusd, largeQuantity, OperationEnum.Buy);

            // Assert
            Assert.That(result.TotalCost, Is.EqualTo(expectedTotalCost));// Verify if the total cost is as expected
            Assert.That(result.RequestedQuantity, Is.EqualTo(largeQuantity));
            Assert.That(result.OperationType, Is.EqualTo(OperationEnum.Buy.ToString()));
            Assert.That(result.UsedOrders.Count, Is.EqualTo(3));// All orders should be used for this large quantity
        }

        [Test]
        public void CalculateBestPrice_ThrowsException_ForSellZeroQuantity()
        {
            // Arrange
            float quantity = 0; // Zero quantity for the test

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.CalculateBestPrice(CurrencyPairEnum.btcusd, quantity, OperationEnum.Sell));
        }

        [Test]
        public void CalculateBestPrice_ThrowsException_ForBuyZeroQuantity()
        {
            // Arrange
            float quantity = 0; // Zero quantity for the test

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.CalculateBestPrice(CurrencyPairEnum.btcusd, quantity, OperationEnum.Buy));
        }
        
        [Test]
        public void CalculateBestPrice_ThrowsException_ForSellNegativeQuantity()
        {
            // Arrange
            float quantity = -5; // Negative quantity for the test

            _mockRepository.Setup(r => r.GetLatestBySymbolAsync(CurrencyPairEnum.btcusd))
                            .ReturnsAsync(new CryptoCurrencyEntitie
                            {
                                OrderBook = sampleOrderBook,
                                CurrencyMetrics = currencyMetrics,
                            });
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.CalculateBestPrice(CurrencyPairEnum.btcusd, quantity, OperationEnum.Sell));

            Assert.That(ex.Message, Is.EqualTo("Quantity cannot be 0 or negative."));
        }

        [Test]
        public void CalculateBestPrice_ThrowsException_ForBuyNegativeQuantity()
        {
            // Arrange
            float quantity = -5; // Negative quantity for the test

            _mockRepository.Setup(r => r.GetLatestBySymbolAsync(CurrencyPairEnum.btcusd))
                            .ReturnsAsync(new CryptoCurrencyEntitie
                            {
                                OrderBook = sampleOrderBook,
                                CurrencyMetrics = currencyMetrics,
                            });
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.CalculateBestPrice(CurrencyPairEnum.btcusd, quantity, OperationEnum.Buy));

            Assert.That(ex.Message, Is.EqualTo("Quantity cannot be 0 or negative."));
        }

    }
}
