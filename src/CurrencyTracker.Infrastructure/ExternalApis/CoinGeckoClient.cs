using System.Text.Json;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace CurrencyTracker.Infrastructure.ExternalAPIs;

public class CoinGeckoClient : IPriceProvider
{
    private readonly HttpClient _client;

    private readonly ILogger<CoinGeckoClient> _logger;

    public string ProviderName => "CoinGecko";

    public int Priority => 1;

    private static readonly Dictionary<string,string> _coinMappings = new(StringComparer.OrdinalIgnoreCase)
    {
        {"BTC","bitcoin"},
        {"ETH","ethereum"},
        {"USDT","tehter"},
        {"BNB","binancecoin"},
        {"SOL","solana"},
        {"XRP","ripple"},
        {"USDC","usd-coin"},
        {"ADA","cardano"},
        {"AVAX","avalanche-2"},
        {"DOGE","dogecoin"}

    };
    
   public CoinGeckoClient(HttpClient client , ILogger<CoinGeckoClient> logger)
   {
       _client = client;
       _logger = logger;
   }

   public bool IsSupported(string baseCurrency)
    {
        return _coinMappings.ContainsKey(baseCurrency);
    }

    public async Task<MarketPriceDTO> GetPriceAsync(string baseCurrency, string quoteCurrency)
    {
        var coinGeckoId = _coinMappings[baseCurrency];
        var quoteParam =  quoteCurrency.ToLower();
       try
        {
        var url = $"simple/price?ids={coinGeckoId}&vs_currencies={quoteParam}";

        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(jsonString);
        var root = jsonDoc.RootElement;

        if(root.TryGetProperty(coinGeckoId ,out var coinElement)&& coinElement.TryGetProperty( quoteParam, out var priceElement))
        {
            return new MarketPriceDTO
            {
                BaseCurrency = baseCurrency,
                Price = priceElement.GetDecimal(),
                QuoteCurrency = quoteCurrency,
                Source = ProviderName,
                LastUpdated = DateTime.UtcNow
            };
        }
        else
        {
            throw new KeyNotFoundException($"Price not found payload for {baseCurrency} coinGeckoId: {coinGeckoId} and quote: {quoteParam}");
        }

    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An error occurred while fetching price from {Provider}", ProviderName);
        throw;
    }
            
        }

  
}
