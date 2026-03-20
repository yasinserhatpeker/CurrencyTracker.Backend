using System;
using CurrencyTracker.Application.DTOs;

namespace CurrencyTracker.Application.Interfaces;

public interface IMarketService
{ 
    Task<MarketPriceDTO> GetMarketPriceAsync(string symbol, string quoteCurrency);
}
