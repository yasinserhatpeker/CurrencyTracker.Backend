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
      _providers = providers.OrderByDescending(x => x.Priority);
      _logger = logger;
   }

    public async Task<MarketPriceDTO> GetMarketPriceAsync(string baseCurrency, string quoteCurrency)
    {
        var provider = _providers.FirstOrDefault(x=>x.IsSupported(baseCurrency));
        if(provider is null)
        {
            _logger.LogWarning("no provider is found for the BaseCurrency {BaseCurrency}", baseCurrency);
            throw new KeyNotFoundException("No provider is found for the BaseCurrency");
        }

        try
        {
            var priceData = await provider.GetPriceAsync(baseCurrency, quoteCurrency);
           _logger.LogInformation("Price is fetched for the BaseCurrency {BaseCurrency} and the provider is {Provider}", baseCurrency, provider.ProviderName);
            return priceData;


           
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error while fetching the price for the BaseCurrency {BaseCurrency} and the provider is {Provider}", baseCurrency, provider.ProviderName);
            throw;
            
        }
    }
}
