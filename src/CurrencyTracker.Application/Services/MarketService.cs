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

    public async Task<MarketPriceDTO> GetMarketPriceAsync(string symbol, string quoteCurrency)
    {
        var provider = _providers.FirstOrDefault(x=>x.IsSupported(symbol));
        if(provider is null)
        {
            _logger.LogWarning("no provider is found for the symbol {Symbol}", symbol);
            throw new KeyNotFoundException("No provider is found for the symbol");
        }

        try
        {
            var priceData = await provider.GetPriceAsync(symbol, quoteCurrency);
           _logger.LogInformation("Price is fetched for the symbol {Symbol} and the provider is {Provider}", symbol, provider.ProviderName);
            return priceData;


           
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error while fetching the price for the symbol {Symbol} and the provider is {Provider}", symbol, provider.ProviderName);
            throw;
            
        }
    }
}
