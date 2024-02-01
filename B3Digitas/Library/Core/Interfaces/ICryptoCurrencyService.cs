using Library.Core.DTOs;
using Library.Core.Entities;
using Library.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Interfaces
{
    public interface ICryptoCurrencyService
    {
        Task<IEnumerable<CryptoCurrencyDTO>> GetAllCryptoCurrenciesAsync();
        Task<CryptoCurrencyDTO> GetCryptoCurrencyByIdAsync(Guid id);
        Task<decimal> GetBestPriceAsync(string symbol, decimal quantity, string operationType);
        Task SaveData(CryptoCurrencyEntitie cryptoCurrency);
        Task<decimal> CalculateBestPrice(CurrencyPairEnum symbol, float quantity, bool isBuyOperation);
    }
}
