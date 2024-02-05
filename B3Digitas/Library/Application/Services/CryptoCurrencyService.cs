using AutoMapper;
using Library.Core.DTOs;
using Library.Core.Entities;
using Library.Core.Enum;
using Library.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        private readonly ICryptoCurrencyRepository _cryptoCurrencyRepository;

        public CryptoCurrencyService(ICryptoCurrencyRepository cryptoCurrencyRepository)
        {
            _cryptoCurrencyRepository = cryptoCurrencyRepository;
        }

        public async Task SaveWebSiteData(CryptoCurrencyEntitie cryptoCurrency)
        {
            if (cryptoCurrency is not null && cryptoCurrency.RegisterDate != DateTime.MinValue)
                await _cryptoCurrencyRepository.AddAsync(cryptoCurrency);
            else
                throw new InvalidOperationException($"CryptoCurrencyEntitie cannot be null and RegisterDate MinValue.");
        }

        public async Task<CalculationResult> CalculateBestPrice(CurrencyPairEnum symbol, float quantity, OperationEnum operation)
        {
            if (quantity <= 0)
                throw new InvalidOperationException($"Quantity cannot be 0 or negative.");        

            var cryptoCurrency = await _cryptoCurrencyRepository.GetLatestBySymbolAsync(symbol);

            if (cryptoCurrency == null)
                throw new InvalidOperationException($"No data available for {symbol}");

            var orderBook = operation == OperationEnum.Buy ? cryptoCurrency.OrderBook.Asks : cryptoCurrency.OrderBook.Bids;

            orderBook = operation == OperationEnum.Sell ? orderBook.OrderBy(x => x.Price).ToList() : orderBook.OrderByDescending(x => x.Price).ToList();

            decimal totalCost = 0;
            decimal remainingQuantity = (decimal)(Math.Round(quantity, 7));
            var usedOrders = new List<Order>();

            foreach (var order in orderBook)
            {
                if (remainingQuantity <= 0) break;

                var availableQuantity = Math.Min(remainingQuantity, (decimal)(Math.Round(order.Quantity)));
                totalCost += availableQuantity * order.Price;
                remainingQuantity -= availableQuantity;

                usedOrders.Add(order); // Track the used orders
            }

            if (remainingQuantity > 0)
                throw new InvalidOperationException("Not enough liquidity to complete the operation");

            return new CalculationResult
            {
                Id = cryptoCurrency.Id,
                UsedOrders = usedOrders,
                RequestedQuantity = quantity,
                OperationType = operation.ToString(),
                TotalCost = Math.Round(totalCost, 2)
            };
        }


        public async Task ProcessCurrencyDataAsync(string message, string currencyPairDescription)
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

                await SaveWebSiteData(new CryptoCurrencyEntitie()
                {
                    OrderBook = orderBook,
                    CurrencyMetrics = Metrics,
                    RegisterDate = DateTime.UtcNow
                });
            }
        }

        public OrderBook ManualMappingOrderBookDTO(OrderBookJson orderBookJson)
        {
            var channelSliptArray = orderBookJson.channel.Split('_');
            var orderBookDTO = new OrderBook()
            {
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

        public CurrencyMetrics CalculateAndDisplayMetrics(string currencyPairDescription, Dictionary<string, CurrencyData> _currencyData)
        {
            var data = _currencyData[currencyPairDescription];

            decimal averagePrice = data.Prices.Any() ? data.Prices.Average() : 0;
            float averageQuantity = data.Quantities.Any() ? data.Quantities.Average() : 0;

            if (data.Prices.Any() && data.Quantities.Any())
            {
                Console.WriteLine($"<<<<<<<<<<<<<<< {currencyPairDescription.Replace("/USD", "")} >>>>>>>>>>>>>>>>");
                Console.WriteLine($"Currency Pair: {currencyPairDescription}");
                Console.WriteLine($"Highest Price: {data.HighestPrice}");
                Console.WriteLine($"Lowest Price: {data.LowestPrice}");
                Console.WriteLine($"Average Price: {averagePrice}");
                Console.WriteLine($"Average Quantity: {averageQuantity}");
                Console.WriteLine($"<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>");
            }

            return new CurrencyMetrics
            {
                CurrencyPairDescription = currencyPairDescription,
                HighestPrice = data.HighestPrice,
                LowestPrice = data.LowestPrice,
                AveragePrice = averagePrice,
                AverageQuantity = averageQuantity
            };
        }
    }
}
