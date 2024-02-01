using AutoMapper;
using Library.Core.DTOs;
using Library.Core.Entities;
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
        private readonly IMapper _mapper;

        public CryptoCurrencyService(ICryptoCurrencyRepository cryptoCurrencyRepository, IMapper mapper)
        {
            _cryptoCurrencyRepository = cryptoCurrencyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CryptoCurrencyDTO>> GetAllCryptoCurrenciesAsync()
        {
            var cryptoCurrencies = await _cryptoCurrencyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CryptoCurrencyDTO>>(cryptoCurrencies);
        }

        public async Task<CryptoCurrencyDTO> GetCryptoCurrencyByIdAsync(Guid id)
        {
            var cryptoCurrency = await _cryptoCurrencyRepository.GetByIdAsync(id);
            return _mapper.Map<CryptoCurrencyDTO>(cryptoCurrency);
        }

        public async Task<decimal> GetBestPriceAsync(string symbol, decimal quantity, string operationType)
        {
            var cryptocurrencies = await _cryptoCurrencyRepository.GetAllAsync();
            //var selectedCrypto = cryptocurrencies.Where(c => c.Symbol == symbol);

            // Business logic to calculate best price
            decimal bestPrice = 0;
            if (operationType == "buy")
            {
                // Logic for calculating best buy price
            }
            else if (operationType == "sell")
            {
                // Logic for calculating best sell price
            }

            return bestPrice;
        }

        public async Task SaveData(CryptoCurrencyEntitie cryptoCurrency)
        {
            if (cryptoCurrency is not null && cryptoCurrency.RegisterDate != DateTime.MinValue)
                await _cryptoCurrencyRepository.AddAsync(cryptoCurrency);
        }
    }
}
