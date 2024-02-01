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
        Task SaveData(CryptoCurrencyEntitie cryptoCurrency);
        Task<CalculationResult> CalculateBestPrice(CurrencyPairEnum symbol, float quantity, OperationEnum operation);
    }
}
