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

        [HttpGet("GetBestPrice")]
        public async Task<IActionResult> GetBestPrice(CurrencyPairEnum symbol, float quantity, OperationEnum Operation)
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
