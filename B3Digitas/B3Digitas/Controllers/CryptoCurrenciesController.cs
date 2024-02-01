using Library.Core.Interfaces;
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

        // POST api/<CryptoCurrenciesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CryptoCurrenciesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CryptoCurrenciesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
