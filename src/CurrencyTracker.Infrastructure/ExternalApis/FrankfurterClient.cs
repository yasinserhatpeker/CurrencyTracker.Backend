using System;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;

namespace CurrencyTracker.Infrastructure.ExternalApis;

public class FrankfurterClient : IPriceProvider
{
    public string ProviderName => "Frankfurter";

    public int Priority => 10;

    private readonly HttpClient _client;

    public FrankfurterClient(HttpClient client)
    {
        _client = client;
    }

    public Task<MarketPriceDTO> GetPriceAsync(string symbol, string quoteCurrency)
    {

    }

    public bool IsSupported(string symbol)
    {
        var fiatCurrencies = new[] { "USD", "EUR", "TRY", "GBP", "JPY", "AUD", "CAD" };
        return fiatCurrencies.Contains(symbol.ToUpper());
    }

    private MarketPriceDTO CreateStaticPrice(string symbol, decimal price, string quote) => new()
    {
        Symbol = symbol,
        Price = price,
        QuoteCurrency = quote,
        Source = ProviderName,
        LastUpdated = DateTime.UtcNow

    };
}
