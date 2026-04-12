using System;
using System.Net.Http.Json;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;


namespace CurrencyTracker.Infrastructure.ExternalApis;

public class FrankfurterClient : IPriceProvider
{
    public string ProviderName => "Frankfurter";

    public int Priority => 2;

    private readonly HttpClient _client;

    public FrankfurterClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<MarketPriceDTO> GetPriceAsync(string baseCurrency, string quoteCurrency)
    {
         var baseAsset = baseCurrency.ToUpper();
         var quoteAsset = quoteCurrency.ToUpper();

         if(baseAsset == quoteAsset)
        {
            return CreateStaticPrice(baseCurrency, 1.0m, quoteCurrency);

        }

        var url = $"latest?from={baseAsset}&to={quoteAsset}";

        var response = await _client.GetFromJsonAsync<FrankfurterResponse>(url);
        
        if(response?.Rates is null || !response.Rates.TryGetValue(quoteAsset, out var price))
        {
    
        throw new KeyNotFoundException($"{quoteAsset} is not found for the {baseCurrency}");
        }

        if(price<=0)
        {
            throw new InvalidOperationException($"Invalid market price {price} received for {baseAsset}.");
        }

        return new MarketPriceDTO
        {
            BaseCurrency = baseAsset,
            Price = price,
            QuoteCurrency = quoteAsset,
            Source = ProviderName,
            LastUpdated = response.Date
        };
    }

    public bool IsSupported(string baseCurrency)
    {
        var supportedCurrencies = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "USD", "EUR", "TRY", "GBP", "JPY", "AUD", "CAD", "CHF", "CNY", 
        "SEK", "NZD", "KRW", "SGD", "NOK", "MXN", "INR", "RUB", "ZAR", 
        "BRL", "HKD", "IDR", "ILS", "PHP", "PLN", "RON", "THB", "DKK",
        "HUF", "CZK", "ISK", "BGN", "MYR"
        };
        return !string.IsNullOrWhiteSpace(baseCurrency) && supportedCurrencies.Contains(baseCurrency.Trim());
    }

    private MarketPriceDTO CreateStaticPrice(string BaseCurrency, decimal price, string quote) => new()
    {
        BaseCurrency = BaseCurrency,
        Price = price,
        QuoteCurrency = quote,
        Source = ProviderName,
        LastUpdated = DateTime.UtcNow

    };
}
