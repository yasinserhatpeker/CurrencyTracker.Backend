using System;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;

namespace CurrencyTracker.Infrastructure.ExternalApis;

public class FrankfurterClient : IPriceProvider
{
    public string ProviderName => "Frankfurter";

    public int Priority =>10;

   private readonly HttpClient _client;

   public FrankfurterClient(HttpClient client)
    {
        _client=client;
    }

    public Task<MarketPriceDTO> GetPriceAsync(string symbol, string quoteCurrency)
    {
        throw new NotImplementedException();
    }

    public bool IsSupported(string symbol)
    {
        throw new NotImplementedException();
    }
}
