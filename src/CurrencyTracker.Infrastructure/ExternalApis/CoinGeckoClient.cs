using System;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;

namespace CurrencyTracker.Infrastructure.ExternalAPIs;

public class CoinGeckoClient : IPriceProvider
{
    public string ProviderName => throw new NotImplementedException();

    public int Priority => throw new NotImplementedException();

    public Task<MarketPriceDTO> GetPriceAsync(string baseCurrency, string quoteCurrency)
    {
        throw new NotImplementedException();
    }

    public bool IsSupported(string baseCurrency)
    {
        throw new NotImplementedException();
    }
}
