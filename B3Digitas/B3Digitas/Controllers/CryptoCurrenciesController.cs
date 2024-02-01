using Library.Core.Interfaces;
using Library.Core.Enum;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace B3Digitas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoCurrenciesController : ControllerBase
    {
        private readonly ICryptoCurrencyService _cryptoCurrencyService;

        public CryptoCurrenciesController(ICryptoCurrencyService cryptoCurrencyService)
        {
            _cryptoCurrencyService = cryptoCurrencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cryptoCurrencies = await _cryptoCurrencyService.GetAllCryptoCurrenciesAsync();
            return Ok(cryptoCurrencies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cryptoCurrency = await _cryptoCurrencyService.GetCryptoCurrencyByIdAsync(id);
            if (cryptoCurrency == null)
                return NotFound();

            return Ok(cryptoCurrency);
        }

        [HttpGet("simulateBestPrice")]
        public async Task<IActionResult> SimulateBestPrice(CurrencyPairEnum symbol, float quantity, OperationEnum Operation)
        {
            try
            {
                var result = await _cryptoCurrencyService.CalculateBestPrice(symbol, quantity, Operation);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
