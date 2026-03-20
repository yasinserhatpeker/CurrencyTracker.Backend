using System;
using System.Net.Http.Json;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

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

    public async Task<MarketPriceDTO> GetPriceAsync(string symbol, string quoteCurrency)
    {
          if(symbol.Equals(quoteCurrency, StringComparison.OrdinalIgnoreCase))
        {
            return CreateStaticPrice(symbol, 1.0m, quoteCurrency);
        }

        var url =$"latest?from={quoteCurrency.ToUpper()}&to={symbol.ToUpper()}"; // url -> latest?from=USD&to=EUR

       var response = await _client.GetFromJsonAsync<FrankfurterResponse>(url);

       if(response!.Rates is null || !response.Rates.TryGetValue(symbol.ToUpper(), out var price))
        {
            throw new KeyNotFoundException($"Price for {symbol.ToUpper()} and {quoteCurrency.ToUpper()} is not found");
        }

       return new MarketPriceDTO
       {
           Symbol = symbol.ToUpper(),
           Price = price,
           QuoteCurrency=quoteCurrency.ToUpper(),
           Source = ProviderName,
           LastUpdated = response!.Date,
           ChangePercentage24H = null
       };

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
