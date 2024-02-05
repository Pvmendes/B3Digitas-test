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

        public async Task SaveData(CryptoCurrencyEntitie cryptoCurrency)
        {
            if (cryptoCurrency is not null && cryptoCurrency.RegisterDate != DateTime.MinValue)
                await _cryptoCurrencyRepository.AddAsync(cryptoCurrency);
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
    }
}
