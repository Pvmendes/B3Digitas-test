using Library.Application.Services;
using Library.Core.Interfaces;
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
            await bitstampWebSocketService.GetData();
        }
    }
}
