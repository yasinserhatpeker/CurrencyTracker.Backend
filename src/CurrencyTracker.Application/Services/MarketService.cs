using System;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace CurrencyTracker.Application.Services;

public class MarketService : IMarketService
{
   private readonly IEnumerable<IPriceProvider> _providers;
   private readonly ILogger<MarketService> _logger; 

   public MarketService(IEnumerable<IPriceProvider> providers, ILogger<MarketService> logger)
   {
      _providers = providers.OrderBy(x => x.Priority);
      _logger = logger;
   }

    public async Task<MarketPriceDTO> GetMarketPriceAsync(string baseCurrency, string quoteCurrency)
    {
      
   var sortedProviders = _providers.OrderBy(x => x.Priority);

    foreach (var provider in sortedProviders)
    {
        if (provider.IsSupported(baseCurrency))
        {
            try
            {
                var priceData = await provider.GetPriceAsync(baseCurrency, quoteCurrency);
                
                _logger.LogInformation("Price fetched for {BaseCurrency}/{QuoteCurrency} via {Provider}", 
                    baseCurrency, quoteCurrency, provider.ProviderName);
                    
                return priceData; // Success! Exit the loop and return the data.
            }
            catch (Exception ex)
            {
                
                _logger.LogWarning(ex, "Provider {Provider} failed to fetch price for {BaseCurrency}. Trying fallback...", 
                    provider.ProviderName, baseCurrency);
                
                continue; 
            }
        }
    }
    _logger.LogError("No available provider could fetch the price for {BaseCurrency}", baseCurrency);
    throw new KeyNotFoundException($"No functioning market provider found for {baseCurrency}");
}
    }

