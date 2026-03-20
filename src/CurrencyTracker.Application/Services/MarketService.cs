using System;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;

namespace CurrencyTracker.Application.Services;

public class MarketService : IMarketService
{
    public Task<MarketPriceDTO> GetMarketPriceAsync(string symbol, string quoteCurrency)
    {
        throw new NotImplementedException();
    }
}
