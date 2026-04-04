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
    
   public CoinGeckoClient(HttpClient client , ILogger<CoinGeckoClient> logger)
   {
       _client = client;
       _logger = logger;
   }

    public Task<MarketPriceDTO> GetPriceAsync(string baseCurrency, string quoteCurrency)
    {
        throw new NotImplementedException();
    }

    public bool IsSupported(string baseCurrency)
    {
        throw new NotImplementedException();
    }
}
