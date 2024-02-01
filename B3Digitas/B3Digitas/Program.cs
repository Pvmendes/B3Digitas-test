using Library.Core.Interfaces;
using Library.Infrastructure.Data;
using Library.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using Library.Application.Services;
using Library.Application.Helper;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var ConnectionString = EnvironmentHelper.GetConfig("DATABASE_CONNECTION")??"";

if (string.IsNullOrEmpty(ConnectionString))
{
    ConnectionString = builder.Configuration.GetConnectionString("DefaultConnectionMongoDB");
}

builder.Services.AddSingleton<MongoDbContext>(sp =>new MongoDbContext(ConnectionString, "B3Digitas"));

builder.Services.AddScoped<ICryptoCurrencyRepository, CryptoCurrencyRepository>();
builder.Services.AddScoped<ICryptoCurrencyService, CryptoCurrencyService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
    );

app.UseAuthorization();

app.MapControllers();

app.Run();
