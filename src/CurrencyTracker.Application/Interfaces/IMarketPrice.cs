using System;
using CurrencyTracker.Application.DTOs;

namespace CurrencyTracker.Application.Interfaces;

public interface IMarketPrice
{  
    Task<MarketPriceDTO> GetMarketPriceAsync(string symbol, string quoteCurrency);
}
