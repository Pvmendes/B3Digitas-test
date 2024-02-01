using Library.Core.DTOs;
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
        // ... other operations
    }
}
