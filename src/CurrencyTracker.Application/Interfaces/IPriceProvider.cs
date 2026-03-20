using CurrencyTracker.Application.DTOs;

namespace CurrencyTracker.Application.Interfaces;

public interface IPriceProvider
{
    Task<MarketPriceDTO> GetPriceAsync(string symbol, string quoteCurrency);
    bool IsSupported(string symbol);

    string ProviderName { get; }

    int Priority { get; }

}
