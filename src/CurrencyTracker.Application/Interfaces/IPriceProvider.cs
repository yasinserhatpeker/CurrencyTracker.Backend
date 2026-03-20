using CurrencyTracker.Application.DTOs;

namespace CurrencyTracker.Application.Interfaces;

public interface IPriceProvider
{
    Task <MarketPriceDTO> GetPriceAsync(string baseCurrency, string quoteCurrency);
    bool IsSupported(string baseCurrency);

    string ProviderName { get; }

    int Priority { get; }

}
