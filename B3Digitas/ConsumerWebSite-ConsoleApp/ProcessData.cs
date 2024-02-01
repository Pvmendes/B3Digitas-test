using Library.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerWebSite_ConsoleApp
{
    public static class ProcessData
    {
        public static async Task Process(ServiceProvider serviceProvider, BitstampWebSocketService bitstampWebSocketService)
        {
            // Get data from WebSocket
            var data = await bitstampWebSocketService.GetData();

            //Console.WriteLine(data);

            /*
            var cryptoCurrencyService = serviceProvider.GetService<CryptoCurrencyService>();

            // Process and save data using CryptoCurrencyService
            // (You'll need to implement this logic based on your business requirements)
            await cryptoCurrencyService.SaveData(data);
            */
        }
    }
}
