// See https://aka.ms/new-console-template for more information

using ConsumerWebSite_ConsoleApp;
using Library.Application.Services;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var serviceCollection = new ServiceCollection();

ServicesConfiguration.ConfigureServices(serviceCollection);

ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

//var eventService = serviceProvider.GetService<IEventoService>();

BitstampWebSocketService bitstampWebSocketService = serviceProvider.GetService<BitstampWebSocketService>();

await bitstampWebSocketService.ConnectAsync("wss://ws.bitstamp.net/");

// Set up a timer to process data every 5 seconds
var timer = new System.Timers.Timer(5000);
timer.Elapsed += (sender, e) => ProcessData.Process(serviceProvider, bitstampWebSocketService);
timer.Start();

// Prevent the application from exiting immediately
Console.ReadLine();


