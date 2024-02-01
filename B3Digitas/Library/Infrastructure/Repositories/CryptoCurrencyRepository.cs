using Library.Core.Entities;
using Library.Core.Interfaces;
using Library.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Library.Core.Enum;

namespace Library.Infrastructure.Repositories
{
    public class CryptoCurrencyRepository : ICryptoCurrencyRepository
    {
        private readonly MongoDbContext _context;

        public CryptoCurrencyRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CryptoCurrencyEntitie cryptoCurrency)
        {
            await _context.CryptoCurrencies.InsertOneAsync(cryptoCurrency);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.CryptoCurrencies.DeleteOneAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<CryptoCurrencyEntitie>> GetAllAsync()
        {
            return await _context.CryptoCurrencies.Find(_ => true).ToListAsync();
        }

        public async Task<CryptoCurrencyEntitie> GetByIdAsync(Guid id)
        {
            return await _context.CryptoCurrencies.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(CryptoCurrencyEntitie cryptoCurrency)
        {
            await _context.CryptoCurrencies.ReplaceOneAsync(c => c.Id == cryptoCurrency.Id, cryptoCurrency);
        }
        public async Task<CryptoCurrencyEntitie> GetLatestBySymbolAsync(CurrencyPairEnum symbol)
        {
            return await _context.CryptoCurrencies
                                 .Find(c => c.OrderBook.CurrencyPair == symbol.ToString())
                                 .SortByDescending(c => c.RegisterDate)
                                 .FirstOrDefaultAsync();
        }
    }
}
