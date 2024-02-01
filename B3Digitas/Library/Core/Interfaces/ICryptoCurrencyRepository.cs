using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Interfaces
{
    public interface ICryptoCurrencyRepository
    {
        Task<CryptoCurrencyEntitie> GetByIdAsync(Guid id);
        Task<IEnumerable<CryptoCurrencyEntitie>> GetAllAsync();
        Task AddAsync(CryptoCurrencyEntitie cryptoCurrency);
        Task UpdateAsync(CryptoCurrencyEntitie cryptoCurrency);
        Task DeleteAsync(Guid id);
        // ... other necessary methods for data access
    }
}
